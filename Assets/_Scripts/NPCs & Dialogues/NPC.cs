using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Yarn.Unity;
using System;

/// <summary>
/// abstract class NPC of which they inherit common behaviour such as the lines of the dialogue and the management of their animations
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class NPC : MonoBehaviour
{
    #region exposed fields
    [Header("Settings")]
    [SerializeField] protected string promptText = "Talk";

    [Header("Dialogue Anchor")]
    [SerializeField] protected Transform dialogueAnchor;

    [Header("Manager References")]
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected UIManager uiManager;
    [SerializeField] protected CutsceneManager cutsceneManager;


    [Header("Yarn References & Settings")]
    [SerializeField] protected DialogueSystem dialogueSystem;
    [SerializeField] protected DialogueRunner dialogueRunner;
    [SerializeField] protected bool useYarn;
    [SerializeField] protected string dialogueNode;


    [Header("Old Dialogue System References")]
    [SerializeField] protected NPCDialogueAsset dialogueData;
    #endregion

    public event Action<string> OnStartDialogue;

    #region component references
    protected Animator animator;
    protected Collider triggerCollider;
    protected NavMeshAgent navMeshAgent;
    #endregion


    #region dialogue tracking
    [HideInInspector] public bool isPlayingDialogue = false;
    protected Queue<NPCDialogueAsset.DialogueSegment> QueuedDialogue = new Queue<NPCDialogueAsset.DialogueSegment>();
    
    #endregion

    #region dialogue methods

    /// <summary>
    /// Finds the asked dialogue segment and drops all the lines into a queue for further usage.
    /// </summary>
    public void FetchDialogue(List<NPCDialogueAsset.DialogueSegment> dialogueSegment)
    {
        Debug.Log("Fetching dialogue lines...");
        QueuedDialogue.Clear();
        foreach (NPCDialogueAsset.DialogueSegment dialogue in dialogueSegment)
        {
            QueuedDialogue.Enqueue(dialogue);
            Debug.Log("Queued all initial dialogue lines");
        }
    }

/// <summary>
/// Method that has to be implemented by any child class. Deprecated in favour of yarn system
/// but maintained in order to make a smooth transition. 
/// </summary>
    protected abstract List<NPCDialogueAsset.DialogueSegment> FindCurrentDialogueOldSystem();

    /// <summary>
    /// Dequeues the first dialogue line from the current lines in queue and sends it to the UI Manager.
    /// </summary>
    public void ContinueDialogue()
    {
        if (useYarn) GetYarnLine();
        else  GetOldDialogueLine();
    }

    /// <summary>
    /// To be accessed through Unity Events!
    /// </summary>
    public virtual void Talk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (dialogueAnchor) uiManager.currentDialogueAnchor = dialogueAnchor;

            if (useYarn) StartYarnDialogue();
            else StartOldDialogue();

            ContinueDialogue();
        }
    }

    protected virtual void StartOldDialogue()
    {
        Debug.Log("calling old dialogue start");
        if (!isPlayingDialogue)
        {
            FetchDialogue(FindCurrentDialogueOldSystem());
            uiManager.HideInteractionButton();
            Debug.Log("old dialogue has started");
        }
    }

    protected virtual void GetOldDialogueLine()
    {
        Debug.Log("Getting dialogue lines, old system");

        if (QueuedDialogue.Count == 0)
            {
                Debug.Log("closed dialogue box, old system");
                uiManager.CloseDialogueBox();

                isPlayingDialogue = false;
                uiManager.currentDialogueAnchor = null;
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

    protected virtual void StartYarnDialogue()
    {
            if (!dialogueRunner.IsDialogueRunning)
            {
                Debug.Log("starting dialogue with yarn");

                dialogueRunner.StartDialogue(dialogueNode);
                uiManager.HideInteractionButton();
                isPlayingDialogue = true;
            }
    }

    protected virtual void GetYarnLine()
    {
        Debug.Log("Requesting View advancement");
        dialogueRunner.dialogueViews[0].UserRequestedViewAdvancement();
        uiManager.TriggerPop();
        
        if (!dialogueRunner.IsDialogueRunning)
        {
            uiManager.currentDialogueAnchor = null;
            isPlayingDialogue = false;
        }
    }

    public virtual void ForceTalk()
    {
        if (!isPlayingDialogue)
        {
            FetchDialogue(FindCurrentDialogueOldSystem());
            uiManager.HideInteractionButton();
        }

        ContinueDialogue();
    }
    #endregion

    #region unity events
    protected virtual void Awake()
    {
        FetchDialogue(dialogueData.questStartingDialogueSegments);

        if (!dialogueSystem)
            dialogueSystem = FindObjectOfType<DialogueSystem>();

        if (dialogueSystem)
            dialogueSystem.AddNPC(this);
    }

    protected virtual void OnEnable()
    {
        if (dialogueRunner)
            (dialogueRunner.dialogueViews[0] as LineView).requestInterrupt += OnRequestInterrupt;
    }

    protected virtual void OnRequestInterrupt()
    {
        uiManager.TriggerPop();
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
            if (dialogueAnchor) uiManager.currentDialogueAnchor = dialogueAnchor;

            uiManager.ShowInteractionButton(promptText);
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc.currentInteractingNPC == null) pc.currentInteractingNPC = this;

            Debug.Log("player slot occupied");
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.HideInteractionButton();
            uiManager.currentDialogueAnchor = null;
            
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.currentInteractingNPC = null;
        }
    }

    #endregion
}
