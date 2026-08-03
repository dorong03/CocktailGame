using System;
using System.Collections.Generic;
using UnityEngine;

public class CocktailMaker : MonoBehaviour
{
    private List<Bottle> bottles;
    private Dictionary<string, MixingCup> vesselByToolId;
    private Cup cup;
    private DataRepository repo;
    private RecipeData currentRecipe;
    private Dictionary<string, float> pouredAmounts;
    private Action<MixResult> onComplete;

    [SerializeField] private Transform[] spawn;

    public void Start()
    {
        
    }

    public void StartMix(string recipe, string tool, Action onComplete)
    {
        
    }
    public bool AllPouredAtLeastOnce()
    {
        return true;
    }
    public void ForceFinishMix()
    {

    }
    public void Abort()
    {

    }
    private void HandlePour(string ingredientId,float amount)
    {

    }
    private MixResult BuildResult()
    {
        return null;
    }
}
