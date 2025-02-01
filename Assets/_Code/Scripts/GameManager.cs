using System;
using System.ComponentModel;
using _Code.Scripts.Obstacles;
using _Code.Scripts.Player;
using _Code.Scripts.Points;
using _Code.Scripts.RoadSystem;
using _Code.Scripts.Singleton;
using NaughtyAttributes;
using UnityEngine;

namespace _Code.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        //Components
        private PlayerManager _playerManager;
        private RoadManager _roadManager;
        private ObstacleManager _obstacleManager;
        
        private GameState _gameState;
        private static readonly int State = Animator.StringToHash("GameState");

        public GameState GameState
        {
            get => _gameState;
            set
            {
                if (!Enum.IsDefined(typeof(GameState), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(GameState));
                _gameState = value;
                StateChanged();
            }
        }

        [SerializeField] private float roadSpacing;

        public float RoadSpacing => roadSpacing;


        private void Awake()
        {
            PrepareComponents();
        }

        private void StateChanged()
        {
            _playerManager.playerAnimator.SetInteger(State, (int)GameState);

            switch (GameState)
            {
                case GameState.Stop:
                    Stop();
                    _playerManager.IAmMoving = false;
                    break;
                case GameState.Run:
                    Run();
                    _playerManager.IAmMoving = true;
                    break;
                case GameState.FastRun:
                    FastRun();
                    _playerManager.IAmMoving = true;
                    break;
                case GameState.Fly:
                    Flight();
                    _playerManager.IAmMoving = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Stop()
        {
            _roadManager.roadSpeed = 0f;
            _obstacleManager.obstacleSpeed = 0f;
        }

        private void Run()
        {
            _roadManager.roadSpeed = _playerManager.PlayerSpeed;
            _obstacleManager.obstacleSpeed = _playerManager.PlayerSpeed;
            _playerManager.SwitchPlayerLift(false);
        }

        private void FastRun()
        {
            _roadManager.roadSpeed = _playerManager.PlayerFastSpeed;
            _obstacleManager.obstacleSpeed = _playerManager.PlayerFastSpeed;
            _playerManager.SwitchPlayerLift(false);
        }

        private void Flight()
        {
            _roadManager.roadSpeed = _playerManager.PlayerFlightSpeed;
            _obstacleManager.obstacleSpeed = _playerManager.PlayerFlightSpeed;
            _playerManager.SwitchPlayerLift(true);
        }

        #region Debug

        [Button]
        private void ChangeToStop()
        {
            GameState = GameState.Stop;
        }

        [Button]
        private void ChangeToRun()
        {
            GameState = GameState.Run;
        }

        [Button]
        private void ChangeToFastRun()
        {
            GameState = GameState.FastRun;
        }

        [Button]
        private void ChangeToFlight()
        {
            GameState = GameState.Fly;
        }

        #endregion

        private void PrepareComponents()
        {
            _playerManager = PlayerManager.Instance;
            _playerManager.PrepareComponents();
            _roadManager = RoadManager.Instance;
            _obstacleManager = ObstacleManager.Instance;
            GameState = GameState.Run;
        }
    }

    public enum GameState
    {
        Stop,
        Run,
        FastRun,
        Fly,
    }
}