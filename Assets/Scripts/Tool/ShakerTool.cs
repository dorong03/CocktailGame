using UnityEngine;

public class ShakerTool : ToolBase
{
    #region 근호 연습 코드
    //private Vector2 delta;
    //private float contentDeltaUp = 0.3f;
    //private float contentDeltaDown = -0.3f;
    //private bool deltaUp;
    //private bool deltaDown;

    //private void ShakeJurge()
    //{
    //    if (delta.y < contentDeltaUp)
    //    {
    //        deltaUp = true;
    //        deltaDown = false;
    //    }
    //    else if(delta.y > -contentDeltaDown)
    //    {
    //        deltaDown = true;
    //        deltaUp = false;
    //    }
    //    else
    //    {
    //        deltaUp = false;
    //        deltaDown = false;
    //    }
    //}
    #endregion 
    private const int ShakerToolCount = 5;

    private float speedThreshold = 0.3f;
    private bool direction = true;

    private Vector2 originPos;


    public void Start()
    {
        originPos = transform.position;
        requiredCount = ShakerToolCount;

        /*
         * 잠시 테스트 용으로 만든 초기화 메소드 입니다.
         */
        //ActiveTool(TestReturn, null, null, null);
    }

    /*
     * 테스트에 사용한 메소드
     */
    //public bool TestReturn()
    //{
    //    return true;
    //}

    public override void HandleDelta(Vector2 delta)
    {
        transform.position = drag.CurrentWorldPos;
        //this.delta = delta;

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
