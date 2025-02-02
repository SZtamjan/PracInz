using UnityEngine;
using UnityEngine.UI;

namespace _Code.Scripts.UIScripts
{
    public class ChangeToUIState : MonoBehaviour
    {
        [SerializeField] private InGameUIStates toUIState;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ChangeToState);
        }

        private void ChangeToState()
        {
            UIController.Instance.InGameUIState = toUIState;
        }
    }
}