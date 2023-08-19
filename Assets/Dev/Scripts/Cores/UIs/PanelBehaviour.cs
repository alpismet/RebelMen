using Dev.Scripts.Managers;

namespace Dev.Scripts.Cores.UIs
{
    public class PanelBehaviour : Panel
    {
        
        //---------------------------------------------------------------------------------
        protected virtual void OnEnable()
        {
            Manager.Canvas.Panels.Add(GetType(), this);
        }
        
        
        //---------------------------------------------------------------------------------
        protected virtual void OnDisable()
        {
            Manager.Canvas.Panels.Remove(GetType());
        }
    }
}