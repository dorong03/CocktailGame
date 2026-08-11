using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlassSpritesData
{
    public Sprite FrontSprite { get; private set; }
    public Sprite BackSprite { get; private set; }

    public GlassSpritesData(Sprite front, Sprite back)
    {
        FrontSprite = front;
        BackSprite = back;
    }
}

public class SpriteRepository : MonoBehaviour
{
    public static SpriteRepository Instance { get; private set; }

    private Dictionary<string, Sprite> npcSprites;
    private Dictionary<string, Sprite> ingreSprites;
    private Dictionary<string, GlassSpritesData> glassSprites;
    private Dictionary<string, Sprite> cocktailSprites;

    private const string NpcPath = "Images/Sprites/Npc";
    private const string IngrePath = "Images/Sprites/Ingredients";
    private const string GlassPath = "Images/Sprites/Glasses";
    private const string CocktailPath = "Images/Sprites/Cocktails";

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

    private void Initialize()
    {
        npcSprites = LoadSprites(NpcPath);
        ingreSprites = LoadSprites(IngrePath);  
        glassSprites = LoadGlassSprites(GlassPath);
        cocktailSprites = LoadSprites(CocktailPath);
        
        Debug.Log($"npc Sprite Count : {npcSprites.Count}, ingre Sprite Count : {ingreSprites.Count}, " +
                  $"glass Sprite Coint : {glassSprites.Count}, cocktail Sprite Count : {cocktailSprites.Count}");
    }

    private Dictionary<string, Sprite> LoadSprites(string resourcePath)
    {
        return Resources.LoadAll<Sprite>(resourcePath).ToDictionary(x => x.name, x => x);
    }
    
    private Dictionary<string, GlassSpritesData> LoadGlassSprites(string resourcePath)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(resourcePath);
        Dictionary<string, GlassSpritesData> result = new Dictionary<string, GlassSpritesData>();
        
        Dictionary<string, Sprite> backLookup = new Dictionary<string, Sprite>();
        Dictionary<string, Sprite> frontLookup = new Dictionary<string, Sprite>();

        foreach (Sprite sprite in sprites)
        {
            if (!sprite.name.Contains("Front") && !sprite.name.Contains("Back"))
            {
                Debug.LogError(sprite.name + " <- 잘못된 양식의 이름");
                return null;
            }
            if (sprite.name.EndsWith("_Back"))
            {
                string glassId = sprite.name.Substring(0, sprite.name.Length - "_Back".Length);
                backLookup[glassId] = sprite;
            }
            else if (sprite.name.EndsWith("_Front"))
            {
                string glassId = sprite.name.Substring(0, sprite.name.Length - "_Front".Length);
                frontLookup[glassId] = sprite;
            }
        }

        foreach (string glassId in backLookup.Keys)
        {
            frontLookup.TryGetValue(glassId, out Sprite frontSprite);
            result[glassId] = new GlassSpritesData(frontSprite, backLookup[glassId]);
        }

        return result;
    }

    public Sprite GetNpcSprite(string npcId)
    {
        if (!npcSprites.TryGetValue(npcId, out Sprite sprite))
        {
            Debug.LogError($"{npcId} 에 해당하는 스프라이트가 존재하지 않습니다");
            return null;
        }

        return sprite;
    }
    
    public Sprite GetIngredientSprite(string ingredientId)
    {
        if (!ingreSprites.TryGetValue(ingredientId, out Sprite sprite))
        {
            Debug.LogError($"{ingredientId} 에 해당하는 스프라이트가 존재하지 않습니다");
            return null;
        }

        return sprite;
    }
    
    public Sprite GetCocktailSprite(string cocktailId)
    {
        if (!cocktailSprites.TryGetValue(cocktailId, out Sprite sprite))
        {
            Debug.LogError($"{cocktailId} 에 해당하는 스프라이트가 존재하지 않습니다");
            return null;
        }

        return sprite;
    }
    
    public GlassSpritesData GetGlassSprites(string glassId)
    {
        if (!glassSprites.TryGetValue(glassId, out var data))
        {
            Debug.LogError($"{glassId} 에 해당하는 스프라이트가 존재하지 않습니다");
            return null;
        }

        return data;
    }
}
