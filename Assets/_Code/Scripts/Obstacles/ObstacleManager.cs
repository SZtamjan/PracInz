using System;
using System.Collections.Generic;
using System.ComponentModel;
using _Code.Scripts.Singleton;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Code.Scripts.Obstacles
{
    public class ObstacleManager : Singleton<ObstacleManager>
    {
        private Transform _playersPos;
        
        [SerializeField] private List<GameObject> obstacles;
        [SerializeField] private float spawnInterval;
        public Transform lastRoadElement { private get; set; }
        [Tooltip("Can not be smaller than spawnDistance!")] [SerializeField] private float roadDisappearDistance;
        public List<ObstacleMover> currentObstacles { get; set; } = new List<ObstacleMover>();
        
        private float _obstacleSpeed;
        public float obstacleSpeed
        {
            get => _obstacleSpeed;
            set
            {
                _obstacleSpeed = value;
                UpdateSpeedOnEveryObstacle();
            }
        }
        
        private ObstaclesSpawnState _obstaclesSpawnState;
        
        public ObstaclesSpawnState obstaclesSpawnState
        {
            private get => _obstaclesSpawnState;
            set
            {
                if (!Enum.IsDefined(typeof(ObstaclesSpawnState), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(ObstaclesSpawnState));
                _obstaclesSpawnState = value;
                StateChanged();
            }
        }

        private void StateChanged()
        {
            switch (obstaclesSpawnState)
            {
                case ObstaclesSpawnState.SpawnNewObstacle:
                    SpawnObstacle();
                    break;
                case ObstaclesSpawnState.WaitUntilCloseToPlayer:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Button]
        private void SpawnObstacle()
        {
            Instantiate(GetRandomObstacle(), CalculateNewPos(), Quaternion.identity);
        }
        
        private GameObject GetRandomObstacle()
        {
            return obstacles[Random.Range(0, obstacles.Count - 1)];
        }

        private Vector3 CalculateNewPos()
        {
            if (!lastRoadElement) return new Vector3(0,-1,0);
            Vector3 newSpawnPos = lastRoadElement.position;
            newSpawnPos += new Vector3(0, 1f, 0);
            return newSpawnPos;
        }

        private void UpdateSpeedOnEveryObstacle()
        {
            
        }
    }

    public enum ObstaclesSpawnState
    {
        SpawnNewObstacle,
        WaitUntilCloseToPlayer
    }
}