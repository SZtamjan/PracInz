using UnityEngine;

namespace _Code.Scripts.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
    
        public static T Instance => GetInstance();
    
        private static T GetInstance()
        {
            if (_instance == null) FindInstance();
            return _instance;
        }
    
        private static void FindInstance()
        {
            var singleton = FindObjectOfType<T>();
    
            if (singleton == null)
                //Can be ignored if is triggered by OnDisable
                Debug.LogWarning($"<color=red>No Object With Script: {nameof(T)}</color>");
            else
                _instance = singleton;
        }
    }
}

