using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XT.ScriptableObjects {
    [CreateAssetMenu(fileName = "GodDB", menuName = "GodDB", order = 0)]
    public class GodDB : SerializedScriptableObject {
        // put members members we want to keep here
        
#if UNITY_EDITOR
        public static T[] GetAllInstances<T>() where T : ScriptableObject {
            //FindAssets uses tags check documentation for more info
            string[] guids = AssetDatabase.FindAssets("t:"+ typeof(T).Name);
            T[] a = new T[guids.Length];
            //probably could get optimized 
            for(int i =0;i<guids.Length;i++){
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
 
            return a;
        }

        public static T[] GetAllInScene<T>() where T : MonoBehaviour {
            var a = new List<T>();
            var rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects) {
                a.AddRange(rootGameObject.GetComponentsInChildren<T>(true));
            }
            return a.ToArray();
        }

        [Button("Recompute From Assets")]
        public void RecomputeFromAssets() {
            // click this button before doing builds
            /*
            var assetsWeWant = GetAllInstances<AssetWeWant>();
            foreach (var assetWeWant in assetsWeWant) {
                if (!assetDB.Contains(assetWeWant)) {
                    assetDB.Add(assetWeWant);
                }
                if (!God.progress.assetDB.ContainsKey(assetWeWant.name)) {
                    God.progress.assetDB.Add(assetWeWant.name, 99);
                }
            }
            */
        }
#endif
    }
}