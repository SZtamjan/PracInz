using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Code.Scripts.AppBehaviour
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private int newSceneIndex;
        
        public void ChangeScene()
        {
            SceneManager.LoadScene(newSceneIndex);
        }
    }
}