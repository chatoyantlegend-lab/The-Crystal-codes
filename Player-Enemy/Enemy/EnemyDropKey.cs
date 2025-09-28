using UnityEngine;

public class EnemyDropKey : MonoBehaviour
{
    [Header("Drop Settings")]
    public GameObject keyPrefab;   // assign in inspector
    public bool alwaysDrop = true; // if false, we can add % chance later

    private AttributesManager attributesManager;

    private void Awake()
    {
        attributesManager = GetComponent<AttributesManager>();
        if (attributesManager != null)
        {
            attributesManager.OnDeath += HandleDeath;
        }
    }

    private void OnDestroy()
    {
        if (attributesManager != null && attributesManager.health <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (alwaysDrop && keyPrefab != null)
        {
            Instantiate(keyPrefab, transform.position + Vector3.up, Quaternion.identity);
            Debug.Log("Key dropped!");
        }
    }
}
