using System;
using UnityEngine;

namespace _Code.Scripts.RoadSystem
{
    public class RoadMover : MonoBehaviour
    {
        private float _mySpeed;
        public float mySpeed { private get; set; } = 5f;
        private void Update()
        {
            Vector3 newPos = transform.position - new Vector3(0, 0, 0.01f * mySpeed);
            transform.position = newPos;
        }
    }

    
}