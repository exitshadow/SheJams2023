using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Game Manager is a script that interfaces with a Game State Asset.
/// It is meant to be reached by other scripts in the scene that inform it
/// of the changes that have to be done. Modification of the asset is only
/// done by the GameManager.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameStateAsset gameStateData;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private CutsceneManager cutsceneManager;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    #region dad’s quest
    public bool DidSpeakToDadFirstTime()
    {
        return gameStateData.hasSpokenToDadAtHome;
    }

    public bool HasCapturedAllButterfies()
    {
        return gameStateData.hasCapturedButterflies;
    }

    public bool HasResetDadsComputer()
    {
        return gameStateData.hasResetDadsComputer;
    }
    #endregion

    #region cat’s and neighbour quest
    public bool HasVisitedNeighbour()
    {
        return gameStateData.hasVisitedNeighbour;
    }

    public bool HasFoundTheButterflyBranch()
    {
        return gameStateData.hasFoundTreeBranch;
    }

    public bool HasEncounteredCat()
    {
        return gameStateData.hasEncounteredCat;
    }

    public bool HasFoundTuna()
    {
        return gameStateData.hasFoundTuna;
    }

    public bool HasFedCat()
    {
        return gameStateData.hasFedCat;
    }

    public bool HasSpokenToVet()
    {
        return gameStateData.hasSpokenToVet;
    }
    #endregion

    /// <summary>
    /// To be called by a Unity Event after the first dad conversation.
    /// </summary>
    public void ConfirmTalkDadAtHome()
    {
        gameStateData.hasSpokenToDadAtHome = true;
        uiManager.ChangeMissionPrompt("Go to speak to the neighbor.");
    }

    public void ConfirmResettingDadsComputer()
    {
        gameStateData.hasResetDadsComputer = true;
        uiManager.ChangeMissionPrompt("Like an subscribe!!");
    }

    /// <summary>
    /// To be called by a Unity Event after the first neighbour conversation.
    /// </summary>
    public void ConfirmReceptionNeighbourFirstCall()
    {
        gameStateData.hasReceivedNeighbourFirstCall = true;
        uiManager.ChangeMissionPrompt("Dad wanted to ask for something.");
    }

    /// <summary>
    /// To be called by a Unity Event after having spoken to the neighbour in front of her house.
    /// </summary>
    public void ConfirmVisitNeighbourFirstTime()
    {
        gameStateData.hasVisitedNeighbour = true;
        uiManager.ChangeMissionPrompt("Go find the butterfly tree branch.");
    }

    /// <summary>
    /// To be called by a Unity Event after having found the tree branch.
    /// </summary>
    public void ConfirmFoundTreeBranch()
    {
        gameStateData.hasFoundTreeBranch = true;
        uiManager.ChangeMissionPrompt("Go back to the neighbor bring her the butterfly tree branch.");
    }

    /// <summary>
    /// To be called by a Unity Event after having captured the butterflies.
    /// </summary>
    public void ConfirmButterflyCapture()
    {
        gameStateData.hasCapturedButterflies = true;
        uiManager.ChangeMissionPrompt("Rembember dad needed you with the computer.");
    }

    /// <summary>
    /// To be called by a Unity Event after having found the cat blocking the alley.
    /// </summary>
    public void ConfirmCatEncounter()
    {
        gameStateData.hasEncounteredCat = true;
        uiManager.ChangeMissionPrompt("Find something for the cat to eat! like tuna!");
    }

    /// <summary>
    /// To be called by a Unity Event after having found tuna cans.
    /// </summary>
    public void ConfirmTunaCansFound()
    {
        gameStateData.hasFoundTuna = true;
        uiManager.ChangeMissionPrompt("Find the cat to give it tuna!");
    }

    /// <summary>
    /// To be called by a Unity Event after having fed the cat.
    /// </summary>
    public void ConfirmCatIsFed()
    {
        gameStateData.hasFedCat = true;
        uiManager.ChangeMissionPrompt("Oh right, the butterfly tree.");
    }

    /// <summary>
    /// To be called by a Unity Event after having been scolded by the veterinarian.
    /// </summary>
    public void ConfirmSpokeToVet()
    {
        gameStateData.hasSpokenToVet = true;
    }

}
