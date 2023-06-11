using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class that represents the inventory of the player. We opted to track a
/// number of items with ints for scope purposes. All changes in this asset
/// should be done by the InventoryManager class.
/// This system should be replaced in the case the game increases in scope.
/// </summary>
[CreateAssetMenu(menuName = "Handiman Data Assets/Inventory Asset", fileName = "Inventory Asset")]
public class InventoryStateAsset : ScriptableObject
{
    [Header("Tree branch info")]
    public int treeBranchesAmount = 0;
    public string treeBranchName;
    public string treeBranchDesc;

    [Header("Tree branch with butterfly info")]
    public int treeBranchesWithButterfliesAmount = 0;
    public string treeBranchesWithButterfliesName;
    public string treeBranchesWithButterfliesDesc;

    [Header("Tuna cans info")]
    public int tunaCansAmount = 0;
    public string tunaCanName;
    public string tunaCanDesc;
    public string tunaCanAltDesc;
}
