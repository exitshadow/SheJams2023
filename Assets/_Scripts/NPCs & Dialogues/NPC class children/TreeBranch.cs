using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TreeBranch : InanimatedNPC
{
    [Tooltip("Has to be the one with the renderer on it!")]
    [SerializeField] private GameObject treeBranchObject;
    [SerializeField] private Transform targetHand;
    public bool isTriggerActive = true;


    [YarnCommand("pass_tree_branch_to_player")]
    public void PassTreeBranchToPlayer()
    {
        treeBranchObject.transform.SetParent(targetHand);
        treeBranchObject.transform.localRotation = Quaternion.identity;
        treeBranchObject.transform.localPosition = new Vector3(0, 0, -.1f);
    }

    [YarnCommand("disable_tree_branch")]
    public void DisableTreeBranch()
    {
        if (player) player.currentInteractingNPC = null;
        isTriggerActive = false;

    }

    [YarnCommand("enable_tree_branch")]
    public void EnableTreeBranch()
    {
        isTriggerActive = true;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (isTriggerActive)
        {
            base.OnTriggerEnter(other);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (isTriggerActive)
        {
            base.OnTriggerExit(other);
        }
    }
}
