using System;
using _Code.Scripts.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Code.Scripts.InputSys
{
    public class InputManager : Singleton<InputManager>
    {

        private Vector3 _wsadInput;
        public Vector3 WSADInput
        {
            get
            {
                return _wsadInput;
            }
            private set
            {
                _wsadInput = value;
                
            }
        }

        

        public void WSADPlayerInput(InputAction.CallbackContext ctx)
        {
            if (!ctx.started) return;
            WSADInput = ctx.ReadValue<Vector3>();
        }
    }
}