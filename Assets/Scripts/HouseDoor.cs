using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class HouseDoor : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            sceneLoader.LoadNextScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
