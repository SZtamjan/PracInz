using System;
using System.ComponentModel;
using _Code.Scripts.Obstacles;
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

        //Components
        private RoadManager _roadManager;
        private ObstacleManager _obstacleManager;
        
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
                    break;
                case PlayersState.Run:
                    Run();
                    break;
                case PlayersState.FastRun:
                    FastRun();
                    break;
                case PlayersState.Fly:
                    Flight();
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
            _roadManager.roadSpeed = playerSpeed;
            _obstacleManager.obstacleSpeed = playerSpeed;
        }

        private void FastRun()
        {
            _roadManager.roadSpeed = playerFastSpeed;
            _obstacleManager.obstacleSpeed = playerFastSpeed;
        }

        private void Flight()
        {
            _roadManager.roadSpeed = playerFlightSpeed;
            _obstacleManager.obstacleSpeed = playerFlightSpeed;
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