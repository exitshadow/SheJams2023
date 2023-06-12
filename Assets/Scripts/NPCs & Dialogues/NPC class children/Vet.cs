using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vet : NPC
{
    public override void InitializeDialogue()
    {
        base.InitializeDialogue();
        Debug.Log("initializing dialogue vet");
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        InitializeDialogue();
    }
}
