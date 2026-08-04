using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DragInput), typeof(SpriteRenderer))]
public class Bottle : MonoBehaviour
{
    
    /*
     * 물방울 프리팹이랑 물방울이 떨어지는 지점
     */
    [Header("오브젝트 설정")]
    [SerializeField] private Droplet dropletPrefab;
    [SerializeField] private Transform mouthPoint;
    
    /*
     * 위에서 부터 기울기 속도, 최대 기울기 각도, 물방울이 떨어지는 기준 각도
     */
    [Header("기울기 조절")]
    [SerializeField] private float tiltSpeed = 90f;
    [SerializeField] private float maxTiltAngle = 120f;
    [SerializeField] private float pourThresholdAngle = 30f;
    
    
    /*
     * 위에서 부터 물방울 떨어지는 주기, 최소 힘, 최대 힘, 초당 떨어지는 음료 ml
     */
    [Header("물방울 설정")]
    [SerializeField] private float dropInterval = 0.05f;
    [SerializeField] private float dropMinPower = 1f;
    [SerializeField] private float dropMaxPower = 4f;
    [SerializeField] private float pourRate = 30f;

    private DragInput drag;
    private SpriteRenderer spriteRenderer;

    private string ingredientId;
    private float poured;
    private float tilt;
    private IPourTarget currentTarget;
    private Action<string, float> onPour;

    private Vector3 offset;
    private Vector2 homePos;
    private float dropTimer;

    private void Awake()
    {
        homePos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        drag = GetComponent<DragInput>();
        drag.onGrab = HandleGrab;
        drag.onDragDelta = HandleDrag;
        drag.onRelease = HandleRelease;
    }
    
    // 현재 스프라이트가 없어서 잠시 꺼둠
    public void Init(string ingredientId, Sprite sprite, IPourTarget target, Action<string, float> onPour)
    {
        gameObject.SetActive(true);
        drag.interactable = true;
        this.ingredientId = ingredientId;
        currentTarget = target;
        this.onPour = onPour;
        poured = 0f;
        tilt = 0f;
        // spriteRenderer.sprite = sprite;
    }
    
    public void DeActivate()
    {
        drag.interactable = false;
        drag.Test();
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
    
    private void HandleGrab()
    {
        offset = transform.position - drag.GrabWorldPos;
    }

    private void HandleDrag(Vector2 delta)
    {
        transform.position = drag.CurrentWorldPos + offset;
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

    private void ApplyTiltRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, tilt);
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