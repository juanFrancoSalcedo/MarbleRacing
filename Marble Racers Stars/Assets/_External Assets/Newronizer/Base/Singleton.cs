using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newronizer
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad = false;
        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var buffer = FindObjectsOfType<T>();
                    if (buffer.Length > 1)
                    {
                        Debug.LogWarning($"[{typeof(T)}] There are more than one instance in scene, are have been deleted.");
                        DeleteInstance(buffer);
                        instance = buffer[0];
                    }
                    else
                        instance = FindObjectOfType<T>();

                    if (instance == null)
                        Debug.LogError($"Instance of type {typeof(T)} not found. Make sure it's on the scene.");
                }
                return instance;
            }
        }

        private static void DeleteInstance(T[] type)
        {
            for (int i = 1; i < type.Length; i++)
                Destroy(type[i].gameObject);
        }

        protected virtual void Awake()
        {
            if (!dontDestroyOnLoad) return;
            if (FindObjectsOfType<T>().Length > 1)
                Destroy(gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }
    }
}