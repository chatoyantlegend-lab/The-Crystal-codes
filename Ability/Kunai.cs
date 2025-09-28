using UnityEngine;

public class KunaiAbility : Ability
{
    public GameObject kunaiPrefab;
    public float kunaiSpeed = 20f;
    public float kunaiLifetime = 2f;
    public int kunaiDamage = 25;

    private void Awake()
    {
        abilityName = "Kunai";
        cooldown = 5f; // 5 Seconds cooldown
    }

    public override void Activate(GameObject player)
    {
        if (kunaiPrefab == null)
        {
            Debug.LogWarning("Kunai prefab not assigned!");
            return;
        }

        // Fire 8 kunai in 45° increments
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * player.transform.forward;

            // Spawn at player position + a bit forward
            GameObject kunai = GameObject.Instantiate(
                kunaiPrefab,
                player.transform.position + direction * 1f + Vector3.up * 0.5f,
                Quaternion.LookRotation(direction)
            );

            Rigidbody rb = kunai.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.linearVelocity = direction.normalized * kunaiSpeed; // ← this gives movement
            }

            KunaiProjectile proj = kunai.GetComponent<KunaiProjectile>();
            if ( proj != null)
            {
                proj.SetDamage(kunaiDamage);
            }

            // Auto-destroy after lifetime
            GameObject.Destroy(kunai, kunaiLifetime);
        }
    }


    private void OnDestroy()
    {
        Debug.Log ("Kunai destroyed : " + gameObject.name + " at " + Time.time);
    }
}
