using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEntitySpawner : MonoBehaviour
{
    [SerializeField] GameObject prototype;
    [SerializeField] Transform parent;
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] AiTargetingManager targetingManager;

    [SerializeField] Vector2 spawnDelayMinMax;
    [SerializeField] bool activeOnStart;

    void Start()
    {
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
            yield return new WaitForSeconds(waitTime);
            Transform target = (spawnLocations.Length == 0) ? transform : spawnLocations[Random.Range(0, spawnLocations.Length)];
            var go = Instantiate(prototype, target.position, target.rotation, parent);

            //inject dependencies
            foreach (var component in go.GetComponentsInChildren<IAIControlledEntity>())
            {
                component.InjectTargetingManager(targetingManager);
            }
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
