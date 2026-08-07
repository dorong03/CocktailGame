using System.Collections.Generic;
using UnityEngine;
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
    
    [Header("레시피 UI")]
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

    private void Awake()
    {
        gameManager.OnPhaseChange += OnPhaseChange;
        gameTimer.OnTimeTick += OnTimeTick;
        scoreSystem.OnCurrentScoreAdd += OnCurrentScoreAdd;
        scoreSystem.OnCommitScore += OnCommitScore;
        cocktailMaker.OnIngredientChanged += OnIngredientChanged;
    }

    private void OnPhaseChange(GamePhase newPhase)
    {
        switch(newPhase)
        {
            case GamePhase.MainMenu:
                mainMenuPanel.SetActive(true);
                inGamePanel.SetActive(false);
                gameEndPanel.SetActive(false);
                break;
            case GamePhase.Ready:
            case GamePhase.Ordering:
            case GamePhase.Processing:
                mainMenuPanel.SetActive(false);
                inGamePanel.SetActive(true);
                gameEndPanel.SetActive(false);
                break;
            case GamePhase.Result:
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
        finalScoreText.text = $"{score} 점";
        int best = PlayerPrefs.GetInt("highScore", 0);
        highScoreText.text = isHigh ? "신기록!" : $"최고 점수 {best} 점";
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