using UnityEngine;

public class DashAbility : Ability 

{
    public float dashForce = 50f;
    public GameObject dashVFXPrefab;
    public float vfxDuration = 0.5f;

    private void Awake()
    {
        abilityName = "Dash";
        cooldown = 3f; //Cooldown is 3 seconds
    }

    public override void Activate(GameObject Player)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(Player.transform.forward * dashForce, ForceMode.VelocityChange);
            Debug.Log("Dashed Forward");

            //Spawn Dash VFX
            if (dashVFXPrefab != null)
            {
                GameObject vfx  = Instantiate(dashVFXPrefab, Player.transform.position, Player.transform.rotation);

                Destroy(vfx, vfxDuration);  
            }
        }
    }
}

