using System;
using System.Collections;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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

    // npc 가 특정 자리에 나오는 연출
    public void SpawnNpc(NpcData npc, Seat seat, Action onArrived)
    {
        //gameObject.SetActive(true);
        transform.position = seat.transform.position;
        StartCoroutine(PositionUp());
        onArrived?.Invoke();
    }

    //떠날때
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

    // 초기화
    public void Clear()
    {
        //gameObject.SetActive(false);
    }
}
