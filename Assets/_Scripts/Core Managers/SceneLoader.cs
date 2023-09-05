using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

///<summary>
/// This needs to be put in each scene of the build. In build settings, order the index acordingly (menu at index 0, etc.)
///<\summary>

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private Animator transitionAnim;
    [SerializeField] private AudioMixerSnapshot loadingAudioMixer;
    [SerializeField] private AudioMixerSnapshot walkingAudioMixer;
    public event Action onLoadScene;


    public void StartGame()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public IEnumerator LoadScene(int sceneIndex)
    {
        onLoadScene?.Invoke();
        transitionAnim.SetTrigger("StartTrigger");
        loadingAudioMixer.TransitionTo(0.5f);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
        walkingAudioMixer.TransitionTo(0.5f);
    }

    public void MainMenu()
    {
        StartCoroutine(LoadScene(0));
    }

    public void Credits()
    {
        StartCoroutine(LoadScene(3));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
