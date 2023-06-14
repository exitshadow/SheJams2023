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
    [Header("Mission Indicator Reference")]
    [SerializeField] private TextMeshProUGUI missionPromptTMP;

    [Header("Dialogue Box References")]
    [SerializeField] private RectTransform dialogueBoxGroup;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerNameTMP;
    [SerializeField] private TextMeshProUGUI dialogueContentTMP;

    [Header("Annoying Phone References")]
    [SerializeField] private RectTransform phoneNotificationGroup;
    [SerializeField] private RectTransform phoneIconGroup;
    [SerializeField] private RectTransform phoneUIBoxGroup;
    [SerializeField] private Image senderAvatar;
    [SerializeField] private TextMeshProUGUI senderName;
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

    #region mission prompt
    public void ChangeMissionPrompt(string prompt)
    {
        missionPromptTMP.text = prompt;
    }

    #endregion

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

    public void ClosePhoneUI()
    {
        phoneUIBoxGroup.gameObject.SetActive(false);
    }

    public void SetSender(Sprite avatar, string name)
    {
        senderAvatar.sprite = avatar;
        senderName.text = name;
    }

    public void ShowNotificationOnPhone()
    {
        phoneNotificationGroup.gameObject.SetActive(true);
    }

    public void EraseNotificationsOnPhone()
    {
        phoneNotificationGroup.gameObject.SetActive(false);
    }

    public void ClearMessageBox()
    {
        foreach (Transform child in messageTextArea.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Instantiates a message inside of the phone box vertical group
    /// </summary>
    public void ShowNewMessage(AnnoyingTextMessageAsset.TextMessage message)
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

        // calculates box size for said message (no need to do it for app messages)
        if (message.senderName != "")
        {
            CalculateAndApplyBoxSize(messageSnippetBox, message.textContent);
        }

        // append as child to the vertical layout group
        messageSnippetBox.transform.SetParent(messageTextArea.transform);
        messageSnippetBox.transform.localRotation = Quaternion.Euler(0,0,0);

        // apply the text to the TMP component in the prefab’s children
        messageSnippetText = messageSnippetBox.GetComponentInChildren<TextMeshProUGUI>();
        messageSnippetText.text = message.textContent;

    }

    /// <summary>
    /// Calculates how large the text message box should be according to the
    /// length of the string provided. This method assumes that the messageBox
    /// provided fits the right pattern.
    /// </summary>
    /// <param name="messageBox"> The box on which the transformation will be applied. Must be a RectTransform.</param>
    /// <param name="text"> The text that is used to calculate the size.</param>
    private void CalculateAndApplyBoxSize(GameObject messageBox, string text)
    {
        // the reason to fetch both parent and child RectTransforms and change
        // their respective heights and width comes from the necessity to
        // fit with the vertical align group that parents them

        // The parent has a height that is taken into account and is forced
        // to horizontally expand

        // The child is set to fill the parent’s height but is allowed to
        // control its width

        RectTransform parentRT = messageBox.GetComponent<RectTransform>();
        RectTransform childRT = messageBox.transform.Find("MessageBox").GetComponent<RectTransform>();

        RectTransform verticalGroupRT = messageTextArea.GetComponent<RectTransform>();

        Debug.Log("found message box: " + (childRT != null));

        float newWidth;
        float newHeight;

        int maxCharsFirstLine;

        int nbLines = (int)Mathf.Ceil(text.Length / maxCharsPerLine);

        if (nbLines < 2 ) maxCharsFirstLine = text.Length;
        else maxCharsFirstLine = maxCharsPerLine;

        newWidth = (charWidth * maxCharsFirstLine) + (2 * messageHorizontalMargin);
        newHeight = (nbLines * charHeight * 2) + (2 * messageVerticalMargin);

        parentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
        childRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        verticalGroupRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalGroupRT.sizeDelta.y + newHeight);
    }
    #endregion
}
