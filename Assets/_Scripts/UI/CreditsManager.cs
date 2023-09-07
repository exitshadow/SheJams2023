using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

    void Start()
    {
        StartCoroutine(WaitingForEndCredits(53));
    }

    IEnumerator WaitingForEndCredits(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        sceneLoader.MainMenu();
    }
}
