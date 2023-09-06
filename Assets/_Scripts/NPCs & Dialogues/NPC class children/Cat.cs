using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cat : NPC
{
    protected override List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogueOldSystem()
    {
        List<NPCDialogueAsset.DialogueSegment> currentDialogue;

        if (!gameManager.HasEncounteredCat())
        {
            currentDialogue = dialogueData.questStartingDialogueSegments;
            gameManager.ConfirmCatEncounter();
        }
        else if (!gameManager.HasFoundTuna())
        {
            currentDialogue = dialogueData.questWaitingDialogueSegments;
        }
        else if (gameManager.HasFoundTuna() && !gameManager.HasFedCat())
        {
            currentDialogue = dialogueData.questEndingDialogueSegments;
            gameManager.ConfirmCatIsFed();
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
            if (gameManager.HasFedCat() && cutsceneManager != null)
            {
                cutsceneManager.PlayCatCutscene();
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
}
