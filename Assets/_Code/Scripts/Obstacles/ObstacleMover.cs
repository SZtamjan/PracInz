using UnityEngine;

namespace _Code.Scripts.Obstacles
{
    public class ObstacleMover : MonoBehaviour
    {
        public float disappearDistance { private get; set; }
        public float mySpeed { private get; set; } = 5f;
        public float speedMultiplier { private get; set; } = 1f;
        
        public ObstacleManager obstacleManager { private get; set; }

        private void Update() // zmienic na state machine xD
        {
            Vector3 myCurrPos = transform.position;
            Vector3 newPos = myCurrPos - new Vector3(0, 0, Time.deltaTime * mySpeed * speedMultiplier);
            transform.position = newPos;

            if (myCurrPos.z < -disappearDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}