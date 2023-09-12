using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TunaCan : NPC
{
    [YarnCommand("disable_tuna_can")]
    public void DisableTunaCan()
    {
        this.gameObject.SetActive(false);
    }
}
    
