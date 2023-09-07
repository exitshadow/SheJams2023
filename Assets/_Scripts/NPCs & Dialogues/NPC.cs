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
    [SerializeField] protected bool useInteractionPrompt = true;
    [SerializeField] protected string promptText = "Talk";
    [SerializeField] protected Transform dialogueAnchor;

    [Header("Yarn Settings")]
    [SerializeField] protected DialogueRunner dialogueRunner;
    [SerializeField] protected bool useYarn;
    [SerializeField] protected string dialogueNode;
    protected DialogueSystem dialogueSystem;

    [Header("NPC Events")]
    public UnityEvent onDialogueStarted;

    [Header("Manager References")]
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected UIManager uiManager;
    protected CutsceneManager cutsceneManager;

    [Header("Old Dialogue System References")]
    [SerializeField] protected NPCDialogueAsset dialogueData;
    #endregion

    public event Action<string> onDialogueRequest;


    #region component references
    protected Collider triggerCollider;
    protected NavMeshAgent navMeshAgent;
    protected PlayerController player;
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
        // Debug.Log("Fetching dialogue lines...");
        QueuedDialogue.Clear();
        foreach (NPCDialogueAsset.DialogueSegment dialogue in dialogueSegment)
        {
            QueuedDialogue.Enqueue(dialogue);
            //Debug.Log("Queued all initial dialogue lines");
        }
    }

/// <summary>
/// Method that has to be implemented by any child class. Deprecated in favour of yarn system
/// but maintained in order to make a smooth transition.
/// </summary>
    [Obsolete("Please use yarn methods instead")]
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
            RequestDialogueStart();
        }
    }

    protected void RequestDialogueStart()
    {
        // todo for future integration with DialogueSystem
        onDialogueRequest?.Invoke(dialogueNode);
        // todo end

        if (dialogueAnchor) uiManager.currentDialogueAnchor = dialogueAnchor;

        if (useYarn) StartYarnDialogue();
        else StartOldDialogue();

        ContinueDialogue();
    }

    protected virtual void StartOldDialogue()
    {
        if (!isPlayingDialogue)
        {
            FetchDialogue(FindCurrentDialogueOldSystem());
            uiManager.HideInteractionButton();
            //Debug.Log("old dialogue has started");
        }
    }

    [Obsolete("to replace by yarn system")]
    protected virtual void GetOldDialogueLine()
    {
        //Debug.Log("Getting dialogue lines, old system");

        if (QueuedDialogue.Count == 0)
            {
                //Debug.Log("closed dialogue box, old system");
                uiManager.CloseDialogueBox();

                isPlayingDialogue = false;
                uiManager.currentDialogueAnchor = null;
                return;
            }

            NPCDialogueAsset.DialogueSegment currentDialogue = QueuedDialogue.Dequeue();

            if (!isPlayingDialogue)
            {
                onDialogueStarted?.Invoke();
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
                //Debug.Log("starting dialogue with yarn");
                onDialogueStarted?.Invoke();
                dialogueRunner.StartDialogue(dialogueNode);
                uiManager.HideInteractionButton();
                isPlayingDialogue = true;
            }
    }

    protected virtual void GetYarnLine()
    {
        //Debug.Log("Requesting View advancement");
        dialogueRunner.dialogueViews[0].UserRequestedViewAdvancement();
        uiManager.TriggerPop();
        
        if (!dialogueRunner.IsDialogueRunning)
        {
            uiManager.currentDialogueAnchor = null;
            isPlayingDialogue = false;
        }
    }
    #endregion

    #region unity events
    protected virtual void Awake()
    {
        if (!useYarn) FetchDialogue(dialogueData.questStartingDialogueSegments);

        if (!dialogueSystem)
            dialogueSystem = FindObjectOfType<DialogueSystem>();

        if (dialogueSystem)
            dialogueSystem.AddNPC(this);
    }

    protected virtual void OnEnable()
    {
        if (dialogueRunner)
        {
            (dialogueRunner.dialogueViews[0] as LineView).requestInterrupt += OnRequestInterrupt;
            dialogueRunner.onDialogueComplete.AddListener(ClearPlayerSlot);
        }
    }
    #endregion

    #region trigger events
    public virtual void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PlayerController>();

        if (player)
        {
            if (dialogueAnchor) uiManager.currentDialogueAnchor = dialogueAnchor;
            if (useInteractionPrompt) uiManager.ShowInteractionButton(promptText);
            OccupyPlayerSlot();
        }
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.HideInteractionButton();
            uiManager.currentDialogueAnchor = null;

            ClearPlayerSlot();
        }
    }

    [YarnCommand("clear_player_slot")]
    public void ClearPlayerSlot()
    {
        if (player) player.currentInteractingNPC = null;
    }

    #endregion

    protected void OccupyPlayerSlot()
    {
        if (player) player.currentInteractingNPC = this;
    }

    protected virtual void OnRequestInterrupt()
    {
        uiManager.TriggerPop();
    }

    //! pending review
    public virtual void ForceTalk()
    {
        if (!isPlayingDialogue)
        {
            FetchDialogue(FindCurrentDialogueOldSystem());
            uiManager.HideInteractionButton();
        }

        ContinueDialogue();
    }
}
