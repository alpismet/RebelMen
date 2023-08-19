using UnityEngine;

namespace Dev.Scripts.Managers
{
    public class ManagerBaseSingle<T> : ManagerBase<T> where T : Component
    {
        private static T _instance;
            
        private static T Instance
        {
            get
            {
                if (_instance)
                {
                    Debug.Log("Supplied from instance: " + typeof(T).Name);
                    return _instance;
                }
                
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs is { Length: > 0 }) _instance = objs[0];
                if (objs is { Length: > 1 }) Debug.LogError("There is more than one " + typeof(T).Name + "in the scene");
               
                if (_instance != null) 
                    return _instance;
                
                var obj = new GameObject
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                
                _instance = obj.AddComponent<T>();
                
                return _instance;
            }
        }

        
        //---------------------------------------------------------------------------------
        protected override T GetInstance() => Instance;
        
        
        //---------------------------------------------------------------------------------
        private void OnDisable()
        {
            _instance = null; 
        }
    }
}