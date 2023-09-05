using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Handiman Data Assets/Variables Storage Asset")]
public class GameVariablesStorage : ScriptableObject
{
    public string missionStatusText;

    [Header("Variables Lists")]
    public Dictionary<string, bool> boolVariables;
    public Dictionary<string, float> floatVariables;
    public Dictionary<string, string> stringVariables;
}
