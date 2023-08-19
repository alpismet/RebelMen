using System.Reflection;
using UnityEngine;

namespace Dev.Scripts.Managers
{
    [DefaultExecutionOrder(-10)]
    public class ManagerBase<T> : MonoBehaviour where T : Component
    {   


        //---------------------------------------------------------------------------------
        protected virtual T GetInstance() => this as T;
        
        
        //---------------------------------------------------------------------------------
        protected virtual void Awake()
        {
            //Debug.Log(" ✓ "+ typeof(T).ToString().Replace(typeof(T).Namespace + ".", string.Empty));
            foreach (FieldInfo field in typeof(Manager).GetFields())
            {
                if (field.FieldType == typeof(T))
                {
                    field.SetValue(null, GetInstance());;
                    break;
                }
            }
            
            if (GetType().BaseType == typeof(ManagerBaseSingle<T>) )
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}