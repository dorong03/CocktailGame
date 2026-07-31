using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float remaining;
    private bool isRunning;
    private float maxTime;

    public event Action OnTimeOver;

    private void Update()
    {

        if (!isRunning)
        {
            return;
        }

        remaining -= Time.deltaTime;

        if (remaining <= 0)
        {
            remaining = 0;
            isRunning = false;
            OnTimeOver?.Invoke();
        }
    }

    public void Prepare(float second)
    {
        isRunning = false;
        maxTime = second;
        remaining = second;
    }

    public void Begin()
    {
        isRunning = true;
    }

    public void AddBonus(float second)
    {

        remaining += second;
        if (remaining > maxTime)
        {
            remaining = maxTime;
        }
    }

    public void Stop()
    {

        isRunning = false;
    }
}
