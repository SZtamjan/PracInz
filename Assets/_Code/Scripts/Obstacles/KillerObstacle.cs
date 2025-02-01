using _Code.Scripts.Player;
using UnityEngine;

namespace _Code.Scripts.Obstacles
{
    public class KillerObstacle : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PlayerManager playerManager))
            {
                if(GameManager.Instance.GameState == GameState.Fly) return;
                GameManager.Instance.GameState = GameState.Stop;
            }
        }
    }
}