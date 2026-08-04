using System;
using UnityEngine;

public class ServingController : MonoBehaviour
{
    // 던지는 컵
    private Cup cup;

    // npc 자리 앞에 있는 시트
    private Seat targetSeat;

    // 판정이 완료 됐을때 실행할 메소드
    private Action<Grade> onComplete;

    // 외부에서 서빙을 시작하는 메소드
    public void StartServing(Seat seat, Action<Grade> onComplete)
    {
        targetSeat = seat;
        this.onComplete = onComplete;
        cup.SetMode(CupMode.Throwable);
        cup.SetThrowHandler(CupLandHandler);
    }

    // 중지하는 메소드 pause
    public void Abort()
    {
        cup.SetMode(CupMode.Locked);
        targetSeat = null;
        onComplete = null;
    }

    //판정
    private void CupLandHandler(Vector2 cupPosition)
    {
        Grade grade = targetSeat.EvaluateLanding(cupPosition);
        onComplete(grade);
    }
}
