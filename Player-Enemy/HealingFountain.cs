using UnityEngine;

public class HealingFountain : MonoBehaviour, IInteractable
{
    [Header("Healing Settings")]
    public int healAmount = 100;
    public GameObject healVFXPrefab;
    public Transform vfxSpawnPoint;
    public string usedMessage = "Fountain is dry.";

    private bool hasBeenUsed = false;

    public void Interact(GameObject player)
    {
        if (hasBeenUsed)
        {

            return;
        }

        // Heal player
        AttributesManager am = player.GetComponent<AttributesManager>();
        if (am != null)
            am.Heal(healAmount);

        // Spawn VFX
        if (healVFXPrefab != null)
        {
            Transform spawnPoint = vfxSpawnPoint != null ? vfxSpawnPoint : transform;
            GameObject vfx = Instantiate(healVFXPrefab, spawnPoint.position, Quaternion.identity);
            Destroy(vfx, 3.5f);
        }

        hasBeenUsed = true;
    }

    public string GetInteractText()
    {
        return hasBeenUsed ? usedMessage : "PRESS E TO INTERACT";
    }
}
