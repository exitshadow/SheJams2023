using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBranch : NPC
{
    [Tooltip("Has to be the one with the renderer on it!")]
    [SerializeField] private GameObject treeBranchObject;
    [SerializeField] private Transform targetHand;
    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogueOldSystem()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (!gameManager.HasFoundTheButterflyBranch())
        {
            currentDialogue = dialogueData.questProgressingDialogueSegments;
            gameManager.ConfirmFoundTreeBranch();

            //! this will need a yarn implementation!
            PassTreeBranchToPlayer();
        }
        else
        {
            currentDialogue = dialogueData.questWaitingDialogueSegments;
        }

        return currentDialogue;
    }

    private void PassTreeBranchToPlayer()
    {
        treeBranchObject.transform.SetParent(targetHand);
        treeBranchObject.transform.localRotation = Quaternion.identity;
        treeBranchObject.transform.localPosition = new Vector3(0, 0, -.1f);
    }

    protected override void OnTriggerExit(Collider other)
    {
        Debug.Log("exiting branch trigger");
        base.OnTriggerExit(other);
    }
}
