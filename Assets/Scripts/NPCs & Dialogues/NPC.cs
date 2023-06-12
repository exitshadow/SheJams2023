using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// abstract class NPC of which they inherit common behaviour such as the lines of the dialogue and the management of their animations
/// </summary>
[RequireComponent(typeof(Collider))]
public class NPC : MonoBehaviour
{
    #region member fields
    #region global references
    [Header("GlobalReferences")]
    [SerializeField] protected NPCDialogueAsset dialogueData;
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected UIManager uiManager;
    #endregion

    #region inner references
    protected Animator animator;
    protected Collider triggerCollider;
    protected NavMeshAgent navMeshAgent;
    #endregion

    #region dialogue tracking
    private bool isPlayingDialogue;
    protected Queue<NPCDialogueAsset.DialogueSegment> QueuedDialogue = new Queue<NPCDialogueAsset.DialogueSegment>();
    
    #endregion
    #endregion

    #region dialogue methods

    /// <summary>
    /// Finds the asked dialoue segment and drops all the lines into a queue for further usage.
    /// </summary>
    public void FetchDialogue(List<NPCDialogueAsset.DialogueSegment> dialogueSegment)
    {
        QueuedDialogue.Clear();
        foreach (NPCDialogueAsset.DialogueSegment dialogue in dialogueSegment)
        {
            QueuedDialogue.Enqueue(dialogue);
            Debug.Log("Queued all initial dialogue lines");
        }
    }

    /// <summary>
    /// Dequeues the first dialogue line from the current lines in queue and sends it to the UI Manager.
    /// </summary>
    public void InjectDialogue()
    {
        if (QueuedDialogue.Count == 0) return;

        NPCDialogueAsset.DialogueSegment currentDialogue = QueuedDialogue.Dequeue();

        Debug.Log(currentDialogue.speakerName + ": " + currentDialogue.dialogueText);

        // send currentDialogue to UI Manager
        uiManager.InjectDialogueLine(   currentDialogue.speakerName,
                                        currentDialogue.dialogueText    );
    }

    /// <summary>
    /// To be accessed through Unity Events!
    /// </summary>
    public virtual void GoToDialogueNewLine(InputAction.CallbackContext context)
    {
        if (context.performed) InjectDialogue();
    }

    public virtual void InitializeDialogue()
    {

    }
    #endregion

    #region unity events
    protected virtual void Awake()
    {
        FetchDialogue(dialogueData.questStartingDialogueSegments);
    }

    protected virtual void Start()
    {

    }
    #endregion

    #region trigger events
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc.currentInteractingNPC == null) pc.currentInteractingNPC = this;
            Debug.Log("player slot occupied");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.currentInteractingNPC = null;
        }
    }
    #endregion
}
