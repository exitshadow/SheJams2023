using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TreeBranch : NPC
{
    [Tooltip("Has to be the one with the renderer on it!")]
    [SerializeField] private GameObject treeBranchObject;
    [SerializeField] private Transform targetHand;


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
        this.gameObject.SetActive(false);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
