using TMPro;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    private Vector3 targetPosition;
    public Vector3 offset = new Vector3(0, 12, 0);
    public float smoothSpeed = 0.125f;
    private Quaternion fixedRotation = Quaternion.Euler(90,0,0);

    

    private void LateUpdate()
    {
        
            // Smooth position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            // Apply fixed rotation instantly
            transform.rotation = fixedRotation;
        
    }

    public void SnapToRoom(Vector3 roomCenter)
    {
        targetPosition = roomCenter + offset;  
    }    
}
