using UnityEngine;

namespace NewOtherGame.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 2f;

        private Vector2 direction;
        private float speed;
        private float damage;

        public void Initialize(Vector2 dir, float projectileSpeed, float projectileDamage)
        {
            direction = dir.normalized;
            speed = projectileSpeed;
            damage = projectileDamage;
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
