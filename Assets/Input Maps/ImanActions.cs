//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Input Maps/ImanActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ImanActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ImanActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ImanActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""3a9d598b-96e0-4357-a8c9-a38194ea5b5d"",
            ""actions"": [
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""f63a90f7-6a2c-4079-a9b1-447cad2ba74d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""58a67143-251d-4ef8-9353-422e6bb9a945"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pick Up Phone"",
                    ""type"": ""Button"",
                    ""id"": ""cf557c0a-650d-4b64-a617-d6e7a84266e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""528cdc86-6b9a-4cab-8b51-a635b769fdc3"",
                    ""expectedControlType"": ""Delta"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""dbb9be56-b39b-4ff9-a332-3c3ac016ebf4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1ea85592-bc44-4ca3-9dbb-80cf59d63492"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""185a943c-432a-4341-aa5e-4a9aadae7ef7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb06971b-090c-4e08-bb7b-03dc97340935"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ffe87e77-782f-4e93-8884-d2a3daa08146"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c27505d-b2f4-451c-8545-42adf3cbdbbc"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pick Up Phone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""64fc57fb-090c-4401-9b26-651635d46c02"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pick Up Phone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector (Keyboard)"",
                    ""id"": ""c2f7b257-4d4f-48c6-b27c-fe3b724459d3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""410c62c0-e7c6-452d-a833-7fb7c44e88ae"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""19426abc-81f8-4907-8249-37882775b804"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""85373cff-188c-4e5a-9a05-408d6645c8e7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6b3cf14e-9d6e-4370-9213-0194b250e3bd"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector (Gamepad Stick)"",
                    ""id"": ""42a21e32-14b6-46a7-a6a9-cfabd64eb114"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bb6a5991-feed-412e-a6c8-831d6c46c448"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2337830c-e990-4fb3-8d69-11cad0b3e266"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4454b06d-f37c-4ed5-81b9-f5632219b982"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d4c6ff18-df8a-4834-8974-f843d04b16cd"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""281c4a0b-e3ff-4eaf-b06b-1f6d8b53a459"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18c577ce-75b3-42cb-9be4-69e58681f937"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector (Gamepad D-Pad)"",
                    ""id"": ""0b342cd8-afea-4d87-a0be-be5f391f54fd"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c0507c8b-4090-42b0-8487-f22c9bd194b9"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0b8c2f95-a921-4136-a0d4-69d8854de120"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fd559170-937c-4728-9bc9-8195b17e33c0"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f8fb480d-5b4b-4f2f-9704-dfc1a1dc3254"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""UI / Phone"",
            ""id"": ""a9a2a03e-cf7a-4d22-80a0-d67b4c6e2c70"",
            ""actions"": [
                {
                    ""name"": ""Accept"",
                    ""type"": ""Button"",
                    ""id"": ""615635ee-9aa1-4a62-8967-3e75b05bf8b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel / Back"",
                    ""type"": ""Button"",
                    ""id"": ""c6730f2c-2637-44fd-aaa4-8e4a1a95a0a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Leave UI"",
                    ""type"": ""Button"",
                    ""id"": ""ff42ce43-eb3d-4185-862e-0fd956cc7f28"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""ed9f83c0-ea43-4a54-b0ab-5a7a903df30a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a8811cf5-9f86-42d9-afed-4831ab73ebd2"",
                    ""expectedControlType"": ""Delta"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e4e83a21-6556-4df1-ae1d-e1dd77de5506"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3fe3137f-40ea-429f-beeb-e9da0df41589"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""189d680d-0f8d-4895-8469-0c65b21939a3"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel / Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4f66197-f0ab-4c88-80c9-65590341e3c2"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel / Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10fdd8af-5c5a-43d5-957b-197309b33821"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Leave UI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9363ff13-6879-48a6-929d-4d47d92375ff"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Leave UI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b28d694-a012-416f-a22f-f413e0ed193c"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b25c0c05-25cc-43c3-885f-fea4b97838b4"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""35079906-98b5-40e3-a741-72a015ff6e3b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""db6f0b94-9398-4b6e-b46d-776bc70fe9e9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b50e93cc-82a0-4609-959b-b62cd0b07fa9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ac551c4e-2e08-4fdf-818c-c6be03e03ab8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d6d1f35b-458f-4632-8b0c-7b3c5a30cb40"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""d3b6853f-3f4f-4f7c-b66d-dcf5dc02932f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""386be227-8d0c-41b8-8d87-40fdd7032646"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b27675ab-97d4-41d2-b34a-495108eb8bf6"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""222df013-aa01-4a39-800f-6a834f548521"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c4a6270c-823e-4ef0-b2ee-8de36f1d1582"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Cancel = m_Player.FindAction("Cancel", throwIfNotFound: true);
        m_Player_PickUpPhone = m_Player.FindAction("Pick Up Phone", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        // UI / Phone
        m_UIPhone = asset.FindActionMap("UI / Phone", throwIfNotFound: true);
        m_UIPhone_Accept = m_UIPhone.FindAction("Accept", throwIfNotFound: true);
        m_UIPhone_CancelBack = m_UIPhone.FindAction("Cancel / Back", throwIfNotFound: true);
        m_UIPhone_LeaveUI = m_UIPhone.FindAction("Leave UI", throwIfNotFound: true);
        m_UIPhone_Pause = m_UIPhone.FindAction("Pause", throwIfNotFound: true);
        m_UIPhone_Move = m_UIPhone.FindAction("Move", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Cancel;
    private readonly InputAction m_Player_PickUpPhone;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Look;
    public struct PlayerActions
    {
        private @ImanActions m_Wrapper;
        public PlayerActions(@ImanActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Cancel => m_Wrapper.m_Player_Cancel;
        public InputAction @PickUpPhone => m_Wrapper.m_Player_PickUpPhone;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Cancel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @PickUpPhone.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUpPhone;
                @PickUpPhone.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUpPhone;
                @PickUpPhone.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUpPhone;
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @PickUpPhone.started += instance.OnPickUpPhone;
                @PickUpPhone.performed += instance.OnPickUpPhone;
                @PickUpPhone.canceled += instance.OnPickUpPhone;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI / Phone
    private readonly InputActionMap m_UIPhone;
    private IUIPhoneActions m_UIPhoneActionsCallbackInterface;
    private readonly InputAction m_UIPhone_Accept;
    private readonly InputAction m_UIPhone_CancelBack;
    private readonly InputAction m_UIPhone_LeaveUI;
    private readonly InputAction m_UIPhone_Pause;
    private readonly InputAction m_UIPhone_Move;
    public struct UIPhoneActions
    {
        private @ImanActions m_Wrapper;
        public UIPhoneActions(@ImanActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Accept => m_Wrapper.m_UIPhone_Accept;
        public InputAction @CancelBack => m_Wrapper.m_UIPhone_CancelBack;
        public InputAction @LeaveUI => m_Wrapper.m_UIPhone_LeaveUI;
        public InputAction @Pause => m_Wrapper.m_UIPhone_Pause;
        public InputAction @Move => m_Wrapper.m_UIPhone_Move;
        public InputActionMap Get() { return m_Wrapper.m_UIPhone; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIPhoneActions set) { return set.Get(); }
        public void SetCallbacks(IUIPhoneActions instance)
        {
            if (m_Wrapper.m_UIPhoneActionsCallbackInterface != null)
            {
                @Accept.started -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnAccept;
                @Accept.performed -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnAccept;
                @Accept.canceled -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnAccept;
                @CancelBack.started -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnCancelBack;
                @CancelBack.performed -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnCancelBack;
                @CancelBack.canceled -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnCancelBack;
                @LeaveUI.started -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnLeaveUI;
                @LeaveUI.performed -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnLeaveUI;
                @LeaveUI.canceled -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnLeaveUI;
                @Pause.started -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnPause;
                @Move.started -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_UIPhoneActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_UIPhoneActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Accept.started += instance.OnAccept;
                @Accept.performed += instance.OnAccept;
                @Accept.canceled += instance.OnAccept;
                @CancelBack.started += instance.OnCancelBack;
                @CancelBack.performed += instance.OnCancelBack;
                @CancelBack.canceled += instance.OnCancelBack;
                @LeaveUI.started += instance.OnLeaveUI;
                @LeaveUI.performed += instance.OnLeaveUI;
                @LeaveUI.canceled += instance.OnLeaveUI;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public UIPhoneActions @UIPhone => new UIPhoneActions(this);
    public interface IPlayerActions
    {
        void OnInteract(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnPickUpPhone(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
    public interface IUIPhoneActions
    {
        void OnAccept(InputAction.CallbackContext context);
        void OnCancelBack(InputAction.CallbackContext context);
        void OnLeaveUI(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
    }
}
