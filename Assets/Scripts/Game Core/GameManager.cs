using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Order currentOder;
    private MixResult currentMixResult;
    public GameTimer createGameTimer()
    {
        return new GameTimer();
    }
   public void UseGaneTimer(GameTimer gameTimer)
    {

    }
    public ScoreSystem createScoreSystem()
    {
        return new ScoreSystem();
    }
    public void UseScoreSystem(ScoreSystem scoreSystem)
    {
         
    }
    
    public UIMatager createUIManager()
    {
        return new UIMatager();
    }

    public void UseUImanager(UIMatager uIMatager)
    {

    }
    public SoundManager createSoundManager()
    {
        return new SoundManager();
    }

    public void UseSoundManager(SoundManager soundManager)
    {

    }

    public void StartGame()
    {

    }
    public void RequestNextOrder()
    {

    }
    public void FinishMixing()
    {

    }
    public void FinishTool()
    {

    }
    public void FinishServing()
    {

    }
    public void EndGame()
    {

    }
}
