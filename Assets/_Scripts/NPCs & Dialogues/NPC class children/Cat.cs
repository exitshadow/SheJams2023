using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cat : NPC
{
    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (!gameManager.HasEncounteredCat())
        {
            currentDialogue = dialogueData.questStartingDialogueSegments;
            gameManager.ConfirmCatEncounter();
        }
        else if (!gameManager.HasFoundTuna())
        {
            currentDialogue = dialogueData.questWaitingDialogueSegments;
        }
        else if (gameManager.HasFoundTuna() && !gameManager.HasFedCat())
        {
            currentDialogue = dialogueData.questEndingDialogueSegments;
            gameManager.ConfirmCatIsFed();
        }
        else
        {
            currentDialogue = dialogueData.questWaitingDialogueSegments;
        }

        return currentDialogue;
    }
}
