using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataRepository : MonoBehaviour
{
    private Dictionary<string, NpcData> npcData;
    private Dictionary<string, OrderGroupData> orderGroupData;
    private Dictionary<string, RecipeData> recipeData;
    private Dictionary<string, IngredientData> ingredientData;
    private Dictionary<string, ToolData> toolData;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        DataLoader dataLoader = new DataLoader();

        npcData = dataLoader.LoadJsonData<NpcData>("JsonData/Npc").ToDictionary(x => x.Id, x => x);
        orderGroupData = dataLoader.LoadJsonData<OrderGroupData>("JsonData/OrderGroup").ToDictionary(x => x.Id);
        recipeData = dataLoader.LoadJsonData<RecipeData>("JsonData/Recipe").ToDictionary(x => x.Id);
        ingredientData = dataLoader.LoadJsonData<IngredientData>("JsonData/Ingredient").ToDictionary(x => x.Id);
        toolData = dataLoader.LoadJsonData<ToolData>("JsonData/Tool").ToDictionary(x => x.Id);
    }

    public NpcData GetNpc(string id)
    {
        if (npcData.TryGetValue(id, out var npc))
        {
            return npcData[id];
        }
        Debug.Log("�˸��� �ʰų� �������� ���� ���� �Դϴ�.");
        return null; ;
    }
    public OrderGroupData GetOrderGroup(string id)
    {
        if (orderGroupData.TryGetValue(id, out var order))
        {
            return orderGroupData[id];
        }

        Debug.Log("�˸��� �ʰų� �������� ���� ���� �Դϴ�.");
        return null;
    }
    public RecipeData GetRecipeData(string id)
    {
        if (recipeData.TryGetValue(id, out var recipe))
        {
            return recipeData[id];
        }

        Debug.Log("�˸��� �ʰų� �������� ���� ���� �Դϴ�.");
        return null;
    }
    public IngredientData GetIngredientData(string id)
    {
        if (ingredientData.TryGetValue(id, out var ingredient))
        {
            return ingredientData[id];
        }

        Debug.Log("�˸��� �ʰų� �������� ���� ���� �Դϴ�.");
        return null;
    }
    public ToolData GetTool(string id)
    {
        if (toolData.TryGetValue(id, out var tool))
        {
            return toolData[id];
        }

        Debug.Log("�˸��� �ʰų� �������� ���� ���� �Դϴ�.");
        return null;
    }
}
