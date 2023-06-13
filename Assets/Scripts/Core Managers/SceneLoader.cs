using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float transitionTime = 1f;
    // public Animator transition;
    // public AudioMixerSnapshot loadingAudioMixer;
    // public AudioMixerSnapshot walkingAudioMixer;

    public void StartGame()
    {
        Debug.Log("Start!");
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        // transition.SetTrigger("Start");
        // loadingAudioMixer.TransitionTo(0.5f);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
        // walkingAudioMixer.TransitionTo(0.5f);
    }

    public void MainMenu()
    {
        Debug.Log("Menu");
        StartCoroutine(LoadScene(0));
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
