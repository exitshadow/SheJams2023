using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VehicleController))]
public class VehicleAudio : MonoBehaviour
{
    [Header("Running Sound Settings")]
    [SerializeField] private AudioSource runningSound;
    [SerializeField] private float runningMaxVolume;
    [SerializeField] private float runningMaxPitch;

    [Header("Idle Sound Settings")]
    [SerializeField] private AudioSource idleSound;
    [SerializeField] private float idleMaxVolume;
    [SerializeField] private float idleMaxPitch;

    private VehicleController vehicleController;

    private void Awake()
    {
        vehicleController = GetComponent<VehicleController>();
    }
}