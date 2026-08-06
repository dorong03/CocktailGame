using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timeLimit;
    
    private GamePhase phase;
    private Order currentOrder;
    
    [SerializeField]
    private OrderService orderService;
    [SerializeField]
    private NpcController npcController;
    [SerializeField]
    private GameTimer timer;
    [SerializeField] 
    private PlayerStationController station;

    public event Action<GamePhase> OnPhaseChange;

    public void Start()
    {
        GoToMainMenu();
    }
    
    public void GoToMainMenu()
    {
        ChangePhase(GamePhase.MainMenu);
    }
    
    public void StartGame()
    {
        timer.Prepare(timeLimit);
        timer.OnTimeOver += HandleTimeOver;
        NextRound();
    }

    private void CreateNewOrder()
    {
        NpcData npcData = orderService.PickNpc();
        RecipeData recipeData = orderService.PickRecipe(npcData);
        Seat selectedSeat = orderService.PickSeat();
        
        currentOrder = new Order(npcData, recipeData, selectedSeat);
    }

    public void EndGame()
    {
        station.Abort();
        npcController.Clear();
        Debug.Log("Game Ended!!!");
        ChangePhase(GamePhase.Result);
    }

    private void NextRound()
    {
        ChangePhase(GamePhase.Ready);
        CreateNewOrder();
        npcController.SpawnNpc(currentOrder.NpcData, currentOrder.Seat, OnNpcArrived);
    }

    private void OnNpcArrived()
    {
        ChangePhase(GamePhase.Ordering);
        timer.Begin();
        station.BeginOrder(currentOrder.Recipe, currentOrder.Seat, OnStationComplete);
    }

    private void OnStationComplete(MixResult mixResult,Grade grade)
    {
        ChangePhase(GamePhase.Processing);
        // ScoreCalcutae 써서 mixResult 랑 grade 를 통해 점수 내서 추가하기
        npcController.Depart(NextRound);
    }

    private void HandleTimeOver()
    {
        EndGame();
    }

    private void ChangePhase(GamePhase newPhase)
    {
        phase = newPhase;
        OnPhaseChange?.Invoke(phase);
    }
}