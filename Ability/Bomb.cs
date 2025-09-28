using UnityEngine;

public class BombAbility : Ability
{
    [Header("Bomb Prefab & Settings")]
    public GameObject bombPrefab;  // Assign prefab in ShopManager
    public float fuseTime = 2f;
    public float aoeRadius = 3f;
    public int bombDamage = 30;
    public GameObject explosionVFXPrefab;
    public LayerMask damageLayer;
    public float explosionForce = 300f;
    public bool damageFalloff = true;

    private void Awake()
    {
        abilityName = "Bomb";
        cooldown = 3f;
    }

    public override void Activate(GameObject player)
    {
        if (bombPrefab == null)
        {
            Debug.LogWarning("Bomb Prefab not assigned in BombAbility.");
            return;
        }

        // Spawn position slightly above player and slightly forward
        Vector3 spawnPos = player.transform.position + Vector3.up * 0.5f + player.transform.forward * 0.5f;

        GameObject bombGO = Instantiate(bombPrefab, spawnPos, Quaternion.identity);

       
    }
}
