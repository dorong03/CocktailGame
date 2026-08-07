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
    
    [Header("타이머 필드")]
    [SerializeField]
    private Image timerFillImage;
    [SerializeField] 
    private float pixelStep = 16f;
    
    [Header("점수 필드")]
    [SerializeField] 
    private Text scoreText;
    
    public void Start()
    {
        gameManager.OnPhaseChange += OnPhaseChange;
        gameTimer.OnTimeTick += OnTimeTick;
        scoreSystem.OnCurrentScoreAdd += OnCurrentScoreAdd;
        scoreSystem.OnCommitScore += OnCommitScore;
    }

    private void OnPhaseChange(GamePhase newPhase)
    {
        switch(newPhase)
        {
            case GamePhase.MainMenu:
                // 메인 메뉴 Panel 띄우기
                break;
            case GamePhase.Ready:
                // 메인 메뉴 패널 치우고 게임 패널 오픈
                break;
            case GamePhase.Ordering:
                // 이런 곳에서 레시피나 그런거 띄우는걸로
                break;
            case GamePhase.Processing:
                break;
            case GamePhase.Result:
                // 결과창 띄우기
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
        // isHigh 가 True 이면 최고 점수 도달한 화면
        // False 라면 이전 최고점수와 현재점수 띄우기
    }
}
