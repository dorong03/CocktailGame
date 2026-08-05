using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataRepository : MonoBehaviour
{
    public static DataRepository Instance { get; private set; }

    private Dictionary<string, NpcData> npcData;
    private Dictionary<string, OrderGroupData> orderGroupData;
    private Dictionary<string, RecipeData> recipeData;
    private Dictionary<string, IngredientData> ingredientData;
    private Dictionary<string, ToolData> toolData;

    private List<string> npcKeyList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

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

        npcKeyList = new List<string>(npcData.Keys);
    }

    public NpcData GetNpc(string id)
    {
        if (npcData.TryGetValue(id, out var npc))
        {
            return npc;
        }
        Debug.Log("일치하는 항목이 없거나 존재하지 않는 ID입니다.");
        return null;
    }

    public List<string> GetNpcKey()
    {
        return npcKeyList;
    }

    public OrderGroupData GetOrderGroup(string id)
    {
        if (orderGroupData.TryGetValue(id, out var order))
        {
            return order;
        }
        Debug.Log("일치하는 항목이 없거나 존재하지 않는 ID입니다.");
        return null;
    }

    public RecipeData GetRecipeData(string id)
    {
        if (recipeData.TryGetValue(id, out var recipe))
        {
            return recipe;
        }
        Debug.Log("일치하는 항목이 없거나 존재하지 않는 ID입니다.");
        return null;
    }

    public IngredientData GetIngredientData(string id)
    {
        if (ingredientData.TryGetValue(id, out var ingredient))
        {
            return ingredient;
        }
        Debug.Log("일치하는 항목이 없거나 존재하지 않는 ID입니다.");
        return null;
    }

    public ToolData GetTool(string id)
    {
        if (toolData.TryGetValue(id, out var tool))
        {
            return tool;
        }
        Debug.Log("일치하는 항목이 없거나 존재하지 않는 ID입니다.");
        return null;
    }
}