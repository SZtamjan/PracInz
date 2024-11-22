using UnityEngine;

namespace _Code.Scripts.AppBehaviour
{
    public class LeaveApp : MonoBehaviour
    {
        public void Leave()
        {
            Application.Quit();
        }
    }
}