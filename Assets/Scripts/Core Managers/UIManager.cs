using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Dispatches all the texts in the game into their right places in the UI and manages opening and closing UI groups.
/// </summary>
///
public class UIManager : MonoBehaviour
{
    [Header("Dialogue Box References")]
    [SerializeField] private RectTransform dialogueBoxGroup;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerNameTMP;
    [SerializeField] private TextMeshProUGUI dialogueContentTMP;

    [Header("Annoying Phone References")]
    [SerializeField] private RectTransform phoneUIBoxGroup;
    [SerializeField] private VerticalLayoutGroup messageTextArea;
    [SerializeField] private GameObject messageFromImanPrefab;
    [SerializeField] private GameObject messageFromSenderPrefab;
    [SerializeField] private GameObject messageFromAppPrefab;
    
    [Header("Annoying Messages Display Parameters")]
    [SerializeField] private int maxCharsPerLine = 25;
    [SerializeField] private float charWidth = 8.5f;
    [SerializeField] private float charHeight = 22f;
    [SerializeField] private float messageVerticalMargin = 20f;
    [SerializeField] private float messageHorizontalMargin = 30f;
    [SerializeField] private float messageSpacingBetweenSenders = 10f;

    #region dialogue box
    public void OpenDialogueBox()
    {
        dialogueBoxGroup.gameObject.SetActive(true);
    }

    public void CloseDialogueBox()
    {
        dialogueBoxGroup.gameObject.SetActive(false);
    }

    public void InjectDialogueLine(string speakerName, string dialogueLine)
    {
        dialogueSpeakerNameTMP.text = speakerName;
        dialogueContentTMP.text = dialogueLine;
    }
    #endregion

    #region phone ui box stuffsies

    public void OpenPhoneUI()
    {
        phoneUIBoxGroup.gameObject.SetActive(true);
    }

    /// <summary>
    /// Instantiates a message inside of the phone box vertical group
    /// </summary>
    private void InstantiateMessage(AnnoyingTextMessageAsset.TextMessage message)
    {
        GameObject messageSnippetBox;
        TextMeshProUGUI messageSnippetText;

        // find correct prefab to look right in the convo
        if (message.senderName == "Iman")
        {
            messageSnippetBox = Instantiate(messageFromImanPrefab);
        }
        else if (message.senderName == "")
        {
            messageSnippetBox = Instantiate(messageFromAppPrefab);
        }
        else
        {
            messageSnippetBox = Instantiate(messageFromSenderPrefab);
        }

        // append as child to the vertical layout group
        messageSnippetBox.transform.parent = messageTextArea.transform;

        // calculates box size for said message (no need to do it for app messages)
        if (message.senderName != "")
        {
            CalculateAndApplyBoxSize(messageSnippetBox, message.textContent);
        }

        // apply the text to the TMP component in the prefab’s children
        messageSnippetText = messageSnippetBox.GetComponentInChildren<TextMeshProUGUI>();
        messageSnippetText.text = message.textContent;

    }

    /// <summary>
    /// Calculates how large the text message box should be according to the
    /// length of the string provided.
    /// </summary>
    /// <param name="messageBox"> The box on which the transformation will be applied. Must be a RectTransform.</param>
    /// <param name="text"> The text that is used to calculate the size.</param>
    private void CalculateAndApplyBoxSize(GameObject messageBox, string text)
    {
        RectTransform rT = messageBox.GetComponent<RectTransform>();

        float newWidth;
        float newHeight;

        int nbLines = (int)Mathf.Ceil(text.Length / maxCharsPerLine);

        newWidth = (charWidth * text.Length) + (2 * messageHorizontalMargin);
        newHeight = (nbLines * charHeight * 2) + (2 * messageVerticalMargin);

        rT.sizeDelta = new Vector2(newWidth, newHeight);
    }
    #endregion
}
