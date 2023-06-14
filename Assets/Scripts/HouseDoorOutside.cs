using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoorOutside : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sceneLoader.LoadScene(1);
        }
    }
}
