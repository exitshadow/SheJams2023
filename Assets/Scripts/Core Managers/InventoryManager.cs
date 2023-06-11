using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager that modifies the state of an Inventory Asset.
/// It should be the only class allowed to do so.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private InventoryStateAsset inventoryData;

    public void AddTreeBranches(int amount)
    {
        if (amount < 0) return;
        inventoryData.treeBranchesAmount++;
    }

    public void SwapToButterfliedBranches()
    {
        int currentAmount = inventoryData.treeBranchesAmount;
        inventoryData.treeBranchesAmount = 0;
        inventoryData.treeBranchesWithButterfliesAmount = currentAmount;
    }

    public void AddTunaCans(int amount)
    {
        if (amount < 0) return;
        inventoryData.tunaCansAmount++;
    }

    public void UseTunaCans(int amount)
    {
        if (amount < 0) return;
        inventoryData.tunaCansAmount--;
    }
}
