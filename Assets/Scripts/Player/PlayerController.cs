using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class offers methods that are to be called by the Unity Events of the Player Input attached to the same game object.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnnoyingPhone phone;

    #region controls
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float moveSpeed;
    [SerializeField] private float steeringSpeed;
    [SerializeField] private Canvas controlsCanvas;
    private bool isEnabled = false;

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
        Vector3 translation = new Vector3(0, 0, movement.y);

        // movement
        translation = transform.TransformDirection(translation);
        transform.position += translation;

        // steering (rotation) and walk/idle animation
        if(moveDirection != Vector2.zero)
        {
            transform.Rotate(0, moveDirection.x * steeringSpeed * Time.deltaTime * 100, 0);
            animator.SetFloat("walkingSpeed", walkSpeed);
        }
        else animator.SetFloat("walkingSpeed", 0);

    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveSpeed = runSpeed;
            animator.SetBool("isShiftPressed", true);
            Debug.Log(animator.GetBool("isShiftPressed"));
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("isShiftPressed", false);
            Debug.Log(animator.GetBool("isShiftPressed"));
        }
    }


    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Debug.Log("Interact Unity Event Called");

            if (currentInteractingNPC != null && !phone.IsReadingPhone)
            {
                currentInteractingNPC.Talk(context);
            }

            if (phone.IsReadingPhone)
            {
                phone.GetNewMessage();
            }
        }
    }

    public void PickUpPhone(InputAction.CallbackContext context)
    {
        if (context.performed && currentInteractingNPC == null)
        {
            phone.PickUpPhone(context);
        }
    }

    public void HelpControls(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isEnabled = !isEnabled;
            controlsCanvas.enabled = isEnabled;
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
        if (    currentInteractingNPC == null
            || !currentInteractingNPC.isPlayingDialogue)
        {
            //if (!phone.IsReadingPhone)
                Move();
        }
    }
}
