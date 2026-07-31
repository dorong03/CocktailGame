using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class DragInput : MonoBehaviour
{
    // Ȥ�� ���� �ٸ� ��ü�� �巡�� �ϰ� �ִ��� �˻�?
    private static DragInput dragItem;

    // ���� �ش� ��ü�� ��ȣ�ۿ� ������ ��������
    public bool interactable;

    // ���� �׷��� �ϰ� �ִ°�?
    public bool IsGrabbed { get; private set; }
    // �׷��� ������ ���콺�� ��ġ
    public Vector3 GrabWorldPos { get; private set; }
    // �׷��� �� ���¿��� ���콺�� ��ġ
    public Vector3 CurrentWorldPos { get; private set; }
    // �� �̵� �Ÿ�
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

    public void SetCollider(Collider2D collider)
    {
        _collider = collider;
    }
}