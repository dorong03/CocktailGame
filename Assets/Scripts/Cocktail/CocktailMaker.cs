using System;
using System.Collections.Generic;
using UnityEngine;

public class CocktailMaker : MonoBehaviour
{
    // 얘는 칵테일 제조를 맡는 친구
    // 어떤 칵테일을 만들고 언제 재료를 넣고 그런건 PlayStation 에서 결정할 예정

    [SerializeField]
    private List<Bottle> bottles;
    [SerializeField]
    private Cup cup;
    [SerializeField] 
    private MixingCup shaker;
    [SerializeField] 
    private MixingCup barSpoon;
    
    private DataRepository data;

    private RecipeData currentRecipe;
    private Dictionary<string, float> pouredAmounts = new Dictionary<string, float>();

    private Action onMixReady;
    private Action<MixResult> onComplete;

    private void Start()
    {
        data = DataRepository.Instance;
        if (data == null)
        {
            Debug.LogError("DataRepository is not created");
        }
    }

    public void StartMix(RecipeData recipe, string tool, Action onMixReady, Action<MixResult> onComplete)
    {
        currentRecipe = recipe;
        pouredAmounts.Clear();
        this.onMixReady = onMixReady;
        this.onComplete = onComplete;
        
        IPourTarget target = null;
        if (string.IsNullOrEmpty(tool))
        {
            target = cup;
            cup.SetMode(CupMode.Submit);
            cup.SetSubmitHandler(TryFinishCup);
        }
        else
        {
            switch (tool)
            {
                case "TOOL_000":
                    target = shaker;
                    break;
                case "TOOL_001":
                    target = barSpoon;
                    break;
                default:
                    Debug.LogError("Unknown tool: " + tool);
                    target = null;
                    break;
            }
        }
        
        for (int i = 0; i < recipe.Ingredients.Count; i++)
        {
            IngredientAmount ingre = recipe.Ingredients[i];
            pouredAmounts.Add(ingre.IngredientId, 0);
            bottles[i].Init(ingre.IngredientId, null, target, HandlePour);
        }
    }

    private void TryFinishCup()
    {
        if (!AllPouredAtLeastOnce()) return;
        FinishMix();
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

    private void HandlePour(string ingredientId, float amount)
    {
        pouredAmounts[ingredientId] += amount;
        if (!string.IsNullOrEmpty(currentRecipe.ToolId) && AllPouredAtLeastOnce())
        {
            onMixReady?.Invoke();
            onMixReady = null;
        }
    }

    public void FinishMix()
    {
        MixResult mixResult = GetBuildResult();
        onComplete?.Invoke(mixResult);
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
}
