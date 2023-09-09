using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKLookatAnimation : MonoBehaviour
{
    #region Variables
    private Rig headRig;
    //private float targetWeight;

    private Coroutine currentCoroutine;
    #endregion

    #region Méthodes
    void Start()
    {
        headRig = GetComponent<Rig>();
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
     #endregion
}
