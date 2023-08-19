using Dev.Scripts.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dev.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/Game Data")]
    public class GameData : ScriptableObject
    {
        [BoxGroup("Settings")] public int level;
        [BoxGroup("Settings")] public int activeScene;
        [BoxGroup("Settings")] public int totalScene;


        
        //---------------------------------------------------------------------------------
        [HorizontalGroup("Buttons"), Button] private void ResetData()
        {
            level = 0;
            activeScene = 0;
            
            PlayerPrefs.DeleteAll();
            SaveSystem.Save(this);
        }
        
        
        //---------------------------------------------------------------------------------
        [HorizontalGroup("Buttons"), Button] private void SaveData() => SaveSystem.Save(this);
    }
}
