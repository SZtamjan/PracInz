using System.Collections;
using _Code.Scripts.Points;
using _Code.Scripts.Singleton;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace _Code.Scripts.Player
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private GameManager _gameManager;
        private Coroutine _movePlayerCor;
        private Coroutine _liftPlayerCor;
        
        private float _playerSpacing;
        private int _currentPosition = 1;

        [SerializeField] private float playerNormalHeight;
        [SerializeField] private float playerLiftHeight;
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

        [FormerlySerializedAs("_iAmRunning")] [HideInInspector] public bool iAmMoving;

        public bool IAmMoving
        {
            get => iAmMoving;
            set
            {
                iAmMoving = value;
                pointCounter.IAmRunning = value;
            }
        }

        public void PrepareComponents()
        {
            playerAnimator = GetComponent<Animator>();
            pointCounter = PointCounter.Instance;
            _gameManager = GameManager.Instance;
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
            float theoreticalPosition = _currentPosition switch
            {
                0 => 0 - _playerSpacing,
                1 => 0,
                2 => 0 + _playerSpacing,
                _ => -5
            };

            var position = transform.position;
            return direction switch
            {
                -1 => new Vector3(theoreticalPosition - _playerSpacing, position.y, position.z),
                1 => new Vector3(theoreticalPosition + _playerSpacing, position.y, position.z),
                _ => new Vector3(0, 10, 0)
            };
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
            _movePlayerCor = null;
        }

        
        public void SwitchPlayerLift(bool liftPlayer)
        {
            if (_liftPlayerCor != null)
            {
                StopCoroutine(_liftPlayerCor);
                _liftPlayerCor = null;
            }
            _liftPlayerCor = StartCoroutine(ChangePlayerHeight(liftPlayer));
        }
        private IEnumerator ChangePlayerHeight(bool liftPlayer)
        {
            Vector3 updatedGoalPosition;
            Vector3 direction;
            
            if (liftPlayer)
            {
                updatedGoalPosition = new Vector3(transform.position.x, playerLiftHeight, transform.position.z);
                direction = (updatedGoalPosition - transform.position).normalized;
                while (Vector3.Distance(transform.position,updatedGoalPosition) > 0.05f)
                {
                    updatedGoalPosition = new Vector3(transform.position.x, playerLiftHeight, transform.position.z);
                    transform.position += direction * (playerVerticalSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.position = updatedGoalPosition;
                _liftPlayerCor = null;
                yield break;
            }
            
            updatedGoalPosition = new Vector3(transform.position.x, playerNormalHeight, transform.position.z);
            direction = (updatedGoalPosition - transform.position).normalized;
            while (Vector3.Distance(transform.position,updatedGoalPosition) > 0.05f)
            {
                updatedGoalPosition = new Vector3(transform.position.x, playerNormalHeight, transform.position.z);
                transform.position += direction * (playerVerticalSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = updatedGoalPosition;
            _liftPlayerCor = null;
            yield break;
        }
    }
}