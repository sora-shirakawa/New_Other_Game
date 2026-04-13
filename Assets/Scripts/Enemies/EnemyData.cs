using UnityEngine;

namespace NewOtherGame.Enemies
{
    [CreateAssetMenu(menuName = "NewOtherGame/Enemy Data", fileName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public string enemyId;
        public float baseHp = 30f;
        public float moveSpeed = 2.5f;
        public float contactDamage = 8f;
        public float attackCooldown = 1f;
        public int xpDrop = 3;
    }
}
