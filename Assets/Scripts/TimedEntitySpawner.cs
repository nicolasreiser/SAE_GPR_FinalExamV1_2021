using System.Collections;
using UnityEngine;

public class TimedEntitySpawner : MonoBehaviour
{
    [SerializeField] GameObject prototype;
    [SerializeField] Transform parent;
    [SerializeField] Transform[] spawnLocations;
    
    [SerializeField] Vector2 spawnDelayMinMax;
    [SerializeField] bool activeOnStart;

    [Header("To Inject")]
    [SerializeField] AiTargetingManager targetingManager;
    [SerializeField] DropSpawner dropSpawner;

    void Start()
    {
        Debug.Assert(targetingManager != null, "TargetingManager is null, injection will fail.");
        Debug.Assert(dropSpawner != null, "DropSpawner is null, injection will fail.");

        if (activeOnStart)
        {
            Activate();
        }
    }

    private IEnumerator SpawnRoutine()
    {
        if (prototype == null)
        {
            Debug.LogError("Trying to run EntitySpawner with no protoype set.");
            yield break;
        }

        while (true)
        {
            float waitTime = Random.Range(spawnDelayMinMax.x, spawnDelayMinMax.y);
            yield return new WaitForSeconds(waitTime); // TODO: convert into yield return null with own timer to avoid memory allocation
            SpawnEntity();
        }
    }

    private void SpawnEntity()
    {
        Transform target = (spawnLocations.Length == 0) ? transform : spawnLocations[Random.Range(0, spawnLocations.Length)];
        var go = Instantiate(prototype, target.position, target.rotation, parent);

        //inject dependencies
        foreach (var component in go.GetComponentsInChildren<IEnemy>()) // TODO: foreach is bad for memory management
        {
            component.InjectTargetingManager(targetingManager);
            component.InjectDropSpawner(dropSpawner);
        }
    }

    public void Deactivate()
    {
        StopAllCoroutines();
    }

    public void Activate()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var loc in spawnLocations)
        {
            Gizmos.DrawWireSphere(loc.position, 1);
        }
    }
}
