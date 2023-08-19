using UnityEngine;

namespace Dev.Scripts.Systems
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        private static T Instance
        {
            get
            {
                if (_instance) 
                    return _instance;
                
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs is { Length: > 0 }) _instance = objs[0];
                if (objs is { Length: > 1 }) Debug.LogError("There is more than one " + typeof(T).Name + "in the scene");
               
                if (_instance != null) 
                    return _instance;
                var obj = new GameObject();
                obj.hideFlags = HideFlags.HideAndDontSave;
                _instance = obj.AddComponent<T>();
                
                return _instance;
            }
        }
    }
}
