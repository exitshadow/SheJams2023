using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;


/// <summary>
/// Dispatches all the texts in the game into their right places in the UI and manages opening and closing UI groups.
/// </summary>
///
public class UIManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool showMissionStatusAtStart;
    [SerializeField] private bool showInteractionPromptAtStart;
    [SerializeField] private bool showControlsPromptAtStart;
    [SerializeField] private bool showPhoneAtStart;
    [SerializeField] private bool showDialogueBoxAtStart;
    [SerializeField] private bool debug = false;
    private RectTransform anchorIndicator;
    private GameObject anchorDebug;

    [Header("Dialogue Box")]
    [Tooltip("Safe margin area between the dialogue box and the screen limits")]
    [SerializeField] private float screenMargin = 45f;

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
    [SerializeField] private TextMeshProUGUI secondaryMissionTMP;

    [Header("Interaction Prompt Reference")]
    [SerializeField] private GameObject interactionPromptGroup;
    [SerializeField] private TextMeshProUGUI interactionPromptTMP;

    [Header("Dialogue Box References")]
    [SerializeField] private RectTransform dialogueBoxGroup;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerNameTMP;
    [SerializeField] private TextMeshProUGUI dialogueContentTMP;
    [SerializeField] private Collider playerCollider;
    public Transform dialogueAnchor;
    public Transform playerDialogueAnchor;
    public Transform currentDialogueAnchor;

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
    [YarnCommand("change_mission_prompt")]
    public void ChangeMissionPrompt(string prompt)
    {
        missionPromptTMP.text = prompt;
    }

    [YarnCommand("change_secondary_mission_prompt")]
    public void ChangeSecondaryMissionPrompt(string prompt)
    {
        secondaryMissionTMP.text = prompt;
    }

    public string MissionPrompt { get { return missionPromptTMP.text; }}
    public string SecondaryMissionPrompt { get { return secondaryMissionTMP.text; }}

    #endregion

    #region dialogue box

    public void TriggerPop()
    {
        dialogueBoxGroup.GetComponent<Animator>().SetTrigger("Pop");
    }

    public void OpenDialogueBox()
    {
        dialogueBoxGroup.gameObject.SetActive(true);
        dialogueBoxGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
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
    #endregion

    #region dialogue box dynamic placing
    public void PlaceDialogueBoxInScreen()
    {
        if (playerCollider) CalculatePlayerBounds();

        // world position to screen
        Vector2 screenPos = WorldToCanvasPoint(currentDialogueAnchor.position);

        // space/distance around the coordinates
        float distToTop = boundTop - screenPos.y - boundBottom;
        float distToRight = boundRight - screenPos.x - boundLeft;
        float distToLeft = screenPos.x - boundLeft;
        float distToBottom = screenPos.y - boundBottom;

        float pivotX;
        float pivotY;

        // setting the pivots
        if (distToLeft > distToRight)
            pivotX = 0;
        else pivotX = 1;

        if (distToTop > distToBottom)
            pivotY = 0;
        else pivotY = 1;

        // assigning the pivot
        dialogueBoxGroup.pivot = new Vector2(pivotX, pivotY);

        // assigning the position
        dialogueBoxGroup.anchoredPosition = new Vector2(screenPos.x, screenPos.y);

    }

    private void CalculatePlayerBounds()
    {
        // bounds of the player collider
        Vector3 pC = playerCollider.bounds.center;
        Vector3 pE = playerCollider.bounds.extents;

        // bounds corners positions in world space
        Vector3[] pCornersWS = new[]
        {
            new Vector3( pC.x + pE.x, pC.y + pE.y, pC.z + pE.z ),
            new Vector3( pC.x + pE.x, pC.y + pE.y, pC.z - pE.z ),
            new Vector3( pC.x + pE.x, pC.y - pE.y, pC.z + pE.z ),
            new Vector3( pC.x + pE.x, pC.y - pE.y, pC.z - pE.z ),
            new Vector3( pC.x - pE.x, pC.y + pE.y, pC.z + pE.z ),
            new Vector3( pC.x - pE.x, pC.y + pE.y, pC.z - pE.z ),
            new Vector3( pC.x - pE.x, pC.y - pE.y, pC.z + pE.z ),
            new Vector3( pC.x - pE.x, pC.y - pE.y, pC.z - pE.z )
        };

        // bounds corners positions in canvas space
        Vector2[] pCornersCS = new Vector2[8];


        // convert world corner points to screen corners and scale
        for (int i = 0; i < pCornersWS.Length; i++)
        {
            pCornersCS[i] = WorldToCanvasPoint(pCornersWS[i]);
        }

        // initialize canvas space bounds
        float playerBoundRight = pCornersCS[0].x;
        float playerBoundLeft = pCornersCS[0].x;
        float playerBoundBottom = pCornersCS[0].y;
        float playerBoundTop = pCornersCS[0].y;

        // find extremes
        for (int i = 1; i < pCornersCS.Length; i++)
        {
            if (pCornersCS[i].x > playerBoundRight)
                playerBoundRight = pCornersCS[i].x;

            if (pCornersCS[i].x < playerBoundLeft)
                playerBoundLeft = pCornersCS[i].x;

            if (pCornersCS[i].y > playerBoundTop)
                playerBoundTop = pCornersCS[i].y;

            if (pCornersCS[i].y < playerBoundBottom)
                playerBoundBottom = pCornersCS[i].y;
        }
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

    public void ShowInteractionButton(string interactionText)
    {
        interactionPromptGroup.SetActive(true);
        interactionPromptTMP.text = interactionText;
    }

    public void HideInteractionButton()
    {
        interactionPromptGroup.SetActive(false);
    }

    private void PlaceInteractionButtonOnScreen()
    {
        if (!interactionPromptGroup.activeSelf) return;
        
        if (!currentDialogueAnchor) currentDialogueAnchor = dialogueAnchor;
        
        Vector2 screenPos = WorldToCanvasPoint(currentDialogueAnchor.position);
        interactionPromptGroup.GetComponent<RectTransform>().anchoredPosition = screenPos;
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

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

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

        if (dialogueAnchor != null)
        {
            PlaceInteractionButtonOnScreen();
        }
    }

#region utils
    private Vector2 ScaleToScreen(Vector2 coordinate)
    {
        float x = coordinate.x / Camera.main.pixelWidth / (boundRight + screenMargin);
        float y = coordinate.y / Camera.main.pixelHeight / (boundTop + screenMargin);

        return new Vector2(x, y);
    }

    private Vector2 WorldToCanvasPoint(Vector3 position)
    {
        Vector2 coordinate = Camera.main.WorldToScreenPoint(position);

        float x = coordinate.x / (Camera.main.pixelWidth / (boundRight + screenMargin));
        float y = coordinate.y / (Camera.main.pixelHeight / (boundTop + screenMargin));

        return new Vector2(x, y);
    }

    private void OnTextChanged(Object objectChanged)
    {
        if (objectChanged == dialogueSpeakerNameTMP)
        {
            if (dialogueSpeakerNameTMP.text == "Iman")
            {
                currentDialogueAnchor = playerDialogueAnchor;
            }
            else
            {
                if (currentDialogueAnchor == null)
                    currentDialogueAnchor = dialogueAnchor;
            }
        }
    }
}

#endregion