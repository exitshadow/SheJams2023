using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKLookatAnimation : MonoBehaviour
{
    #region Variables
    private Rig headRig;
    //[SerializeField] private RigBuilder rigBuilder;
    private float targetWeight;
    private WeightedTransformArray sources;

    private Coroutine currentCoroutine;
    #endregion

    #region Méthodes
    void Start()
    {
        headRig = GetComponent<Rig>();
        Debug.LogWarning(headRig);
        //sources = headRig.data.sourceObjects;
    }

    void Update()
    {
        //todo wtf??????????????????????
        // headRig.weight += Time.deltaTime;
    }

    IEnumerator TurnHead(float targetWeight)
    {
        Debug.Log("inCoroutine");
        float actionTime = 1f;
        float timer = 0f;

        while (timer < actionTime)
        {
            float ratio = timer/actionTime;
            yield return new WaitForEndOfFrame();
            headRig.weight = Mathf.Lerp(headRig.weight, targetWeight, ratio);
            //headRig.data.sourceObjects.SetWeight(0,Mathf.Lerp(headRig.weight, targetWeight, ratio));
            Debug.Log(headRig.weight);
            timer += Time.deltaTime;
            yield return null;
        }
        headRig.weight = targetWeight;
        Debug.Log(headRig.weight);
    }

  
    
    public void ActivateLookat()
    {
        Debug.Log("Activating");
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); 
        }
        currentCoroutine = StartCoroutine(TurnHead(1));

        headRig.weight = 1f;
    }

    public void DeactivateLookat()
    {
        Debug.Log("Deactivating");
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); 
        }
        currentCoroutine = StartCoroutine(TurnHead(0));

        headRig.weight = 0f;
    }
     #endregion
}
