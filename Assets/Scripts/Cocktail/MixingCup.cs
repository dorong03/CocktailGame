using System;
using UnityEngine;

public class MixingCup : MonoBehaviour
{
    private DragInput drag;
    private SpriteRenderer mouthRenderer;
    private SpriteRenderer fillRenderer;
    private CupMode mode;
    private Vector3 homePos;
    private Action onSubmit;
    private Action<Vector2> onThrowRelease;

    public void SetMode(CupMode mode)
    {

    }
    public void SetSubmitHandler()
    {

    }
}
