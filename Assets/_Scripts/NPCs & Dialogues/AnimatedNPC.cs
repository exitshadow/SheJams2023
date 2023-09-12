using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
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
        GetIKRig();

    }

    protected virtual void SetAnimator()
    {
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(2, 0);
    }

    protected void GetIKRig()
    {
        lookAt = GetComponentInChildren<IKLookatAnimation>();
    }

    [YarnCommand("enable_look_at")]
    public void EnableLookAt()
    {
        lookAt.ActivateLookat();
    }

    [YarnCommand("disable_look_at")]
    public void DisableLookAt()
    {
        lookAt.DeactivateLookat();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (player && useLookAtOnTrigger)
        {
            EnableLookAt();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (player)
        {
            DisableLookAt();
        }
    }
}
