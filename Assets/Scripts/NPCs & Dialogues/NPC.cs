using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// abstract class NPC of which they inherit common behaviour such as the lines of the dialogue and the management of their animations
/// </summary>
public class NPC : MonoBehaviour
{
    [Header("GlobalReferences")]
    [SerializeField] protected NPCDialogueAsset dialogueData;
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected UIManager uiManager;
    
    
    // subscription to a delegate that listens to player interacting actions ?

    #region inner references
    protected Animator animator;
    protected Collider triggerCollider;
    protected NavMeshAgent navMeshAgent;
    #endregion

    #region state tracking
    private bool isPlayingDialogue;
    protected Queue<NPCDialogueAsset.DialogueSegment> QueuedDialogue = new Queue<NPCDialogueAsset.DialogueSegment>();
    #endregion

    public void FetchDialogue(List<NPCDialogueAsset.DialogueSegment> dialogueSegment)
    {
        QueuedDialogue.Clear();
        foreach (NPCDialogueAsset.DialogueSegment dialogue in dialogueSegment)
        {
            QueuedDialogue.Enqueue(dialogue);
            Debug.Log("fetched dialogue line");
        }
    }

    public void InjectDialogue()
    {
        if (QueuedDialogue.Count == 0) return;

        NPCDialogueAsset.DialogueSegment currentDialogue = QueuedDialogue.Dequeue();

        Debug.Log(currentDialogue.speakerName + ": " + currentDialogue.dialogueText);
        // send currentDialogue to UI Manager
    }

    public virtual void InitializeDialogue(NPCDialogueAsset _dialogue)
    {

    }

    protected virtual void Awake()
    {
        FetchDialogue(dialogueData.questStartingDialogueSegments);
    }

    protected virtual void Start()
    {

    }
}
