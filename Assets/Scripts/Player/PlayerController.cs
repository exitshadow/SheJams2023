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
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float moveSpeed;

    [SerializeField] private float steeringSpeed;

    private ImanActions actions;
    private InputAction move;
    private CharacterController controller;
    private Rigidbody rb;
    private Animator animator;


    public void OnMoveCharacter()
    {

    }

    private void Move()
    {

    }

    public void Run(InputAction.CallbackContext context){
        if (context.performed) moveSpeed = runSpeed;
        else moveSpeed = walkSpeed;
    }

    private void Steer()
    {

    }

    public void Activate(InputAction.CallbackContext context){
        if (context.performed) Debug.Log("Interact Unity Event Called");
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        actions = new ImanActions();
        controller = GetComponent<CharacterController>();

        move = actions.Player.Move;
        move.Enable();
        moveSpeed = walkSpeed;

    }

    private void FixedUpdate(){
        Vector2 moveDirection = move.ReadValue<Vector2>();

        float verticalSpeed = 0;

        if(!controller.isGrounded){
            verticalSpeed += -9.81f * Time.fixedDeltaTime;
        } else verticalSpeed = 0;

        Vector3 verticalMovement = Vector3.up * verticalSpeed * Time.fixedDeltaTime;

        Vector2 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        Vector3 translation = new Vector3(-movement.x, 0, -movement.y);

        controller.Move(translation + verticalMovement);

        if(moveDirection != Vector2.zero){
            transform.forward = Vector3.Slerp (transform.forward, translation, Time.fixedDeltaTime * 10);
        }
    }
}
