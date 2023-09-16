using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class WheelPhysicsSettings : MonoBehaviour
{
    [Header("References")]

    [Tooltip("The rigibody of the jeep")]
    public Rigidbody jeepRb;

    [Tooltip("The visual representation of the wheel")]
    public GameObject wheelObject;

    [Header("Suspension Settings")]
    public float restLength = .5f;
    public float springTravel = .3f;
    public float springStiffness = 100f;
    public float damperStiffness = 400f;

    [Header("Steering Settings")]
    public float steerBackTime = 1f;

    [Header("Wheel Settings")]
    public float wheelRadius = .3f;

    private WheelPhysics p;

    void Awake()
    {
        p = GetComponent<WheelPhysics>();
    }

    void OnDrawGizmos()
    {
        // drawing the rest length
        Gizmos.color = Color.red;
        Gizmos.DrawLine(    transform.position,
                            transform.position + Vector3.down * restLength  );

        // drawing the wheel
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(  transform.position + Vector3.down * (restLength - wheelRadius),
                                wheelRadius );

    #if UNITY_EDITOR
        if (!EditorApplication.isPlaying) return;
    #endif

        // drawing debug raycasts
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(p.hitInfo.point, transform.position);
        Gizmos.DrawSphere(p.hitInfo.point, .1f);

        // drawing steering direction
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + Vector3.down * restLength - transform.forward * .5f,
                        transform.position + Vector3.down * restLength + transform.forward * .5f);

    }


}
