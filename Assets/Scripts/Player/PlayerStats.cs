using UnityEngine;

namespace NewOtherGame.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField] private float maxHp = 100f;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float attackPower = 10f;
        [SerializeField] private float attackInterval = 0.8f;
        [SerializeField] private float attackRange = 8f;
        [SerializeField] private float projectileSpeed = 10f;

        public float MaxHp => maxHp;
        public float CurrentHp { get; private set; }
        public float MoveSpeed => moveSpeed;
        public float AttackPower => attackPower;
        public float AttackInterval => attackInterval;
        public float AttackRange => attackRange;
        public float ProjectileSpeed => projectileSpeed;

        private void Awake()
        {
            CurrentHp = maxHp;
        }

        public void Heal(float amount)
        {
            CurrentHp = Mathf.Min(MaxHp, CurrentHp + amount);
        }

        public void TakeDamage(float amount)
        {
            CurrentHp = Mathf.Max(0f, CurrentHp - amount);
        }

        public bool IsDead()
        {
            return CurrentHp <= 0f;
        }
    }
}
