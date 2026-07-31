using UnityEngine;

public class BarSpoonTool : ToolBase
{
    [SerializeField] private Transform stirCenter;
    [SerializeField] private float minStirRadius = 0.5f;

    [SerializeField] private Collider2D mixingCupCollider;
    [SerializeField] private Collider2D barSpoonCollider;
    
    private const int BarSpoonToolCount = 10;

    private Vector2 originPos;
    private float lastAngle;
    private float accumulatedAngle;
    private bool hasLastAngle;

    public void Start()
    {
        originPos = transform.position;
        requiredCount = BarSpoonToolCount;
        drag.SetCollider(mixingCupCollider);
        
        // Test 용 코드
        ActiveTool(TestReturn, null, null, null);
    }

    //Test 용 코드
    private bool TestReturn()
    {
        return true;
    }

    protected override void GrabHandler()
    {
        base.GrabHandler();
        if (started)
        {
            drag.SetCollider(barSpoonCollider);
        }
    }
    
    public override void HandleDelta(Vector2 delta)
    {
        transform.position = drag.CurrentWorldPos;

        Vector2 dir = (Vector2)drag.CurrentWorldPos - (Vector2)stirCenter.position;

        if (dir.magnitude < minStirRadius)
        {
            hasLastAngle = false;
            return;
        }

        float currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (!hasLastAngle)
        {
            lastAngle = currentAngle;
            hasLastAngle = true;
            return;
        }

        accumulatedAngle += Mathf.DeltaAngle(lastAngle, currentAngle);
        lastAngle = currentAngle;

        if (Mathf.Abs(accumulatedAngle) >= 360f)
        {
            AddCount();
            accumulatedAngle = 0f;
        }
    }

    public override void ResetMotion()
    {
        transform.position = originPos;
        hasLastAngle = false;
        accumulatedAngle = 0f;
    }
}