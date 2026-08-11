using System;
using UnityEngine;

public abstract class ToolBase : MonoBehaviour
{
    protected string toolId;
    protected int requiredCount;
    [SerializeField]
    protected int count;
    protected bool started;
    protected DragInput drag;


    protected Func<bool> canStart;
    protected Action onBlocked;
    protected Action onStarted;
    protected Action onComplete;

    protected virtual void Awake()
    {
        drag = GetComponent<DragInput>();
        drag.onGrab = GrabHandler;
        drag.onDragDelta = DragHandler;
        drag.onRelease = ReleaseHandler;
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

    protected virtual void GrabHandler()
    {
        if (started || canStart == null) return;
        if (!canStart())
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

    protected void AddCount(Action onFinished)
    {
        count++;
        if (count >= requiredCount)
        {
            started = false;
            drag.interactable = false;
            count = 0;
            ResetMotion();
            onFinished?.Invoke();
            onComplete?.Invoke();
        }
    }
    
    /*
     * 도구 사용 중단
     */
    public void Abort()
    {
        count = 0;
        started = false;
        drag.interactable = false;
        ResetMotion();
    }

    private void ReleaseHandler()
    {
        if (!started) return;
        ResetMotion();
    }
    
    public abstract void HandleDelta(Vector2 delta);
    
    public abstract void ResetMotion();

    public virtual void Show() { }

    public virtual void Hide() { }
}
