using System;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    private DragInput drag;
    private string ingredientId;
    private float targetAmount;
    private float poured;
    private float tilt;
    private IPourTarget currentTarget;
    private Action<string, float> onPour;

    public void Assign(string ingredientId, float amount,Sprite sprite,IPourTarget target, Action<string, float> onPour)
    {
        
    }

    public void Start()
    {
        drag = GetComponent<DragInput>();
        drag.onGrab = PrintLog;
        drag.onDragDelta = FollowMouse;
        drag.onRelease = InitPosition;
    }

    public void PrintLog()
    {
        Debug.Log("Test");
    }

    public void FollowMouse(Vector2 vec)
    {
        transform.position = drag.CurrentWorldPos;
    }

    public void InitPosition()
    {
        transform.position = drag.GrabWorldPos;
    }
}
