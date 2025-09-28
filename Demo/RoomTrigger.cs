using UnityEngine;

public class RoomTrigger : MonoBehaviour
{

    public Transform roomCenter;
    public Vector3 cameraRotationEuler; //Fixed rotation for camera

    private RoomCamera roomCamera;

    
    void Start()
    {
        roomCamera = Camera.main.GetComponent<RoomCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1")) { roomCamera.SnapToRoom(roomCenter.position); }
    }


}
