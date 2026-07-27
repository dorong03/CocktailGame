using System.Collections.Generic;
using UnityEngine;

public class DataRepository : MonoBehaviour
{
    private Dictionary<string, NpcData> npcData;
    private Dictionary<string, OrderGroupData> orderGroupData;
    private Dictionary<string, RecipeData> recipeData;
    private Dictionary<string, IngredientData> ingredientData;
    private Dictionary<string, ToolData> toolData;

    public DataLoader createDataLoader()
    {
        return new DataLoader();
    }

    public void UseDataLoader(DataLoader dataLoader)
    {

    }

    public NpcData GetNpc(string Id)
    {
        return null;
    }
    public OrderGroupData GetOrderGroup(string Id)
    {
        return null;
    }
    public RecipeData GetRecipeData(string Id)
    {
        return null;
    }
    public IngredientData GetIngredientData(string Id)
    {
        return null;
    }
    public ToolData GetTool(string Id)
    {
        return null;
    }
}
