using System.Collections.Generic;
using UnityEngine;

public class RecipeData : MonoBehaviour
{
    public string Id;
    public string Name;
    public int BaseScore;
    public string ToolId;
    public List<IngredientAmount> Ingredients;
}
