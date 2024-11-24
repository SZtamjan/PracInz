using System;
using _Code.Scripts.Singleton;
using _Code.Scripts.UIScripts;
using UnityEngine;

namespace _Code.Scripts.Points
{
    public class PointCounter : Singleton<PointCounter>
    {
        private DisplayPoints _displayPoints;
        
        private float _points;

        private float Points
        {
            get => _points;
            set
            {
                _points = value;
                _displayPoints.UpdatePoints(_points);
            }
        }
        
        private bool _iAmRunning;

        public bool IAmRunning
        {
            private get => _iAmRunning;
            set => _iAmRunning = value;
        }

        public float currentSpeed
        {
            private get;
            set;
        }

        private float _time;

        private void Awake()
        {
            _displayPoints = UIController.Instance.displayPointsProp;
        }

        private void Update()
        {
            if(!IAmRunning) return;
            
            Points += currentSpeed * Time.deltaTime;
        }
    }
}