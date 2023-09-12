using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rig))]
public class IKLookatAnimation : MonoBehaviour
{
    #region Variables
    [SerializeField] private RigBuilder rigBuilder; 
    private Rig headRig;
    private MultiAimConstraint multiAimConstraint;
    //private float targetWeight;

    private Coroutine currentCoroutine;
    #endregion

    #region Méthodes
    void Awake()
    {
        headRig = GetComponent<Rig>();
        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();

        if (!rigBuilder) Debug.LogWarning("Please provide a Rig Builder reference!");
    }



    IEnumerator TurnHead(float targetWeight)
    {
        float actionTime = 1f;
        float timer = 0f;

        while (timer < actionTime)
        {
            float ratio = timer/actionTime;
            //yield return new WaitForEndOfFrame();
            headRig.weight = Mathf.Lerp(headRig.weight, targetWeight, ratio);
            timer += Time.deltaTime;
            yield return null;
        }
        headRig.weight = targetWeight;
    }

  
    
    public void ActivateLookat()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); 
        }
        currentCoroutine = StartCoroutine(TurnHead(1));
    }

    public void DeactivateLookat()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); 
        }
        currentCoroutine = StartCoroutine(TurnHead(0));
    }

    public void SetAimTarget(Transform target)
    {   
        var data = multiAimConstraint.data.sourceObjects;
        data.SetTransform(0, target);
        multiAimConstraint.data.sourceObjects = data;
        rigBuilder.Build();
    }
     #endregion
}
