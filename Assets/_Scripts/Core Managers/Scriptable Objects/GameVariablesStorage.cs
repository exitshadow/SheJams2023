using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Handiman Data Assets/Variables Storage Asset")]
public class GameVariablesStorage : ScriptableObject
{
    public string missionStatusText;

    [Header("Variables Lists")]
    [SerializeField] private List<BoolVariable> boolValues;
    [SerializeField] private List<StringVariable> stringValues;
    [SerializeField] private List<FloatVariable> floatValues;

    // non serialized fields
    public Dictionary<string, bool> boolVariables = new Dictionary<string, bool>();
    public Dictionary<string, float> floatVariables = new Dictionary<string, float>();
    public Dictionary<string, string> stringVariables = new Dictionary<string, string>();

    public void GenerateDictionaries()
    {
        boolVariables.Clear();
        floatVariables.Clear();
        stringVariables.Clear();

        foreach (BoolVariable boolVal in boolValues)
        {
            boolVariables.Add(boolVal.Key, boolVal.Value);
        }

        foreach (StringVariable stringVal in stringValues)
        {

            stringVariables.Add(stringVal.Key, stringVal.Value);
        }

        foreach (FloatVariable floatVal in floatValues)
        {
            floatVariables.Add(floatVal.Key, floatVal.Value);
        }

        Debug.Log("Dictionaries have been generated");
    }

    [Serializable]
    public struct BoolVariable
    {
        public string Key;
        public bool Value;
    }

    [Serializable]
    public struct FloatVariable
    {
        public string Key;
        public float Value;
    }

    [Serializable]
    public struct StringVariable
    {
        public string Key;
        public string Value;
    }

}
