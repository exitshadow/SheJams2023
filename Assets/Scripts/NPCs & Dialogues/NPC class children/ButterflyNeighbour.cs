using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyNeighbour : NPC
{
    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (!gameManager.HasVisitedNeighbour())
        {
            currentDialogue = dialogueData.questStartingDialogueSegments;
            gameManager.ConfirmVisitNeighbourFirstTime();
        }
        else if (gameManager.HasFoundTheButterflyBranch() && !gameManager.HasCapturedAllButterfies())
        {
            currentDialogue = dialogueData.questProgressingDialogueSegments;
            gameManager.ConfirmButterflyCapture();
        }
        else if(gameManager.HasCapturedAllButterfies())
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
