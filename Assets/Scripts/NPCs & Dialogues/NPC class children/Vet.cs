using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vet : NPC
{
    public override void InitializeDialogue(NPCDialogueAsset _dialogue)
    {
        base.InitializeDialogue(_dialogue);
        Debug.Log("initializing dialogue vet");
        InjectDialogue();
        InjectDialogue();
        InjectDialogue();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        InitializeDialogue(this.dialogueData);
    }
}
