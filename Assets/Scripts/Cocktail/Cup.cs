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

    [Header("던지기 설정")]
    [SerializeField]
    private float maxPullDistance = 20f;
    [SerializeField]
    private float minThrowDistance = 4f;
    [SerializeField]
    private float maxThrowDistance = 32f;
    [SerializeField]
    private float minFlightDuration = 0.25f;
    [SerializeField]
    private float maxFlightDuration = 0.9f;
    [SerializeField]
    private CupAimIndicator aimIndicator;

    [Header("테이블 이탈 설정")]
    [SerializeField]
    private Collider2D tableBoundary;
    [SerializeField]
    private int offTableSortingOrder = -998;
    [SerializeField]
    private float fallDuration = 0.35f;
    [SerializeField]
    private float fallDistance = 25f;
    [SerializeField]
    private float fallAngle = 120f;

    private SpriteRenderer[] cupRenderers;
    private int[] baseSortingOrders;

    private DragInput drag;
    private BoxCollider2D cupCollider;
    private CupMode mode;

    private Vector2 homePos;
    private float fillHeight;

    private Action onSubmit;
    private Action<Vector2> onCupLand;

    // 술이 차오르는 픽셀 기준
    const int PixelStep = 32;

    private void Awake()
    {
        drag = GetComponent<DragInput>();
        cupCollider = GetComponent<BoxCollider2D>();
        homePos = transform.position;
        drag.onGrab = HandleGrab;
        drag.onDragDelta = HandleDrag;
        drag.onRelease = HandleRelease;

        cupRenderers = new[] { cupBackRenderer, cupFillRenderer, cupFrontRenderer };
        baseSortingOrders = new int[cupRenderers.Length];
        for (int i = 0; i < cupRenderers.Length; i++)
        {
            baseSortingOrders[i] = cupRenderers[i].sortingOrder;
        }
        
        Hide();
    }

    private bool IsOutsideTable(Vector2 point)
    {
        Bounds b = tableBoundary.bounds;
        return point.x < b.min.x || point.x > b.max.x || point.y < b.min.y || point.y > b.max.y;
    }

    private void PushBehindTable()
    {
        int highest = int.MinValue;
        for (int i = 0; i < baseSortingOrders.Length; i++)
        {
            highest = Mathf.Max(highest, baseSortingOrders[i]);
        }

        int delta = offTableSortingOrder - highest;
        for (int i = 0; i < cupRenderers.Length; i++)
        {
            cupRenderers[i].sortingOrder = baseSortingOrders[i] + delta;
        }
    }

    private void RestoreSortingOrder()
    {
        for (int i = 0; i < cupRenderers.Length; i++)
        {
            cupRenderers[i].sortingOrder = baseSortingOrders[i];
        }
    }

    private Vector2 PullOrigin => cupCollider != null ? (Vector2)cupCollider.bounds.center : homePos;

    private void HandleGrab()
    {
        if (mode != CupMode.Throwable || aimIndicator == null) return;
        aimIndicator.Show();
        UpdateAimIndicator();
    }

    private void HandleDrag(Vector2 delta)
    {
        if (mode != CupMode.Throwable || aimIndicator == null) return;
        UpdateAimIndicator();
    }

    private void UpdateAimIndicator()
    {
        Vector2 pull = (Vector2)drag.CurrentWorldPos - PullOrigin;
        aimIndicator.UpdateAim(PullOrigin, GetLandingPosition(pull));
    }

    private float GetPullRatio(Vector2 pull)
    {
        return Mathf.Clamp01(pull.magnitude / maxPullDistance);
    }

    public float GetThrowAngle(Vector2 pull)
    {
        return Vector2.SignedAngle(Vector2.right, -pull.normalized);
    }

    public Vector2 GetLandingPosition(Vector2 pull)
    {
        float distance = Mathf.Lerp(minThrowDistance, maxThrowDistance, GetPullRatio(pull));
        return PullOrigin + (-pull.normalized) * distance;
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
            if (aimIndicator != null) aimIndicator.Hide();

            Vector2 pull = (Vector2)drag.CurrentWorldPos - PullOrigin;
            float duration = Mathf.Lerp(minFlightDuration, maxFlightDuration, GetPullRatio(pull));
            StartCoroutine(ThrowRoutine(GetLandingPosition(pull), duration));
        }
    }

    private IEnumerator ThrowRoutine(Vector2 landing, float duration)
    {
        Vector2 start = transform.position;
        Vector2 target = landing - (PullOrigin - start);
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(start, target, EaseOutCubic(timer / duration));

            if (tableBoundary != null && IsOutsideTable(transform.position))
            {
                yield return StartCoroutine(FallOffRoutine(transform.position.y > tableBoundary.bounds.max.y));
                yield break;
            }
            yield return null;
        }
        transform.position = target;
        onCupLand?.Invoke(landing);
    }

    private IEnumerator FallOffRoutine(bool exitedUpward)
    {
        if (exitedUpward) PushBehindTable();

        Vector2 start = transform.position;
        Vector2 target = start + Vector2.down * fallDistance;
        float startAngle = transform.eulerAngles.z;
        float timer = 0f;

        while (timer < fallDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fallDuration);
            transform.position = Vector2.Lerp(start, target, t * t);
            transform.rotation = Quaternion.Euler(0f, 0f, startAngle + fallAngle * t);
            yield return null;
        }

        SetFill(0f, Color.white);
        onCupLand?.Invoke(PullOrigin);
    }

    private static float EaseOutCubic(float t)
    {
        t = Mathf.Clamp01(t);
        return 1f - Mathf.Pow(1f - t, 3f);
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

    private void ResizeColliderWithSprite(Sprite sprite)
    {
        if (sprite == null) return;

        cupCollider.size = sprite.bounds.size;
        cupCollider.offset = sprite.bounds.center;
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

        ResizeColliderWithSprite(glassFront);

        SetFill(0f, Color.white);
        RestoreSortingOrder();
        transform.position = homePos;
        transform.rotation = Quaternion.identity;
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
