using UnityEngine;

public class HeroShield : MonoBehaviour
{
    [SerializeField] private AttributesManager am;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shield"))  // Hero's Shield, Reduce damage taken.
        {
            AttributesManager am = GetComponent<AttributesManager>();

            if (am != null)
            {
                am.damageReduction += 5;

            }
                Destroy(other.gameObject);
                Debug.Log("Shield Picked up, Damage reduced by " + am.damageReduction);
                // Damage Reduction
            
        }
    }
}
