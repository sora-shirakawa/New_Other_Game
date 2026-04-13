using UnityEngine;

namespace NewOtherGame.Core
{
    public class AutoStartRun : MonoBehaviour
    {
        [SerializeField] private bool startOnAwake = true;

        private void Awake()
        {
            if (!startOnAwake)
            {
                return;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartRun();
            }
        }
    }
}
