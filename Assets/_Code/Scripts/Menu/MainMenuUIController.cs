using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Code.Scripts.Menu
{
    public class MainMenuUIController : MonoBehaviour
    {
        [Tooltip("Select first state")] 
        [SerializeField] private Menus currentUIState;

        [SerializeField] private List<GameObject> menus;

        public void ChangeUIState(int newUIStateInt)
        {
            currentUIState = (Menus)newUIStateInt;

            switch (currentUIState)
            {
                case Menus.MainMenu:
                    ChangeVis((int)Menus.MainMenu);
                    break;
                case Menus.Options:
                    ChangeVis((int)Menus.Options);
                    break;
                case Menus.Creators:
                    ChangeVis((int)Menus.Creators);
                    break;
                default:
                    Debug.LogError("Incorrect Menu State");
                    break;
            }
        }

        private void ChangeVis(int menu)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if (i == menu)
                {
                    menus[menu].SetActive(true);
                    continue;
                }

                menus[i].SetActive(false);
            }
        }
    }

    public enum Menus
    {
        MainMenu,
        Options,
        Creators
    }
}