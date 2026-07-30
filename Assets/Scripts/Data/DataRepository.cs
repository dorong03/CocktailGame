using System.Collections.Generic;
using UnityEngine;

public class DataRepository : MonoBehaviour
{
    private Dictionary<string, NpcData> npcData;
    private Dictionary<string, OrderGroupData> orderGroupData;
    private Dictionary<string, RecipeData> recipeData;
    private Dictionary<string, IngredientData> ingredientData;
    private Dictionary<string, ToolData> toolData;
    private Dictionary<string, Sprite> ingredientSprites;
    private Dictionary<string, Sprite> npcSprite;

    public NpcData GetNpc(string id)
    {
        if (npcData.TryGetValue(id, out var value))
        {
            return value;
        }
        return null;
    }

    public RecipeData GetRecipeData(string id)
    {
        if (recipeData.TryGetValue(id, out var value))
        {
            return value;
        }
        return null;
    }

    public IngredientData GetIngredientData(string id)
    {
        if (ingredientData.TryGetValue(id, out var value))
        {
            return value;
        }
        return null;
    }

    public ToolData GetTool(string id)
    {
        if (toolData.TryGetValue(id, out var value))
        {
            return value;
        }
        return null;
    }

    public OrderGroupData GetOrderGroup(string id)
    {
        if (orderGroupData.TryGetValue(id, out var value))
        {
            return value;
        }
        return null;
    }

    public Sprite GetIngredientSprite(string id)
    {
        if (ingredientSprites.TryGetValue(id, out var value))
        {
            return value;
        }
        return null;
    }

    public Sprite GetNpcSprite(string id)
    {
        if (npcSprite.TryGetValue(id, out var value))
        {
            return value;
        }
        return null;
    }
}
