using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Annoying phone functions in the game: rings, gets the messages from data
/// to send it to the UI and display them as needed. It doesn’t touch the UI
/// itself.
/// </summary>
public class AnnoyingPhone : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioSource source;
    [SerializeField] private PlayerController player;
    public AnnoyingTextMessageAsset textMessagesData;
    
    private AnnoyingTextMessageAsset.TextMessageConversation currentConvo;
    private Queue<AnnoyingTextMessageAsset.TextMessage> queuedTextMessages = new Queue<AnnoyingTextMessageAsset.TextMessage>();
    private bool hasNewMessages; 
    public static bool IsReadingPhone { get; private set; }

    public bool HasPhoneMessages()
    {
        return hasNewMessages;
    }

    public void ShowNotification()
    {
        hasNewMessages = true;
        source.Play(0);
        uiManager.ShowNotificationOnPhone();
    }


    /// <summary>
    /// To be accessed through the Player Action Event OnPickUp
    /// </summary>
    public void PickUpPhone(InputAction.CallbackContext context)
    {
        if (context.performed)
        {   
            Debug.Log("picking phone up");
            if (hasNewMessages)
            {
                uiManager.OpenPhoneUI();
                uiManager.SetSender(currentConvo.conversationAvatar, currentConvo.conversationName);
                uiManager.DisplayReadingMessagesOnPhone();

                GetNewMessage();

                hasNewMessages = false;
                IsReadingPhone = true;
                player.MakeTypeOnPhone(true);
            }
            else
            {
                // asks UI to show a dialogue box that says there are no new messages
                // or just nothing lol
            }
        }
    }
    public void GetNewMessage()
    {
        if (queuedTextMessages.Count == 0)
        {
            uiManager.EraseNotificationsOnPhone();
            uiManager.ClearMessageBox();
            uiManager.ClosePhoneUI();
            player.MakeTypeOnPhone(false);
            IsReadingPhone = false;
            return;
        }

        uiManager.ShowNewMessage(queuedTextMessages.Dequeue());
    }
    public void FetchDialogue(int dialogueIndex)
    {
        currentConvo = textMessagesData.textMessageConversations[dialogueIndex];
        Debug.Log(currentConvo);

        queuedTextMessages.Clear();
        foreach(AnnoyingTextMessageAsset.TextMessage textMessage in currentConvo.conversationMessages)
        {
            queuedTextMessages.Enqueue(textMessage);
            Debug.Log("Queuing text message lines");
        }
        Debug.Log(queuedTextMessages);
    }

}
