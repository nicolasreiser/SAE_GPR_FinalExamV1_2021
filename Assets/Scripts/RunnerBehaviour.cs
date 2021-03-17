using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemy
{
    void InjectTargetingManager(AiTargetingManager targetingManager);
    void InjectDropSpawner(DropSpawner dropSpawner);
}


public class RunnerBehaviour : MonoBehaviour, IEnemy
{
    [SerializeField] Material[] skinMaterials;
    [SerializeField] Renderer skinRenderer;
    [SerializeField] Animator animator;
    [SerializeField] LootDescription lootDescription;

    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] HeathComponent heathComponent;

    [Header("Injected")]
    [SerializeField] AiTargetingManager targetingManager;
    [SerializeField] DropSpawner dropSpawner;

    Transform target;

    private void Start()
    {
        if (skinMaterials != null && skinMaterials.Length > 0)
            skinRenderer.material = skinMaterials[Random.Range(0, skinMaterials.Length)];

        Debug.Assert(heathComponent != null, "HealthComponent not referenced in inspector");
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
        navMeshAgent.enabled = false;

        yield return new WaitForSeconds(3f);

        if (lootDescription != null)
        {
            var drop = lootDescription.SelectDropRandomly();
            dropSpawner.SpawnDropAt(drop, transform.position);
        }

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

    public void InjectDropSpawner(DropSpawner dropSpawner)
    {
        this.dropSpawner = dropSpawner;
    }
}
