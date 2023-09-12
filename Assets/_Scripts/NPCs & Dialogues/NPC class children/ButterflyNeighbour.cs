using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ButterflyNeighbour : AnimatedNPC
{
    [Header("Butterfly Neighbour special fields")]
    [SerializeField] private Transform targetHand;
    [SerializeField] private GameObject treeBranchObject;

    [YarnCommand("give_branch_to_neighbour")]
    public void GiveBranchToNeighbour()
    {
        treeBranchObject.transform.SetParent(targetHand);
        treeBranchObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        treeBranchObject.transform.localPosition = new Vector3(0, 0, 0.001f);
    }
}
