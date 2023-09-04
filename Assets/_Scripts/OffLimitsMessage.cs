using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class OffLimitsMessage : NPC
{
    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue()
    {
        return dialogueData.questStartingDialogueSegments;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if(pc.currentInteractingNPC == null) pc.currentInteractingNPC = this;
            Debug.Log("player slot occupied");

            if (!isPlayingDialogue)
            {
                FetchDialogue(FindCurrentDialogue());
                uiManager.HideInteractionButton();
            }

            ContinueDialogue();
        }
    }
}
