using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunaCan : NPC
{
    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (!gameManager.HasFoundTuna())
        {
            currentDialogue = dialogueData.questStartingDialogueSegments;
            gameManager.ConfirmTunaCansFound();
        }
        else
        {
            currentDialogue = dialogueData.questEndingDialogueSegments;
        }

        return currentDialogue;

    }

    public override void InjectDialogue()
    {
        if (QueuedDialogue.Count == 0)
        {
            uiManager.CloseDialogueBox();
            isPlayingDialogue = false;
            gameObject.SetActive(false);
            return;
        }

        NPCDialogueAsset.DialogueSegment currentDialogue = QueuedDialogue.Dequeue();

        if (!isPlayingDialogue)
        {
            uiManager.OpenDialogueBox();
            isPlayingDialogue = true;
        }

        uiManager.InjectDialogueLine(   currentDialogue.speakerName,
                                        currentDialogue.dialogueText    );
    }
}
    
