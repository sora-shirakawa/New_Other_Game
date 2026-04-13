using NewOtherGame.Combat;
using NewOtherGame.Core;
using UnityEngine;

namespace NewOtherGame.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyData enemyData;

        private Transform target;
        private Rigidbody2D rb;
        private float currentHp;
        private float contactCooldown;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            currentHp = enemyData != null ? enemyData.baseHp : 30f;
        }

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        private void Update()
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlaying())
            {
                rb.velocity = Vector2.zero;
                return;
            }

            contactCooldown -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (target == null)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            float moveSpeed = enemyData != null ? enemyData.moveSpeed : 2f;
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player") || contactCooldown > 0f)
            {
                return;
            }

            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damage = enemyData != null ? enemyData.contactDamage : 5f;
                damageable.TakeDamage(damage);
                contactCooldown = enemyData != null ? enemyData.attackCooldown : 1f;
            }
        }

        public void TakeDamage(float damage)
        {
            currentHp -= damage;
            if (currentHp <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
