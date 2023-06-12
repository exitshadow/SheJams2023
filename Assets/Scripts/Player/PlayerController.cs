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
    #region controls
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float moveSpeed;
    [SerializeField] private float steeringSpeed;

    private ImanActions actions;
    private InputAction playerMove;
    private InputAction playerInteract;
    private InputAction playerCancel;
    private InputAction playerPickUpPhone;
    private InputAction playerRun;

    private CharacterController controller;
    private Rigidbody rb;
    private Animator animator;
    #endregion

    #region interaction
    [HideInInspector] public NPC currentInteractingNPC;
    #endregion

    public void OnMoveCharacter()
    {

    }

    private void Move()
    {
        // read from inputs
        Vector2 moveDirection = playerMove.ReadValue<Vector2>();

        Vector2 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        Vector3 translation = new Vector3(movement.x, 0, movement.y);

        // movement
        translation = transform.TransformDirection(translation);
        transform.position += translation;

        // steering (rotation)
        if(moveDirection != Vector2.zero)
        {
            Debug.Log("rotating " + transform.name);
            transform.Rotate(0, moveDirection.x * steeringSpeed * Time.deltaTime * 100, 0);
        }

    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed) moveSpeed = runSpeed;
        else moveSpeed = walkSpeed;
    }


    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Interact Unity Event Called");

            if (currentInteractingNPC != null)
            {
                currentInteractingNPC.GoToDialogueNewLine(context);
            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        actions = new ImanActions();
        controller = GetComponent<CharacterController>();

        playerInteract = actions.Player.Interact;
        playerInteract.Enable();

        playerCancel = actions.Player.Cancel;
        playerCancel.Enable();

        playerPickUpPhone = actions.Player.PickUpPhone;
        playerPickUpPhone.Enable();

        playerRun = actions.Player.Run;
        playerRun.Enable();

        playerMove = actions.Player.Move;
        playerMove.Enable();


        moveSpeed = walkSpeed;

    }

    private void FixedUpdate()
    {
        if (currentInteractingNPC == null || !currentInteractingNPC.isPlayingDialogue) Move();
    }
}
