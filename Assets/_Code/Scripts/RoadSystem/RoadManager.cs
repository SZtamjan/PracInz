using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using _Code.Scripts.Player;
using _Code.Scripts.Singleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Code.Scripts.RoadSystem
{
    public class RoadManager : Singleton<RoadManager>
    {
        [SerializeField] private Transform currentRoadElement;
        [SerializeField] private GameObject roadPrefab;

        private Transform _playersPos; //dodaj to do awake
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

        private RoadSpawnState roadSpawnState;
        
        public RoadSpawnState currentState
        {
            private get => roadSpawnState;
            set
            {
                if (!Enum.IsDefined(typeof(RoadSpawnState), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(RoadSpawnState));
                roadSpawnState = value;
                StateChanged();
            }
        }

        private void Awake()
        {
            _playersPos = PlayerManager.Instance.transform;
            SetupRoadElement(); //setup of the first element
            currentState = RoadSpawnState.WaitUntilCloseToPlayer;
        }

        private void StateChanged()
        {
            switch (currentState)
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
                Vector3.Distance(_playersPos.position, currentRoadElement.position) < spawnDistance);
            currentState = RoadSpawnState.SpawnNewRoadElement;
        }

        private void SpawnNewRoadElement()
        {
            currentRoadElement = Instantiate(roadPrefab, CalculateNewPos(), currentRoadElement.rotation).transform;
            SetupRoadElement();
            currentState = RoadSpawnState.WaitUntilCloseToPlayer;
        }
        
        private Vector3 CalculateNewPos()
        {
            Vector3 newSpawnPos = currentRoadElement.position;
            newSpawnPos.z += roadPrefab.transform.localScale.z * 10f;
            return newSpawnPos;
        }

        private void SetupRoadElement()
        {
            RoadMover currRoadMover = currentRoadElement.GetComponent<RoadMover>();
            currentRoads.Add(currRoadMover);
            currRoadMover.playerPos = _playersPos;
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