using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Grabber : MonoBehaviour
{
    private GrabManager grabManager;
    public List<Transform> grabbingAnchors;

    void Awake()
    {
        grabManager = FindFirstObjectByType<GrabManager>();
        grabManager.grabbers.Add(this);
    }

    [YarnCommand("set_grabber")]
    public void SetGrabber(int index)
    {
        grabManager.grabberTarget = grabbingAnchors[index];
    }
}
