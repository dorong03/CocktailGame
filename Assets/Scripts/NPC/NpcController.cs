using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NpcController : MonoBehaviour
{
    private Coroutine routine;
    private SpriteRenderer spriteRenderer;
    private NpcData currentNpc;
    private DataRepository data;

    [SerializeField]
    private float moveDuration = 1f;
    [SerializeField]
    private float spawnYOffset = -10f;
    [SerializeField]
    private float moveDistance = 6f;

    private Seat seat;

    private void Awake()
    {
        data = DataRepository.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Clear();
    }

    public void SpawnNpc(NpcData npc, Seat seat, Action onArrived)
    {
        spriteRenderer.sprite = SpriteRepository.Instance.GetNpcSprite(npc.Id);
        gameObject.SetActive(true);
        this.seat = seat;
        transform.position = SpawnPosition(seat);
        if(routine != null) StopCoroutine(routine);
        routine = StartCoroutine(MoveTo(transform.position + Vector3.up * moveDistance, onArrived));
    }

    public void Depart(Action onGone)
    {
        if(routine != null) StopCoroutine(routine);
        routine = StartCoroutine(MoveTo(transform.position - Vector3.up * moveDistance, onGone));
    }

    private Vector3 SpawnPosition(Seat targetSeat)
    {
        Vector3 seatPos = targetSeat.transform.position;
        return new Vector3(seatPos.x, seatPos.y + spawnYOffset, seatPos.z);
    }

    private IEnumerator MoveTo(Vector3 target, Action onDone)
    {
        Vector3 start = transform.position;
        float timer = 0f;

        while (timer < moveDuration)
        {
            transform.position = Vector3.Lerp(start, target, timer / moveDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        onDone?.Invoke();
        routine = null;
    }
    
    public void Clear()
    {
        if(routine != null) StopCoroutine(routine);
        routine = null;
        gameObject.SetActive(false);
    }
}
