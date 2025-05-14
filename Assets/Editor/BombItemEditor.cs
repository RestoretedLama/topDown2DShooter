#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Weapons.Bombs;

[CustomEditor(typeof(Item))]
public class BombItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default inspector which now handles all bomb properties directly
        DrawDefaultInspector();
        
        // Get the Item being edited
        Item item = (Item)target;
        
        // Only show additional info for bombs
        if (item.itemType == ItemType.Bomb)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("The bomb properties are now directly editable above.\nMake sure to set the bombPrefab for proper functioning.", MessageType.Info);
            
            // Additional helper buttons if needed
            if (GUILayout.Button("Reset Bomb to Default Values"))
            {
                Undo.RecordObject(item, "Reset Bomb Values");
                
                // Reset to default values
                item.bombType = BombType.Frag;
                item.fuseTime = 3f;
                item.blastRadius = 5f;
                item.bombDamage = 50;
                item.dotDuration = 5f;
                item.blindDuration = 2f;
                
                EditorUtility.SetDirty(item);
            }
        }
    }
}
#endif