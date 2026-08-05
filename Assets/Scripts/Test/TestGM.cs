using UnityEngine;
using UnityEngine.InputSystem;

public class TestGM : MonoBehaviour
{
    public PlayerStationController station;
    public OrderService orderService;

    public Seat[] seats;
    
    private void Run()
    {
        NpcData npc = orderService.PickNpc();
        RecipeData recipe = orderService.PickRecipe(npc);
        Seat seat = orderService.PickSeat(seats);
        
        station.BeginOrder(recipe, seat, PrintGrade);
    }

    private void PrintGrade(Grade grade)
    {
        Debug.Log($"최종 등급은 {grade} 입니다.");
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Run();
        }
    }
}
