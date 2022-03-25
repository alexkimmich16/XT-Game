using System;
using UnityEngine;

// via: https://blog.mzikmund.com/2019/01/a-modern-singleton-in-unity/
namespace XT.MonoBehaviours {
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

        public static T Instance => LazyInstance.Value;

        private static T CreateSingleton() {
            var other = FindObjectOfType<T>();
            if (other != null && !LazyInstance.IsValueCreated) {
#if !UNITY_EDITOR
            DontDestroyOnLoad(other.gameObject);
#endif
                return other;
            }
        
            var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
            var instance = ownerObject.AddComponent<T>();
            DontDestroyOnLoad(ownerObject);
            return instance;
        }
    }
}