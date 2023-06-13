using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaintingRoomBehaviour : MonoBehaviour
{
    public Transform cube;
    public List<NavMeshAgent> listCharacters;
    public List<GameObject> listDestinations;
    private Bounds floor;
    
    // Start is called before the first frame update
    void Start()
    {
        floor = GetComponent<Renderer>().bounds;
        foreach(NavMeshAgent character in listCharacters)
        {
            character.GetComponent<WaitingRoomCharacter>().waintingRoomBehaviour = this;
            SetRandomPosition(character);
        }
    }

    private void SetRandomPosition(NavMeshAgent character)
    {   
        //float rX = Random.Range(floor.min.x, floor.max.x);
        //float rZ = Random.Range(floor.min.z, floor.max.z);

        Vector3 randomPoint =  new Vector3((floor.center.x + Random.Range(floor.min.x, floor.max.x)),0,(floor.center.z + Random.Range(floor.min.z, floor.max.z)));

        character.transform.position = randomPoint;
        //Debug.Log("RandomPoint spawn : " + randomPoint);

       // character.transform.position = new Vector3(rX, transform.position.y, rZ);
        character.transform.rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

        // character.transform.position = transform.position;
        // character.transform.rotation = transform.rotation;

        SetRandomDestination(character, FindGameObjectInChildWithTag(character.gameObject,"destination"));
    } 

    public void SetRandomDestination(NavMeshAgent character, Transform destination)
    {
        // float rX = Random.Range(floor.min.x, floor.max.x);
        // float rZ = Random.Range(floor.min.z, floor.max.z);
        Vector3 randomPoint =  new Vector3((floor.center.x + Random.Range(floor.min.x, floor.max.x)),0,(floor.center.z + Random.Range(floor.min.z, floor.max.z)));

        //Vector3 point = new Vector3(rX, transform.position.y, rZ);
        cube.position = randomPoint;
        Debug.Log(destination.name);
        destination.position = new Vector3((floor.center.x + Random.Range(floor.min.x, floor.max.x)),0,(floor.center.z + Random.Range(floor.min.z, floor.max.z)));
        //character.SetDestination(randomPoint);
    } 

    public Transform FindGameObjectInChildWithTag (GameObject parent, string tag)
    {
        Transform t = parent.transform;
        for (int i = 0; i < t.childCount; i++)
        {
            if(t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).transform;
            }
        }
        return null;
    }
}
