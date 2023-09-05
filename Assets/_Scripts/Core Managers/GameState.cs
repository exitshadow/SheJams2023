using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Class that syncs the yarn and scriptable assets game states.
/// Add as component to a GameObject to track 
/// </summary>
public class GameState : MonoBehaviour
{
    [SerializeField] private InMemoryVariableStorage yarnState;
    [SerializeField] private GameVariablesStorage gameState;

    private void Start()
    {
        SetValuesFromGameToYarn();
    }

    public string MissionStatusText
    {
        get { return gameState.missionStatusText; }
        set { gameState.missionStatusText = value; }
    }

    public void SetValuesFromGameToYarn()
    {
        foreach (var gameVar in gameState.boolVariables)
        {
            if (yarnState.TryGetValue(gameVar.Key, out bool b))
                yarnState.SetValue(gameVar.Key, gameVar.Value);

        }

        foreach (var gameVar in gameState.floatVariables)
        {
            if (yarnState.TryGetValue(gameVar.Key, out float f))
                yarnState.SetValue(gameVar.Key, gameVar.Value);
        }

        foreach(var gameVar in gameState.stringVariables)
        {
            if (yarnState.TryGetValue(gameVar.Key, out string s))
                yarnState.SetValue(gameVar.Key, gameVar.Value);
        }
            
    }

    public void SetValuesFromYarnToGame()
    {
        (var floats, var strings, var bools) = yarnState.GetAllVariables();

        foreach (var yarnBool in bools)
        {
            if (gameState.boolVariables.TryGetValue(yarnBool.Key, out bool b))
            {
                gameState.boolVariables[yarnBool.Key] = b;
            }
        }

        
    }

    public void SetValue(string key, bool value)
    {
        if (gameState.boolVariables.TryGetValue(key, out bool result))
        {
            yarnState.SetValue(key, value);
            gameState.boolVariables[key] = value;
        }
        else Debug.Log($"No variable with the key {key} has been found in yarn storage");
    }

    public void SetValue(string key, string value)
    {
        if (gameState.stringVariables.TryGetValue(key, out string result))
        {
            yarnState.SetValue(key, value);
            gameState.stringVariables[key] = value;
        }
        else Debug.Log($"No variable with the key {key} has been found in yarn storage");
    }

    public void SetValue(string key, float value)
    {
        if (gameState.floatVariables.TryGetValue(key, out float result))
        {
            yarnState.SetValue(key, value);
            gameState.floatVariables[key] = value;
        }
        else Debug.Log($"No variable with the key {key} has been found in yarn storage");
    }
}
