using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(NPC))]
public class Speech : MonoBehaviour
{
    private NPC npc;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> intonationClips;
    [SerializeField] private List<AudioClip> speechClips;

    private int currentSpeechClipIndex = 0;

    void Awake()
    {
        npc = GetComponent<NPC>();
    }

    [YarnCommand("play_intonation_sound")]
    public void PlayIntonationSound(int index)
    {
        audioSource.Stop();
        audioSource.clip = intonationClips[index];
        audioSource.Play();
    }

    public void PlaySpeechSounds()
    {
        if (audioSource.isPlaying)
        {
            Debug.Log(audioSource.clip);
            foreach (AudioClip intonationClip in intonationClips)
                if (audioSource.clip == intonationClip) return;
        }

        if (currentSpeechClipIndex < speechClips.Count)
        {
            audioSource.clip = speechClips[currentSpeechClipIndex];
            audioSource.Play();
            currentSpeechClipIndex++;
        }
        else currentSpeechClipIndex = 0;
    }
}
