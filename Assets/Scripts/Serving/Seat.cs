using UnityEngine;
using UnityEngine.InputSystem;

public class Seat : MonoBehaviour
{
    private string seatId;
    private float[] zoneHalfWidth = { 3f, 5f, 7f, 10f };

    public Grade EvaluateLanding(Vector2 landPos)
    {
        float xDistanceFromCenter = Mathf.Abs(transform.position.x - landPos.x);
        
        if(xDistanceFromCenter < zoneHalfWidth[0]) return Grade.A;
        if(xDistanceFromCenter < zoneHalfWidth[1]) return Grade.B;
        if(xDistanceFromCenter < zoneHalfWidth[2]) return Grade.C;
        if(xDistanceFromCenter < zoneHalfWidth[3]) return Grade.D;
        return Grade.Miss;
    }
    
    // 실제로는 아직 Y 축 검사는 안해서 박스 좌우 선분만 확인할 것
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < zoneHalfWidth.Length; i++)
        {
            Vector3 boxSize = new Vector3(zoneHalfWidth[i] * 2, zoneHalfWidth[i] * 2, 0);
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }
}
