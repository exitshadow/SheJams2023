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

    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogueOldSystem()
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

    protected override void GetOldDialogueLine()
    {
        Debug.Log("Getting dialogue lines, old system");

        if (QueuedDialogue.Count == 0)
            {
                Debug.Log("closed dialogue box, old system");
                uiManager.CloseDialogueBox();
                // camera manager switch camera (todo)
                isPlayingDialogue = false;
                uiManager.currentDialogueAnchor = null;

                //! again, not implemented by yarn yet
                if (gameManager.HasResetDadsComputer())
                {
                    sceneLoader.Credits();
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

    [YarnCommand("start_credits_roll")]
    public void CreditsRoll()
    {
        sceneLoader.Credits();
    }
}
