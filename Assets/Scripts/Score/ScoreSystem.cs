using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int Total { get; private set; }

    public void Add(int score)
    {
        Total += score;
    }

    public bool CommitHigScore()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            int best = PlayerPrefs.GetInt("highScore");
            if (best < Total)
            {
                PlayerPrefs.SetInt("highScore", Total);
            }
            return true;
        }
        else
        {
            PlayerPrefs.SetFloat("highScore", Total);
            return false;
        }
    }
}