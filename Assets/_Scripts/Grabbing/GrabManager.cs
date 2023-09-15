using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class GrabManager : MonoBehaviour
{
    public List<Grabber> grabbers;
    public List<Grabbable> grabbables;

    public Transform grabberTarget;
    public Transform grabbableTarget;

    public int currentIndex;

    public Transform previousGrabberTarget;
    public int previousIndex;

    public event Action<Transform, int, bool> onGrab;

    // todo
    // settings for loc and rot of grabberTarget and grabbableTarget
    // to be passed in by Grabber and Grabbable components
    
    [YarnCommand("grab")]
    public void Grab()
    {
        grabbableTarget.transform.parent = grabberTarget;
        grabbableTarget.localPosition = Vector3.zero;
        grabbableTarget.localRotation = Quaternion.identity;

        onGrab?.Invoke(previousGrabberTarget, previousIndex, false);
        onGrab?.Invoke(grabberTarget, currentIndex, true);

        previousGrabberTarget = grabberTarget;
        previousIndex = currentIndex;
    }
}
