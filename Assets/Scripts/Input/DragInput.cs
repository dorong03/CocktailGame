using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class DragInput : MonoBehaviour
{
    // 혹시 현재 다른 객체가 드래고 하고 있는지 검사?
    private static DragInput dragItem;

    // 현재 해당 객체가 상호작용 가능한 상태인지
    public bool interactable;

    // 현재 그랩을 하고 있는가?
    public bool IsGrabbed { get; private set; }
    // 그랩을 시작한 마우스의 위치
    public Vector3 GrabWorldPos { get; private set; }
    // 그랩을 한 상태에서 마우스의 위치
    public Vector3 CurrentWorldPos { get; private set; }
    // 총 이동 거리
    public float TotalDragDistance { get; private set; }

    public Action onGrab;
    public Action<Vector2> onDragDelta;
    public Action onRelease;

    private Collider2D _collider;
    private Camera _camera;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _camera = Camera.main;
    }

    private void Update()
    {
        
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = _camera.ScreenToWorldPoint(screenPos);

        if(!interactable)
        {
            CancleGrab();
        }

        if(!IsGrabbed)
        {
            if(Mouse.current.leftButton.wasPressedThisFrame && dragItem == null && _collider.OverlapPoint(worldPos))
            {
                IsGrabbed = true;
                dragItem = this;
                GrabWorldPos = worldPos;
                CurrentWorldPos = worldPos;
                TotalDragDistance = 0f;
                onGrab?.Invoke();
            }
        }
        else
        {
            Vector2 delta = worldPos - (Vector2)CurrentWorldPos;
            if(delta.magnitude > 0f)
            {
                TotalDragDistance += delta.magnitude;
                CurrentWorldPos = worldPos;
                onDragDelta?.Invoke(delta);
            }
            if(Mouse.current.leftButton.wasReleasedThisFrame)
            {
                CancleGrab();
            }
        }
    }

    private void CancleGrab()
    {
        IsGrabbed = false;
        if (dragItem == this) dragItem = null;
        onRelease?.Invoke();
    }
}