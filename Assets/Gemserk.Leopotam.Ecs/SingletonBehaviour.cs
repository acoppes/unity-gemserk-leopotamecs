using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static string InstanceName => $"~{typeof(T).Name}";

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instanceGameObject = GameObject.Find(InstanceName);
                    if (instanceGameObject != null)
                    {
                        _instance = instanceGameObject.GetComponentInChildren<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR 
            
            #if UNITY_2019
            if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null)
                return;
            #elif UNITY_2020_1_OR_NEWER
            if ( UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null)
                return;
            #endif
#endif
            
            // if there is only one Component in the system?
            if (gameObject.GetComponents<MonoBehaviour>().Length == 1)
                gameObject.name = InstanceName;
        }
    }
}