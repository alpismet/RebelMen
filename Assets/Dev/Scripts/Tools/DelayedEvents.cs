using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.Scripts.Tools
{
    public class DelayedEvents : MonoBehaviour
    {
        [SerializeField] 
        private bool initializeAtStart;
        
        [SerializeField][ShowIf("initializeAtStart")]
        private float initializeDelay;
        
        [SerializeField] 
        private UnityEvent delayedEvents;
        
        
        //---------------------------------------------------------------------------------
        private void Start()
        {
            if (initializeAtStart)
                this.DelayedCall(initializeDelay, delayedEvents.Invoke);
        }
        
        
        //---------------------------------------------------------------------------------
        public void DelayedInvoke(float delay) => this.DelayedCall(delay, delayedEvents.Invoke);
    }
}