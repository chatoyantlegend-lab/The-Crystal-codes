using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour
{
    public string abilityName;
    public float cooldown = 1f;
    private bool isOnCooldown = false;

    public void TryActivate(GameObject player)
    {
        if (!isOnCooldown)
        {
            Debug.Log($"[{abilityName}] TryActivate SUCCESS");
            Activate(player);
            StartCoroutine(CooldownRoutine());
        }
        else
        {
            Debug.Log($"[{abilityName}] still on cooldown");
        }
    }

    public abstract void Activate(GameObject player);

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
