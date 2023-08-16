using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// contains the lines that a NPC can say
/// </summary>
[CreateAssetMenu(menuName = "Handiman Data Assets/NPC Dialogue Asset", fileName = "NPC Dialogue Asset")]
public class NPCDialogueAsset : ScriptableObject
{
    [Header("Quest start dialogue")]
    public List<DialogueSegment> questStartingDialogueSegments = new List<DialogueSegment>();
    public List<DialogueSegment> questWaitingDialogueSegments = new List<DialogueSegment>();

    [Header("Quest end dialogue")]
    public List<DialogueSegment> questProgressingDialogueSegments = new List<DialogueSegment>();
    public List<DialogueSegment> questEndingDialogueSegments = new List<DialogueSegment>();

    [System.Serializable]
    public struct DialogueSegment{
        [Tooltip("Please specify the speaker's name every time")]
        public string speakerName;
        public string dialogueText;
    }
}
