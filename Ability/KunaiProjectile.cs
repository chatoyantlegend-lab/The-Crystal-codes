using UnityEngine;

public class KunaiProjectile : MonoBehaviour
{
    private int damage;

    public void SetDamage(int dmg) { damage = dmg; }


    private void OnTriggerEnter(Collider other)
    {
        //Damage only enemies

        if(other.CompareTag("Enemy"))
        {
            AttributesManager enemyAM = other.GetComponent<AttributesManager>();
            if(enemyAM != null ) { enemyAM.TakeDamage(damage); }

            Destroy(gameObject);
        }

        
    }
}
