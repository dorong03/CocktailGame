using UnityEngine;
using UnityEngine.UIElements;

public interface IPourTarget
{
    public bool IsInsideMouth(Vector3 point);
    public bool SetFill(float ratio, Color color);
}
