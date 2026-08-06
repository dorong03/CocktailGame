using System;
using System.Collections;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    private Coroutine routine;
    private SpriteRenderer spriteRenderer;
    private NpcData currentNpc;
    private DataRepository data;

    [SerializeField]
    private float moveDuration = 1f;
    [SerializeField]
    private float moveDistance = 6f;
    private float moveSpeed;

    private Seat seat;

    private void Awake()
    { 
        data = DataRepository.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = moveDistance / moveDuration;
        Clear();
    }
    
    public void SpawnNpc(NpcData npc, Seat seat, Action onArrived)
    {
        gameObject.SetActive(true);
        this.seat = seat;
        transform.position = seat.transform.position;
        if(routine != null) StopCoroutine(routine);
        routine = StartCoroutine(PositionUp(onArrived));
        onArrived?.Invoke();
    }
    
    public void Depart(Action onGone)
    {
        transform.position = seat.transform.position + Vector3.up * moveDistance;
        if(routine != null) StopCoroutine(routine);
        routine = StartCoroutine(PositionDown(onGone));
    }

    private IEnumerator PositionUp(Action onArrived)
    {
        float timer = 0f;

        while (timer < moveDuration)
        {
            transform.position += Vector3.up * (Time.deltaTime * moveSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = seat.transform.position + moveDistance * Vector3.up;
        onArrived?.Invoke();
        routine = null;
    }

    private IEnumerator PositionDown(Action onGone)
    {
        float timer = 0f;

        while (timer < moveDuration)
        {
            transform.position -= Vector3.up * (Time.deltaTime * moveSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = seat.transform.position;
        onGone?.Invoke();
        routine = null;
    }
    
    public void Clear()
    {
        if(routine != null) StopCoroutine(routine);
        routine = null;
        gameObject.SetActive(false);
    }
}
