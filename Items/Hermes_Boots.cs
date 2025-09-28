using UnityEngine;

public class Hermes_Boots : MonoBehaviour

{

    public PlayerMovement playerMovement;  // Reference Movement script, so i can access player movementspeed to increase it


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boots"))
        {
            playerMovement.MoveSpeed += 7;

            Destroy(other.gameObject); // Destroy boots gameObject 
            Debug.Log("Hermes Boots equipped, movementspeed is now " + playerMovement.MoveSpeed);
        }
    }
}
