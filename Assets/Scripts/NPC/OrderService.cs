using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrderService : MonoBehaviour
{
    private DataRepository repo;
    private System.Random rand;
    [SerializeField]private Seat[] seats;

    public void Start()
    {
        repo = DataRepository.Instance;
        rand = new System.Random();

        //TestSeat();
    }
    public NpcData PickNpc()
    {
        List<string> npcKey = repo.GetNpcKey();
        int randomKey = rand.Next(npcKey.Count);
        NpcData data = repo.GetNpc(npcKey[randomKey]);
        return data;
    }

    public Seat PickSeat(Seat[] seats)
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
    #region Test
    private void TestSeat()
    {
        for (int i = 0; i < 10; i++)
        {
            Seat seat = PickSeat(seats);

            if (seat != null)
            {
                Debug.Log($"{i + 1}번째 : {seat.name}");
            }
        }
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TestPickRecipe();
        }
    }

    private void TestPickRecipe()
    {
        NpcData npc = PickNpc();
        RecipeData recipe = PickRecipe(npc);

        Debug.Log($"손님 : {npc.Name} + 주문 : {recipe.Name}+ 레시피 ID : {recipe.Id}");
    }
    #endregion
}