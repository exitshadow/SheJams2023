using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Inherits from the NPC abstract class.
/// </summary>
public class Dad : AnimatedNPC
{
    [Header("Dad Character Specific References")]
    [SerializeField] private SceneLoader sceneLoader;

    [YarnCommand("start_credits_roll")]
    public void CreditsRoll()
    {
        sceneLoader.Credits();
    }
}
