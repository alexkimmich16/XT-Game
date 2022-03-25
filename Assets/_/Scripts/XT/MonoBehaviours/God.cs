using XT.ScriptableObjects;

namespace XT.MonoBehaviours {
    public class God : Singleton<God> {
        public static GodDB db => Instance.database;
        public GodDB database;

        // == use this space to track our global / static singletons ==
        // public static GlobalGameObject globalGameObject;

        // private void Awake() {
            // if(null == globalGameObject) globalGameObject = FindObjectOfType<GlobalGameObject>();
        // }
    }
}