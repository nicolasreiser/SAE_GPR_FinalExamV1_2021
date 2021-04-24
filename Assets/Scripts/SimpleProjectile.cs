using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float velocity;
    [SerializeField] private float damage;
    [SerializeField] private float selfdestructTime = 10;
    [SerializeField] private GameObject electricHitEffectPrefab;

    private void Start()
    {
        _rigidbody.velocity = transform.forward * velocity;
        Destroy(gameObject, selfdestructTime);
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
}
