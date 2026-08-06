using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrderService : MonoBehaviour
{
    private DataRepository repo;
    private System.Random rand;
    
    [SerializeField]
    private Seat[] seats;

    public void Start()
    {
        repo = DataRepository.Instance;
        rand = new System.Random();
    }
    public NpcData PickNpc()
    {
        List<string> npcKey = repo.GetNpcKey();
        int randomKey = rand.Next(npcKey.Count);
        NpcData data = repo.GetNpc(npcKey[randomKey]);
        return data;
    }

    public Seat PickSeat()
    {
        if (seats == null || seats.Length == 0) return null;

        int randomIndex = rand.Next(seats.Length);
        return seats[randomIndex];
    }

    public RecipeData PickRecipe(NpcData npc)
    {
        if (npc == null) return null;

        OrderGroupData orderGroup = repo.GetOrderGroup(npc.OrderGroupId);

        if (orderGroup == null) return null;

        int randomIndex = rand.Next(orderGroup.RecipeIds.Count);
        string recipeId = orderGroup.RecipeIds[randomIndex];

        return repo.GetRecipeData(recipeId);
    }
}