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
    [Header("Interaction Prompt")]
    [SerializeField] protected bool useInteractionPrompt = true;
    [SerializeField] protected string promptText = "Talk";
    [SerializeField] protected Transform dialogueAnchor;
    [Tooltip("Yarn dialogue runner component. One per scene")]

    [Header("Look At Options")]
    [SerializeField] protected bool usePlayerLookAtOnTrigger = true;
    [SerializeField] protected Transform playerLookAimTarget;

    [Header("Yarn Settings")]
    [Tooltip("Anchor used to place dialogue boxes and interaction prompt")]
    [SerializeField] protected DialogueRunner dialogueRunner;
    [SerializeField] protected string dialogueNode;

    [Header("Graphics")]
    [Tooltip("UI Manager to manage all the graphics")]
    [SerializeField] protected UIManager uiManager;

    [Header("NPC / Dialogue Events")]
    public UnityEvent onDialogueStarted;
    #endregion

    #region component references
    protected PlayerController player;
    protected IKLookatAnimation playerLookAt;
    #endregion


    #region dialogue tracking tools
    public event Action<string> onDialogueRequest;
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
        onDialogueRequest?.Invoke(dialogueNode);

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
            if (usePlayerLookAtOnTrigger) EnablePlayerLookAt();
            OccupyPlayerSlot();
        }
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.HideInteractionButton();
            uiManager.currentDialogueAnchor = null;
            DisablePlayerLookAt();
            ClearPlayerSlot();
        }
    }
    #endregion


    [YarnCommand("clear_player_slot")]
    public void ClearPlayerSlot()
    {
        if (player) player.currentInteractingNPC = null;
    }

    protected void EnablePlayerLookAt()
    {
        playerLookAt = player.GetComponentInChildren<IKLookatAnimation>();
        playerLookAt.SetAimTarget(playerLookAimTarget);
        playerLookAt.ActivateLookat();
    }

    protected void DisablePlayerLookAt()
    {
        playerLookAt = player.GetComponentInChildren<IKLookatAnimation>();
        playerLookAt.DeactivateLookat();
    }


    protected void OccupyPlayerSlot()
    {
        if (player) player.currentInteractingNPC = this;
    }

    protected virtual void OnRequestInterrupt()
    {
        uiManager.TriggerPop();
    }

}
