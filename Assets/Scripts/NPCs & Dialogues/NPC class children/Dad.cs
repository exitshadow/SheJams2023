using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from the NPC abstract class.
/// </summary>
public class Dad : NPC
{

    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (!gameManager.DidSpeakToDadFirstTime())
        {
            currentDialogue = dialogueData.questStartingDialogueSegments;
            gameManager.ConfirmTalkDadAtHome();
        }
        else if (   gameManager.HasCapturedAllButterfies()
                && !gameManager.HasResetDadsComputer()      )
        {
            currentDialogue = dialogueData.questProgressingDialogueSegments;
            gameManager.ConfirmResettingDadsComputer();
        }
        else if (gameManager.HasResetDadsComputer())
        {
            currentDialogue = dialogueData.questEndingDialogueSegments;
        }
        else
        {
            currentDialogue = dialogueData.questWaitingDialogueSegments;
        }

        return currentDialogue;

    }
}
