using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class SpeechDefaults : MonoBehaviour
{
    public AudioSource audioSource;
    // [SerializeField] private AudioClip ImanIntonationClip;
    [SerializeField] private List<AudioClip> defaultIntonationClips;
    [SerializeField] private List<AudioClip> defaultTypewritingClips;
    private int currentTypewriterClipIndex = 0;
    public event Action onCharacterTyped;
    // [SerializeField] private TextMeshProUGUI dialogueSpeakerNameTMP;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayDefaultIntonation(int index)
    {
        if (audioSource.isPlaying) return;
        // if (dialogueSpeakerNameTMP.text == "Iman"){
        //     PlayImanIntonation();
        //     return;
        // }
        
        audioSource.clip = defaultIntonationClips[index];
        audioSource.Play();
    }

    // public void PlayImanIntonation()
    // {
    //     audioSource.clip = ImanIntonationClip;
    //     audioSource.Play();
    // }

    public void PlayDefaultTypewriter()
    {
        if (audioSource.isPlaying) return;
        if (currentTypewriterClipIndex < defaultTypewritingClips.Count)
        {
            audioSource.clip = defaultTypewritingClips[currentTypewriterClipIndex];
            audioSource.Play();
            currentTypewriterClipIndex++;
        }
        else currentTypewriterClipIndex = 0;
    }

    //! manually subscribe to the yarn Line View as we’re using their defaults :facepalm:
    public void OnCharacterTyped()
    {
        onCharacterTyped?.Invoke();
    }
}