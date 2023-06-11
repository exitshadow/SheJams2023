using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing the Game State that will be used by the GameManager script in a scene.
/// It allows for persistent data within an execution run.
/// </summary>
[CreateAssetMenu(menuName = "Game State Asset", fileName = "Game State Asset")]
public class GameStateAsset : ScriptableObject
{
    [Header("Game State Tracking Variables — Exposed for debugging purposes only")]
    [Header("Butterfly Neighbour Quest")]
    public bool hasSpokenToDadAtHome = false;
    public bool hasReceivedNeighbourFirstCall = false;
    public bool hasVisitedNeighbour = false;
    public bool hasFoundTreeBranch = false;
    public bool hasCapturedButterflies = false;
    [Header("Cat Cans Quest")]
    public bool hasEncounteredCat = false;
    public bool hasFoundTuna = false;
    public bool hasFedCat = false;
    [Header("Dad Quest")]
    public bool hasResetDadsComputer = false;
}
