using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DealDamage : MonoBehaviour
{
    public AttributesManager PlayerAtm;
    public AttributesManager enemyAtm;
    
    private void Update()
    {
        //Deal Player damage
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlayerAtm.DealDamage(enemyAtm.gameObject);

        }

        //Deal enemy damage
        if(Mouse.current.rightButton.wasPressedThisFrame)
        {
            enemyAtm.DealDamage(PlayerAtm.gameObject);
        }

    }
}
