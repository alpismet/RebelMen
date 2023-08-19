using Dev.Scripts.Scriptables;
using Dev.Scripts.Systems;

namespace Dev.Scripts.Managers
{
    public class ScriptableManager : ManagerBaseSingle<ScriptableManager>
    {
        public GameData gameData;


        
        //---------------------------------------------------------------------------------
        protected override void Awake()
        { 
            base.Awake();
            SaveSystem.Load(gameData);
        }
    }
}