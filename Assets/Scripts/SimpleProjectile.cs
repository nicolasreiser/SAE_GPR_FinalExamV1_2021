using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] float velocity;
    [SerializeField] float damage;
    [SerializeField] float selfdestructTime = 10;

    private void Start()
    {
        rigidbody.velocity = transform.forward * velocity;
        Destroy(gameObject, selfdestructTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
