using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private GameManager gameManager;
    [SerializeField] 
    private GameTimer gameTimer;
    [SerializeField]
    private Image timerFillImage;

    [SerializeField] 
    private float pixelStep = 16f;
    
    public void Start()
    {
        gameManager.OnPhaseChange += OnPhaseChange;
        gameTimer.OnTimeTick += OnTimeTick;
    }

    private void OnPhaseChange(GamePhase newPhase)
    {
        
    }

    private void OnTimeTick(float remaining)
    {
        float ratio = remaining / gameTimer.MaxTime;
        float stepped = Mathf.Floor(ratio * pixelStep) / pixelStep;
        timerFillImage.fillAmount = stepped;
    }
}
