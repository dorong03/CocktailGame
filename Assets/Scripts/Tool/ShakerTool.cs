using System.Collections;
using UnityEngine;

public class ShakerTool : ToolBase
{
    private const int ShakerToolCount = 5;

    private float speedThreshold = 0.3f;
    private bool direction = true;

    private Vector2 originPos;


    public void Start()
    {
        originPos = transform.position;
        requiredCount = ShakerToolCount;
    }

    protected override void GrabHandler()
    {
        base.GrabHandler();
        StartCoroutine(CloseShaker());
    }

    private IEnumerator CloseShaker()
    {
        // 뚜껑이 닫히는 코드 구현
        yield return null;
    }
    
    private IEnumerator OpenShaker()
    {
        // 뚜껑이 열리는 코드
        yield return null;
    }
    
    public override void HandleDelta(Vector2 delta)
    {
        transform.position = drag.CurrentWorldPos;
        ShakeJudge(delta);
    }

    private void ShakeJudge(Vector2 delta)
    {
        bool isUp = (delta.y > 0);
        float yDeltaSpeed = Mathf.Abs(delta.y);

        if (!(yDeltaSpeed > speedThreshold)) return;

        if (direction == isUp)
        {
            direction = !direction;
            AddCount();
            return;
        }
    }

    public override void ResetMotion()
    {
        transform.position = originPos;
    }
}
