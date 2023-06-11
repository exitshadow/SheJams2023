using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class that represents the inventory of the player. We opted to track a
/// number of items with ints for scope purposes. All changes in this asset
/// should be done by the InventoryManager class.
/// This system should be replaced in the case the game increases in scope.
/// </summary>
[CreateAssetMenu(menuName = "Inventory Asset", fileName = "Inventory Asset")]
public class InventoryStateAsset : ScriptableObject
{
    public int treeBranchesAmount = 0;
    public int tunaCansAmount = 0;
}
