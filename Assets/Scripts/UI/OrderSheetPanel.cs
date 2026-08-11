using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderSheetPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum PanelState { Peek, Moving, Expanded }

    [SerializeField] private RectTransform panel;
    [SerializeField] private Vector2 peekPos;
    [SerializeField] private Vector2 expandedPos;
    [SerializeField] private float hoverOffset = 30f;
    [SerializeField] private float moveDuration = 0.2f;

    private PanelState state = PanelState.Peek;
    private Coroutine moveRoutine;

    private Vector2 Direction
    {
        get
        {
            return (expandedPos - peekPos).normalized;
        }
    }

    /*
     * withAnimation 이 true 라면 애니메이션을 출력하며 움직이고 아니라면 바로 위치만 이동
     */
    public void SetPosition(PanelState moveState, bool withAnimation)
    {
        Vector2 target;
        switch (moveState)
        {
            case PanelState.Peek:
                target = peekPos;
                break;
            case PanelState.Expanded:
                target = expandedPos;
                break;
            default:
                Debug.LogWarning($"SetPosition에는 Peek/Expanded만 넘길 수 있음 -> {moveState}");
                return;
        }

        if (withAnimation)
        {
            TransitionTo(moveState, target);
        }
        else
        {
            if (moveRoutine != null) StopCoroutine(moveRoutine);
            panel.anchoredPosition = target;
            state = moveState;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (state)
        {
            case PanelState.Peek:
                MoveVisual(peekPos + Direction * hoverOffset);
                break;
            case PanelState.Expanded:
                // MoveVisual(expandedPos - Direction * hoverOffset);
                break;
            case PanelState.Moving:
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (state)
        {
            case PanelState.Peek:
                MoveVisual(peekPos);
                break;
            case PanelState.Expanded:
                MoveVisual(expandedPos);
                break;
            case PanelState.Moving:
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.Equals(PointerEventData.InputButton.Right)) return;
        
        if (state == PanelState.Peek)
            TransitionTo(PanelState.Expanded, expandedPos);
        else if (state == PanelState.Expanded)
            TransitionTo(PanelState.Peek, peekPos);
    }
    
    private void TransitionTo(PanelState nextState, Vector2 target)
    {
        state = PanelState.Moving;
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveRoutine(target, () => state = nextState));
    }
    
    private void MoveVisual(Vector2 target)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveRoutine(target, null));
    }

    private IEnumerator MoveRoutine(Vector2 target, System.Action onDone)
    {
        Vector2 start = panel.anchoredPosition;
        float t = 0f;
        while (t < moveDuration)
        {
            t += Time.deltaTime;
            panel.anchoredPosition = Vector2.Lerp(start, target, t / moveDuration);
            yield return null;
        }
        panel.anchoredPosition = target;
        onDone?.Invoke();
    }
}