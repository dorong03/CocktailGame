using System;
using System.Collections.Generic;
using UnityEngine;

public class CocktailMaker : MonoBehaviour
{
    // 액체 집계 담당
    // 어디에 붓는지(Cup / MixingCup)는 PlayerStation 이 정해서 IPourTarget 으로 넘겨줌

    [SerializeField]
    private List<Bottle> bottles;

    private RecipeData currentRecipe;
    private IPourTarget currentTarget;
    private Dictionary<string, float> pouredAmounts = new Dictionary<string, float>();

    private Action onMixReady;
    private Action<MixResult> onComplete;

    public void StartMix(RecipeData recipe, IPourTarget target, Action onMixReady, Action<MixResult> onComplete)
    {
        currentRecipe = recipe;
        currentTarget = target;
        this.onMixReady = onMixReady;
        this.onComplete = onComplete;
        pouredAmounts.Clear();

        for (int i = 0; i < recipe.Ingredients.Count; i++)
        {
            IngredientAmount ingre = recipe.Ingredients[i];
            pouredAmounts.Add(ingre.IngredientId, 0);
            bottles[i].Show();
            bottles[i].Init(ingre.IngredientId, null, target, HandlePour);
        }
    }

    public bool AllPouredAtLeastOnce()
    {
        foreach (var amount in pouredAmounts)
        {
            if (amount.Value == 0)
            {
                return false;
            }
        }
        return true;
    }

    // 부은 총량 / 필요 총량. 도구 완료 후 MixingCup -> Cup 연출에도 사용
    public float ComputeFillRatio()
    {
        if (currentRecipe == null) return 0f;

        float poured = 0f;
        float required = 0f;
        foreach (var ingre in currentRecipe.Ingredients)
        {
            poured += pouredAmounts[ingre.IngredientId];
            required += ingre.Amount;
        }
        return required > 0f ? Mathf.Clamp01(poured / required) : 0f;
    }

    public void FinishMix()
    {
        MixResult mixResult = GetBuildResult();
        onComplete?.Invoke(mixResult);
    }

    private void HandlePour(string ingredientId, float amount)
    {
        
        if (!pouredAmounts.TryGetValue(ingredientId, out float value)) return;
        pouredAmounts[ingredientId] += amount;
        #region 테스트코드 추후 삭제
        Dictionary<string, float> dic = new();
        
        foreach(var a in currentRecipe.Ingredients)
        {
            dic[a.IngredientId] = a.Amount;
        }
        Debug.Log($"{ingredientId} 투입량: {pouredAmounts[ingredientId]} / {dic[ingredientId]}");

        #endregion
        if (currentTarget is Cup) currentTarget?.SetFill(ComputeFillRatio(), Color.cyan);

        if (!string.IsNullOrEmpty(currentRecipe.ToolId) && AllPouredAtLeastOnce())
        {
            onMixReady?.Invoke();
            onMixReady = null;
        }
    }

    private MixResult GetBuildResult()
    {
        MixResult mixResult = new MixResult();
        foreach (var ingre in pouredAmounts)
        {
            IngredientResult result = new IngredientResult();
            result.IngredientId = ingre.Key;
            result.Amount = ingre.Value;
            mixResult.Ingredients.Add(result);
        }
        return mixResult;
    }

    // 주문이 끝났을때 병 상태 초기화 + 숨김
    public void HideBottles()
    {
        foreach (var bottle in bottles)
        {
            bottle.DeActivate();
            bottle.Hide();
        }
    }

    public void Abort()
    {
        pouredAmounts.Clear();
        currentRecipe = null;
        currentTarget = null;
        HideBottles();
    }
}
