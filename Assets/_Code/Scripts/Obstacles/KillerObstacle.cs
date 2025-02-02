using _Code.Scripts.Player;
using _Code.Scripts.UIScripts;
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
                UIController.Instance.InGameUIState = InGameUIStates.GameOverIU;
                GameManager.Instance.GameState = GameState.Stop;
            }
        }
    }
}