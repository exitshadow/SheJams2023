using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Class is obsolete.
/// </summary>
[Obsolete("Game Manager class is obsolete, use GameState instead. Class is preserved for the mouse lock and will be replaced.", false)]
public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

}
