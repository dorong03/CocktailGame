using System;
using UnityEngine;

/*
     * 목적: 게임의 시간을 관리한다.
    private float remaining; - 남아있는시간
    private bool isRunning; - 시간이 흐르고있는가?
    private float maxtime; - 가지고있던 최대시간
*/

public class GameTimer : MonoBehaviour
{
    private float remaining;
    private bool isRunning;
    private float maxTime;

    public event Action OnTimeOver;

    /*
     * 시간 업데이트 
     * 시간 음수 방지
     */
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

    /*
     * 목적: 시간을 셋팅한다
     * float second로 가져온 값으로 셋팅한다.
     */
    public void Prepare(float second)
    {
        isRunning = false;
        maxTime = second;
        remaining = second;
    }

    /*
     * 목적: 시간을 흐르도록 한다.
     */
    public void Begin()
    {
        isRunning = true;
    }

    /*
     * 목적: 현재시간에 시간을 추가한다
     * 쵀대시간은 시작시간을 넘을 수 없다
     */
    public void AddBonus(float second)
    {
        remaining += second;
        if (remaining > maxTime)
        {
            remaining = maxTime;
        }
    }

    /*
     * 목적: 시간을 멈추게 한다.
     */
    public void Stop()
    {
        isRunning = false;
    }
}