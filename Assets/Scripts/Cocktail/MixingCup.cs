using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MixingCup : MonoBehaviour, IPourTarget
{
    // 이게 액체가 들어갔는 판정을 할 렌더러
    [SerializeField] 
    private SpriteRenderer mouthRenderer;
    // 액체가 담겼을때 채워질 렌더러
    [SerializeField] 
    private SpriteRenderer fillRenderer;

    private Coroutine pourCoroutine;
    
    private Vector2 originPos;
    
    private void Start()
    {
        originPos = transform.position;
    }
    
    // 컵에 액체를 따르는 코루틴을 실행하는 메소드
    // ratio 는 액체가 차오르는 목표 비율
    public void PourInto(Cup cup, float ratio, float duration, Color color ,Action onDone)
    {
        if(pourCoroutine != null) StopCoroutine(pourCoroutine);
        pourCoroutine = StartCoroutine(PourCoroutine(cup, ratio, duration, color, onDone));
    }
    
    // duration 에 비례해서 액체가 줄어들고 컵의 액체는 늘어나게 연출
    private IEnumerator PourCoroutine(Cup cup, float ratio, float duration , Color color ,Action onDone)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float percent = timer / duration;
            cup.SetFill(ratio * percent, color);
            SetFill(ratio * (1 - percent), color);
            yield return null;
        }
        cup.SetFill(ratio, color);
        SetFill(0f, color);
        pourCoroutine = null;
        onDone?.Invoke();
    }
    
    // 원래 자리로 돌아가게 하기
    // 나중에 컵이 따르는 연출 할때 쓰는걸로
    public void ResetToHome()
    {
        transform.position = originPos;
        transform.rotation = Quaternion.identity;
    }

    // 중간에 멈추게 될때 사용
    public void AbortPour()
    {
        if(pourCoroutine != null)
        {
            StopCoroutine(pourCoroutine);
            pourCoroutine = null;
        }
        SetFill(0f, Color.white);
    }

    
    // 도구 용기에 재료가 들어갔는지 판정
    public bool IsInsideMouth(Vector2 point)
    {
        return mouthRenderer.bounds.Contains(point);
    }
    
    // 특정 색으로 채우기
    public void SetFill(float ratio, Color color)
    {
        fillRenderer.color = color;
        Vector3 localScale = fillRenderer.transform.localScale;
        localScale.y = ratio;
        fillRenderer.transform.localScale = localScale;
    }
}
