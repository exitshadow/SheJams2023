using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class AnimatedNPC : NPC
{
    protected Animator animator;

    protected override void Awake()
    {
        base.Awake();

        SetAnimator();
    }

    protected virtual void SetAnimator()
    {
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(2, 0);
    }
}
