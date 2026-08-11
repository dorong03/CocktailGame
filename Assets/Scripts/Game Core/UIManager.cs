using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("컴포넌트 필드")]
    [SerializeField] 
    private GameManager gameManager;
    [SerializeField] 
    private GameTimer gameTimer;
    [SerializeField] 
    private ScoreSystem scoreSystem;
    [SerializeField] 
    private CocktailMaker cocktailMaker;

    [Header("패널")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private GameObject settingPanel;
    
    [Header("레시피 UI")]
    [SerializeField]
    OrderSheetPanel orderSheetPanel;
    [SerializeField]
    private Text[] ingreTexts;
    [SerializeField]
    private Text cocktailNameText;
    
    [Header("타이머 필드")]
    [SerializeField]
    private Image timerFillImage;
    [SerializeField] 
    private float pixelStep = 16f;
    
    [Header("점수 필드")]
    [SerializeField] 
    private Text scoreText;
    [SerializeField]
    private Text finalScoreText;
    [SerializeField]
    private Text highScoreText;

    private GamePhase currentPhase;
    private bool isPaused;

    private void Awake()
    {
        gameManager.OnPhaseChange += OnPhaseChange;
        gameManager.OnGameStart += OnGameStart;
        gameManager.OnRecipeSelected += OnRecipeSelected;
        gameTimer.OnTimeTick += OnTimeTick;
        scoreSystem.OnCurrentScoreAdd += OnCurrentScoreAdd;
        scoreSystem.OnCommitScore += OnCommitScore;
        cocktailMaker.OnIngredientChanged += OnIngredientChanged;
    }

    private void Update()
    {
        if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;

        if (isPaused)
        {
            ResumeGame();
        }
        else if (IsInGamePhase(currentPhase))
        {
            PauseGame();
        }
    }

    private bool IsInGamePhase(GamePhase p)
    {
        return p == GamePhase.Ready || p == GamePhase.Ordering || p == GamePhase.Processing;
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (DragInput.DragItem != null)
        {
            DragInput dragItem = DragInput.DragItem;
            dragItem.CancleGrab();
        }
        settingPanel.SetActive(true);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        settingPanel.SetActive(false);
    }

    private void OnGameStart()
    {
        orderSheetPanel.SetPosition(OrderSheetPanel.PanelState.Peek, false);
    }

    private void OnRecipeSelected()
    {
        orderSheetPanel.SetPosition(OrderSheetPanel.PanelState.Expanded, true);
    }

    private void OnPhaseChange(GamePhase newPhase)
    {
        currentPhase = newPhase;

        switch(newPhase)
        {
            case GamePhase.MainMenu:
                if (isPaused) ResumeGame();
                settingPanel.SetActive(false);
                mainMenuPanel.SetActive(true);
                inGamePanel.SetActive(false);
                gameEndPanel.SetActive(false);
                break;
            case GamePhase.Ready:
            case GamePhase.Ordering:
            case GamePhase.Processing:
                if (isPaused) ResumeGame();
                settingPanel.SetActive(false);
                mainMenuPanel.SetActive(false);
                inGamePanel.SetActive(true);
                gameEndPanel.SetActive(false);
                break;
            case GamePhase.Result:
                if (isPaused) ResumeGame();
                mainMenuPanel.SetActive(false);
                inGamePanel.SetActive(true);
                gameEndPanel.SetActive(true);
                break;
        }
    }

    private void OnCurrentScoreAdd(int score)
    {
        scoreText.text = $"{score} 점";
    }

    private void OnTimeTick(float remaining)
    {
        float ratio = remaining / gameTimer.MaxTime;
        float stepped = Mathf.Floor(ratio * pixelStep) / pixelStep;
        timerFillImage.fillAmount = stepped;
    }

    private void OnCommitScore(bool isHigh, int score)
    {
        finalScoreText.text = $"{score}";
        int best = PlayerPrefs.GetInt("highScore", 0);
        // 일단 같지만 추후 변경
        highScoreText.text = isHigh ? $"{best}" : $"{best}";
    }

    private void InitIngredientSheet()
    {
        
    }
    
    private void OnIngredientChanged(RecipeData recipe, Dictionary<string, float> poured)
    {
        cocktailNameText.text = recipe.Name;

        for (int i = 0; i < ingreTexts.Length; i++)
        {
            if (i < recipe.Ingredients.Count)
            {
                IngredientAmount ingre = recipe.Ingredients[i];
                string ingreName = DataRepository.Instance.GetIngredientData(ingre.IngredientId).Name;
                float current = poured[ingre.IngredientId];

                ingreTexts[i].gameObject.SetActive(true);
                ingreTexts[i].text = $"{ingreName}  {current:0} / {ingre.Amount:0}";
            }
            else
            {
                ingreTexts[i].text = "";
            }
        }
    }
}