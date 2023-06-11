using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// contains the lines that a NPC can say
/// </summary>
[CreateAssetMenu(menuName = "Handiman Data Assets/NPC Dialogue Asset", fileName = "NPC Dialogue Asset")]
public class NPCDialogueAsset : ScriptableObject
{
    public string dialogueName;

    [Header("First dialogue")]
    public List<DialogueSegment> firstDialogueSegments = new List<DialogueSegment>();

    [Header("Quest dialogue")]
    public List<DialogueSegment> questDialogueSegments = new List<DialogueSegment>();
    public List<DialogueSegment> recapDialogueSegments = new List<DialogueSegment>();

    [Header("Quest done dialogue")]
    public List<DialogueSegment> endDialogueSegments = new List<DialogueSegment>();

    [System.Serializable]
    public struct DialogueSegment{
        public string dialogueText;
        // public List<DialogueChoice> dialogueChoices;
    }
}
