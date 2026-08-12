using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DragInput), typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Bottle : MonoBehaviour
{
    
    /*
     * 물방울 프리팹이랑 물방울이 떨어지는 지점
     */
    [Header("오브젝트 설정")]
    [SerializeField] 
    private Droplet dropletPrefab;
    [SerializeField] 
    private Transform mouthPoint;
    
    /*
     * 위에서 부터 기울기 속도, 최대 기울기 각도, 물방울이 떨어지는 기준 각도
     */
    [Header("기울기 조절")]
    [SerializeField] 
    private float tiltSpeed = 120f;
    [SerializeField]
    private float maxTiltAngle = 120f;
    [SerializeField] 
    private float pourThresholdAngle = 30f;
    
    
    /*
     * 위에서 부터 물방울 떨어지는 주기, 최소 힘, 최대 힘, 초당 떨어지는 음료 ml
     */
    [Header("물방울 설정")]
    [SerializeField] 
    private float dropInterval = 0.05f;
    [SerializeField] 
    private float dropMinPower = 1f;
    [SerializeField]
    private float dropMaxPower = 4f;
    [SerializeField]
    private float pourRate = 30f;

    
    private DragInput drag;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private string ingredientId;
    private float tilt;
    private IPourTarget currentTarget;
    private Action<string, float> onPour;
    
    private Vector2 homePos;
    private float dropTimer;

    private void Awake()
    {
        homePos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        drag = GetComponent<DragInput>();
        boxCollider = GetComponent<BoxCollider2D>();
        drag.onGrab = HandleGrab;
        drag.onDragDelta = HandleDrag;
        drag.onRelease = HandleRelease;
        Hide();
    }
    
    // 현재 스프라이트가 없어서 잠시 꺼둠
    public void Init(string ingredientId, Sprite sprite, IPourTarget target, Action<string, float> onPour)
    {
        drag.interactable = true;
        this.ingredientId = ingredientId;
        currentTarget = target;
        this.onPour = onPour;
        tilt = 0f;
     
        spriteRenderer.sprite = sprite;
        ResizeBoxColliderWithSprite(sprite);
    }

    private void ResizeBoxColliderWithSprite(Sprite sprite)
    {
        boxCollider.size = sprite.bounds.size;
        boxCollider.offset = sprite.bounds.center;
        
        Vector3 mouthLocalPos = mouthPoint.localPosition;
        mouthLocalPos.x = sprite.bounds.center.x;
        mouthLocalPos.y = sprite.bounds.max.y;
        mouthPoint.localPosition = mouthLocalPos;
    }
    
    public void DeActivate()
    {
        drag.interactable = false;
        ingredientId = null;
        currentTarget = null;
        onPour = null;
        tilt = 0f;
        transform.position = homePos;
        drag.Test();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (!drag.IsGrabbed) return;

        if (Mouse.current.rightButton.isPressed)
        {
            Tilt();
            if(tilt > pourThresholdAngle)
            {
                SpawnDroplets();
            }
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            ResetTilt();
        }
    }
    
    // 콜라이더(스프라이트)의 중심. 스프라이트 피벗이 아래쪽이라 트랜스폼 원점과 다르다
    private Vector3 ColliderCenter => transform.TransformPoint(boxCollider.offset);

    // 콜라이더 중심이 지정한 월드 좌표에 오도록 트랜스폼을 옮긴다
    private void MoveCenterTo(Vector3 worldPos)
    {
        transform.position += worldPos - ColliderCenter;
    }

    // 조작감을 위해 잡은 위치와 상관없이 콜라이더 중심이 커서에 오도록 맞춘다
    private void HandleGrab()
    {
        MoveCenterTo(drag.GrabWorldPos);
    }

    private void HandleDrag(Vector2 delta)
    {
        MoveCenterTo(drag.CurrentWorldPos);
    }
    
    private void HandleRelease()
    {
        transform.position = homePos;
        ResetTilt();
    }
    

    private void Tilt()
    {
        tilt = Mathf.Clamp(tilt + tiltSpeed * Time.deltaTime, 0f, maxTiltAngle);
        ApplyTiltRotation();
    }
    private void ResetTilt()
    {
        tilt = 0f;
        ApplyTiltRotation();
    }

    // 피벗이 아래쪽이라 그냥 돌리면 병이 휘둘리듯 움직인다
    // 회전 후 중심을 커서 위치로 되돌려서 중앙 기준으로 도는 것처럼 보이게 한다
    private void ApplyTiltRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, tilt);
        if (drag.IsGrabbed) MoveCenterTo(drag.CurrentWorldPos);
    }
    
    private void SpawnDroplets()
    {
        dropTimer -= Time.deltaTime;
        if (dropTimer > 0f) return;

        dropTimer = dropInterval;
        SpawnDroplet();
    }

    private void SpawnDroplet()
    {
        Droplet d = Instantiate(dropletPrefab, mouthPoint.position, Quaternion.identity);
        float power = Mathf.Lerp(dropMinPower, dropMaxPower, tilt / maxTiltAngle);
        Vector2 v = (Vector2)mouthPoint.up * power;
        
        float amountPerDrop = pourRate * dropInterval;
        d.Launch(v, 1f, ingredientId, amountPerDrop, currentTarget ,onPour);
    }
}