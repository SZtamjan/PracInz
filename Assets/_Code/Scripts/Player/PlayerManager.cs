using System;
using System.ComponentModel;
using _Code.Scripts.Obstacles;
using _Code.Scripts.Points;
using _Code.Scripts.RoadSystem;
using _Code.Scripts.Singleton;
using NaughtyAttributes;
using UnityEngine;

namespace _Code.Scripts.Player
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private float playerSpeed;
        [SerializeField] private float playerFastSpeed;
        [SerializeField] private float playerFlightSpeed;

        private float PlayerSpeed
        {
            get
            {
                _pointCounter.currentSpeed = playerSpeed;
                return playerSpeed;
            }
        }

        private float PlayerFastSpeed
        {
            get
            {
                _pointCounter.currentSpeed = playerFastSpeed;
                return playerFastSpeed;
            }
        }

        private float PlayerFlightSpeed
        {
            get
            {
                _pointCounter.currentSpeed = playerFlightSpeed;
                return playerFlightSpeed;
            }
        }
        
        private bool _iAmRunning;

        private bool IAmRunning
        {
            get => _iAmRunning;
            set
            {
                _iAmRunning = value;
                _pointCounter.IAmRunning = value;
            }
        }
        
        //Components
        private RoadManager _roadManager;
        private ObstacleManager _obstacleManager;
        private PointCounter _pointCounter;
        
        private PlayersState _playersState;

        public PlayersState playersState
        {
            get => _playersState;
            private set
            {
                if (!Enum.IsDefined(typeof(PlayersState), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(PlayersState));
                _playersState = value;
                StateChanged();
            }
        }

        private void Awake()
        {
            _pointCounter = PointCounter.Instance;
            _roadManager = RoadManager.Instance;
            _obstacleManager = ObstacleManager.Instance;
            playersState = PlayersState.Run;
        }

        private void StateChanged()
        {
            switch (playersState)
            {
                case PlayersState.Stop:
                    Stop();
                    IAmRunning = false;
                    break;
                case PlayersState.Run:
                    Run();
                    IAmRunning = true;
                    break;
                case PlayersState.FastRun:
                    FastRun();
                    IAmRunning = true;
                    break;
                case PlayersState.Fly:
                    Flight();
                    IAmRunning = true;
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
            _roadManager.roadSpeed = PlayerSpeed;
            _obstacleManager.obstacleSpeed = PlayerSpeed;
        }

        private void FastRun()
        {
            _roadManager.roadSpeed = PlayerFastSpeed;
            _obstacleManager.obstacleSpeed = PlayerFastSpeed;
        }

        private void Flight()
        {
            _roadManager.roadSpeed = PlayerFlightSpeed;
            _obstacleManager.obstacleSpeed = PlayerFlightSpeed;
        }

        #region Debug

        [Button]
        private void ChangeToStop()
        {
            playersState = PlayersState.Stop;
        }
        
        [Button]
        private void ChangeToRun()
        {
            playersState = PlayersState.Run;
        }
        
        [Button]
        private void ChangeToFastRun()
        {
            playersState = PlayersState.FastRun;
        }
        
        [Button]
        private void ChangeToFlight()
        {
            playersState = PlayersState.Fly;
        }

        #endregion
    }

    public enum PlayersState
    {
        Stop,
        Run,
        FastRun,
        Fly,
    }
}