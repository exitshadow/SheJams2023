using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI.ProceduralImage;


public class DialogueBoxUI : MonoBehaviour
{
    [Header("Dialogue Box References")]
    [SerializeField] private RectTransform dialogueBoxGroup;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerNameTMP;
    [SerializeField] private TextMeshProUGUI dialogueContentTMP;
    [SerializeField] private Collider playerCollider;
    [HideInInspector] public Collider currentInteractingNPCCollider;

    [Header("Dialogue Box Color Change by Interlocutor")]
    [SerializeField] private TextMeshProUGUI dialogueSpeakerNameSuperTMP;
    [SerializeField] private ProceduralImage dialogueSpeakerNameBackground;
    [SerializeField] private ProceduralImage dialogueContentBackground;
    [SerializeField] private Color dialogueSpeakerSuperColorChange;
    [SerializeField] private Color dialogueSpeakerSubColorChange;
    [SerializeField] private Color dialogueSpeakerBckgColorChange;
    [SerializeField] private Color dialogueContentTextColorChange;
    [SerializeField] private Color dialogueContentBckgColorChange;
    private Color dialogueSpeakerNameSuperColor;
    private Color dialogueSpeakerNameSubColor;
    private Color dialogueSpeakerNameBckgColor;
    private Color dialogueContentTextColor;
    private Color dialogueContentBackgroundtColor;

    [Header("Dialogue Box Dynamic Placement")]
    [Tooltip("Safe margin area between the dialogue box and the screen limits")]
    [SerializeField] private float screenMargin = 45f;
    [SerializeField] private float screenMarginY = 100f;
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

    private Canvas canvas;
    private AnchorsHandler anchorsHandler;

    [Header("Options")]
    [SerializeField] private bool showDialogueAtStart;
    [SerializeField] private bool debug;

    #region Unity Messages
    void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        anchorsHandler = FindFirstObjectByType<AnchorsHandler>();

        RegisterColorScheme();

        playerCollider = FindFirstObjectByType<PlayerController>().GetComponent<Collider>();
        GetScreenBoundsWithMargins();

        if (showDialogueAtStart) OpenDialogueBox();
        else CloseDialogueBox();

        playerCollider = FindObjectOfType<PlayerController>().GetComponent<CapsuleCollider>();
        GetScreenBoundsWithMargins();

        RegisterColorScheme();
    }

    void Update()
    {
        #region place dialogue box on screen
        if (anchorsHandler.CurrentDialogueAnchor != null)
        {
            PlaceDialogueBoxInScreen();
            TrackDialogueBoxBounds();
            RepositionDialogueBoxOnScreen();
        }
        #endregion
    }
    #endregion

    #region trigger methods
    public void TriggerPop()
    {
        dialogueBoxGroup.GetComponent<Animator>().SetTrigger("Pop");
        SetColorScheme();
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

    public void UpdateDialogueLine(string speakerName, string dialogueLine)
    {
        dialogueSpeakerNameTMP.text = speakerName;
        dialogueContentTMP.text = dialogueLine;
    }
    #endregion

    #region manage colors
    private void RegisterColorScheme()
    {
        dialogueSpeakerNameSuperColor = dialogueSpeakerNameSuperTMP.color;
        dialogueSpeakerNameSubColor = dialogueSpeakerNameTMP.color;
        dialogueSpeakerNameBckgColor = dialogueSpeakerNameBackground.color;

        dialogueContentTextColor = dialogueContentTMP.color;
        dialogueContentBackgroundtColor = dialogueContentBackground.color;
    }

    private void ChangeColorScheme( Color speakerSuperColor,
                                    Color speakerSubColor,
                                    Color speakerBckgColor,
                                    Color textColor,
                                    Color backgroundColor)
    {
        dialogueSpeakerNameSuperTMP.color = speakerSuperColor;
        dialogueSpeakerNameTMP.color= speakerSubColor;
        dialogueSpeakerNameBackground.color = speakerBckgColor;

        dialogueContentBackground.color = backgroundColor;
        dialogueContentTMP.color = textColor;
    }

    public void SetColorScheme()
    {
        // change color scheme
        if (dialogueSpeakerNameTMP.text == "Iman")
        {
            ChangeColorScheme(  dialogueSpeakerSuperColorChange,
                                dialogueSpeakerSubColorChange,
                                dialogueSpeakerBckgColorChange,
                                dialogueContentTextColorChange,
                                dialogueContentBckgColorChange);
        }
        else
        {
            ChangeColorScheme(  dialogueSpeakerNameSuperColor,
                                dialogueSpeakerNameSubColor,
                                dialogueSpeakerNameBckgColor,
                                dialogueContentTextColor,
                                dialogueContentBackgroundtColor);
        }
    }
    #endregion

    #region dynamic dialogue box placing
        public void PlaceDialogueBoxInScreen()
    {
        // initial values;
        Vector2 screenPos = UIManager.WorldToCanvasPoint(   anchorsHandler.CurrentDialogueAnchor.position,
                                                            screenBoundRight, screenBoundTop,
                                                            0, 0);

        float pivotX = 0;
        float pivotY = 0;

        // PLACING AFTER CONTROLLING FOR MASKING BOX
        if (playerCollider && currentInteractingNPCCollider)
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

        Vector3 npC = currentInteractingNPCCollider.bounds.center;
        Vector3 npE = currentInteractingNPCCollider.bounds.extents;

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
            pCornersCS[i] = UIManager.WorldToCanvasPoint(   pCornersCS[i],
                                                            screenBoundRight, screenBoundTop,
                                                            0, 0);

            npCornersCS[i] = UIManager.WorldToCanvasPoint(   npCornersCS[i],
                                                            screenBoundRight, screenBoundTop,
                                                            0, 0);
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
                placeHolderBox.transform.parent = canvas.transform;
                placeHolderBoxRect = placeHolderBox.AddComponent<RectTransform>();
                placeHolderImg = placeHolderBox.AddComponent<ProceduralImage>();
                placeHolderImg.BorderWidth = 3;
                placeHolderBoxRect.pivot = new Vector2(0, 0);
                placeHolderBoxRect.anchorMin = new Vector2(0, 0);
                placeHolderBoxRect.anchorMax = new Vector2(0, 0);
            }

            placeHolderBoxRect.anchoredPosition = new Vector2(maskBoundLeft, maskBoundBottom);

            placeHolderBoxRect.SetSizeWithCurrentAnchors(   RectTransform.Axis.Horizontal, 
                                                            (maskBoundRight - maskBoundLeft) * canvas.scaleFactor);
            placeHolderBoxRect.SetSizeWithCurrentAnchors(   RectTransform.Axis.Vertical, 
                                                            (maskBoundTop - maskBoundBottom) * canvas.scaleFactor);
        }
    }

    private void GetScreenBoundsWithMargins()
    {
        screenBoundTop = canvas.GetComponent<RectTransform>().rect.height - screenMarginY;
        screenBoundRight = canvas.GetComponent<RectTransform>().rect.width - screenMargin;
        screenBoundBottom = screenMarginY;
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
}
