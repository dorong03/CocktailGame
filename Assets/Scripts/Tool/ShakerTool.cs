using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShakerTool : ToolBase
{
    private const int ShakerToolCount = 5;

    private float speedThreshold = 0.3f;
    private bool direction = true;

    private Vector2 originPos;

    [SerializeField]
    private Transform closeHeadPos;
    [SerializeField]
    private Transform homeHeadPos;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform targetParent;
    [SerializeField]
    private Transform homeTargetParent;


    protected override void Awake()
    {
        base.Awake();
        Hide();
        originPos = transform.position;
        requiredCount = ShakerToolCount;
    }

    protected override void GrabHandler()
    {
        base.GrabHandler();
        if (started) StartCoroutine(CloseShaker());
    }

    private IEnumerator CloseShaker()
    {
        // 뚜껑이 닫히는 코드 구현
        // 추후 애니메이션 구현
        head.SetParent(targetParent);
        head.position = closeHeadPos.position;
        yield return null;
    }

    private void StartOpenShaker()
    {
        StartCoroutine(OpenShaker());
    }

    private IEnumerator OpenShaker()
    {
        // 뚜껑이 열리는 코드
        // 추후 애니메이션 구현
        OpenHeadImmediate();
        yield return null;
    }

    // 연출 없이 뚜껑 상태만 초기화
    private void OpenHeadImmediate()
    {
        head.SetParent(homeTargetParent);
        head.position = homeHeadPos.position;
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
            AddCount(StartOpenShaker);
            return;
        }
    }

    public override void ResetMotion()
    {
        transform.position = originPos;
    }

    // 흔들기를 끝내지 못하고 중단되면(시간 종료 / 주문 취소) 뚜껑이 닫힌 채로 남는다
    public override void Abort()
    {
        base.Abort();
        StopAllCoroutines();
        OpenHeadImmediate();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        head.gameObject.SetActive(true);
        OpenHeadImmediate();
    }
    
    public override void Hide()
    {
        gameObject.SetActive(false);
        head.gameObject.SetActive(false);
    }
}
