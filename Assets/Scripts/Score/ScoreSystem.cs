using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public event Action<int> OnCurrentScoreAdd;
    public event Action<bool, int> OnCommitScore;
    
    public int Total { get; private set; }

    public void AddCurrentScore(int score)
    {
        Total += score;
        OnCurrentScoreAdd?.Invoke(Total);
    }

    public void ResetCurrentScore()
    {
        Total = 0;
        OnCurrentScoreAdd?.Invoke(0);
    }

    public void CommitHighScore()
    {
        if (!PlayerPrefs.HasKey("highScore"))
        {
            PlayerPrefs.SetInt("highScore", 0);
        }


        bool isHighScore = false;
        int best =  PlayerPrefs.GetInt("highScore");
        
        if (Total > best)
        {
            PlayerPrefs.SetInt("highScore", Total);
            isHighScore = true;
        }
        OnCommitScore?.Invoke(isHighScore, Total);
        
        // if (PlayerPrefs.HasKey("highScore"))
        // {
        //     int best = PlayerPrefs.GetInt("highScore");
        //     if (best < Total)
        //     {
        //         PlayerPrefs.SetInt("highScore", Total);
        //     }
        // }
        // else
        // {
        //     PlayerPrefs.SetFloat("highScore", Total);
        // }
    }
}