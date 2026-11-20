using UnityEngine;
using UnityEngine.AI;

public class BonVision : MonoBehaviour
{
    [Header("Configuración de Visión y Ataque")]
    public Transform player;
    public float detectionRadius = 12f;
    public float attackDistance = 2.5f;
    public float chaseSpeed = 7f;
    public int attackDamage = 100;      // 100 = mata de un golpe
    public float attackCooldown = 1f;

    private NavMeshAgent agent;
    private WanderAI wanderScript;
    private PlayerHealth playerHealth;
    private bool isChasing = false;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderScript = GetComponent<WanderAI>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (player != null)
            playerHealth = player.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null)
            playerHealth = Object.FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        if (player == null || agent == null || !agent.isOnNavMesh) return;

        // Distancia solo en el plano horizontal (ignora la altura)
        Vector3 diff = player.position - transform.position;
        diff.y = 0f;
        float distanceToPlayer = diff.magnitude;

        if (distanceToPlayer <= detectionRadius)
            StartChase();

        if (isChasing)
        {
            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackDistance && Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
                Attack();
            }
        }
    }

    void StartChase()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        if (wanderScript != null) wanderScript.enabled = false;
    }

    void Attack()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(attackDamage);
        else
            Debug.LogError("Bon no encuentra el script PlayerHealth en la escena.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}