using UnityEngine;

public class FullSingleton<T> : MonoBehaviour where T : Component {
    private static T instance;

    public static T Instance {
        get {
            if(instance == null) {
                instance = FindObjectOfType(typeof(T)) as T;
                if(instance == null) {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent(typeof(T)) as T;
                }
            }
            return instance;
        }
    }

    public virtual void Awake() {
        DontDestroyOnLoad(this.gameObject);
        if(instance == null) {
            instance = this as T;
        }else {
            Destroy(this.gameObject);
        }
    }

    public virtual void Initial() {

    }
}
