using UnityEngine;

public class VacuumEffect : MonoBehaviour
{
    private Transform player;
    private bool attracted = false;

    [Header("Vacuum Settings")]
    public float attractrange = 5f;
    public float flySpeed = 10f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player1").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        {

            //Start vacuum effect
            if (dist <= attractrange)
                attracted = true;

            if (attracted )
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.position,
                    flySpeed * Time.deltaTime
                    );
            }
        }
    }
}
