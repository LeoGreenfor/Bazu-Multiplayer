using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerHealth target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindAnyObjectByType<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        agent.SetDestination(target.transform.position);
    }
}
