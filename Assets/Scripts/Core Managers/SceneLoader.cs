using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

///<summary>
/// This needs to be put in each scene of the build. In build settings, order the index acordingly (menu at index 0, etc.)
///<\summary>

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private Animator transitionAnim;
    [SerializeField] private AudioMixerSnapshot loadingAudioMixer;
    [SerializeField] private AudioMixerSnapshot walkingAudioMixer;

    public void StartGame()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
