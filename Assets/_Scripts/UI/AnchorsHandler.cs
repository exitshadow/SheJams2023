using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class AnchorsHandler : MonoBehaviour
{
    [SerializeField] private Transform _playerDialogueAnchor;
    public Transform TargetDialogueAnchor { get; private set; }
    public Transform CurrentDialogueAnchor { get; private set; }
    public Transform PlayerDialogueAnchor { get { return _playerDialogueAnchor; } }

    [SerializeField] private TextMeshProUGUI speakerName;

    [Header("Debug options")]
    [SerializeField] private bool debug;
    private GameObject debugGO;
    private DialogueRunner dialogueRunner;


    #region unity lifecycle
    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    void Awake()
    {
        if (debug) debugGO = new GameObject("DEBUG ANCHOR POSITION");
        CurrentDialogueAnchor = PlayerDialogueAnchor;
    }

    void Update()
    {
        if (debug) debugGO.transform.position = CurrentDialogueAnchor.position;
    }
    #endregion

    public void PlaceAnchors(string speakerName)
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
        Debug.Log($"setting target dialogue anchor");
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
            PlaceAnchors(speakerName.text);

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