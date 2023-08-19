using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dev.Scripts.Editor
{
    public class AssetShortcutWindow : EditorWindow
    {
        private Object draggedObject; // Reference to the dragged object
        private string newShortcutName = ""; // The name of the new shortcut to be added
        private readonly List<Object> shortcuts = new List<Object>(); // List to hold the shortcuts
        private readonly List<string> shortcutNames = new List<string>(); // List to hold the names of shortcuts
        private Vector2 scrollPosition; // Scroll position
        private int dragIndex = -1; // Index of the shortcut being dragged
        private Vector2 dragStartPosition = Vector2.zero;


        //---------------------------------------------------------------------------------
        [MenuItem("Tools/Alp/", priority = -int.MaxValue)]
        [MenuItem("Tools/Alp/Asset Shortcut Window", priority = 100)]
        public static void ShowWindow()
        {
            GetWindow<AssetShortcutWindow>("Asset Shortcuts");
        } 


        //---------------------------------------------------------------------------------
        private void OnEnable()
        {
            LoadShortcuts();
        }
        private void OnDisable()
        {
            SaveShortcuts();
        }


        //---------------------------------------------------------------------------------
        private void OnGUI()
        {
            GUILayout.Label("Asset Shortcuts", EditorStyles.boldLabel);

            // Position the Hamburger Menu (3 dots) in the top-right corner
            Rect rectPosition = new Rect(this.position.width - 22, 2, 20, 20);
            if (GUI.Button(rectPosition, "", EditorStyles.toolbarDropDown))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Clear All"), false, ClearAllShortcuts);
                menu.ShowAsContext();
            }

            // Maximum scroll area height
            float maxScrollHeight = position.height - 100f;

            // Calculate the total height of the shortcuts section
            float shortcutsHeight = shortcuts.Count * 32f;

            // Calculate the scroll area height based on the available space
            float scrollAreaHeight = Mathf.Min(shortcutsHeight, maxScrollHeight);

            // Show scroll view only if necessary
            if (scrollAreaHeight < shortcutsHeight)
            {
                // Begin the scroll view
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.Height(scrollAreaHeight));
            }
            
            // Scroll to show the highlighted shortcut in the view
            if (dragIndex >= 0 && dragIndex < shortcuts.Count)
            {
                float targetScrollY = 32f * dragIndex ;
                scrollPosition.y = Mathf.Lerp(scrollPosition.y, targetScrollY, 0.1f);
            }

            // Show existing shortcuts as a vertical layout
            for (int i = 0; i < shortcuts.Count; i++)
            {
                Object shortcut = shortcuts[i];
                if (shortcut != null)
                {
                    // Check if the shortcut is being dragged
                    bool isDragging = (dragIndex == i);

                    // Set the background color based on whether it is being dragged or not
                    Color backgroundColor = isDragging ? Color.red : Color.white;
                    GUI.backgroundColor = backgroundColor;

                    EditorGUILayout.BeginHorizontal();

                    // Drag & Drop handling for reordering shortcuts
                    Rect shortcutRect = EditorGUILayout.BeginHorizontal(GUILayout.Height(32));
                    if (DragAndDrop.objectReferences.Length>0){} 
                        //Do not move when a new object is dragged.
                    else if (Event.current.type == EventType.MouseDrag && shortcutRect.Contains(Event.current.mousePosition))
                    {
                        // If the shortcut is clicked and being dragged, start dragging it
                        if (dragStartPosition == Vector2.zero)
                            dragStartPosition = Event.current.mousePosition;
                        if (!isDragging && dragStartPosition != Event.current.mousePosition)
                        {
                            dragIndex = i;
                            dragStartPosition = Event.current.mousePosition;
                        }
                    }
                    else if (isDragging)
                    {
                        // If the shortcut is being dragged, update its position in the list
                        int newIndex = Mathf.Clamp((int)((Event.current.mousePosition.y - 20) / 32f), 0, shortcuts.Count - 1);
                        if (newIndex != dragIndex)
                        {
                            Object draggedShortcut = shortcuts[dragIndex];
                            string draggedShortcutName = shortcutNames[dragIndex];

                            shortcuts.RemoveAt(dragIndex);
                            shortcutNames.RemoveAt(dragIndex);

                            shortcuts.Insert(newIndex, draggedShortcut);
                            shortcutNames.Insert(newIndex, draggedShortcutName);

                            dragIndex = newIndex;
                        }
                    }
                    else if (Event.current.type == EventType.MouseUp)
                    {
                        // If the mouse is released, stop dragging the shortcut
                        dragIndex = -1;
                        dragStartPosition = Vector2.zero;
                    }

                    GUIContent content = new GUIContent(EditorGUIUtility.ObjectContent(null, shortcut.GetType()));
                    if (GUILayout.Button(content.image, GUILayout.Width(32), GUILayout.Height(32)))
                    {
                        OpenAsset(shortcut);
                    }

                    if (GUILayout.Button(shortcutNames[i], GUILayout.Height(32)))
                    {
                        SelectAsset(shortcut);
                    }

                    if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.Width(32), GUILayout.Height(32)))
                    {
                        RemoveShortcut(i);
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndHorizontal();
                }
            }

            // End the scroll view if necessary
            if (scrollAreaHeight < shortcutsHeight)
            {
                GUILayout.EndScrollView();
            }

            // Add some space before the new shortcut section
            GUILayout.Space(10);

            // Check if an object is being dragged into the window
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                // Check if the dragged object is already in the shortcuts list
                int existingIndex = shortcuts.IndexOf(DragAndDrop.objectReferences[0]);
                if (existingIndex >= 0)
                {
                    // If it exists, highlight the existing shortcut
                    dragIndex = existingIndex;
                }
                else
                {
                    // If it does not exist, reset the highlight
                    dragIndex = -1;
                }

                Event.current.Use();
            }
            else if (Event.current.type == EventType.DragPerform)
            {
                draggedObject = DragAndDrop.objectReferences[0];
                newShortcutName = draggedObject.name;

                // Check if the dragged object is already in the shortcuts list
                int existingIndex = shortcuts.IndexOf(draggedObject);
                if (existingIndex < 0)
                {
                    // If it does not exist, accept the drag and add the new shortcut
                    DragAndDrop.AcceptDrag();
                }
                else
                {
                    // If it exists, reset the draggedObject reference to prevent adding it again
                    draggedObject = null;
                    newShortcutName = "";
                }

                dragIndex = -1; // Reset the highlight
                Event.current.Use();
            }

            // Add flexible space to move the new shortcut section to the bottom
            GUILayout.FlexibleSpace();

            // Show the dragged object as a shortcut if available
            if (draggedObject != null)
            {
                if (string.IsNullOrEmpty(newShortcutName))
                {
                    // If newShortcutName is empty, use the asset name as the default name
                    newShortcutName = draggedObject.name;
                }

                EditorGUILayout.LabelField("Shortcut Name:");
                newShortcutName = EditorGUILayout.TextField(newShortcutName);

                if (GUILayout.Button("Add Shortcut"))
                {
                    if (string.IsNullOrEmpty(newShortcutName.Trim()))
                    {
                        // If the name is empty, use the asset name as the default name
                        newShortcutName = draggedObject.name;
                    }
                    AddShortcut(draggedObject, newShortcutName);
                }
            }

            // Add the custom text to the bottom of the window with 10 spaces before it
            GUILayout.Space(10);
            GUILayout.Label("Imagined by İsmet ALP. Designed by ChatGPT. - 04.08.2023");
        }



        //---------------------------------------------------------------------------------
        // Function to load the shortcuts and their names from EditorPrefs
        private void LoadShortcuts()
        {
            shortcuts.Clear();
            shortcutNames.Clear();

            int count = EditorPrefs.GetInt("ShortcutCount", 0);

            for (int i = 0; i < count; i++)
            {
                string shortcutPath = EditorPrefs.GetString("Shortcut_" + i + "_Path", "");
                Object shortcut = AssetDatabase.LoadAssetAtPath<Object>(shortcutPath);

                if (shortcut != null)
                {
                    shortcuts.Add(shortcut);
                    shortcutNames.Add(EditorPrefs.GetString("Shortcut_" + i + "_Name", shortcut.name));
                }
            }
        }


        //---------------------------------------------------------------------------------
        // Function to save the shortcuts and their names to EditorPrefs
        private void SaveShortcuts()
        {
            EditorPrefs.SetInt("ShortcutCount", shortcuts.Count);

            for (int i = 0; i < shortcuts.Count; i++)
            {
                Object shortcut = shortcuts[i];
                string shortcutPath = AssetDatabase.GetAssetPath(shortcut);

                EditorPrefs.SetString("Shortcut_" + i + "_Path", shortcutPath);
                EditorPrefs.SetString("Shortcut_" + i + "_Name", shortcutNames[i]);
            }
        }


        //---------------------------------------------------------------------------------
        // Clear all shortcuts
        private void ClearAllShortcuts()
        {
            shortcuts.Clear();
            shortcutNames.Clear();
        }


        //---------------------------------------------------------------------------------
        // Function to add the dragged object as a new shortcut
        private void AddShortcut(Object shortcutObject, string shortcutName)
        {
            // Enable undo for the change
            Undo.RecordObject(this, "Add Shortcut");

            // Add the shortcut and its name to the lists
            shortcuts.Add(shortcutObject);
            shortcutNames.Add(shortcutName);

            draggedObject = null; // Reset the draggedObject reference
            newShortcutName = ""; // Reset the newShortcutName field
        }


        //---------------------------------------------------------------------------------
        // Function to remove a shortcut
        private void RemoveShortcut(int index)
        {
            // Enable undo for the change
            Undo.RecordObject(this, "Remove Shortcut");

            // Remove the shortcut and its name from the lists
            shortcuts.RemoveAt(index);
            shortcutNames.RemoveAt(index);
        }


        //---------------------------------------------------------------------------------
        // Function to open the asset
        private void OpenAsset(Object asset)
        {
            AssetDatabase.OpenAsset(asset);
        }


        //---------------------------------------------------------------------------------
        // Function to select the asset
        private void SelectAsset(Object asset)
        {
            Selection.activeObject = asset;
        }
    }
}