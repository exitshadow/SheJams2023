using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PhoneTriggerArea : MonoBehaviour
{
    [SerializeField] private int conversationIndex;
    
    [SerializeField] private AnnoyingPhone phoneManager;

    private bool hasBeenSent = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenSent) return;

        if (other.CompareTag("Player"))
        {
            if (!phoneManager.HasPhoneMessages())
            {
                phoneManager.FetchDialogue(conversationIndex);
                phoneManager.ShowNotification();
                hasBeenSent = true;
            }
        }
    }

}
