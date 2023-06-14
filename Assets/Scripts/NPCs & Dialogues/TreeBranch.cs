using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBranch : NPC
{
    [Tooltip("Has to be the one with the renderer on it!")]
    [SerializeField] private GameObject treeBranchObject;
    [SerializeField] private Transform targetHand;
    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (gameManager.HasFedCat())
        {
            currentDialogue = dialogueData.questProgressingDialogueSegments;
            gameManager.ConfirmFoundTreeBranch();
        }
        else
        {
            currentDialogue = dialogueData.questWaitingDialogueSegments;
        }

        return currentDialogue;
    }

    public override void InjectDialogue()
    {
        if (QueuedDialogue.Count == 0)
        {
            uiManager.CloseDialogueBox();
            isPlayingDialogue = false;

            // todo
            // abstract this bit in another virtual method in the parent class
            if (gameManager.HasFoundTheButterflyBranch())
            {
                treeBranchObject.transform.SetParent(targetHand);
                treeBranchObject.transform.localRotation = Quaternion.identity;
                treeBranchObject.transform.localPosition = new Vector3(0, 0, -.1f);
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

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (gameManager.HasFoundTheButterflyBranch())
            gameObject.SetActive(false);
    }
}
