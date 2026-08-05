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

    private RecipeData recipe;
    private Seat seat;
    private Action<Grade> onOrderComplete;

    public void BeginOrder(RecipeData recipe, Seat seat, Action<Grade> onComplete)
    {
        this.recipe = recipe;
        this.seat = seat;
        this.onOrderComplete = onComplete;

        if (string.IsNullOrEmpty(recipe.ToolId))
        {
            maker.StartMix(recipe, null, null, OnMixDone);
        }
        else
        {
            maker.StartMix(recipe, recipe.ToolId, OnIngredientsReady, OnMixDone);
        }
    }

    private void OnIngredientsReady()
    {
        tool.BeginStart(recipe.ToolId, maker.AllPouredAtLeastOnce, OnToolBlocked, OnToolStarted, OnToolFinished);
    }

    private void OnToolFinished()
    {
        maker.FinishMix();
    }

    private void OnMixDone(MixResult mix)
    {
        serve.StartServing(seat, OnServeDone);
    }

    private void OnServeDone(Grade grade)
    {
        onOrderComplete?.Invoke(grade);
    }
    
    // 재료를 다 안넣어서 도구 사용이 안될때 효과 추후 추가할 듯
    private void OnToolBlocked()
    {
        
    }

    // 도구 시작할때 이펙트나 뭐 그런거
    private void OnToolStarted()
    {
        
    }

    public void Abort()
    {
        maker.Abort();
        tool.Abort();
        serve.Abort();
        onOrderComplete = null;
    }
}