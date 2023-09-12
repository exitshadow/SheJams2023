using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class GrabManager : MonoBehaviour
{
    public List<Grabber> grabbers;
    public List<Grabbable> grabbables;

    public Transform grabberTarget;
    public Transform grabbableTarget;

    // todo
    // settings for loc and rot of grabberTarget and grabbableTarget
    // to be passed in by Grabber and Grabbable components
    
    [YarnCommand("grab")]
    public void Grab()
    {
        grabbableTarget.transform.parent = grabberTarget;
        grabbableTarget.localPosition = Vector3.zero;
        grabbableTarget.localRotation = Quaternion.identity;
    }
}
