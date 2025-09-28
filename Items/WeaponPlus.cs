using UnityEngine;

public class WeaponPlus : MonoBehaviour
{
    // [SerializeField] private SwordHitbox swordHitbox;
   [SerializeField] private AttributesManager am;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon+")) // Knife Sharpener, Increase damage dealt.
        {

            AttributesManager am = GetComponent<AttributesManager>();

            if (am != null)
            {
                am.attack += 50;
            }

            Destroy(other.gameObject);
            Debug.Log("Weapon Sharpened, damage is now " + am.attack);
            // Increase damage
        }
    }

}


