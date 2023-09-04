using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vet : NPC
{
    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (gameManager.HasFedCat() && !gameManager.HasSpokenToVet())
        {
            currentDialogue = dialogueData.questStartingDialogueSegments;
            gameManager.ConfirmSpokeToVet();
        }
        else
        {
            currentDialogue = dialogueData.questWaitingDialogueSegments;
        }

        return currentDialogue;
    }

    public override void ContinueDialogue()
    {
        if (QueuedDialogue.Count == 0)
        {
            uiManager.CloseDialogueBox();
            isPlayingDialogue = false;
            if (!gameManager.HasSpokenToVet() && cutsceneManager != null)
            {
                cutsceneManager.PlayVetLeave();
            }
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

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        animator.SetLayerWeight(2, 0);
    }
}
