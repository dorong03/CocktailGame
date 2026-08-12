using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class DragInput : MonoBehaviour
{
    // Ȥ�� ���� �ٸ� ��ü�� �巡�� �ϰ� �ִ��� �˻�?
    public static DragInput DragItem { get; private set; }

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

    private void OnDisable()
    {
        CancleGrab();
    }

    private void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = _camera.ScreenToWorldPoint(screenPos);

        // 상호작용이 꺼져있으면 잡고 있던것만 놓고 새로 잡지는 못하게 막는다
        if(!interactable)
        {
            CancleGrab();
            return;
        }

        if(!IsGrabbed)
        {
            if(Mouse.current.leftButton.wasPressedThisFrame && DragItem == null && _collider.OverlapPoint(worldPos))
            {
                IsGrabbed = true;
                DragItem = this;
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
    
    public void CancleGrab()
    {
        bool wasGrabbed = IsGrabbed;
        IsGrabbed = false;
        if (DragItem == this) DragItem = null;
        // 잡은 적이 없으면 놓는 콜백도 없어야 함
        // (interactable == false 일때 Update 에서 매 프레임 호출되기 때문)
        if (!wasGrabbed) return;
        onRelease?.Invoke();
    }

    public void SetCollider(Collider2D collider)
    {
        _collider = collider;
    }

    public void Test()
    {
        DragItem = null;
    }
}