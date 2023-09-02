using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Yarn.Unity;

/// <summary>
/// abstract class NPC of which they inherit common behaviour such as the lines of the dialogue and the management of their animations
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class NPC : MonoBehaviour
{
    #region member fields
    #region global references
    [Header("Global References")]
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected UIManager uiManager;
    [SerializeField] protected CutsceneManager cutsceneManager;

    #endregion

    [Header("Yarn References & Settings")]
    [SerializeField] protected DialogueRunner dialogueRunner;
    [SerializeField] protected bool useYarn;


    [Header("Old Dialogue System References")]
    [SerializeField] protected NPCDialogueAsset dialogueData;

    #region inner references
    protected Animator animator;
    protected Collider triggerCollider;
    protected NavMeshAgent navMeshAgent;
    #endregion

    #region dialogue tracking
    [HideInInspector] public bool isPlayingDialogue = false;

    protected Queue<NPCDialogueAsset.DialogueSegment> QueuedDialogue = new Queue<NPCDialogueAsset.DialogueSegment>();
    
    #endregion
    #endregion

    #region dialogue methods

    /// <summary>
    /// Finds the asked dialogue segment and drops all the lines into a queue for further usage.
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

    protected abstract List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogue();

    /// <summary>
    /// Dequeues the first dialogue line from the current lines in queue and sends it to the UI Manager.
    /// </summary>
    public virtual void InjectDialogue()
    {
        if (!useYarn)
        {
            if (QueuedDialogue.Count == 0)
            {
                uiManager.CloseDialogueBox();
                // camera manager switch camera (todo)
                isPlayingDialogue = false;
                return;
            }

            NPCDialogueAsset.DialogueSegment currentDialogue = QueuedDialogue.Dequeue();

            if (!isPlayingDialogue)
            {
                uiManager.OpenDialogueBox();
                // camera manager switch camera (todo)
                isPlayingDialogue = true;
            }

            uiManager.InjectDialogueLine(   currentDialogue.speakerName,
                                            currentDialogue.dialogueText    );
        }
        else
        {
        }
        Debug.Log("Requesting View advancement");
        dialogueRunner.dialogueViews[0].UserRequestedViewAdvancement();
    }

    /// <summary>
    /// To be accessed through Unity Events!
    /// </summary>
    public virtual void Talk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (useYarn)
            {
                if (!dialogueRunner.IsDialogueRunning)
                {
                    dialogueRunner.StartDialogue("DadQuest1");
                    Debug.Log("starting dialogue with yarn");
                    InjectDialogue();
                }
                else
                {
                    Debug.Log("entering dialogue running condition");
                    InjectDialogue();
                } 
            }
            else
            {
                if (!isPlayingDialogue)
                {
                    FetchDialogue(FindCurrentDialogue());
                    uiManager.HideInteractionButton();
                }

                InjectDialogue();
            }
        }
    }

    public virtual void InitializeDialogue()
    {

    }

    public virtual void ForceTalk()
    {
        if (!isPlayingDialogue)
        {
            FetchDialogue(FindCurrentDialogue());
            uiManager.HideInteractionButton();
        }

        InjectDialogue();
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
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.ShowInteractionButton();
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc.currentInteractingNPC == null) pc.currentInteractingNPC = this;
            // target group = this; (todo)
            Debug.Log("player slot occupied");
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.HideInteractionButton();
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.currentInteractingNPC = null;
        }
    }

    #endregion
}
