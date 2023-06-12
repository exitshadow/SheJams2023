using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vet : NPC
{
    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (gameManager.HasFedCat())
        {
            currentDialogue = dialogueData.questStartingDialogueSegments;
        }
        else
        {
            currentDialogue = dialogueData.questWaitingDialogueSegments;
        }

        return currentDialogue;
    }
}
