using NewOtherGame.Core;
using NewOtherGame.Player;
using UnityEngine;

namespace NewOtherGame.Combat
{
    [RequireComponent(typeof(PlayerStats))]
    public class AutoAttackController : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask enemyLayer;

        private PlayerStats stats;
        private float cooldown;

        private void Awake()
        {
            stats = GetComponent<PlayerStats>();
            if (firePoint == null)
            {
                firePoint = transform;
            }
        }

        private void Update()
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlaying())
            {
                return;
            }

            cooldown -= Time.deltaTime;
            if (cooldown > 0f)
            {
                return;
            }

            Transform target = FindNearestEnemy();
            if (target == null)
            {
                return;
            }

            Fire(target);
            cooldown = stats.AttackInterval;
        }

        private Transform FindNearestEnemy()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stats.AttackRange, enemyLayer);
            Transform nearest = null;
            float nearestSqr = float.MaxValue;

            for (int i = 0; i < hits.Length; i++)
            {
                float sqr = (hits[i].transform.position - transform.position).sqrMagnitude;
                if (sqr < nearestSqr)
                {
                    nearestSqr = sqr;
                    nearest = hits[i].transform;
                }
            }

            return nearest;
        }

        private void Fire(Transform target)
        {
            if (projectilePrefab == null)
            {
                return;
            }

            Vector3 spawnPos = firePoint.position;
            Vector2 dir = (target.position - spawnPos).normalized;

            Projectile projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            projectile.Initialize(dir, stats.ProjectileSpeed, stats.AttackPower);
        }
    }
}
