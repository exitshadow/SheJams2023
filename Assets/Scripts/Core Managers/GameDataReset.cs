using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataReset : MonoBehaviour
{
    [SerializeField] GameStateAsset gameState;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        gameState.hasSpokenToDadAtHome = false;
        gameState.hasReceivedNeighbourFirstCall = false;
        gameState.hasVisitedNeighbour = false;
        gameState.hasFoundTreeBranch = false;

        gameState.hasEncounteredCat = false;
        gameState.hasFoundTuna = false;
        gameState.hasSpokenToVet = false;
        gameState.hasFedCat = false;

        gameState.hasResetDadsComputer = false;
    }
}
