using UnityEngine;
using UnityEngine.UIElements;

public interface IPourTarget
{
    public bool IsInsideMouth(Vector2 point);
    public void SetFill(float ratio, Color color);
}
