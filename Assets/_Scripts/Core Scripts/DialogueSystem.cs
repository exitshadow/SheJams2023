using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    #region exposed fields
    [SerializeField] private DialogueRunner dialogueRunner;
    #endregion

    #region private fields
    private LineView lineView;
    private UIManager ui;
    private List<NPC> nPCs;
    private ImanActions actions;
    private InputAction interact;
    private InputAction playerMove;
    private bool canDialogueRun;
    #endregion

    #region properties / public methods
    public void AddNPC(NPC npc)
    {
        nPCs.Add(npc);
    }

    public void EnableDialogue()
    {
        canDialogueRun = true;
    }

    public void DisableDialogue()
    {
        canDialogueRun = false;
    }

    #endregion

    #region unity messages
    void Start()
    {
        ui = FindObjectOfType<UIManager>();
    }
    
    private void OnEnable()
    {
        lineView = dialogueRunner.dialogueViews[0] as LineView;
        lineView.requestInterrupt += OnRequestInterrupt;
    }

    private void OnDisable()
    {
        lineView.requestInterrupt -= OnRequestInterrupt;
    }
    #endregion

    private void SubscribeToNPCActions()
    {
        foreach (NPC nPC in nPCs)
        {
            nPC.onDialogueRequest += OnStartDialogue;
        }
    }

    private void OnStartDialogue(string node)
    {
        if (!canDialogueRun) return;

        dialogueRunner.StartDialogue(node);
        Debug.Log($"Dialogue starting using Yarn at node {node}");
    }

    private void AdvanceDialogue()
    {
        // advance to next line
        lineView.UserRequestedViewAdvancement();
    }

    private void OnRequestInterrupt()
    {
        ui.TriggerPop();
    }

}
