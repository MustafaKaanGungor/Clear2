using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed; // Speed of the projectile
    private Transform target;
    private float damage; // Damage dealt by the projectile
    public float destroyThreshold = 0.5f; // Threshold distance to consider hitting the target

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("No target set for projectile.");
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Check if the projectile is close enough to the target to be considered a hit
        if (Vector3.Distance(transform.position, target.position) <= destroyThreshold)
        {
            DamageTarget();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            DamageTarget();
            Destroy(gameObject);
        }
    }

    void DamageTarget()
    {
        UnitStats unitController = target.GetComponent<UnitStats>();
        if (unitController != null)
        {
            unitController.TakeDamage(damage, DamageType.Physical);
        }
    }
}
