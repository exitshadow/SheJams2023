using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from the NPC abstract class.
/// </summary>
public class Dad : NPC
{
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
}
