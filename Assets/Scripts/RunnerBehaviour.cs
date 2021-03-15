using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IAIControlledEntity
{
    void InjectTargetingManager(AiTargetingManager targetingManager);
}
public class RunnerBehaviour : MonoBehaviour, IAIControlledEntity
{
    [SerializeField] Material[] skinMaterials;
    [SerializeField] Renderer skinRenderer;
    [SerializeField] Animator animator;

    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] HeathComponent heathComponent;
    [SerializeField] AiTargetingManager targetingManager;

    Transform target;

    private void Start()
    {
        if (skinMaterials != null && skinMaterials.Length > 0)
            skinRenderer.material = skinMaterials[Random.Range(0, skinMaterials.Length)];

        heathComponent.Death += OnDeath;
        heathComponent.Hit += OnHit;

        Debug.Assert(targetingManager != null, "TargetingManager not injected or referenced.");

        target = targetingManager.GetDefaultAITarget();
        navMeshAgent.SetDestination(target.position);
    }

    private void Update()
    {
        animator.SetFloat("MovementSpeed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
    }
    private void OnHit(HeathComponent obj)
    {
        if (heathComponent.IsAlive)
            StartCoroutine(HitRoutine());
    }

    private void OnDeath(HeathComponent obj)
    {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        animator.SetTrigger("Dead");
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }

    private IEnumerator HitRoutine()
    {
        animator.SetBool("IsHit", true);
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1);
        animator.SetBool("IsHit", false);
        navMeshAgent.isStopped = false;
    }

    public void InjectTargetingManager(AiTargetingManager targetingManager)
    {
        this.targetingManager = targetingManager;
    }
}
