using System;
using UnityEngine;

namespace _Code.Scripts.RoadSystem
{
    public class RoadMover : MonoBehaviour
    {
        public float disappearDistance { private get; set; }
        
        private float _mySpeed;
        public float mySpeed { private get; set; } = 5f;
        public RoadManager roadManager { private get; set; }
        
        private void Update() // zmienic na state machine xD
        {
            Vector3 myCurrPos = transform.position;
            Vector3 newPos = myCurrPos - new Vector3(0, 0, Time.deltaTime * mySpeed);
            transform.position = newPos;

            if (myCurrPos.z < -disappearDistance)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            roadManager.currentRoads.Remove(this);
        }
    }

    
}