using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;


        private void Start() { currentHealth = maxHealth; }

        public void TakeDamage (int damage)
        {
            currentHealth -= damage;
            Debug.Log(gameObject.name + "took " + damage + " damage. Remaining: " + currentHealth + "Health");

            if (currentHealth <= 0) 
            { 
                Die(); 
            }

        }
        private void Die()
    {
        Debug.Log(gameObject.name + "died!");
        Destroy(gameObject);
    }
}

