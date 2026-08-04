using System;
using System.Collections.Generic;
using UnityEngine;

public class CocktailMaker : MonoBehaviour
{
    // 얘는 칵테일 제조를 맡는 친구
    // 어떤 칵테일을 만들고 언제 재료를 넣고 그런건 PlayStation 에서 결정할 예정

    [SerializeField]
    private List<Bottle> bottles;
    [SerializeField]
    private Cup cup;
    
    [SerializeField]
    private DataRepository data;

    private RecipeData recipe;
    private Dictionary<string, float> pouredAmounts = new Dictionary<string, float>();

    private Action<MixResult> onComplete;

    private void Awake()
    {

    }
}
