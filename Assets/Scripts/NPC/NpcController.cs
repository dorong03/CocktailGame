using System;
using System.Collections;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private NpcData currentNpc;
    private DataRepository data;

    [SerializeField]
    private float moveDuration = 1f;
    [SerializeField]
    private float moveDistance = 6f;
    private float moveSpeed;

    private bool isMoving;

    public Seat seat;

    private void Awake()
    { 
        data = DataRepository.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = moveDistance / moveDuration;
        isMoving = false;
        Clear();
    }

    // npc �� Ư�� �ڸ��� ������ ����
    public void SpawnNpc(NpcData npc, Seat seat, Action onArrived)
    {
        //gameObject.SetActive(true);
        transform.position = seat.transform.position;
        StartCoroutine(PositionUp());
        onArrived?.Invoke();
    }

    //������
    public void Depart(Action onGone)
    {
        transform.position = seat.transform.position + Vector3.up * moveDistance;
        StartCoroutine(PositionDown());
        onGone?.Invoke();
    }

    private IEnumerator PositionUp()
    {
        isMoving = true;
        float timer = 0f;

        while (timer < moveDuration)
        {
            transform.position += Vector3.up * (Time.deltaTime * moveSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = seat.transform.position + moveDistance * Vector3.up;
        isMoving = false;
    }

    private IEnumerator PositionDown()
    {
        isMoving = true;
        float timer = 0f;

        while (timer < moveDuration)
        {
            transform.position -= Vector3.up * (Time.deltaTime * moveSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = seat.transform.position;
        isMoving = false;
    }

    // �ʱ�ȭ
    public void Clear()
    {
        //gameObject.SetActive(false);
    }
}
