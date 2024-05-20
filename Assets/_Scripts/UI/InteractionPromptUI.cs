using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionPromptGroup;
    [SerializeField] private TextMeshProUGUI interactionPromptTMP;

    private AnchorsHandler anchorsHandler;

    #region unity lifecycle
    void Awake()
    {
        anchorsHandler = FindFirstObjectByType<AnchorsHandler>();
    }

    void Start()
    {
        HideInteractionButton();
    }

    void Update()
    {
        if (anchorsHandler.CurrentDialogueAnchor != null)
        {
            PlaceInteractionButtonOnScreen();
        }
    }
    #endregion

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
        
        //if (dialogueAnchor) currentDialogueAnchor = dialogueAnchor;
        
        Vector2 screenPos = UIManager.WorldToCanvasPoint( anchorsHandler.CurrentDialogueAnchor.position, 50, 50, 0, 0);
        screenPos.y += 100f;
        interactionPromptGroup.GetComponent<RectTransform>().anchoredPosition = screenPos;
    }
}
