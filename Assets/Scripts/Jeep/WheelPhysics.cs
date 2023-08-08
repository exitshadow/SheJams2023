using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WheelPhysicsSettings))]
public class WheelPhysics : MonoBehaviour
{
    [Header("Values Monitoring Only")]

    private WheelPhysicsSettings w;

    private float minLength;
    private float maxLength;
    private float lastLength;

    private float springLength;
    private float springVelocity;
    private float springForce;

    private float damperForce;
    private float forwardForce; // local forward axis
    private float rightForce; // local right / side axis

    private Vector3 suspensionForce;
    private Vector3 localVelocity;

    private float wheelAngle;


    public RaycastHit hitInfo;
    public float steerAngle;
    public float verticalInput;

    void Awake()
    {
        w = GetComponent<WheelPhysicsSettings>();
        minLength = w.restLength - w.springTravel;
        maxLength = w.restLength + w.springTravel;
    }

    void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, w.steerBackTime * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(    transform.position,
                                - transform.up,
                                out RaycastHit hit,
                                maxLength + w.wheelRadius   ))
        {
            hitInfo = hit;

            lastLength = springLength;

            springLength    = hit.distance - w.wheelRadius;
            springLength    = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity  = (lastLength - springLength) / Time.fixedDeltaTime;

            springForce     = w.springStiffness * (w.restLength - springLength);
            damperForce     = w.damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;            

            localVelocity = transform.InverseTransformDirection(w.jeepRb.GetPointVelocity(hit.point));

            forwardForce = verticalInput * springForce;
            rightForce = localVelocity.x * springForce;

            w.jeepRb.AddForceAtPosition(    suspensionForce + (forwardForce * transform.forward) 
                                            + (rightForce * -transform.right), // counteraction value
                                            hit.point   );
        }
    }
}
