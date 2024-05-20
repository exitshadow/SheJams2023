using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Yarn.Unity;

public class Grabber : MonoBehaviour
{
    private GrabManager grabManager;
    private Coroutine currentCoroutine;

    public List<Transform> grabbingAnchors;
    [SerializeField] private bool useGrabRigAnimation;
    [Tooltip("Each holding rig matches a grabbing anchor for animations")]
    public List<Rig> grabbingRigs;
    [SerializeField] private float animationDuration = 1f;

    void Awake()
    {
        grabManager = FindFirstObjectByType<GrabManager>();
        grabManager.grabbers.Add(this);
        if(useGrabRigAnimation) grabManager.onGrab += PlayHoldingAnimation;
    }

    void OnDisable()
    {
        if (useGrabRigAnimation) grabManager.onGrab -= PlayHoldingAnimation;
    }

    [YarnCommand("set_grabber")]
    public void SetGrabber(int index)
    {
        grabManager.grabberTarget = grabbingAnchors[index];
        grabManager.currentIndex = index;
    }

    private void PlayHoldingAnimation(Transform target, int index, bool grabDirection)
    {
        bool hasCheckPassed = false;

        foreach (Transform anchor in grabbingAnchors)
        {
            if (target == anchor)
            {
                hasCheckPassed = true;
                Debug.Log($"{target} has been found in {grabbingAnchors}, animation will be applied to it.");
                break;
            }
        }

        if (!hasCheckPassed) return;

        if (!grabbingRigs[index])
        {
            Debug.LogWarning("No Rig Component has been referenced, please provide one");
            return;
        }

        if (grabDirection) StartRigLerp(grabbingRigs[index], 1f);
        else StartRigLerp(grabbingRigs[index], 0f);
    }

    // TODO decouple this in the future and refactor with IKLookAtAnimation

    private IEnumerator RigLerpCoroutine(Rig targetRig, float targetWeight)
    {
        float timer = 0f;

        while (timer < animationDuration)
        {
            float ratio = timer / animationDuration;
            targetRig.weight = Mathf.Lerp(targetRig.weight, targetWeight, ratio);
            timer += Time.deltaTime;
            yield return null;
        }

        targetRig.weight = targetWeight;
    }

    private void StartRigLerp(Rig targetRig, float targetWeight)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(RigLerpCoroutine(targetRig, targetWeight));
    }
}
