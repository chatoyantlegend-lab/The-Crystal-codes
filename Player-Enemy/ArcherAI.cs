using UnityEngine;
using UnityEngine.AI;

public class ArcherAI : MonoBehaviour
{
    [Header("Detection Ranges")]
    public float attackRange = 10f;
    public float fleeRange = 3f;

    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    private AttributesManager am;

    [Header("Shooting")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float shootForce = 20f;
    public float attackCooldown = 1f;
    private float cooldownTimer = 0f;

    private Animator anim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        am = GetComponent<AttributesManager>();
        anim = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player1").transform;
        }
    }

    private void Update()
    {
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.position);
        cooldownTimer -= Time.deltaTime;

        if (distance <= fleeRange)

        {
            // Flee from player
            Vector3 dirToPlayer = transform.position - player.position;
            Vector3 newPos = transform.position + dirToPlayer.normalized;
            agent.SetDestination(newPos);

            anim.SetBool("isShooting", false);
            anim.SetBool("isRunning", true);
        }
        else if (distance <= attackRange)
        {
            //Shoot player
            agent.SetDestination(transform.position);
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            if (cooldownTimer <= 0f)
            {
                ShootArrow();
                cooldownTimer = attackCooldown;
            }

            anim.SetBool("isRunning", false);
            anim.SetBool("isShooting", true);
        }
        else
        {
            //Chase until in atk range
            agent.SetDestination(player.position);

            anim.SetBool("isShooting", false);
            anim.SetBool("isRunning", true);

        }
    }

    void ShootArrow()
    {
        if (arrowPrefab != null || shootPoint == null) return;

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootPoint.forward * shootForce;

            // Reuse KunaiProjectile.cs
            KunaiProjectile proj = arrow.GetComponent<KunaiProjectile>();
            if (proj != null) { proj.SetDamage(am.attack); }

            Debug.Log("Archer fired an arrow");

        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, fleeRange);
        }

    }
}
