using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameState))]
public class GameStateEditor : Editor
{
    #region Custom Serialized Properties
    SerializedProperty varNames;
    SerializedProperty varValues;

    #endregion

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }

    private void OnEnable()
    {
        ((GameState)target).onSetValue += OnDataChange;
        ((GameState)target).onSetValuesFromGameToYarn += OnDataChange;
        ((GameState)target).onSetValuesFromYarnToGame += OnDataChange;

        FetchProperties();
    }

    private void OnDisable()
    {
        ((GameState)target).onSetValue -= OnDataChange;
        ((GameState)target).onSetValuesFromGameToYarn -= OnDataChange;
        ((GameState)target).onSetValuesFromYarnToGame -= OnDataChange;
    }

    private void OnDataChange()
    {
        FetchProperties();
    }

    private void FetchProperties()
    {
        varNames = serializedObject.FindProperty("variableNames");
        varValues = serializedObject.FindProperty("variableValues");
    }

}
