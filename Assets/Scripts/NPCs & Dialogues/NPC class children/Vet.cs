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

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        animator.SetLayerWeight(2, 0);
    }
}
