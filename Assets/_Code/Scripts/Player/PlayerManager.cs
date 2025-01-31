using _Code.Scripts.Points;
using _Code.Scripts.Singleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Code.Scripts.Player
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private float playerSpeed;
        [SerializeField] private float playerFastSpeed;
        [SerializeField] private float playerFlightSpeed;

        [HideInInspector] public PointCounter pointCounter;
        [HideInInspector] public Animator playerAnimator;
        
        public float PlayerSpeed
        {
            get
            {
                pointCounter.currentSpeed = playerSpeed;
                return playerSpeed;
            }
        }

        public float PlayerFastSpeed
        {
            get
            {
                pointCounter.currentSpeed = playerFastSpeed;
                return playerFastSpeed;
            }
        }

        public float PlayerFlightSpeed
        {
            get
            {
                pointCounter.currentSpeed = playerFlightSpeed;
                return playerFlightSpeed;
            }
        }
        
        [HideInInspector] public bool _iAmRunning;

        public bool IAmRunning
        {
            get => _iAmRunning;
            set
            {
                _iAmRunning = value;
                pointCounter.IAmRunning = value;
            }
        }

        public void PrepareComponents()
        {
            playerAnimator = GetComponent<Animator>();
            pointCounter = PointCounter.Instance;
        }
    }
}