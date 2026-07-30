using System;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bottle : MonoBehaviour
{
    private DragInput drag;
    private string ingredientId;
    private float targetAmount;
    private float poured;
    private float tilt;
    private IPourTarget currentTarget;
    private Action<string, float> onPour;
    private Vector3 offset;

    public void Assign(string ingredientId, float amount,Sprite sprite,IPourTarget target, Action<string, float> onPour)
    {
        
    }

    public void Start()
    {
        drag = GetComponent<DragInput>();
        drag.onGrab = SaveCoordinate;
        drag.onDragDelta = FollowMouse;
        drag.onRelease = InitPosition;
    }

    public void Update()
    {
        if (drag.IsGrabbed)
        {
            if (Mouse.current.rightButton.isPressed)
            {
                TiltObject();
            }
            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                ResetTilt();
            }
        }
    }
    public void SaveCoordinate()
    {
        offset = transform.position - drag.GrabWorldPos;
    }
    public void FollowMouse(Vector2 vec)
    {
        transform.position = drag.CurrentWorldPos;
    }

    public void InitPosition()
    {
        transform.position = drag.GrabWorldPos + offset;
        tilt = 0f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, tilt));
    }

    public void TiltObject()
    {
        tilt += 1f;
        tilt = Math.Clamp(tilt, 0f, 90f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, tilt));
    }
    public void ResetTilt()
    {
        tilt = 0f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, tilt));
    }
}
