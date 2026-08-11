using System;
using UnityEngine;

public class PlayerStationController : MonoBehaviour
{
    [SerializeField]
    private CocktailMaker maker;
    [SerializeField]
    private ToolController tool;
    [SerializeField]
    private ServingController serve;
    
    [SerializeField]
    private Cup cup;
    [SerializeField]
    private MixingCup shaker;
    [SerializeField]
    private MixingCup barSpoon;

    // MixingCup -> Cup 으로 옮겨 담는 연출 시간
    private const float TransferDuration = 2.5f;

    private RecipeData recipe;
    private Seat seat;
    private MixingCup currentMixingCup;
    private MixResult currentMixResult;
    private Action<MixResult, Grade> onOrderComplete;
    
    private void Start()
    {
        shaker.gameObject.SetActive(false);
        barSpoon.gameObject.SetActive(false);
    }

    public void BeginOrder(RecipeData recipe, Seat seat, Action<MixResult, Grade> onComplete)
    {
        this.recipe = recipe;
        this.seat = seat;
        this.onOrderComplete = onComplete;

        cup.Show(recipe);

        if (string.IsNullOrEmpty(recipe.ToolId))
        {
            // 도구 X : 병에서 잔으로 바로 따르고, 잔을 제출하면 제조 확정
            currentMixingCup = null;
            cup.SetMode(CupMode.Submit);
            cup.SetSubmitHandler(TrySubmitCup);
            maker.StartMix(recipe, cup, null, OnMixDone);
        }
        else
        {
            // 도구 O : 병에서 도구 용기로 따르고, 도구를 다 쓰면 잔으로 옮겨 담음
            currentMixingCup = GetMixingCup(recipe.ToolId);
            currentMixingCup.gameObject.SetActive(true);
            maker.StartMix(recipe, currentMixingCup, OnIngredientsReady, OnMixDone);
            tool.ShowTool(recipe.ToolId);
        }
    }

    private MixingCup GetMixingCup(string toolId)
    {
        switch (toolId)
        {
            case "TOOL_000":
                return shaker;
            case "TOOL_001":
                return barSpoon;
            default:
                Debug.LogError($"존재하지 않는 도구 아이디 -> {toolId}");
                return null;
        }
    }

    // 도구 X 레시피에서 유저가 잔을 짧게 잡았다 놓아 제출했을때 호출
    private void TrySubmitCup()
    {
        if (!maker.AllPouredAtLeastOnce()) return;
        maker.FinishMix();
    }

    // 도구를 사용하는 레시피에서 도구 용기안에 재료를 다 1번씩 넣었을때 호출
    private void OnIngredientsReady()
    {
        tool.BeginStart(maker.AllPouredAtLeastOnce, OnToolBlocked, OnToolStarted, OnToolFinished);
    }

    // 도구를 사용하는 레시피에서 도구 기능을 완료 했을때 호출
    private void OnToolFinished()
    {
        currentMixingCup.PourInto(cup, maker.ComputeFillRatio(), TransferDuration, Color.cyan, maker.FinishMix);
    }

    // 모든 레시피에서 제조가 확정된 순간 호출
    private void OnMixDone(MixResult mix)
    {
        currentMixResult = mix;
        serve.StartServing(seat, OnServeDone);
    }

    // cup 을 던지고 난 후 등급을 판정하고 호출함
    private void OnServeDone(Grade grade)
    {
        MixResult mix = currentMixResult;
        currentMixResult = null;
        ClearStage();
        onOrderComplete?.Invoke(mix, grade);
    }

    // 재료를 다 안넣어서 도구 사용이 안될때 효과 추후 추가할 듯
    private void OnToolBlocked()
    {

    }

    // 도구 시작할때 이펙트나 뭐 그런거
    private void OnToolStarted()
    {
        maker.HideBottles();
    }

    // 무대에 올라온 오브젝트 전부 정리
    private void ClearStage()
    {
        maker.HideBottles();
        tool.Abort();

        cup.SetFill(0f, Color.white);
        cup.SetMode(CupMode.Locked);
        cup.ReturnHome();
        cup.Hide();

        if (currentMixingCup != null)
        {
            currentMixingCup.AbortPour();
            currentMixingCup.gameObject.SetActive(false);
            currentMixingCup = null;
        }
    }

    public void Abort()
    {
        maker.Abort();
        serve.Abort();
        currentMixResult = null;
        ClearStage();
        onOrderComplete = null;
    }
}
