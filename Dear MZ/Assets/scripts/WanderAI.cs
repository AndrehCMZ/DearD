using UnityEngine;
using UnityEngine.AI;

public class WanderAI : MonoBehaviour
{
    [Header("Configuración de Patrulla")]
    public float wanderRadius = 50f;  // Radio para buscar destinos en el mapa
    public float wanderTimer = 4f;     // Segundos entre cada nuevo destino

    private NavMeshAgent agent;
    private Animator animator;
    private float timer;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Cambia de destino al cumplir el tiempo o al llegar al punto anterior
        if (timer >= wanderTimer || (agent != null && !agent.pathPending && agent.remainingDistance < 0.5f))
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            if (agent != null && agent.isOnNavMesh)
            {
                agent.SetDestination(newPos);
            }
            timer = 0;
        }

        // Pasa la velocidad del NavMeshAgent al Animator para cambiar animaciones
        if (animator != null && agent != null)
        {
            float currentSpeed = agent.velocity.magnitude;
            animator.SetFloat("Speed", currentSpeed);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
