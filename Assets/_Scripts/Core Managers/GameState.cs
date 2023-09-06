using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;

/// <summary>
/// Class that syncs the yarn and scriptable assets game states.
/// Add as component to a GameObject to track 
/// </summary>
public class GameState : MonoBehaviour
{
    [SerializeField] private InMemoryVariableStorage yarnState;
    [SerializeField] private GameVariablesStorage gameState;

    private SceneLoader sceneLoader;

    public event Action onSetValuesFromGameToYarn;
    public event Action onSetValuesFromYarnToGame;
    public event Action onSetValue;

    [HideInInspector] public List<string> variableNames = new List<string>();
    [HideInInspector] public List<string> variableValues = new List<string>();

    private void OnEnable()
    {
        onSetValue += OnDataChange;
        onSetValuesFromGameToYarn += OnDataChange;
        onSetValuesFromYarnToGame += OnDataChange;
    }

    void OnDisable()
    {
        onSetValue -= OnDataChange;
        onSetValuesFromGameToYarn -= OnDataChange;
        onSetValuesFromYarnToGame -= OnDataChange;
    }

    void Awake()
    {
        sceneLoader = FindFirstObjectByType<SceneLoader>();
        sceneLoader.onLoadScene += SetValuesFromYarnToGame;
    }

    private void Start()
    {
        SetValuesFromGameToYarn();
        RefreshVariablesViewingLists();
    }

    private void RefreshVariablesViewingLists()
    {
        variableNames.Clear();
        variableValues.Clear();
        foreach (var boolVar in gameState.boolVariables)
        {
            variableValues.Add(boolVar.Key);
            variableNames.Add(boolVar.Value.ToString());
        }
    }

    public string MissionStatusText
    {
        get { return gameState.missionStatusText; }
        set { gameState.missionStatusText = value; }
    }

    public void SetValuesFromGameToYarn()
    {
        Debug.LogWarning("Updated values from game to yarn:");

        yarnState.SetAllVariables(gameState.floatVariables, gameState.stringVariables, gameState.boolVariables);

        onSetValuesFromGameToYarn?.Invoke();
    }

    public void SetValuesFromYarnToGame()
    { 
        Debug.LogWarning("Updated values from yarn to game:");

        (var floats, var strings, var bools) = yarnState.GetAllVariables();

        foreach (var yarnBool in bools)
        {
            if (gameState.boolVariables.TryGetValue(yarnBool.Key, out bool b))
            {
                Debug.Log("gamestate bool variables exist with key");
                gameState.boolVariables[yarnBool.Key] = yarnBool.Value;
            }
            else gameState.boolVariables.Add(yarnBool.Key, yarnBool.Value);

            Debug.Log($"{yarnBool.Key} - yarn: {yarnBool.Value} game: {gameState.boolVariables[yarnBool.Key]}");
        }

        foreach (var yarnFloat in floats)
        {
            if (gameState.floatVariables.TryGetValue(yarnFloat.Key, out float f))
            {
                gameState.floatVariables[yarnFloat.Key] = yarnFloat.Value;
            }
            else gameState.floatVariables.Add(yarnFloat.Key, yarnFloat.Value);
        }

        foreach (var yarnString in strings)
        {
            if (gameState.stringVariables.TryGetValue(yarnString.Key, out string s))
            {
                gameState.stringVariables[yarnString.Key] = yarnString.Value;
            }
            else gameState.stringVariables.Add(yarnString.Key, yarnString.Value);
        }
        
        onSetValuesFromYarnToGame?.Invoke();
    }

    /// <summary>
    /// Set a boolean value in both yarn and game memory
    /// </summary>
    public void SetValue(string key, bool value)
    {
        if (gameState.boolVariables.TryGetValue(key, out bool result))
        {
            yarnState.SetValue(key, value);
            gameState.boolVariables[key] = value;
            onSetValue?.Invoke();
        }
        else Debug.Log($"No variable with the key {key} has been found in yarn storage");
    }

/// <summary>
/// Set a string value in both yarn and game memory
/// </summary>
    public void SetValue(string key, string value)
    {
        if (gameState.stringVariables.TryGetValue(key, out string result))
        {
            yarnState.SetValue(key, value);
            gameState.stringVariables[key] = value;
            onSetValue?.Invoke();
        }
        else Debug.Log($"No variable with the key {key} has been found in yarn storage");
    }

    /// <summary>
    /// Set a float value in both yarn and game memory
    /// </summary>
    public void SetValue(string key, float value)
    {
        if (gameState.floatVariables.TryGetValue(key, out float result))
        {
            yarnState.SetValue(key, value);
            gameState.floatVariables[key] = value;
            onSetValue?.Invoke();
        }
        else Debug.Log($"No variable with the key {key} has been found in yarn storage");
    }

    public void OnDataChange()
    {
        RefreshVariablesViewingLists();
    }
}
