using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

/// <summary>
/// This class offers methods that are to be called by the Unity Events of the Player Input attached to the same game object.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private UIManager ui;
    [SerializeField] private AnnoyingPhone phone;
    [SerializeField] private Transform cameraTransform;

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
    private InputAction playerLook; 

    private CharacterController controller;
    private Rigidbody rb;
    private Animator animator;
    #endregion

    #region interaction
    [HideInInspector] public NPC currentInteractingNPC;
    #endregion

    public void MakeTypeOnPhone(bool isTexting)
    {
        // animator.SetBool("isTexting", isTexting);

        if (isTexting)
        {
            animator.SetBool("isTexting", true);
            animator.SetLayerWeight(2, 100);
        }
        else
        {
            animator.SetBool("isTexting", false);
            animator.SetLayerWeight(2, 0);
        }
    }

    private void Move()
    {
        // Lire les entr�es du joueur
        Vector2 moveDirection = playerMove.ReadValue<Vector2>();
        moveDirection.x = 0;    
        Vector2 cameraLook = playerLook.ReadValue<Vector2>();

        // Rotation bas�e sur la rotation de la cam�ra
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;

        Vector3 desiredMoveDirection = forward * moveDirection.y + right * moveDirection.x;

        if (desiredMoveDirection != Vector3.zero)
        {
            // Calculer la rotation d�sir�e avec la direction de mouvement et la rotation de la cam�ra
            Quaternion desiredRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, steeringSpeed * Time.deltaTime);
        }

        // D�placer le personnage
        transform.position += desiredMoveDirection.normalized * moveSpeed * Time.deltaTime;

        // Appliquer la rotation de la cam�ra sur le joueur
        transform.Rotate(Vector3.up, cameraLook.x * steeringSpeed * Time.deltaTime);

        // Animation de marche/idle
        if (moveDirection != Vector2.zero)
        {
            animator.SetFloat("walkingSpeed", walkSpeed);
        }
        else
        {
            animator.SetFloat("walkingSpeed", 0);
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveSpeed = runSpeed;
            animator.SetBool("isShiftPressed", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("isShiftPressed", false);
        }
    }


    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Debug.Log("Interact Unity Event Called");

            StopWalkingAnimation();

            if (currentInteractingNPC != null && !AnnoyingPhone.IsReadingPhone)
            {
                currentInteractingNPC.Talk(context);
            }

            if (AnnoyingPhone.IsReadingPhone)
            {
                phone.GetNewMessage();
            }
        }
    }

    public void StopWalkingAnimation()
    {
        animator.SetFloat("walkingSpeed", 0);
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
            if (ui.isControlHelpDisplayed)
            {
                playerMove.Enable();
                playerInteract.Enable();
                playerPickUpPhone.Enable();
                ui.CloseControlsHelp();
            }
            else
            {
                playerMove.Disable();
                playerInteract.Disable();
                playerPickUpPhone.Disable();
                ui.OpenControlsHelp();
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

        playerLook = actions.Player.Look;
        playerLook.Enable();


        moveSpeed = walkSpeed;

        animator.SetLayerWeight(2, 0);

    }

    private void FixedUpdate()
    {
        if (currentInteractingNPC == null
            || !currentInteractingNPC.IsPlayingDialogue)
        {
            //if (!phone.IsReadingPhone)
            Move();
        }
    }
}
