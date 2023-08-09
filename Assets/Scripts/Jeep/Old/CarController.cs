using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WheelPhysics wheelFR;
    [SerializeField] private WheelPhysics wheelFL;
    [SerializeField] private WheelPhysics wheelRR;
    [SerializeField] private WheelPhysics wheelRL;
    [SerializeField] private Rigidbody rbJeep;
    [SerializeField] private Transform centerOfMass;

    [Header("Controller Settings")]
    [SerializeField] private float steeringSpeed;
    [SerializeField] private float accelerationStrength = 1.5f;

    [Header("Vehicle Specifications (in meters)")]
    [SerializeField] [Tooltip("Distance between front and back wheels")] private float wheelBase;
    [SerializeField] [Tooltip("Distance between left and right back wheels")] private float rearTrack;
    [SerializeField] [Tooltip("The turn radius of the vehicle")] private float turnRadius;

    [Header("Values Monitoring")]
    public float wheelAngleLeft;
    public float wheelAngleRight;

    private ImanActions actions;
    private InputAction moveAction;
    private InputAction breakAction;

    private float steeringValue;
    private float accelerationValue;

    private WheelPhysics[] wheels;

    private void Awake()
    {
        actions = new ImanActions();
        breakAction = actions.Player.Interact;
        moveAction = actions.Player.Move;

        breakAction.Enable();
        moveAction.Enable();

        wheels = new WheelPhysics[] { wheelFL, wheelFR, wheelRL, wheelRR };
        rbJeep.centerOfMass = centerOfMass.transform.localPosition;
    }

    void Update()
    {
        accelerationValue = moveAction.ReadValue<Vector2>().y * accelerationStrength;
        steeringValue = moveAction.ReadValue<Vector2>().x * steeringSpeed;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].verticalInput = accelerationValue;
        }

        // turning right
        if (steeringValue > 0)
        {
            wheelAngleLeft = CalculateAckermannSteering(true, false, steeringValue);
            wheelAngleRight = CalculateAckermannSteering(true, true, steeringValue);
        }
        // turning left
        else if (steeringValue < 0)
        {
            wheelAngleLeft = CalculateAckermannSteering(false, false, steeringValue);
            wheelAngleRight = CalculateAckermannSteering(false, true, steeringValue);
        }
        // straight
        else
        {
            wheelAngleLeft = 0;
            wheelAngleRight = 0;
        }

        wheelFR.steerAngle = wheelAngleRight;
        wheelFL.steerAngle = wheelAngleLeft;
    }

    private float CalculateAckermannSteering(bool isSteeringRight, bool isRightWheel, float steerValue)
    {
        float dir = 1;
        float wheel = 1;

        if (!isSteeringRight) dir = -1;
        if (!isRightWheel) dir = -1;

        float fac = dir * wheel;

        return Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (fac * rearTrack / 2)) ) * steerValue ;
    }

}
