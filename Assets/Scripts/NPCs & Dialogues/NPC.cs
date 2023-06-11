using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// abstract class NPC of which they inherit common behaviour such as the lines of the dialogue and the management of their animations
/// </summary>
public class NPC : MonoBehaviour
{
    [SerializeField]
    protected NPCDialogueAsset npcDialogueData;
    protected Queue<NPCDialogueAsset.DialogueSegment> dialogueQueued = new Queue<NPCDialogueAsset.DialogueSegment>();
    protected GameManager gameManager;
    protected UIManager uiManager;
    // subscription to a delegate that listens to player interacting actions ?

    protected Animator npcAnimator;
    protected Collider npcTriggerCollider;
    protected NavMeshAgent npcNavMeshAgent;
    private bool isPlayingDialogue;

    public void FetchDialogue(){ // from the NPCDialogueAsset
        dialogueQueued.Clear();

    }

    public void InjectDialogue(){ // into the UIManager
        if (dialogueQueued.Count == 0){
            // End Dialogue
            return;
        }
        NPCDialogueAsset.DialogueSegment currentDialogue = dialogueQueued.Dequeue();
        // send currentDialogue to UI Manager
        // play "talking" sound?
    }

    public virtual void InitializeDialogue(NPCDialogueAsset _dialogue){ // specifics are defined by the inherited class

        // set npcAnimator to talking

    }
}
