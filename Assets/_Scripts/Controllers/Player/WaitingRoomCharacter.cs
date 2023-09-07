using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitingRoomCharacter : MonoBehaviour
{
    public WaintingRoomBehaviour waintingRoomBehaviour;
    private Transform destination;
    private NavMeshAgent character;
    void Start()
    {
        character = GetComponent<NavMeshAgent>();
    }
    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("other.name : " + other.name);
        // Debug.Log("other.transform.parent.name : " + other.transform.parent.name);
        // Debug.Log("transform.name : " + transform.name);
        if(other.transform.parent.name == transform.name)
        {
            Debug.Log("rentré");
            destination = other.transform;
            Invoke("SetDestination", 3f) ;
        }
    }

    void SetDestination()
    {
        waintingRoomBehaviour.SetRandomDestination(character,destination);
    }
}
