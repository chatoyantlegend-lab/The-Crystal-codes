using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.HasKey = true;
                Debug.Log("Player picked up the key!");

                Destroy(gameObject); // remove key from scene
            }
        }
    }
}
