using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Inherits from the NPC abstract class.
/// </summary>
public class Dad : NPC
{
    [Header("Dad Character Specific References")]
    [SerializeField] private SceneLoader sceneLoader;

    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (!gameManager.DidSpeakToDadFirstTime())
        {
            currentDialogue = dialogueData.questStartingDialogueSegments;
            gameManager.ConfirmTalkDadAtHome();
        }
        else if (   gameManager.HasCapturedAllButterfies()
                && !gameManager.HasResetDadsComputer()      )
        {
            currentDialogue = dialogueData.questProgressingDialogueSegments;
            gameManager.ConfirmResettingDadsComputer();
        }
        else if (gameManager.HasResetDadsComputer())
        {
            currentDialogue = dialogueData.questEndingDialogueSegments;
        }
        else
        {
            currentDialogue = dialogueData.questWaitingDialogueSegments;
        }

        return currentDialogue;

    }

    public override void InjectDialogue()
    {
        if (!useYarn)
        {
            if (QueuedDialogue.Count == 0)
            {
                uiManager.CloseDialogueBox();
                isPlayingDialogue = false;
            if (gameManager.HasResetDadsComputer()) sceneLoader.Credits();
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
        else
        {
            Debug.Log("Requesting View advancement");
            dialogueRunner.dialogueViews[0].UserRequestedViewAdvancement();
            
            
            //uiManager.TriggerPop();

            if (!dialogueRunner.IsDialogueRunning) uiManager.currentDialogueAnchor = null;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        animator.SetLayerWeight(2, 0);
    }
}
