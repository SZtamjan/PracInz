using System;
using System.Globalization;
using _Code.Scripts.Points;
using _Code.Scripts.Singleton;
using TMPro;
using UnityEngine;

namespace _Code.Scripts.UIScripts
{
    public class UIController : Singleton<UIController>
    {
        private GameManager _gameManager;
        
        [SerializeField] private RectTransform gameUI;
        [SerializeField] private RectTransform gameOverUI;
        [SerializeField] private RectTransform pauseUI;
        [SerializeField] private RectTransform optionsUI;

        private Vector3 previousMenuPosition;
        private RectTransform currentMenuPosition;
        
        [SerializeField] private DisplayPoints displayPoints;
        [SerializeField] private TextMeshProUGUI displayPointsGameOverUI;
        public DisplayPoints DisplayPointsProp => displayPoints;

        private InGameUIStates _inGameUIState;
        public InGameUIStates InGameUIState
        {
            get
            {
                return _inGameUIState;
            }
            set
            {
                if (_inGameUIState == InGameUIStates.GameOverIU) return;
                _inGameUIState = value;
                UIStateChanged();
            }
        }

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            InGameUIState = InGameUIStates.GameUI;
        }

        private void UIStateChanged()
        {
            switch (InGameUIState)
            {
                case InGameUIStates.GameUI:
                    ShowUI(gameUI);
                    _gameManager.GameState = _gameManager.PreviousGameState;
                    break;
                case InGameUIStates.GameOverIU:
                    ShowUI(gameOverUI);
                    displayPoints.DisplayPointsOnGameOverUI();
                    _gameManager.GameState = GameState.Stop;
                    break;
                case InGameUIStates.PauseUI:
                    ShowUI(pauseUI);
                    _gameManager.GameState = GameState.Stop;
                    break;
                case InGameUIStates.Options:
                    ShowUI(optionsUI);
                    break;
                default:
                    Debug.LogError("Nieprawidłowy state");
                    break;
            }
        }

        private void ShowUI(RectTransform newUI)
        {
            if (currentMenuPosition != null) currentMenuPosition.localPosition = previousMenuPosition;
            previousMenuPosition = newUI.localPosition;
            newUI.localPosition = new Vector3(0, 0, 0);
            currentMenuPosition = newUI;
        }
    }

    public enum InGameUIStates
    {
        GameUI,
        GameOverIU,
        PauseUI,
        Options
    }
}