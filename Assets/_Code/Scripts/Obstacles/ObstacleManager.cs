using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using _Code.Scripts.RoadSystem;
using _Code.Scripts.Singleton;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Code.Scripts.Obstacles
{
    public class ObstacleManager : Singleton<ObstacleManager>
    {
        private GameManager _gameManager;

        private Transform _playersPos;

        [SerializeField] private List<ObstacleData> obstacles;

        private Transform _currentObstacle;

        public Transform CurrentObstacle
        {
            private get => _currentObstacle;
            set { _currentObstacle = value; }
        }

        [SerializeField] private float spawnInterval;
        public Transform lastRoadElement { private get; set; }

        [FormerlySerializedAs("roadDisappearDistance")]
        [Tooltip("Can not be smaller than spawnDistance!")]
        [SerializeField]
        private float obstacleDisappearDistance;
        private float obstaclesSpacing;
        
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

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            obstaclesSpacing = _gameManager.RoadSpacing;
            obstaclesSpawnState = ObstaclesSpawnState.WaitUntilCloseToPlayer;
        }

        private void StateChanged()
        {
            switch (obstaclesSpawnState)
            {
                case ObstaclesSpawnState.SpawnNewObstacle:
                    SpawnObstacle();
                    break;
                case ObstaclesSpawnState.WaitUntilCloseToPlayer:
                    StartCoroutine(WaitForRoadToGetCloser());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator WaitForRoadToGetCloser()
        {
            yield return new WaitForSeconds(spawnInterval);
            obstaclesSpawnState = ObstaclesSpawnState.SpawnNewObstacle;
        }

        [Button]
        private void SpawnObstacle()
        {
            if (_gameManager.GameState == GameState.Stop)
            {
                StartCoroutine(WaitForUnPause());
                return;
            }

            ObstacleData data = GetRandomObstacle();
            CurrentObstacle = Instantiate(data.Prefab, CalculateNewPos(data.spawnHeight), Quaternion.identity).transform;
            SetupObstacle(data.MySpeedMultiplier);
            obstaclesSpawnState = ObstaclesSpawnState.WaitUntilCloseToPlayer;
        }

        private IEnumerator WaitForUnPause()
        {
            yield return new WaitUntil(() => _gameManager.GameState != GameState.Stop);
            obstaclesSpawnState = ObstaclesSpawnState.WaitUntilCloseToPlayer;
        }

        private void SetupObstacle(float speedMultiplier)
        {
            ObstacleMover currObstacleMover = CurrentObstacle.GetComponent<ObstacleMover>();
            currentObstacles.Add(currObstacleMover);
            currObstacleMover.disappearDistance = obstacleDisappearDistance;
            currObstacleMover.mySpeed = obstacleSpeed;
            currObstacleMover.speedMultiplier = speedMultiplier;
            currObstacleMover.obstacleManager = this;
        }

        private ObstacleData GetRandomObstacle()
        {
            float totalChance = 0f;

            foreach (var obj in obstacles)
            {
                totalChance += obj.spawnChance;
            }

            float randomValue = Random.Range(0, totalChance);

            float cumulativeChance = 0f;

            foreach (var obj in obstacles)
            {
                cumulativeChance += obj.spawnChance;

                if (randomValue <= cumulativeChance)
                {
                    return obj;
                    break;
                }
            }
            
            Debug.LogError("Nie powinno sie to stac");
            return obstacles[Random.Range(0, obstacles.Count)];
        }

        private Vector3 CalculateNewPos(float spawnHeight)
        {
            if (!lastRoadElement) return new Vector3(0, -1, 0);
            Vector3 newSpawnPos = lastRoadElement.position;
            newSpawnPos += new Vector3(0, spawnHeight, 0);

            int newSpacing = Random.Range(0, 3);
            switch (newSpacing)
            {
                case 0:
                    newSpawnPos = new Vector3(-obstaclesSpacing, newSpawnPos.y, newSpawnPos.z);
                    break;
                case 1:
                    newSpawnPos = new Vector3(0, newSpawnPos.y, newSpawnPos.z);
                    break;
                case 2:
                    newSpawnPos = new Vector3(obstaclesSpacing, newSpawnPos.y, newSpawnPos.z);
                    break;
            }
            
            return newSpawnPos;
        }

        private void UpdateSpeedOnEveryObstacle()
        {
            foreach (ObstacleMover obstacle in currentObstacles)
            {
                obstacle.mySpeed = _obstacleSpeed;
            }
        }
    }

    public enum ObstaclesSpawnState
    {
        SpawnNewObstacle,
        WaitUntilCloseToPlayer
    }

    [Serializable]
    public struct ObstacleData
    {
        public GameObject Prefab;
        public float spawnHeight;
        public float spawnChance;
        public float MySpeedMultiplier;
    }
}