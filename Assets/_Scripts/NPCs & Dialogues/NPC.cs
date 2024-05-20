using UnityEngine;
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
    [Header("Interaction Prompt & Dialogue Anchors")]
    [SerializeField] protected bool useInteractionPrompt = true;
    [SerializeField] protected string promptText = "Talk";
    [SerializeField] protected Transform dialogueAnchor;
    [SerializeField] protected bool isTriggerActive = true;
    [SerializeField] protected bool isMaskableByDialogueBoxes = false;


    [Header("Look At Options")]
    [Tooltip("Anchor used to place dialogue boxes and interaction prompt")]
    [SerializeField] protected bool usePlayerLookAtOnTrigger = true;
    [SerializeField] protected Transform playerLookAimTarget;

    [Header("Yarn Settings")]
    protected DialogueRunner dialogueRunner;

    [Tooltip("The dialogue node corresponding to the NPC’s yarn script")]
    [SerializeField] protected string dialogueNode;
    public string DialogueNode { get {return dialogueNode;} }

    [Header("Graphics")]
    [Tooltip("UI Manager to manage all the graphics")]
    [SerializeField] protected UIManager uiManager;

    [Header("NPC / Dialogue Events")]
    public UnityEvent onDialogueStarted;
    #endregion

    #region component references
    protected PlayerController player;
    protected IKLookatAnimation playerLookAt;

    protected DialogueBoxUI dialogueUI;
    protected AnchorsHandler anchorsHandler;
    protected InteractionPromptUI interactionPrompt;
    #endregion

    #region dialogue tracking tools
    public event Action<string> onDialogueRequest;
    public bool IsPlayingDialogue { get { return dialogueRunner.IsDialogueRunning ; } }
    #endregion

    #region unity events
    protected virtual void Awake()
    {
        FetchReferences();
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

    #region public methods
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
    #endregion

    #region internal methods
    protected void RequestDialogueStart()
    {
        onDialogueRequest?.Invoke(dialogueNode);

        if (dialogueAnchor) anchorsHandler.SetTargetDialogueAnchor(dialogueAnchor);

        StartYarnDialogue();
        ContinueDialogue();
    }

    protected virtual void StartYarnDialogue()
    {
        if (!dialogueRunner.IsDialogueRunning)
        {
            onDialogueStarted?.Invoke();
            dialogueRunner.StartDialogue(dialogueNode);
            if (interactionPrompt) interactionPrompt.HideInteractionButton();
        }
    }

    protected virtual void GetYarnLine()
    {
        //Debug.Log("Requesting View advancement");
        dialogueRunner.dialogueViews[0].UserRequestedViewAdvancement();
        dialogueUI.TriggerPop();
        
        if (!dialogueRunner.IsDialogueRunning)
        {
            anchorsHandler.ClearDialogueAnchors();
        }
    }

    #endregion


    #region trigger events
    public virtual void OnTriggerEnter(Collider other)
    {
        if (!isTriggerActive) return;

        player = other.GetComponent<PlayerController>();

        if (player)
        {
            OccupyPlayerSlot();

            if (useInteractionPrompt && !AnnoyingPhone.IsReadingPhone && interactionPrompt)
            {
                interactionPrompt.ShowInteractionButton(promptText);
            }
            else if (!interactionPrompt) Debug.LogWarning("No interaction prompt has been found in the scene.");

            if (usePlayerLookAtOnTrigger) EnablePlayerLookAt();
            if (!isMaskableByDialogueBoxes) dialogueUI.currentInteractingNPCCollider = GetComponent<CapsuleCollider>();
        }
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        if (!isTriggerActive) return;

        player = other.GetComponent<PlayerController>();

        if (player)
        {
            if (interactionPrompt) interactionPrompt.HideInteractionButton();
            anchorsHandler.ClearDialogueAnchors();
            dialogueUI.currentInteractingNPCCollider = null;
            DisablePlayerLookAt();
            ClearPlayerSlot();
        }
    }
    #endregion


    [YarnCommand("clear_player_slot")]
    public void ClearPlayerSlot()
    {
        if (player) player.currentInteractingNPC = null;
        else Debug.LogWarning("No player has been referenced!");
    }

    [YarnCommand("enable_player_lookat")]
    public void EnablePlayerLookAt()
    {
        playerLookAt = player.GetComponentInChildren<IKLookatAnimation>();
        playerLookAt.SetAimTarget(playerLookAimTarget);
        playerLookAt.ActivateLookat();
    }

    [YarnCommand("disable_player_lookat")]
    public void DisablePlayerLookAt()
    {
        if (player) playerLookAt = player.GetComponentInChildren<IKLookatAnimation>();
        playerLookAt.DeactivateLookat();
    }

    [YarnCommand("enable_trigger_events")]
    public void EnableTriggerEvents(bool value)
    {
        isTriggerActive = value;
    }


    protected void OccupyPlayerSlot()
    {
        //anchorsHandler.SetAnchors();
        if (dialogueAnchor) anchorsHandler.SetTargetDialogueAnchor(dialogueAnchor);
        else anchorsHandler.SetPlayerAnchorAsTarget();

        if (player) player.currentInteractingNPC = this;
    }

    protected virtual void OnRequestInterrupt()
    {
        dialogueUI.TriggerPop();
    }

    protected void FetchReferences()
    {
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        dialogueUI = FindFirstObjectByType<DialogueBoxUI>();
        anchorsHandler = FindFirstObjectByType<AnchorsHandler>();
        interactionPrompt = FindFirstObjectByType<InteractionPromptUI>();
    }

}
