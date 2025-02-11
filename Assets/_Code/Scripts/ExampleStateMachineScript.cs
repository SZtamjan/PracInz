using UnityEngine;
using UnityEngine.Events;

namespace _Code.Scripts
{
    public enum ExampleGameState
    {
        Menu,
        Gameplay,
        Pause,
        GameOver
    }
    public class ExampleStateMachineScript : MonoBehaviour
    {
        private ExampleGameState _currentState;
        public ExampleGameState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                UpdateState();
            }
        }

        void UpdateState()
        {
            switch (CurrentState)
            {
                case ExampleGameState.Menu:
                    //HandleMenu();
                    break;
                case ExampleGameState.Gameplay:
                    //HandleGameplay();
                    break;
                case ExampleGameState.Pause:
                    //HandlePause();
                    break;
                case ExampleGameState.GameOver:
                    //HandleGameOver();
                    break;
            }
        }
    }
}