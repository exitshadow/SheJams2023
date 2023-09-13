using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Grabbable : MonoBehaviour
{
    private GrabManager grabManager;

    void Awake()
    {
        grabManager = FindFirstObjectByType<GrabManager>();
        grabManager.grabbables.Add(this);
    }

    [YarnCommand("set_grabbable")]
    public void SetGrabbable()
    {
        grabManager.grabbableTarget = transform;
    }

}

