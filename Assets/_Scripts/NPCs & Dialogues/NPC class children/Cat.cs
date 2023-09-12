using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cat : AnimatedNPC
{
    protected override void SetAnimator()
    {
        animator = GetComponent<Animator>();
    }
}
