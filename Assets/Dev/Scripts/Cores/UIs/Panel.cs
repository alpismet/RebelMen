using Dev.Scripts.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.Scripts.Cores.UIs
{
    public abstract class Panel : MonoBehaviour
    {
        [SerializeField]
        protected UnityEvent onOpened;
        
        [SerializeField]
        protected UnityEvent onClosed;
        
        

        //---------------------------------------------------------------------------------
        public virtual void Open()
        {
            gameObject.SetActiveFade(true);
            onOpened?.Invoke();
        }
        

        //---------------------------------------------------------------------------------
        public virtual void Close()
        {
            gameObject.SetActiveFade(false);
            onClosed?.Invoke();
        }
    }
}