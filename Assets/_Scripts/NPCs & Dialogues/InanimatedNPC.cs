using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InanimatedNPC : NPC
{
    [Header("Generic Inanimate Object Options")]
    [SerializeField] private bool startDialogueOnContact;

    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogueOldSystem()
    {
        return dialogueData.questStartingDialogueSegments;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!startDialogueOnContact) base.OnTriggerEnter(other);
        else
        {
            Debug.Log("entering inanimated NPC trigger");
            player = other.GetComponent<PlayerController>();

            if (player)
            {
                Debug.Log("entering object is player");
                OccupyPlayerSlot();
                RequestDialogueStart();
                player.StopWalkingAnimation();
            }
        }
    }
}
