using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Code.Scripts.RoadSystem
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private Transform currentRoadElement;
        [SerializeField] private GameObject roadPrefab;

        [SerializeField] private Transform playersPos; //dodaj to do awake
        [SerializeField] private float spawnDistance;
        [SerializeField] private float roadSpeed;

        private RoadSpawnState _currentState;
        
        public RoadSpawnState currentState
        {
            get => _currentState;
            private set
            {
                if (!Enum.IsDefined(typeof(RoadSpawnState), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(RoadSpawnState));
                _currentState = value;
                StateChanged();
            }
        }

        private void Awake()
        {
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
                Vector3.Distance(playersPos.position, currentRoadElement.position) < spawnDistance);
            currentState = RoadSpawnState.SpawnNewRoadElement;
        }

        private void SpawnNewRoadElement()
        {
            currentRoadElement = Instantiate(roadPrefab, CalculateNewPos(), currentRoadElement.rotation).transform;
            currentRoadElement.GetComponent<RoadMover>().mySpeed = roadSpeed;
            currentState = RoadSpawnState.WaitUntilCloseToPlayer;
        }
        
        private Vector3 CalculateNewPos()
        {
            Vector3 newSpawnPos = currentRoadElement.position;
            newSpawnPos.z += roadPrefab.transform.localScale.z * 10f;
            return newSpawnPos;
        }
    }
    public enum RoadSpawnState
    {
        SpawnNewRoadElement,
        WaitUntilCloseToPlayer
    }
}