using _Code.Scripts.Singleton;
using UnityEngine;

namespace _Code.Scripts.UIScripts
{
    public class UIController : Singleton<UIController>
    {
        [SerializeField] private DisplayPoints displayPoints;

        public DisplayPoints displayPointsProp => displayPoints;
    }
}