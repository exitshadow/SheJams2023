using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Annoying phone functions in the game: rings, gets the messages from data
/// to send it to the UI and display them as needed. It doesn’t touch the UI
/// itself.
/// </summary>
public class AnnoyingPhone : MonoBehaviour
{
    [SerializeField] private AnnoyingTextMessageAsset textMessagesData;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;
    
    private AnnoyingTextMessageAsset.TextMessageConversation currentConvo;
    private Queue<AnnoyingTextMessageAsset.TextMessage> queuedTextMessages = new Queue<AnnoyingTextMessageAsset.TextMessage>();
    private bool hasNewMessages;

    public void ShowNotification()
    {
        // asks the UI to show the notification on the screen
    }


    /// <summary>
    /// To be accessed through the Player Action Event OnPickUp
    /// </summary>
    public void PickUpPhone()
    {
        if (hasNewMessages)
        {
            // asks the UI to show the messages of current conversation
            hasNewMessages = false;
        }
        else
        {
            // asks UI to show a dialogue box that says there are no new messages
            // or just nothing lol
        }
    }
    public void FetchDialogue(int dialogueIndex)
    {
        currentConvo = textMessagesData.textMessageConversations[dialogueIndex];

        queuedTextMessages.Clear();
        foreach(AnnoyingTextMessageAsset.TextMessage textMessage in currentConvo.conversationMessages)
        {
            queuedTextMessages.Enqueue(textMessage);
            Debug.Log("Queuing text message lines");
        }
    }

}
