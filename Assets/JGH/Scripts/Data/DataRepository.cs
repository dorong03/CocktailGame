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
        return npcData.get(Id);
    }
    public OrderGroupData GetOrderGroup(string Id)
    {
        return orderGroupData.get(Id);
    }
    public RecipeData GetRecipeData(string Id)
    {
        return recipeData.get(Id);
    }
    public IngredientData GetIngredientData(string Id)
    {
        return ingredientData.get(Id);
    }
    public ToolData GetTool(string Id)
    {
        return toolData.get(Id);
    }
}
