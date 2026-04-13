using NewOtherGame.Core;
using NewOtherGame.Combat;
using UnityEngine;

namespace NewOtherGame.Player
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private Rigidbody2D rb;
        private PlayerStats stats;

        private Vector2 moveInput;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            stats = GetComponent<PlayerStats>();
        }

        private void Update()
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlaying())
            {
                moveInput = Vector2.zero;
                return;
            }

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(x, y).normalized;
        }

        private void FixedUpdate()
        {
            rb.velocity = moveInput * stats.MoveSpeed;
        }

        public void TakeDamage(float damage)
        {
            stats.TakeDamage(damage);
            if (stats.IsDead())
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SetState(GameState.GameOver);
                }
            }
        }
    }
}
