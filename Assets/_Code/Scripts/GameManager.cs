using System;
using _Code.Scripts.Player;
using UnityEngine;

namespace _Code.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private PlayerManager _playerManager;

        private void Awake()
        {
            PrepareComponents();
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        private void PrepareComponents()
        {
            _playerManager = PlayerManager.Instance;
        }
    }
    
    
}