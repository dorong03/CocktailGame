using UnityEngine;

public class Order
{
    public Order(NpcData npc,RecipeData recipe, Seat seat)
    {
        NpcData = npc;
        Recipe = recipe;
        Seat = seat;
    }
    
    public NpcData NpcData { get; set; }
    public Seat Seat { get; set; }
    public RecipeData Recipe { get; set; }
    public MixResult Result { get; set; }
    public Grade Grade { get; set; }
    public int Score { get; set; }
}
