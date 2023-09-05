using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyNeighbour : NPC
{
    [Header("Butterfly Neighbour special fields")]
    [SerializeField] private Transform targetHand;
    [SerializeField] private GameObject treeBranchObject;

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


    protected override void GetOldDialogueLine()
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

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        animator.SetLayerWeight(2, 0);
    }
}
