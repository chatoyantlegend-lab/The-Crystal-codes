using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BombController : MonoBehaviour
{
    [Header("Fuse / Visuals")]
    public float fuseTime = 2f;
    public Color glowColor = Color.red;
    public float glowPulseFrequency = 4f;
    public float maxGlowIntensity = 2f;
    public Light glowLight; // Optional
    public bool useMaterialEmission = true;

    [Header("Explosion")]
    public float aoeRadius = 5f;
    public int damage = 20;
    public bool damageFalloff = true;
    public float explosionForce = 300f;
    public LayerMask damageLayer;

    [Header("VFX / Sound")]
    public GameObject explosionVFXPrefab;
    public float explosionVFXDuration = 2f;
    public AudioClip explosionSFX;

    private Renderer rend;
    private Material instancedMat;
    private Rigidbody rb;
    private bool detonated = false;

    private void Awake()
    {
        // Material setup
        rend = GetComponentInChildren<Renderer>();
        if (rend != null && useMaterialEmission)
        {
            instancedMat = rend.material;
            instancedMat.EnableKeyword("_EMISSION");
        }

        // Rigidbody setup
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Physics collider
        SphereCollider physicsCol = gameObject.AddComponent<SphereCollider>();
        physicsCol.isTrigger = false;
        physicsCol.radius = 0.5f;
    }

    private void Start()
    {
        StartCoroutine(FuseRoutine());
    }

    private IEnumerator FuseRoutine()
    {
        float elapsed = 0f;
        while (elapsed < fuseTime)
        {
            float intensity = Mathf.Lerp(0.2f, maxGlowIntensity, 0.5f + 0.5f * Mathf.Sin(Time.time * glowPulseFrequency));
            if (instancedMat != null)
                instancedMat.SetColor("_EmissionColor", glowColor * intensity);

            if (glowLight != null)
            {
                glowLight.color = glowColor;
                glowLight.intensity = intensity;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Detonate();
    }

    public void InitializeFrom(BombController template)
    {
        aoeRadius = template.aoeRadius;
        damage = template.damage;
        damageFalloff = template.damageFalloff;
        explosionForce = template.explosionForce;
        damageLayer = template.damageLayer;
        explosionVFXPrefab = template.explosionVFXPrefab;
        explosionVFXDuration = template.explosionVFXDuration;
        explosionSFX = template.explosionSFX;
        fuseTime = template.fuseTime;
        glowColor = template.glowColor;
        glowPulseFrequency = template.glowPulseFrequency;
        maxGlowIntensity = template.maxGlowIntensity;
        glowLight = template.glowLight;
        useMaterialEmission = template.useMaterialEmission;
    }

    private void Detonate()
    {
        if (detonated) return;
        detonated = true;

        // Spawn explosion VFX
        if (explosionVFXPrefab != null)
        {
            GameObject vfx = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
            vfx.transform.localScale = Vector3.one * aoeRadius;
            Destroy(vfx, explosionVFXDuration);
        }

        // Play sound
        if (explosionSFX != null)
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position);

        // Apply damage using OverlapSphere
        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius, damageLayer);
        HashSet<AttributesManager> damagedEnemies = new HashSet<AttributesManager>();

        foreach (Collider c in hits)
        {
            AttributesManager targetAM = c.GetComponentInParent<AttributesManager>();
            if (targetAM != null && !damagedEnemies.Contains(targetAM))
            {
                damagedEnemies.Add(targetAM);

                float dist = Vector2.Distance(
                    new Vector2(transform.position.x, transform.position.z),
                    new Vector2(c.transform.position.x, c.transform.position.z)
                );

                float finalDamageF = damage;
                if (damageFalloff)
                {
                    float factor = Mathf.Clamp01(1f - (dist / aoeRadius));
                    finalDamageF = damage * factor;
                }

                int finalDamage = Mathf.Max(1, Mathf.RoundToInt(finalDamageF));
                targetAM.TakeDamage(finalDamage);

                // Apply explosion force
                Rigidbody rbTarget = c.attachedRigidbody;
                if (rbTarget != null)
                    rbTarget.AddExplosionForce(explosionForce, transform.position, aoeRadius, 0.5f, ForceMode.Impulse);
            }
        }

        // Destroy bomb
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}
