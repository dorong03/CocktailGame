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

    public NpcData GetNpc(string id)
    {
        if (npcData.TryGetValue(id, out var npc))
        {
            return npcData[id];
        }

        return null;
    }
    public OrderGroupData GetOrderGroup(string id)
    {
        if (orderGroupData.TryGetValue(id, out var order))
        {
            return orderGroupData[id];
        }

        return null;
    }
    public RecipeData GetRecipeData(string id)
    {
        if (recipeData.TryGetValue(id, out var recipe))
        {
            return recipeData[id];
        }

        return null;
    }
    public IngredientData GetIngredientData(string id)
    {
        if (ingredientData.TryGetValue(id, out var ingredient))
        {
            return ingredientData[id];
        }

        return null;
    }
    public ToolData GetTool(string id)
    {
        if (toolData.TryGetValue(id, out var tool))
        {
            return toolData[id];
        }

        return null;
    }
}
