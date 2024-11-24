using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using _Code.Scripts.Obstacles;
using _Code.Scripts.Player;
using _Code.Scripts.Singleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Code.Scripts.RoadSystem
{
    public class RoadManager : Singleton<RoadManager>
    {
        [SerializeField] private Transform currentRoadElement;

        public Transform CurrentRoadElement
        {
            get => currentRoadElement;
            set
            {
                currentRoadElement = value;
                _obstacleManager.lastRoadElement = value;
            }
        }
        
        [SerializeField] private GameObject roadPrefab;

        private Transform _playersPos;
        private ObstacleManager _obstacleManager;
        [SerializeField] private float spawnDistance;
        [Tooltip("Can not be smaller than spawnDistance!")] [SerializeField] private float roadDisappearDistance;

        public List<RoadMover> currentRoads { get; set; } = new List<RoadMover>();

        private float _roadSpeed;
        public float roadSpeed
        {
            get => _roadSpeed;
            set
            {
                _roadSpeed = value;
                UpdateSpeedOnEveryRoad();
            }
        }

        private RoadSpawnState _roadSpawnState;
        
        public RoadSpawnState roadSpawnState
        {
            private get => _roadSpawnState;
            set
            {
                if (!Enum.IsDefined(typeof(RoadSpawnState), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(RoadSpawnState));
                _roadSpawnState = value;
                StateChanged();
            }
        }

        private void Awake()
        {
            _obstacleManager = ObstacleManager.Instance;
            _obstacleManager.lastRoadElement = CurrentRoadElement;
            
            _playersPos = PlayerManager.Instance.transform;
            SetupRoadElement(); //setup of the first element
            roadSpawnState = RoadSpawnState.WaitUntilCloseToPlayer;
        }

        private void StateChanged()
        {
            switch (roadSpawnState)
            {
                case RoadSpawnState.SpawnNewRoadElement:
                    SpawnNewRoadElement();
                    break;
                case RoadSpawnState.WaitUntilCloseToPlayer:
                    StartCoroutine(WaitForRoadToGetCloser());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator WaitForRoadToGetCloser()
        {
            yield return new WaitUntil(() =>
                Vector3.Distance(_playersPos.position, CurrentRoadElement.position) < spawnDistance);
            roadSpawnState = RoadSpawnState.SpawnNewRoadElement;
        }

        private void SpawnNewRoadElement()
        {
            CurrentRoadElement = Instantiate(roadPrefab, CalculateNewPos(), CurrentRoadElement.rotation).transform;
            SetupRoadElement();
            roadSpawnState = RoadSpawnState.WaitUntilCloseToPlayer;
        }
        
        private Vector3 CalculateNewPos()
        {
            Vector3 newSpawnPos = CurrentRoadElement.position;
            newSpawnPos.z += roadPrefab.transform.localScale.z * 10f;
            return newSpawnPos;
        }

        private void SetupRoadElement()
        {
            RoadMover currRoadMover = CurrentRoadElement.GetComponent<RoadMover>();
            currentRoads.Add(currRoadMover);
            currRoadMover.mySpeed = roadSpeed;
            currRoadMover.disappearDistance = roadDisappearDistance;
            currRoadMover.roadManager = this;
        }

        private void UpdateSpeedOnEveryRoad()
        {
            foreach (RoadMover road in currentRoads)
            {
                road.mySpeed = roadSpeed;
            }
        }
    }
    public enum RoadSpawnState
    {
        SpawnNewRoadElement,
        WaitUntilCloseToPlayer
    }
}