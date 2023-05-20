using UnityEngine;

namespace SQL_Quest.Creatures.Player
{
    public class PlayerDataHandler : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        public static PlayerDataHandler Instance { get; private set; }
        public static PlayerData PlayerData => Instance._playerData;

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
