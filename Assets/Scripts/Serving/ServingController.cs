using System;
using UnityEngine;

public class ServingController : MonoBehaviour
{
    // 서빙할 잔
    [SerializeField]
    private Cup cup;

    // NPC 자리(손님이 앉은 시트)
    private Seat targetSeat;

    // 서빙이 완료됐을 때 실행할 콜백
    private Action<Grade> onComplete;

    // 외부에서 서빙을 시작할 때 호출하는 메소드
    public void StartServing(Seat seat, Action<Grade> onComplete)
    {
        targetSeat = seat;
        this.onComplete = onComplete;
        cup.SetMode(CupMode.Throwable);
        cup.SetThrowHandler(CupLandHandler);
    }

    // 서빙을 중단할 때 호출하는 메소드
    public void Abort()
    {
        cup.SetMode(CupMode.Locked);
        targetSeat = null;
        onComplete = null;
    }

    // 잔이 던져진 뒤 착지 판정
    private void CupLandHandler(Vector2 cupPosition)
    {
        Grade grade = (cup.IsBroken) ? Grade.Miss : targetSeat.EvaluateLanding(cupPosition);
        onComplete(grade);
    }
}