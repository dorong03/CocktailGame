using UnityEngine;
using UnityEngine.Rendering;

public class CupAimIndicator : MonoBehaviour
{
    [SerializeField]
    private float lineWidth = 0.4f;
    [SerializeField]
    private float markerRadius = 1.5f;
    [SerializeField]
    private Color lineColor = new Color(1f, 1f, 1f, 0.7f);
    [SerializeField]
    private Color markerColor = new Color(1f, 0.85f, 0.2f, 0.9f);
    [SerializeField]
    private int sortingOrder = 100;

    private const int MarkerSegments = 24;

    private LineRenderer aimLine;
    private LineRenderer marker;

    private void Awake()
    {
        EnsureInit();
        Hide();
    }

    // Awake 순서가 보장되지 않아 Cup 쪽에서 먼저 호출될 수 있음
    private void EnsureInit()
    {
        if (aimLine != null) return;
        aimLine = CreateLine("AimLine", lineColor, 2, false);
        marker = CreateLine("AimMarker", markerColor, MarkerSegments, true);
    }

    private LineRenderer CreateLine(string lineName, Color color, int pointCount, bool loop)
    {
        GameObject go = new GameObject(lineName);
        go.transform.SetParent(transform, false);

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.useWorldSpace = true;
        lr.loop = loop;
        lr.positionCount = pointCount;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.startColor = color;
        lr.endColor = color;
        lr.numCapVertices = 4;
        lr.numCornerVertices = 4;
        lr.sortingOrder = sortingOrder;
        lr.shadowCastingMode = ShadowCastingMode.Off;
        lr.receiveShadows = false;
        return lr;
    }

    public void Show()
    {
        EnsureInit();
        aimLine.enabled = true;
        marker.enabled = true;
    }

    public void Hide()
    {
        EnsureInit();
        aimLine.enabled = false;
        marker.enabled = false;
    }

    public void UpdateAim(Vector2 origin, Vector2 landing)
    {
        EnsureInit();
        aimLine.SetPosition(0, origin);
        aimLine.SetPosition(1, landing);

        for (int i = 0; i < MarkerSegments; i++)
        {
            float rad = i / (float)MarkerSegments * Mathf.PI * 2f;
            marker.SetPosition(i, landing + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * markerRadius);
        }
    }
}
