using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameVariablesStorage))]
public class GameVariablesStorageEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("You need to use the same nomenclature as YARN! Please start all varables names with $ and use the same names that are referenced in the yarn scripts.", MessageType.Info);

        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("You need to generate the dictionaries for the game to be able to run. Generating the dictionaries will clear any value that has been set during play time and assign the values at the state defined in this asset. This has to be done every time you add new state variables to the project.", MessageType.Info);

        if (GUILayout.Button("Generate Dictionaries"))
        {
            ((GameVariablesStorage)target).GenerateDictionaries();
        }


    }
}
