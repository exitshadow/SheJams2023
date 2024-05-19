using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InanimatedNPC : NPC
{
    [Header("Generic Inanimate Object Options")]
    [SerializeField] private bool startDialogueOnContact;

    public override void OnTriggerEnter(Collider other)
    {
        if (!startDialogueOnContact) base.OnTriggerEnter(other);
        else
        {
            Debug.Log("entering inanimated NPC trigger");
            player = other.GetComponent<PlayerController>();

            if (player)
            {
                if (dialogueAnchor) dialogueUI.currentDialogueAnchor = dialogueAnchor;
                else dialogueUI.currentDialogueAnchor = dialogueUI.playerDialogueAnchor;

                OccupyPlayerSlot();
                RequestDialogueStart();
                player.StopWalkingAnimation();
            }
        }
    }
}
