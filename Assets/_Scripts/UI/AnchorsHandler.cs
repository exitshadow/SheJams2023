using TMPro;
using UnityEngine;

public class AnchorsHandler : MonoBehaviour
{
    [SerializeField] private Transform _playerDialogueAnchor;
    public Transform TargetDialogueAnchor { get; private set; }
    public Transform CurrentDialogueAnchor { get; private set; }
    public Transform PlayerDialogueAnchor { get { return _playerDialogueAnchor; } }

    [SerializeField] TextMeshProUGUI speakerName;

    #region unity lifecycle
    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }
    #endregion

    public void SetAnchors(string speakerName)
    {
        // place anchors
        if (CurrentDialogueAnchor == null) return;

        if (speakerName == "Iman" || speakerName == "Game Manager")
        {
            CurrentDialogueAnchor = _playerDialogueAnchor;
        }
        else
        {
            if (TargetDialogueAnchor == null) CurrentDialogueAnchor = _playerDialogueAnchor;
            else CurrentDialogueAnchor = TargetDialogueAnchor;
        }
    }

    public void SetPlayerAnchorAsTarget()
    {
        TargetDialogueAnchor = _playerDialogueAnchor;
    }

    public void SetTargetDialogueAnchor(Transform target)
    {
        TargetDialogueAnchor = target;
    }

    public void ClearDialogueAnchors()
    {
        TargetDialogueAnchor = null;
        CurrentDialogueAnchor = null;
    }

    private void OnTextChanged(Object objectChanged)
    {
        if (objectChanged == speakerName)
        {
            if (speakerName.text == "Iman")
            {
                CurrentDialogueAnchor = _playerDialogueAnchor;
            }
            else
            {
                if (!TargetDialogueAnchor) CurrentDialogueAnchor = _playerDialogueAnchor;
                else CurrentDialogueAnchor = TargetDialogueAnchor;
            }
        }
    }
}