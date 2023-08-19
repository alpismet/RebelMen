using System;
using System.Collections.Generic;
using Dev.Scripts.Cores.UIs;
using Sirenix.OdinInspector;


namespace Dev.Scripts.Managers
{
    public class CanvasManager : ManagerBaseSingle<CanvasManager>
    {
        [ShowInInspector] 
        public Dictionary<Type, Panel> Panels = new Dictionary<Type, Panel>();
        
        

        //---------------------------------------------------------------------------------
        public T GetPanel<T>() where T : class => Panels[typeof(T)] as T;
        


        //---------------------------------------------------------------------------------
        public void CloseAllPanels()
        {
            foreach (var panel in Panels)
                panel.Value.gameObject.SetActive(false);
        }
    }
}