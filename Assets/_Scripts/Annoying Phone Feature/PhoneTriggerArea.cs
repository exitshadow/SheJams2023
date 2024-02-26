using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PhoneTriggerArea : MonoBehaviour
{
    [SerializeField] private int conversationIndex;
    
    [SerializeField] private AnnoyingPhone phoneManager;

    private void OnTriggerEnter(Collider other)
    {
        if (phoneManager.textMessagesData.hasBeenRead[conversationIndex]) return;

        if (other.CompareTag("Player"))
        {
            if (!phoneManager.HasPhoneMessages() && !AnnoyingPhone.IsReadingPhone)
            {
                phoneManager.FetchDialogue(conversationIndex);
                phoneManager.ShowNotification();
                phoneManager.textMessagesData.hasBeenRead[conversationIndex] = true;
            }
        }
    }

}
