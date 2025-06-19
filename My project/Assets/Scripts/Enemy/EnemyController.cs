using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float attackCooldown;

    private NavMeshAgent agent;
    private PlayerHealth target;

    private bool _isCanAttack;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindAnyObjectByType<PlayerHealth>();
        _isCanAttack = true;
    }

    private void FixedUpdate()
    {
        if (target != null && target.gameObject.activeSelf)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            target = GameObject.FindAnyObjectByType<PlayerHealth>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (target != null && collision.gameObject == target.gameObject && _isCanAttack)
            StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        target.TakeDamage(damage);
        _isCanAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        _isCanAttack = true;
    }
}
