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
    [SerializeField] 
    private ScoreSystem scoreSystem;

    public event Action<GamePhase> OnPhaseChange;
    public event Action OnGameStart;
    public event Action OnRecipeSelected;
    
    public void Start()
    {
        Application.targetFrameRate = 60;
        timer.OnTimeOver += HandleTimeOver;
        GoToMainMenu();
    }
    
    public void GoToMainMenu()
    {
        station.Abort();
        npcController.Clear();
        ChangePhase(GamePhase.MainMenu);
    }
    
    public void StartGame()
    {
        Debug.Log("Starting Game");
        scoreSystem.ResetCurrentScore();
        timer.Prepare(timeLimit);
        NextRound();
        OnGameStart?.Invoke();
    }

    public void RestartGame()
    {
        station.Abort();
        npcController.Clear();
        StartGame();
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
        scoreSystem.CommitHighScore();
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
        OnRecipeSelected?.Invoke();
    }

    private void OnStationComplete(MixResult mixResult,Grade grade)
    {
        ChangePhase(GamePhase.Processing);
        
        float accuracy = ScoreCalculator.GetMixAccuracy(currentOrder.Recipe, mixResult);
        int currentRecipeScore = ScoreCalculator.GetFinalScore(currentOrder.Recipe.BaseScore, accuracy);
        float bonusSecond = ScoreCalculator.GetBonusSeconds(grade);
        
        scoreSystem.AddCurrentScore(currentRecipeScore);
        timer.AddBonus(bonusSecond);
        npcController.Depart(NextRound);
        // Debug.Log($"레시피 아이디: {currentOrder.Recipe.Id}, 기본 점수: {currentOrder.Recipe.BaseScore}, 정확도: {accuracy}");
        
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

    public void QuitGame()
    {
        Application.Quit();
    }
}