using System;
using UnityEngine;

public abstract class ToolBase : MonoBehaviour
{
    protected string toolId;
    protected int requiredCount;
    protected int count;
    protected bool started;
    protected DragInput drag;


    protected Func<bool> canStart;
    protected Action onBlocked;
    protected Action onStarted;
    protected Action onComplete;

    private void Awake()
    {
        drag = GetComponent<DragInput>();
        drag.onGrab = GrabHandler;
        drag.onDragDelta = DragHandler;
    }
    
    public void ActiveTool(Func<bool> canStart, Action onBlock, Action onStarted, Action onComplete)
    {
        this.canStart = canStart;
        this.onBlocked = onBlock;
        this.onStarted = onStarted;
        this.onComplete = onComplete;
        started = false;
        count = 0;
        drag.interactable = true;
        ResetMotion();
    }

    private void GrabHandler()
    {
        if (started) return;
        if (canStart != null && !canStart())
        {
            onBlocked?.Invoke();
            return;
        }

        started = true;
        onStarted?.Invoke();
    }

    private void DragHandler(Vector2 delta)
    {
        if (!started) return;
        HandleDelta(delta);
    }
    
    public abstract void HandleDelta(Vector2 delta);

    protected void AddCount()
    {
        count++;
        if (count >= requiredCount)
        {
            started = false;
            drag.interactable = false;
            count = 0;
            onComplete?.Invoke();
        }
    }
    
    /*
     * 도구 사용 중단
     */
    public void Abort()
    {
        if (!started) return;
        count = 0;
        drag.interactable = false;
        ResetMotion();
    }

    /*
     * 도구 별로 잡았다가 놓았을때 초기화 되는 부분
     */
    public abstract void ResetMotion();
}
