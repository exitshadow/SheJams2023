using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;
using UnityEngine.UI.ProceduralImage;


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
    [SerializeField] private float maskBoxMargin = 15f;

    private float screenBoundTop;
    private float screenBoundRight;
    private float screenBoundBottom;
    private float screenBoundLeft;

    private float dialogueBoxTop;
    private float dialogueBoxRight;
    private float dialogueBoxBottom;
    private float dialogueBoxLeft;

    private float maskBoundRight;
    private float maskBoundLeft;
    private float maskBoundBottom;
    private float maskBoundTop;

    private GameObject placeHolderBox;
    private RectTransform placeHolderBoxRect;
    private ProceduralImage placeHolderImg;


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
    [HideInInspector] public Collider CurrentInteractingNPCCollider;
    public Transform dialogueAnchor;
    [Tooltip("default dialogue anchor")]
    public Transform otherDialogueAnchor;
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
        // initial values;
        Vector2 screenPos = WorldToCanvasPoint(currentDialogueAnchor.position);
        float pivotX = 0;
        float pivotY = 0;

        // PLACING AFTER CONTROLLING FOR MASKING BOX
        if (playerCollider && CurrentInteractingNPCCollider)
        {
            CalculateMaskBoxBounds();
            
            // when anchor x axis falls inside mask bounds
            if (screenPos.x < maskBoundRight && screenPos.x > maskBoundLeft)
            {
                if (screenPos.y < maskBoundTop && screenPos.y > maskBoundBottom)
                {
                    float rightDist = maskBoundRight - screenPos.x;
                    float leftDist = screenPos.x - maskBoundLeft;

                    if (rightDist < leftDist)
                    {
                        // assign pivot to left side
                        pivotX = 0;

                        // assign position to right side of masking box
                        screenPos.x = maskBoundRight;
                    }
                    else
                    {
                        // assign pivot to right side
                        pivotX = 1;

                        // assign position to left side of masking box
                        screenPos.x = maskBoundLeft;
                    }


                }
            }
        }

        // // space/distance around the coordinates
        float distToTop = screenBoundTop - screenPos.y - screenBoundBottom;
        // float distToRight = screenBoundRight - screenPos.x - screenBoundLeft;
        // float distToLeft = screenPos.x - screenBoundLeft;
        float distToBottom = screenPos.y - screenBoundBottom;


        // // setting the pivots
        // if (distToLeft > distToRight)
        //     pivotX = 0;
        // else pivotX = 1;

        // if (distToTop > distToBottom)
        //     pivotY = 0;
        // else pivotY = 1;

        // assigning the pivot
        dialogueBoxGroup.pivot = new Vector2(pivotX, pivotY);

        // assigning the position
        dialogueBoxGroup.anchoredPosition = new Vector2(screenPos.x, screenPos.y);

    }

    private void CalculateMaskBoxBounds()
    {
        // bounds of the player collider
        Vector3 pC = playerCollider.bounds.center;
        Vector3 pE = playerCollider.bounds.extents;

        Vector3 npC = CurrentInteractingNPCCollider.bounds.center;
        Vector3 npE = CurrentInteractingNPCCollider.bounds.extents;

        // player collider bounds in world space
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

        // current npc collider bounds in world space
        Vector3[] npCornersWS = new[]
        {
            new Vector3( npC.x + npE.x, pC.y + npE.y, npC.z + npE.z ),
            new Vector3( npC.x + npE.x, pC.y + npE.y, npC.z - npE.z ),
            new Vector3( npC.x + npE.x, pC.y - npE.y, npC.z + npE.z ),
            new Vector3( npC.x + npE.x, pC.y - npE.y, npC.z - npE.z ),
            new Vector3( npC.x - npE.x, pC.y + npE.y, npC.z + npE.z ),
            new Vector3( npC.x - npE.x, pC.y + npE.y, npC.z - npE.z ),
            new Vector3( npC.x - npE.x, pC.y - npE.y, npC.z + npE.z ),
            new Vector3( npC.x - npE.x, pC.y - npE.y, npC.z - npE.z )
        };

        // bounds corners positions in canvas space
        Vector2[] pCornersCS = new Vector2[8];
        Vector2[] npCornersCS = new Vector2[8];

        // convert world corner points to screen corners and scale
        for (int i = 0; i < pCornersWS.Length; i++)
        {
            pCornersCS[i] = WorldToCanvasPoint(pCornersWS[i]);
            npCornersCS[i] = WorldToCanvasPoint(npCornersWS[i]);
        }

        // initialize canvas space bounds
        maskBoundRight = pCornersCS[0].x;
        maskBoundLeft = pCornersCS[0].x;
        maskBoundBottom = pCornersCS[0].y;
        maskBoundTop = pCornersCS[0].y;

        // find extremes
        // note that it starts at 0 because we need to compare with npc points
        for (int i = 0; i < pCornersCS.Length; i++)
        {
            // find max across all points x
            if (pCornersCS[i].x > maskBoundRight)
                maskBoundRight = pCornersCS[i].x;
            if (npCornersCS[i].x > maskBoundRight)
                maskBoundRight = npCornersCS[i].x;

            // find min across all points x
            if (pCornersCS[i].x < maskBoundLeft)
                maskBoundLeft = pCornersCS[i].x;
            if (npCornersCS[i].x < maskBoundLeft)
                maskBoundLeft = npCornersCS[i].x;

            // find max across all points y
            if (pCornersCS[i].y > maskBoundTop)
                maskBoundTop = pCornersCS[i].y;
            if (npCornersCS[i].y > maskBoundTop)
                maskBoundTop = npCornersCS[i].y;

            // find min across all points y
            if (pCornersCS[i].y < maskBoundBottom)
                maskBoundBottom = pCornersCS[i].y;
            if (npCornersCS[i].y < maskBoundBottom)
                maskBoundBottom = npCornersCS[i].y;
        }

        // maskBoundBottom *=  uiCanvas.scaleFactor;
        // maskBoundRight *= uiCanvas.scaleFactor;
        // maskBoundTop *= uiCanvas.scaleFactor;
        // maskBoundLeft *= uiCanvas.scaleFactor;

        // draw placeholder to debug
        if (debug)
        {
            if (!placeHolderBox)
            {
                placeHolderBox = new GameObject("debug placeholder box");
                placeHolderBox.transform.parent = uiCanvas.transform;
                placeHolderBoxRect = placeHolderBox.AddComponent<RectTransform>();
                placeHolderImg = placeHolderBox.AddComponent<ProceduralImage>();
                placeHolderImg.BorderWidth = 3;
                placeHolderBoxRect.pivot = new Vector2(0, 0);
                placeHolderBoxRect.anchorMin = new Vector2(0, 0);
                placeHolderBoxRect.anchorMax = new Vector2(0, 0);
            }

            placeHolderBoxRect.anchoredPosition = new Vector2(maskBoundLeft, maskBoundBottom);

            placeHolderBoxRect.SetSizeWithCurrentAnchors(   RectTransform.Axis.Horizontal, 
                                                            (maskBoundRight - maskBoundLeft) * uiCanvas.scaleFactor);
            placeHolderBoxRect.SetSizeWithCurrentAnchors(   RectTransform.Axis.Vertical, 
                                                            (maskBoundTop - maskBoundBottom) * uiCanvas.scaleFactor);
        }
    }

    private void GetScreenBoundsWithMargins()
    {
        screenBoundTop = uiCanvas.GetComponent<RectTransform>().rect.height - screenMargin;
        screenBoundRight = uiCanvas.GetComponent<RectTransform>().rect.width - screenMargin;
        screenBoundBottom = screenMargin;
        screenBoundLeft = screenMargin;
    }

    private void TrackDialogueBoxBounds()
    {

        // pivot horizontal
        if (dialogueBoxGroup.pivot.x < 0.5f)
        {
            dialogueBoxLeft = dialogueBoxGroup.anchoredPosition.x;
            dialogueBoxRight = dialogueBoxGroup.anchoredPosition.x + dialogueBoxGroup.rect.width;
        }
        else
        {
            dialogueBoxLeft = dialogueBoxGroup.anchoredPosition.x - dialogueBoxGroup.rect.width;
            dialogueBoxRight = dialogueBoxGroup.anchoredPosition.x;
        }

        // pivot vertical
        if (dialogueBoxGroup.pivot.y < 0.5f)
        {
            dialogueBoxTop = dialogueBoxGroup.anchoredPosition.y + dialogueBoxGroup.rect.height;
            dialogueBoxBottom = dialogueBoxGroup.anchoredPosition.y;
        }
        else
        {
            dialogueBoxTop = dialogueBoxGroup.anchoredPosition.y;
            dialogueBoxBottom = dialogueBoxGroup.anchoredPosition.y - dialogueBoxGroup.rect.height;
        }
    }

    private void RepositionDialogueBoxOnScreen()
    {
        // declaring new field with default values
        float newX = dialogueBoxGroup.anchoredPosition.x;
        float newY = dialogueBoxGroup.anchoredPosition.y;

        // GENERAL PLACING ACCORDING TO PIVOT
        // rules for left pivot
        if (dialogueBoxGroup.pivot.x < 0.5)
        {
            if (dialogueBoxRight > screenBoundRight)
                newX = dialogueBoxGroup.anchoredPosition.x - (dialogueBoxRight - screenBoundRight);

            if (dialogueBoxLeft < screenBoundLeft)
                newX = screenBoundLeft;
        }
        // rules for right pivot
        else
        {
            if (dialogueBoxRight > screenBoundRight)
                newX = screenBoundRight;

            if (dialogueBoxLeft < screenBoundLeft)
                newX = screenBoundLeft + dialogueBoxGroup.rect.width;
        }

        // rules for bottom pivot
        if (dialogueBoxGroup.pivot.y < 0.5)
        {
            if (dialogueBoxTop > screenBoundTop)
             newY = dialogueBoxGroup.anchoredPosition.y - (dialogueBoxTop - screenBoundTop);

            if (dialogueBoxBottom < screenBoundBottom)
                newY = screenBoundBottom;
        }
        // rules for top pivot
        else
        {
            if (dialogueBoxTop > screenBoundTop)
                newY = screenBoundTop;
            
            if (dialogueBoxBottom < screenBoundBottom)
                newY = screenBoundBottom + dialogueBoxGroup.rect.height;
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
        
        if (dialogueAnchor) currentDialogueAnchor = dialogueAnchor;
        
        Vector2 screenPos = WorldToCanvasPoint(currentDialogueAnchor.position);
        screenPos.y += 100f;
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

        CalculateAndApplyBoxSize(messageSnippetBox, message.textContent);

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

        TextMeshProUGUI messageBoxTMP = messageBox.GetComponentInChildren<TextMeshProUGUI>();
        // messageBoxTMP.margin = new Vector4 (    (messageVerticalMargin / 3) * uiCanvas.scaleFactor,
        //                                         (messageHorizontalMargin / 3) * uiCanvas.scaleFactor,
        //                                         (messageVerticalMargin / 3) * uiCanvas.scaleFactor,
        //                                         (messageHorizontalMargin / 3) * uiCanvas.scaleFactor);

        int maxCharsFirstLine;
        int nbLines = (int)Mathf.Ceil(text.Length / maxCharsPerLine);
        Debug.Log(Mathf.Ceil(text.Length / maxCharsPerLine));
        Debug.Log(nbLines);

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
        // todo to be refactored in a dialogue box method
        playerCollider = FindObjectOfType<PlayerController>().GetComponent<CapsuleCollider>();
        GetScreenBoundsWithMargins();
        // todo end

        ClosePhoneUI();
        CloseControlsHelp();
        HideInteractionButton();

        if (showPhoneAtStart) InitializePhoneIndicator();
        else ClosePhoneIndicator();

        if (showControlsPromptAtStart) OpenControlsPrompt();

        if (showDialogueBoxAtStart) OpenDialogueBox();
        else CloseDialogueBox();

        // todo refactor & correct
        // if (playerDialogueAnchor != null && dialogueAnchor != null)
        //     currentDialogueAnchor = playerDialogueAnchor;
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
        float x = coordinate.x / Camera.main.pixelWidth / (screenBoundRight + screenMargin);
        float y = coordinate.y / Camera.main.pixelHeight / (screenBoundTop + screenMargin);

        return new Vector2(x, y);
    }

    private Vector2 WorldToCanvasPoint(Vector3 position)
    {
        Vector2 coordinate = Camera.main.WorldToScreenPoint(position);

        float x = coordinate.x / (Camera.main.pixelWidth / (screenBoundRight + screenMargin));
        float y = coordinate.y / (Camera.main.pixelHeight / (screenBoundTop + screenMargin));

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
                if (dialogueAnchor) currentDialogueAnchor = dialogueAnchor;
                else currentDialogueAnchor = otherDialogueAnchor;
            }
        }
    }
}

#endregion