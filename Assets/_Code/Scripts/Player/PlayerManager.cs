using System.Collections;
using _Code.Scripts.InputSys;
using _Code.Scripts.Points;
using _Code.Scripts.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace _Code.Scripts.Player
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private GameManager _gameManager;
        private InputManager _InputManager;

        private Coroutine _movePlayerCor;

        private float _playerSpacing; //(najpierw przemieszczanie sie zrob i testy pod button)
        private int _currentPosition = 1;

        [SerializeField] private float playerVerticalSpeed;
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
            _gameManager = GameManager.Instance;
            _InputManager = InputManager.Instance;
            _playerSpacing = _gameManager.RoadSpacing;
        }

        public void WSADPlayerInput(InputAction.CallbackContext ctx)
        {
            if (!ctx.started) return;
            InitMovePlayer(ctx.ReadValue<Vector2>());
            Debug.Log(ctx.ReadValue<Vector2>());
        }

        private void InitMovePlayer(Vector2 inputVector)
        {
            switch (inputVector.x)
            {
                case -1:
                    if (_currentPosition == 0) return;
                    if (_movePlayerCor != null)
                    {
                        StopCoroutine(_movePlayerCor);
                        _movePlayerCor = null;
                    }
                    _movePlayerCor = StartCoroutine(MovePlayer(CalculatePos(-1)));
                    --_currentPosition;
                    break;
                case 1:
                    if (_currentPosition == 2) return;
                    if (_movePlayerCor != null)
                    {
                        StopCoroutine(_movePlayerCor);
                        _movePlayerCor = null;
                    }
                    _movePlayerCor = StartCoroutine(MovePlayer(CalculatePos(1)));
                    ++_currentPosition;
                    break;
            }
        }

        private Vector3 CalculatePos(int direction)
        {
            float theoreticalPosition;
            switch (_currentPosition)
            {
                case 0:
                    theoreticalPosition = 0 - _playerSpacing;
                    break;
                case 1:
                    theoreticalPosition = 0;
                    break;
                case 2:
                    theoreticalPosition = 0 + _playerSpacing;
                    break;
                default:
                    theoreticalPosition = -5;
                    break;
            }

            if (direction == -1)
            {
                return new Vector3(theoreticalPosition - _playerSpacing, transform.position.y, transform.position.z);
            }

            if (direction == 1)
            {
                return new Vector3(theoreticalPosition + _playerSpacing, transform.position.y, transform.position.z);
            }

            return new Vector3(0, 10, 0);
        }

        private IEnumerator MovePlayer(Vector3 newPos)
        {
            Vector3 direction = (newPos - transform.position).normalized;
            while (Vector3.Distance(transform.position,newPos) > 0.05f)
            {
                transform.position += direction * (playerVerticalSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = newPos;
            Debug.Log("Przemieszczono");
            _movePlayerCor = null;
        }
    }
}