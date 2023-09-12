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

    public bool IsPlayingDialogue { get { return dialogueRunner.IsDialogueRunning ; } }
    
    #endregion

    #region dialogue methods


    /// <summary>
    /// Dequeues the first dialogue line from the current lines in queue and sends it to the UI Manager.
    /// </summary>
    public void ContinueDialogue()
    {
        GetYarnLine();
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

        StartYarnDialogue();
        ContinueDialogue();
    }

    protected virtual void StartYarnDialogue()
    {
            if (!dialogueRunner.IsDialogueRunning)
            {
                onDialogueStarted?.Invoke();
                dialogueRunner.StartDialogue(dialogueNode);
                uiManager.HideInteractionButton();
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
        }
    }

    #endregion

    #region unity events
    protected virtual void Awake()
    {
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

}
