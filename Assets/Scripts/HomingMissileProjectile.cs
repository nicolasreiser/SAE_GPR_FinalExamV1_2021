using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float velocity;
    [SerializeField] private float damage;
    [SerializeField] private float selfdestructTime = 10;
    [SerializeField] private GameObject electricHitEffectPrefab;
    [SerializeField] private float Detectionradius;

    private void Start()
    {
        _rigidbody.velocity = transform.forward * velocity;
        Destroy(gameObject, selfdestructTime);
    }

    private void Update()
    {
        Collider[] overlaps = Physics.OverlapSphere(this.transform.position,Detectionradius);
        Collider closestTarget = GetClosestCollider(overlaps);
        FaceDirection(closestTarget.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
            GameObject particle = Instantiate(electricHitEffectPrefab, this.transform.position, this.transform.rotation);
            Destroy(particle, particle.GetComponent<ParticleSystem>().main.duration);

        }
        Destroy(gameObject);
    }

    private void FaceDirection(Transform t)
    {
        Quaternion targetRotation = Quaternion.LookRotation(t.position);
        _rigidbody.MoveRotation(Quaternion.RotateTowards(this.transform.rotation,targetRotation,1));
    }

    private Collider GetClosestCollider(Collider[] colliders)
    {
        Collider closestTarget = colliders[0];
        float distance = Vector3.Distance(this.transform.position, colliders[0].transform.position);
        foreach(Collider c in colliders)
        {
            float newDistance = Vector3.Distance(this.transform.position, c.transform.position);
            if(newDistance < distance)
            {
                distance = newDistance;
                closestTarget = c;
            }

        }
        return closestTarget;
    }
}
