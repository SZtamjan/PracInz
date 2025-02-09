namespace _Code.Scripts
{
    public class ExampleStateMachineScript
    {
        public enum GameState
        {
            Menu,
            Gameplay,
            Pause,
            GameOver
        }

        private GameState _currentState;

        public GameState CurrentState
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
                case GameState.Menu:
                    //HandleMenu();
                    break;
                case GameState.Gameplay:
                    //HandleGameplay();
                    break;
                case GameState.Pause:
                    //HandlePause();
                    break;
                case GameState.GameOver:
                    //HandleGameOver();
                    break;
            }
        }
    }
}