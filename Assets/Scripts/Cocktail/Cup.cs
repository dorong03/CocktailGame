using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DragInput))]
public class Cup : MonoBehaviour, IPourTarget
{
    [SerializeField]
    private SpriteRenderer mouthRenderer;
    [SerializeField]
    private SpriteRenderer fillRenderer;

    private DragInput drag;
    private CupMode mode;

    private Vector2 homePos;

    private Action onSubmit;
    private Action<Vector2> onCupLand;

    // 잔이 날라가는 속도와 줄어드는 속도 크기
    private const float MaxSpeed = 8f;
    private const float Deceleration = 12f;
    
    // 술이 차오르는 픽셀 기준
    const int pixelStep = 8;
    
    private void Awake()
    {
        drag = GetComponent<DragInput>();
        homePos = transform.position;
        drag.onRelease = HandleRelease;
    }
    
    public void SetMode(CupMode mode)
    {
        this.mode = mode;
        switch (mode)
        {
            case CupMode.Locked:
                drag.interactable = false;
                break;
            case CupMode.Submit:
                drag.interactable = true;
                break;
            case CupMode.Throwable:
                drag.interactable = true;
                break;
        }
    }

    private void HandleRelease()
    {
        if (mode == CupMode.Submit)
        {
            //되돌리기
            if (drag.TotalDragDistance < 0.25f)
            {
                onSubmit?.Invoke();
            }
            ReturnHome();
        }
        else if (mode == CupMode.Throwable)
        {
            //던지기
            Vector2 offset = (Vector2)drag.CurrentWorldPos - homePos;
            offset = -offset;
            StartCoroutine(ThrowRoutine(offset));
        }
    }

    
    /*
     * throwVector.magnitude * 4f <- 당긴 거리가 너무 작게 판정되서 임의로 4 곱함
     * 속도 조절은 추후에 더 진행하는걸로 수치 조금씩 바꿔가면서
     */
    private IEnumerator ThrowRoutine(Vector2 throwVector)
    {
        Vector2 direction = throwVector.normalized;
        float speed = Mathf.Min(throwVector.magnitude * 4f, MaxSpeed);

        while (speed > 0.01f)
        {
            transform.position += (Vector3)(direction * (speed * Time.deltaTime));
            speed -= Deceleration * Time.deltaTime;
            yield return null;
        }
        onCupLand?.Invoke(transform.position);
    }
    
    public void SetSubmitHandler(Action onSubmit)
    {
        this.onSubmit = onSubmit;
    }

    public void SetThrowHandler(Action<Vector2> onThrowRelease)
    {
        this.onCupLand = onThrowRelease;
    }

    public void ReturnHome()
    {
        transform.position = homePos;
        transform.rotation = Quaternion.identity;
    }

    public bool IsInsideMouth(Vector2 point)
    {
        return mouthRenderer.bounds.Contains(point);
    }

    public void SetFill(float ratio, Color color)
    {
        ratio = Mathf.Clamp01(ratio);
        
        float steppedRatio = Mathf.Floor(ratio * pixelStep) / pixelStep;
        
        fillRenderer.color = color;
        
        Vector3 scale = fillRenderer.transform.localScale;
        scale.y = Mathf.Max(1f / pixelStep, steppedRatio);
        fillRenderer.transform.localScale = scale;
    }
}
