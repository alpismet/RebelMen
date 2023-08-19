using UnityEditor;
using UnityEditor.SceneManagement;

namespace Dev.Scripts.Editor
{
    [InitializeOnLoad]
    static class EditorAutoSave
    {
        private const string MenuName = "Tools/Alp/Editor Auto Save Before Run";
        private static bool _isEnabled;


        //---------------------------------------------------------------------------------
        static EditorAutoSave()
        {
            EditorApplication.playModeStateChanged -= AutoSaveWhenPlaymodeStart;
            EditorApplication.playModeStateChanged += AutoSaveWhenPlaymodeStart;

            _isEnabled = EditorPrefs.GetBool(MenuName, false);
            Menu.SetChecked(MenuName, _isEnabled);
        }


        //---------------------------------------------------------------------------------
        [MenuItem(MenuName, priority = 0)]
        private static void ToggleAction()
        {
            _isEnabled = !_isEnabled;
            Menu.SetChecked(MenuName, _isEnabled);
            EditorPrefs.SetBool(MenuName, _isEnabled);
        }


        //---------------------------------------------------------------------------------
        private static void AutoSaveWhenPlaymodeStart(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.ExitingEditMode && _isEnabled)
            {
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        }
    }
}