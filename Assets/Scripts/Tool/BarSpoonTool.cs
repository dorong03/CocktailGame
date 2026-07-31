using UnityEngine;
using UnityEngine.InputSystem;
public class BarSpoonTool : ToolBase
{
    [SerializeField]
    private Collider2D mixingCupCollider;
    [SerializeField]
    private Collider2D barSpoonCollider;

    private const int BarSpoonToolCount = 10;

    private Vector2 originPos;

    public void Start()
    {
        originPos = transform.position;
        requiredCount = BarSpoonToolCount;
        drag.SetCollider(mixingCupCollider);
        ActiveTool(TestReturn, null, null, null);
    }

    protected override void GrabHandler()
    {
        base.GrabHandler();
        if(started)
        {
            drag.SetCollider(barSpoonCollider);
        }
    }

    public bool TestReturn()
    {
       return true;
    }

    public override void HandleDelta(Vector2 delta)
    {
        transform.position = drag.CurrentWorldPos;

        // if()
        //{
        //    AddCount();
        //}
        // 만약 바스푼을 잡고 한바퀴를 돌렸다면?
        // -> AddCount(); 를 실행시킨다.
    }

    public override void ResetMotion()
    {
        transform.position = originPos;
        // 처음 위치로 해당 게임 오브젝트를 이동시킨다.
    }
}
