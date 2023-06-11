using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class offers methods that are to be called by the Unity Events of the Player Input attached to the same game object.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float steeringSpeed;

    private ImanActions actions;
    private InputAction move;
    private Rigidbody rb;
    private Animator animator;


    public void OnMoveCharacter()
    {

    }

    private void Move()
    {

    }

    private void Steer()
    {

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        actions = new ImanActions();

        move = actions.Player.Move;
        move.Enable();
    }
}
