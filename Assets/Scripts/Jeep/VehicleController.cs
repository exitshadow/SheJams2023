using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    public enum TransmissionType { frontTransmission, backTransmission, fourByFour };

    #region input controls
    private ImanActions actions;
    private InputAction moveVehicule;
    private InputAction useBrakes;
    private InputAction honk;
    #endregion

    [Header("References")]
    [SerializeField] private WheelCollider wheelFL;
    [SerializeField] private WheelCollider wheelFR;
    [SerializeField] private WheelCollider wheelRL;
    [SerializeField] private WheelCollider wheelRR;

    [SerializeField] private Transform wheelMeshFL;
    [SerializeField] private Transform wheelMeshFR;
    [SerializeField] private Transform wheelMeshRL;
    [SerializeField] private Transform wheelMeshRR;

    [SerializeField] private Transform centerOfMass;

    private WheelCollider[] wheels;
    private Transform[] wheelTransforms;

    [SerializeField] private Rigidbody rb;

    [Header("Controller Settings")]
    [SerializeField] private float accelerationStrength = 1f;
    [SerializeField] private float steeringStrength = 1f;
    [SerializeField] private float steerBackStrength = 1f;

    private float accelerationValue;
    private float steeringValue;

    [Header("Vehicle Settings")]
    [SerializeField] private TransmissionType transmissionType = TransmissionType.fourByFour;
    [SerializeField] private float motorPower;
    [SerializeField] [Tooltip("Distance between front and back wheels")]private float wheelBase;
    [SerializeField] [Tooltip("Distance between left and right back wheels")] private float rearTrack;
    [SerializeField] [Tooltip("The turn radius of the vehicle")] private float turnRadius;

    private void Awake()
    {
        InitializeInputActions();
        InitializeVehicleObjects();
    }

    private void Update()
    {
        accelerationValue = moveVehicule.ReadValue<Vector2>().y * accelerationStrength;
        steeringValue = moveVehicule.ReadValue<Vector2>().x * steeringStrength;

        SetAcceleration();
        SetAckermannSteering();
        UpdateWheelPoses();
    }

    #region wheels animation
    private void UpdateWheelPoses()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheelTransforms[i].SetPositionAndRotation(pos, rot);
        }
    }
    #endregion

    #region acceleration calculations
    private void SetAcceleration()
    {
        float dir = 0;
        if (accelerationValue > 0) dir = 1;
        else if (accelerationValue < 0) dir = -1;

        switch (transmissionType)
        {
            // adds torque to the front wheels
            case TransmissionType.frontTransmission:
                for (int i = 0; i < 2; i++)
                {
                    wheels[i].motorTorque = motorPower * 2.5f / 4f * dir;
                }
                break;
            // adds torque to the back wheels
            case TransmissionType.backTransmission:
                for (int i = 2; i < wheels.Length; i++)
                {
                    wheels[i].motorTorque = motorPower * 2.5f / 4f * dir;
                }
                break;
            // adds torque to all wheels
            case TransmissionType.fourByFour:
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].motorTorque = motorPower * 5f / 4f * dir;
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region steering calculations
    private void SetAckermannSteering()
    {
        float maxAngleLeft = 0;
        float maxAngleRight = 0;

        // turning right
        if (steeringValue > 0)
        {
            maxAngleLeft = CalculateAckermannSteering(true, false, steeringValue);
            maxAngleRight = CalculateAckermannSteering(true, true, steeringValue);
        }

        // turning left
        if (steeringValue < 0)
        {
            maxAngleRight = CalculateAckermannSteering(false, true, steeringValue);
            maxAngleLeft = CalculateAckermannSteering(false, false, steeringValue);
        }

        wheelFL.steerAngle = maxAngleLeft;
        wheelFR.steerAngle = maxAngleRight;
    }

    // todo
    private float ApplySteeringCurve(WheelCollider targetWheel, float angle)
    {
        targetWheel.GetWorldPose(out Vector3 pos, out Quaternion rot);
        float yRot = rot.eulerAngles.y;
        return Mathf.Lerp(yRot, angle, steerBackStrength * Time.fixedDeltaTime * steeringValue);
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
    #endregion

    #region initializers
    private void InitializeVehicleObjects()
    {
        wheels = new WheelCollider[] { wheelFL, wheelFR, wheelRL, wheelRR };
        wheelTransforms = new Transform[] { wheelMeshFL, wheelMeshFR, wheelMeshRL, wheelMeshRR };
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void InitializeInputActions()
    {
        actions = new ImanActions();
        useBrakes = actions.Player.Interact;
        moveVehicule = actions.Player.Move;
        honk = actions.Player.PickUpPhone;

        useBrakes.Enable();
        moveVehicule.Enable();
        honk.Enable();
    }
    #endregion
}
