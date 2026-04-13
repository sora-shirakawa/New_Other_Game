using UnityEngine;

namespace NewOtherGame.Core
{
    public enum GameState
    {
        Title,
        Playing,
        LevelUp,
        Paused,
        GameOver,
        StageClear,
        GameClear
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private GameState currentState = GameState.Title;

        public GameState CurrentState => currentState;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartRun()
        {
            SetState(GameState.Playing);
        }

        public void SetState(GameState state)
        {
            currentState = state;

            Time.timeScale = state == GameState.LevelUp || state == GameState.Paused ? 0f : 1f;
        }

        public bool IsPlaying()
        {
            return currentState == GameState.Playing;
        }
    }
}
