using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Animator))]
public abstract class AnimatedNPC : NPC
{
    [Header("IK LookAt Options")]
    [SerializeField] protected bool useLookAtOnTrigger;
    [SerializeField] protected Transform playerLookAimTarget;
    protected Animator animator;
    protected IKLookatAnimation lookAt;
    protected IKLookatAnimation playerLookAt;

    protected override void Awake()
    {
        base.Awake();

        SetAnimator();
        GetIKRigs();

    }

    protected virtual void SetAnimator()
    {
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(2, 0);
    }

    protected void GetIKRigs()
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
        
        playerLookAt = player.GetComponentInChildren<IKLookatAnimation>();
        playerLookAt.SetAimtarget(playerLookAimTarget);
        playerLookAt.ActivateLookat();
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (player)
        {
            DisableLookAt();
            playerLookAt.DeactivateLookat();
        }
    }
}
