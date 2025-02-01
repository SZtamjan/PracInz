using System;
using _Code.Scripts.Player;
using UnityEngine;

namespace _Code.Scripts.Obstacles
{
    public class BoosterObstacle : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PlayerManager playerManager))
            {
                GameManager.Instance.GameState = GameState.Fly;
            }
        }
    }
}