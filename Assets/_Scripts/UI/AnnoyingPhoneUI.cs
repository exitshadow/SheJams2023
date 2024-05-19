using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnnoyingPhoneUI : MonoBehaviour
{
    [SerializeField] private bool showPhoneAtStart;

    [Header("Phone prompt references")]
    [SerializeField] private RectTransform phoneNotificationGroup;
    [SerializeField] private RectTransform phoneIconGroup;
    [SerializeField] private RectTransform phoneUIBoxGroup;
    [SerializeField] private TextMeshProUGUI phoneStatusIndicator;
    [SerializeField] private string idleStatusText = "no new message";
    [SerializeField] private string newMessageStatusText = "read new message!";

    [Header("Phone messages references")]
    [SerializeField] private Image senderAvatar;
    [SerializeField] private TextMeshProUGUI senderName;
    [SerializeField] private VerticalLayoutGroup messageTextArea;
    [SerializeField] private GameObject messageFromImanPrefab;
    [SerializeField] private GameObject messageFromSenderPrefab;
    [SerializeField] private GameObject messageFromAppPrefab;
    [SerializeField] private AudioSource source;
    
    [Header("Annoying Messages Display Parameters")]
    [SerializeField] private int maxCharsPerLine = 25;
    [SerializeField] private float charWidth = 8.5f;
    [SerializeField] private float charHeight = 22f;
    [SerializeField] private float messageVerticalMargin = 20f;
    [SerializeField] private float messageHorizontalMargin = 30f;
    [SerializeField] private float messageSpacingBetweenSenders = 10f;

    private Canvas uiCanvas;

    #region Unity Messages
    void Start()
    {
        uiCanvas = FindFirstObjectByType<Canvas>();
        
        ClosePhoneUI();
        
        if (showPhoneAtStart) InitializePhoneIndicator();
        else ClosePhoneIndicator();
    }
    #endregion

    
    #region open & close
    private void InitializePhoneIndicator()
    {
        phoneIconGroup.gameObject.SetActive(true);
        phoneNotificationGroup.gameObject.SetActive(false);
        phoneStatusIndicator.text = idleStatusText;
    }

    private void ClosePhoneIndicator()
    {
        phoneIconGroup.gameObject.SetActive(false);
    }

    public void ShowNotificationOnPhone()
    {
        phoneIconGroup.gameObject.SetActive(true);
        phoneNotificationGroup.gameObject.SetActive(true);
        phoneStatusIndicator.text = newMessageStatusText;
    }

    public void OpenPhoneUI()
    {
        phoneUIBoxGroup.gameObject.SetActive(true);
    }

    public void ClosePhoneUI()
    {
        phoneUIBoxGroup.gameObject.SetActive(false);
    }

    public void DisplayReadingMessagesOnPhone()
    {
        phoneNotificationGroup.gameObject.SetActive(false);
        phoneStatusIndicator.text = $"reading a message from {senderName.text}";
    }

    public void EraseNotificationsOnPhone()
    {
        phoneNotificationGroup.gameObject.SetActive(false);
        phoneStatusIndicator.text = idleStatusText;
    }
    #endregion

    #region fetch messages data
    public void SetSender(Sprite avatar, string name)
    {
        senderAvatar.sprite = avatar;
        senderName.text = name;
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
            source.Play(0);

        }
        else if (message.senderName == "")
        {
            messageSnippetBox = Instantiate(messageFromAppPrefab);
        }
        else
        {
            messageSnippetBox = Instantiate(messageFromSenderPrefab);
            source.Play(0);

        }

        CalculateAndApplyBoxSize(messageSnippetBox, message.textContent);

        // append as child to the vertical layout group
        messageSnippetBox.transform.SetParent(messageTextArea.transform);
        messageSnippetBox.transform.localRotation = Quaternion.Euler(0,0,0);

        // apply the text to the TMP component in the prefab’s children
        messageSnippetText = messageSnippetBox.GetComponentInChildren<TextMeshProUGUI>();
        messageSnippetText.text = message.textContent;

    }
    #endregion

    #region utils
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

        TextMeshProUGUI messageBoxTMP = messageBox.GetComponentInChildren<TextMeshProUGUI>();
        // messageBoxTMP.margin = new Vector4 (    (messageVerticalMargin / 3) * uiCanvas.scaleFactor,
        //                                         (messageHorizontalMargin / 3) * uiCanvas.scaleFactor,
        //                                         (messageVerticalMargin / 3) * uiCanvas.scaleFactor,
        //                                         (messageHorizontalMargin / 3) * uiCanvas.scaleFactor);

        int maxCharsFirstLine;
        int nbLines = (int)Mathf.Ceil(text.Length / maxCharsPerLine);

        float newWidth;
        float newHeight;

        charHeight = messageBoxTMP.fontSize;

        if (nbLines < 1 ) maxCharsFirstLine = text.Length;
        else maxCharsFirstLine = maxCharsPerLine;

        newWidth = (charWidth * maxCharsFirstLine) + (2 * messageHorizontalMargin);
        newHeight = (nbLines * charHeight * 2) + (2 * messageVerticalMargin);

        parentRT.localScale *= uiCanvas.scaleFactor;
        parentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
        childRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        verticalGroupRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalGroupRT.sizeDelta.y + newHeight);
    }
    #endregion
}
