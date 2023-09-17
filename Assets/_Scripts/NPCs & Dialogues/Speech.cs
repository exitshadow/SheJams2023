using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;


[RequireComponent(typeof(NPC))]
public class Speech : MonoBehaviour
{
    private NPC npc;
    private DialogueRunner dialogueRunner;
    private AudioSource audioSource;
    private bool useIntonationClipDefaults;
    private bool useTypeWritingClipDefaults;
    [SerializeField] private SpeechDefaults speechDefaults;
    [SerializeField] private List<AudioClip> intonationClips;
    [SerializeField] private List<AudioClip> typewritingClips;

    private int currentSpeechClipIndex = 0;

    void OnEnable()
    {
    }

    void OnDisable()
    {
        speechDefaults.onCharacterTyped -= PlaySpeechSounds;
    }

    void Awake()
    {
        if (intonationClips.Count == 0) useIntonationClipDefaults = true;
        else useIntonationClipDefaults = false;
        Debug.Log(useIntonationClipDefaults + " " + gameObject.name);

        if (typewritingClips.Count == 0) useTypeWritingClipDefaults = true;
        else useIntonationClipDefaults = false;
    }

    void Start()
    {
        npc = GetComponent<NPC>();
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        npc.onDialogueStarted.AddListener(PlayIntonationSound);

        audioSource = speechDefaults.audioSource;
        speechDefaults.onCharacterTyped += PlaySpeechSounds;
    }

    public void PlayIntonationSound()
    {
        if (useIntonationClipDefaults) speechDefaults.PlayDefaultIntonation(0);
        else speechDefaults.PlayClip(intonationClips[0]);
    }

    [YarnCommand("play_intonation_sound")]
    public void PlayIntonationSound(int index)
    {
        if (useIntonationClipDefaults) speechDefaults.PlayDefaultIntonation(index);
        else speechDefaults.PlayClip(intonationClips[index]);   
    }

    public void PlaySpeechSounds()
    {
        if (dialogueRunner.CurrentNodeName != npc.DialogueNode) return;

        if (audioSource.isPlaying)
        {
            Debug.Log(audioSource.clip);
            foreach (AudioClip intonationClip in intonationClips)
                if (audioSource.clip == intonationClip) return;
        }

        if (useTypeWritingClipDefaults)
        {
            speechDefaults.PlayDefaultTypewriter();
            return;
        }

        if (currentSpeechClipIndex < typewritingClips.Count)
        {
            audioSource.clip = typewritingClips[currentSpeechClipIndex];
            audioSource.Play();
            currentSpeechClipIndex++;
        }
        else currentSpeechClipIndex = 0;
    }
}
