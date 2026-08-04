using System;
using UnityEngine;

public class DataTestScirpts : MonoBehaviour
{
    [SerializeField]
    private DataRepository data;

    private void Start()
    {
        RecipeData recipe = data.GetRecipeData("RECIPE_000");
        Debug.Log($"Id : {recipe.Id}, BaseScore : {recipe.BaseScore}");
    }
}
