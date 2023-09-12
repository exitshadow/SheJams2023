using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public abstract class AnimatedNPC : NPC
{
    [Header("IK LookAt Options")]
    [SerializeField] protected bool useLookAtOnTrigger;
    protected Animator animator;
    protected IKLookatAnimation lookAt;

    protected override void Awake()
    {
        base.Awake();

        SetAnimator();
        GetNPCRig();

    }

    protected virtual void SetAnimator()
    {
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(2, 0);
    }

    protected virtual void GetNPCRig()
    {
        lookAt = GetComponentInChildren<IKLookatAnimation>();
    }

    [YarnCommand("enable_look_at")]
    public void EnableNPCLookAt()
    {
        lookAt.ActivateLookat();
    }

    [YarnCommand("disable_look_at")]
    public void DisableNPCLookAt()
    {
        lookAt.DeactivateLookat();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (player && useLookAtOnTrigger)
        {
            EnableNPCLookAt();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            DisableNPCLookAt();
        }
    }
}
