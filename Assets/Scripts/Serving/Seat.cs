using UnityEngine;

public class Seat : MonoBehaviour
{
    private string seatId;
    private float[] zoneHalfWidth = { 0.4f, 0.7f, 1.2f, 2f };

    public Grade EvaluateLanding(Vector2 landPos)
    {
        float xDistanceFromCenter = Mathf.Abs(transform.position.x - landPos.x);
        
        if(xDistanceFromCenter < zoneHalfWidth[0]) return Grade.A;
        if(xDistanceFromCenter < zoneHalfWidth[1]) return Grade.B;
        if(xDistanceFromCenter < zoneHalfWidth[2]) return Grade.C;
        if(xDistanceFromCenter < zoneHalfWidth[3]) return Grade.D;
        return Grade.Miss;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < zoneHalfWidth.Length; i++)
        {
            Vector3 boxSize = new Vector3(zoneHalfWidth[i], zoneHalfWidth[i], 0);
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }
}
