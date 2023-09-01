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
    [Header("Behaviour Settings")]
    [SerializeField] private bool showMissionStatusAtStart;
    [SerializeField] private bool showInteractionPromptAtStart;
    [SerializeField] private bool showControlsPromptAtStart;
    [SerializeField] private bool showPhoneAtStart;
    [SerializeField] private bool showDialogueBoxAtStart;

    [Header("Debug Settings")]
    [SerializeField] private bool debug = false;
    private RectTransform anchorIndicator;
    private GameObject anchorDebug;

    [Header("Dialogue Box Behaviour Settings")]
    [Tooltip("Safe margin area between the dialogue box and the screen limits")]
    [SerializeField] private float screenMargin = 15f;

    private float boundTop;
    private float boundRight;
    private float boundBottom;
    private float boundLeft;

    private float boxBoundTop;
    private float boxBoundRight;
    private float boxBoundBottom;
    private float boxBoundLeft;


    #region exposed references
    [Header("General References")]
    [SerializeField] private Canvas uiCanvas;

    [Header("Controls prompt references")]
    [SerializeField] private RectTransform showControlsPromptGroup;
    [SerializeField] private RectTransform controlsHelpGroup;
    [HideInInspector] public bool isControlHelpDisplayed;

    [Header("Mission Indicator Reference")]
    [SerializeField] private TextMeshProUGUI missionPromptTMP;

    [Header("Interaction Prompt Reference")]
    [SerializeField] private GameObject interactionPromptGroup;

    [Header("Dialogue Box References")]
    [SerializeField] private RectTransform dialogueBoxGroup;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerNameTMP;
    [SerializeField] private TextMeshProUGUI dialogueContentTMP;
    public Transform dialogueAnchor;
    public Transform playerDialogueAnchor;

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
    #endregion

    private Transform currentDialogueAnchor;

    #region show controls prompt
    public void OpenControlsPrompt()
    {
        showControlsPromptGroup.gameObject.SetActive(true);
        isControlHelpDisplayed = true;
    }

    public void CloseControlsPromt()
    {
        showControlsPromptGroup.gameObject.SetActive(false);
        isControlHelpDisplayed = false;
    }

    public void OpenControlsHelp()
    {
        controlsHelpGroup.gameObject.SetActive(true);
        isControlHelpDisplayed = true;
    }

    public void CloseControlsHelp()
    {
        controlsHelpGroup.gameObject.SetActive(false);
        isControlHelpDisplayed = false;
    }
    #endregion

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
        PlaceDialogueBoxInScreen();
    }

    public void CloseDialogueBox()
    {
        dialogueBoxGroup.gameObject.SetActive(false);
    }

    public void InjectDialogueLine(string speakerName, string dialogueLine)
    {
        dialogueSpeakerNameTMP.text = speakerName;
        dialogueContentTMP.text = dialogueLine;

        if (currentDialogueAnchor == null) return;

        if (speakerName == "Iman" || speakerName == "Game Manager")
        {
            currentDialogueAnchor = playerDialogueAnchor;
        }
        else
        {
            if (dialogueAnchor == null) currentDialogueAnchor = playerDialogueAnchor;
            currentDialogueAnchor = dialogueAnchor;
        }
    }

    public void PlaceDialogueBoxInScreen()
    {
        // world position to screen
        Vector2 dialogueScreenPosition = Camera.main.WorldToScreenPoint(currentDialogueAnchor.position);

        // space/distance around the coordinates
        float spaceLeft = dialogueScreenPosition.x - boundLeft;
        float spaceRight = boundRight - dialogueScreenPosition.x;
        float spaceTop = dialogueScreenPosition.y - boundBottom;
        float spaceBottom = boundTop - dialogueScreenPosition.y;

        float pivotX;
        float pivotY;

        float posX = dialogueScreenPosition.x / (Camera.main.pixelWidth / (boundRight + screenMargin));
        float posY = dialogueScreenPosition.y / (Camera.main.pixelHeight / (boundTop + screenMargin));

        // setting the pivots
        if (spaceLeft > spaceRight)
            pivotX = 0;
        else pivotX = 1;

        if (spaceTop > spaceBottom)
            pivotY = 0;
        else pivotY = 1;

        // assigning the pivot
        dialogueBoxGroup.pivot = new Vector2(pivotX, pivotY);
        
        // assigning the position
        dialogueBoxGroup.anchoredPosition = new Vector2(posX, posY);

    }

    private void GetScreenBoundsWithMargins()
    {
        boundTop = uiCanvas.GetComponent<RectTransform>().rect.height - screenMargin;
        boundRight = uiCanvas.GetComponent<RectTransform>().rect.width - screenMargin;
        boundBottom = screenMargin;
        boundLeft = screenMargin;
    }

    private void TrackDialogueBoxBounds()
    {

        // pivot horizontal
        if (dialogueBoxGroup.pivot.x < 0.5f)
        {
            boxBoundLeft = dialogueBoxGroup.anchoredPosition.x;
            boxBoundRight = dialogueBoxGroup.anchoredPosition.x + dialogueBoxGroup.rect.width;
        }
        else
        {
            boxBoundLeft = dialogueBoxGroup.anchoredPosition.x - dialogueBoxGroup.rect.width;
            boxBoundRight = dialogueBoxGroup.anchoredPosition.x;
        }

        // pivot vertical
        if (dialogueBoxGroup.pivot.y < 0.5f)
        {
            boxBoundTop = dialogueBoxGroup.anchoredPosition.y + dialogueBoxGroup.rect.height;
            boxBoundBottom = dialogueBoxGroup.anchoredPosition.y;
        }
        else
        {
            boxBoundTop = dialogueBoxGroup.anchoredPosition.y;
            boxBoundBottom = dialogueBoxGroup.anchoredPosition.y - dialogueBoxGroup.rect.height;
        }
    }

    private void RepositionDialogueBoxOnScreen()
    {
        // declaring new field with default values
        float newX = dialogueBoxGroup.anchoredPosition.x;
        float newY = dialogueBoxGroup.anchoredPosition.y;

        // rules for left pivot
        if (dialogueBoxGroup.pivot.x < 0.5)
        {
            if (boxBoundRight > boundRight)
                newX = dialogueBoxGroup.anchoredPosition.x - (boxBoundRight - boundRight);

            if (boxBoundLeft < boundLeft)
                newX = boundLeft;
        }
        // rules for right pivot
        else
        {
            if (boxBoundRight > boundRight)
                newX = boundRight;

            if (boxBoundLeft < boundLeft)
                newX = boundLeft + dialogueBoxGroup.rect.width;
        }

        // rules for bottom pivot
        if (dialogueBoxGroup.pivot.y < 0.5)
        {
            if (boxBoundTop > boundTop)
             newY = dialogueBoxGroup.anchoredPosition.y - (boxBoundTop - boundTop);

            if (boxBoundBottom < boundBottom)
                newY = boundBottom;
        }
        // rules for top pivot
        else
        {
            if (boxBoundTop > boundTop)
                newY = boundTop;
            
            if (boxBoundBottom < boundBottom)
                newY = boundBottom + dialogueBoxGroup.rect.height;
        }

        dialogueBoxGroup.anchoredPosition = new Vector2(newX, newY);
    }
    #endregion

    #region interaction prompt
    public void ShowInteractionButton()
    {
        interactionPromptGroup.SetActive(true);
    }

    public void HideInteractionButton()
    {
        interactionPromptGroup.SetActive(false);
    }
    #endregion

    #region phone prompt

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

    void Start()
    {
        if (debug)
        {
            anchorDebug = new GameObject();
            anchorDebug.name = "Dialogue Anchor Debug Transform (UI MANAGER)";
            anchorDebug.transform.parent = uiCanvas.transform;
            anchorIndicator = anchorDebug.gameObject.AddComponent<RectTransform>();
        }

        GetScreenBoundsWithMargins();

        ClosePhoneUI();
        CloseControlsHelp();
        HideInteractionButton();

        if (showPhoneAtStart) InitializePhoneIndicator();
        else ClosePhoneIndicator();

        if (showControlsPromptAtStart) OpenControlsPrompt();

        if (showDialogueBoxAtStart) OpenDialogueBox();
        else CloseDialogueBox();

        // todo refactor & correct
        if (playerDialogueAnchor != null && dialogueAnchor != null)
            currentDialogueAnchor = playerDialogueAnchor;
    }

    void Update()
    {
        if (currentDialogueAnchor != null)
        {
            PlaceDialogueBoxInScreen();
            TrackDialogueBoxBounds();
            RepositionDialogueBoxOnScreen();
        }
    }


}
