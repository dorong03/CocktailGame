using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DragInput))]
public class Cup : MonoBehaviour, IPourTarget
{
    

    [Header("스프라이트 렌더러 설정")]
    [SerializeField] 
    private SpriteRenderer cupBackRenderer;
    [SerializeField] 
    private SpriteRenderer cupFrontRenderer;
    [SerializeField]
    private SpriteRenderer cupFillRenderer;
    [SerializeField]
    private SpriteMask fillMask;
    [SerializeField]
    private SpriteRenderer mouthRenderer;
    
    private DragInput drag;
    private CupMode mode;

    private Vector2 homePos;
    private float fillHeight;

    private Action onSubmit;
    private Action<Vector2> onCupLand;

    // 잔이 날라가는 속도와 줄어드는 속도 크기
    private const float MaxSpeed = 10f;
    private const float Deceleration = 11f;
    
    // 술이 차오르는 픽셀 기준
    const int PixelStep = 32;
    
    private void Awake()
    {
        drag = GetComponent<DragInput>();
        homePos = transform.position;
        drag.onRelease = HandleRelease;
        Hide();
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
        
        float steppedRatio = Mathf.Floor(ratio * PixelStep) / PixelStep;

        Vector3 pos = cupFillRenderer.transform.localPosition;
        pos.y = Mathf.Lerp(-fillHeight, 0f, steppedRatio);
        cupFillRenderer.transform.localPosition = pos;
    }

    public void Show(RecipeData recipe)
    {
        // 레시피에 맞게 변경
        Sprite glassFront = SpriteRepository.Instance.GetGlassSprites(recipe.GlassType).FrontSprite;
        Sprite glassBack = SpriteRepository.Instance.GetGlassSprites(recipe.GlassType).BackSprite;
        Sprite fill = SpriteRepository.Instance.GetCocktailSprite(recipe.Id);
        
        cupFrontRenderer.sprite = glassFront;
        cupBackRenderer.sprite = glassBack;
        cupFillRenderer.sprite = fill;
        fillMask.sprite = fill;
        
        fillHeight = cupFillRenderer.sprite != null ?
            cupFillRenderer.sprite.bounds.size.y : 0f;
        
        SetFill(0f, Color.white);
        transform.position = homePos;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        cupBackRenderer.sprite = null;
        cupFrontRenderer.sprite = null;
        cupFillRenderer.sprite = null;
        fillMask.sprite = null;
        fillHeight = 0f;
        
        gameObject.SetActive(false);
    }
}
