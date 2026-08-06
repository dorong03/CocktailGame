using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float remaining;
    private bool isRunning;
    public float MaxTime { get; private set; }

    public event Action OnTimeOver;
    public event Action<float> OnTimeTick;

    private void Update()
    {

        if (!isRunning)
        {
            return;
        }

        remaining -= Time.deltaTime;
        OnTimeTick?.Invoke(remaining);
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
        MaxTime = second;
        remaining = second;
    }

    public void Begin()
    {
        isRunning = true;
    }

    public void AddBonus(float second)
    {

        remaining += second;
        if (remaining > MaxTime)
        {
            remaining = MaxTime;
        }
    }

    public void Stop()
    {

        isRunning = false;
    }
}
