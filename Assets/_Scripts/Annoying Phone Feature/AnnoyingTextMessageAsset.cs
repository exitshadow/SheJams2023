using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Asset class holding the blueprint for all information needed for the phone text messages.
/// </summary>
[CreateAssetMenu(menuName ="Handiman Data Assets/Annoying Phone Messages Asset")]
public class AnnoyingTextMessageAsset : ScriptableObject
{
    public List<TextMessageConversation> textMessageConversations;

    //a list of boolean for each Phone Trigger Area of the scene (index match conversationIndex), that whay the text messages are not read twice
    public List<bool> hasBeenRead = new List<bool>();

    private void OnEnable()
    {
        for (int i = 0; i < hasBeenRead.Count; i++)
        {
            hasBeenRead[i] = false;
        }
    }

    [System.Serializable]
    public struct TextMessageConversation
    {
        public string conversationName;
        public Sprite conversationAvatar;
        public List<TextMessage> conversationMessages;
    }

    [System.Serializable]
    public struct TextMessage
    {
        public string senderName;
        [TextArea] public string textContent;
    }
}
