using UnityEngine;

namespace GombleTask.Utility
{
    static class Singleton
    {
        [RuntimeInitializeOnLoadMethod]
        static void RuntimeInit()
        {
            GameObject = null;
        }
        
        public static GameObject GameObject;
    }
    
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T GetOrNull()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }
            
            if (_instance == null && Singleton.GameObject)
            {
                _instance = Singleton.GameObject.GetComponent<T>();
            }
            
            return _instance;
        }

        public static T GetOrCreate()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }

            if (_instance == null)
            {
                if (!Singleton.GameObject)
                {
                    Singleton.GameObject = new GameObject("Singleton GameObject");
                }
                
                _instance = Singleton.GameObject.AddComponent<T>();
            }

            return _instance;
        }
    }
}