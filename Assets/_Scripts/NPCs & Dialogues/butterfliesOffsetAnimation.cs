using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfliesOffsetAnimation : MonoBehaviour
{
    #region Variables
    //[SerializeField] private Transform butterfliesParent;
    [SerializeField] private Animator[] butterflies;
    float randomOffset;
    #endregion

    #region Methodes
    void Start()
    {
        butterflies = new Animator[transform.childCount];
        int i = 0;
        
        foreach (Transform child in transform)
        {
            butterflies[i] = child.GetComponent<Animator>();
            randomOffset = Random.Range(0f,1f);
            butterflies[i].Play("ButterflyFly", 0, randomOffset);
            i++;
        }
    }
    #endregion
}



