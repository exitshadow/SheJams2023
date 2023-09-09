using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup pressAnyKey;
    [SerializeField] private CanvasGroup buttons;

    private ImanActions inputActions;
    private InputAction pressAnyKeyAction;

    void Awake()
    {
        inputActions = new ImanActions();
        pressAnyKeyAction = inputActions.UIPhone.AnyKey;
        pressAnyKeyAction.Enable();

        pressAnyKeyAction.performed += OnAnyKeyPressed;
    }

    void Start()
    {
        pressAnyKey.gameObject.SetActive(true);
        buttons.gameObject.SetActive(false);
    }

    private void OnAnyKeyPressed(InputAction.CallbackContext action)
    {
        if (action.performed)
        {
            pressAnyKey.gameObject.SetActive(false);
            buttons.gameObject.SetActive(true);
            pressAnyKeyAction.Disable();
        }
    }
}
