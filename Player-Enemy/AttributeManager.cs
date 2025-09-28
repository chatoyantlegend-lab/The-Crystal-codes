using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class AttributesManager : MonoBehaviour
{
    [SerializeField] public GameObject bloodPrefab;
    int minBlood = 1;
    int maxBlood = 5;
    public int health = 10;
    public int attack = 10;
    public int damageReduction = 0;

    public bool isPlayer = false;

    [SerializeField] private GameObject gameOverPanel;

    public int maxHealth = 10;

    [Header("Death FX")]
    [SerializeField] public GameObject deathVFXPrefab;
    [SerializeField] public AudioClip deathSFX;
    [SerializeField] private float vfxDuration = 2f;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private void Start()
    {
        if (maxHealth <= 0) maxHealth = health;
        // Ui Sync on st art
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public void TakeDamage(int attack)
    {
        int finalDamage = attack - damageReduction;
        health -= finalDamage;
        Debug.Log($"{gameObject.name} took {finalDamage} damage, health now {health}");

        OnHealthChanged?.Invoke(health, maxHealth);

        if (health <= 0) { Die(); }
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AttributesManager>();
        if (atm != null)
        {
            atm.TakeDamage(attack);
        }

    }

    public void Die()
    {
        OnDeath?.Invoke();

        if (bloodPrefab != null)
        {
            int dropAmount = UnityEngine.Random.Range(minBlood, maxBlood + 1);

            for (int i = 0; i < dropAmount; i++)
            {
                Vector3 dropPos = transform.position + UnityEngine.Random.insideUnitSphere * 0.5f;
                dropPos.y = 5f;
                Instantiate(bloodPrefab, dropPos, Quaternion.identity);
            }
        }
        if (deathVFXPrefab != null)
        {
            GameObject vfx = Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, vfxDuration);
        }

        if (deathSFX != null)
        {
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
        }

        Destroy(gameObject);
        if (isPlayer)
        {
            // Show Game Over UI
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            // Freeze time
            Time.timeScale = 0f;
        }

    }

    public void Retry()
    {
        Time.timeScale = 1f; // resume
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Called by Exit Button
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();

    }
}


