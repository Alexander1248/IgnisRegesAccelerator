//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Controls/PlayerControl.inputactions
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

public partial class @PlayerControl: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""1e93f760-ce5f-4766-b6a7-c055b9cea613"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""b1e2c888-d0c9-4003-ad53-530d336ff1f6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""c63e90cc-1b00-4239-8344-e2648ab334b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""d2cd9840-8225-4c8f-abe1-e4e5ecaef8e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""78ac5566-4198-4a1e-bf29-2621b0a12713"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""64a11f59-6717-460d-8e04-725acb998cbe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lay"",
                    ""type"": ""Button"",
                    ""id"": ""e48312ff-8764-40d5-a95e-32caf1b3c7bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""02d9cfe3-ba45-4175-881e-9d48e69308d7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""1d5881a6-bfec-4763-850e-afe57206aa4a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""fc45cadc-8ccc-407d-a662-f8dac5b13b4d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""757efaff-bb31-42d5-81e6-2701588afe14"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""7bca4b48-de76-4d1b-a152-9d529cb1fe48"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ef61d071-a51a-4802-80a5-8a8f45ad619d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68414038-a5a9-4399-b51e-cc86e1bd4a01"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d8e56c5-fbfb-49a9-beb7-f48150604ba9"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b1319a6-6fd1-4d2f-87b1-aff4851c0459"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""64ef55cf-acb9-4dd0-adaa-7d617a1a100e"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Lay"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Interaction"",
            ""id"": ""4c5ee6ca-8907-47c0-8f70-333aad3dec43"",
            ""actions"": [
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""2aeda654-6daa-437a-a5e1-28f551c35c94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveItem"",
                    ""type"": ""Button"",
                    ""id"": ""7854f730-b7ee-45f8-95ec-12f26727af62"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Use Item"",
                    ""type"": ""Button"",
                    ""id"": ""a4b01388-97c8-4943-8876-574b7bf54d96"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Use"",
                    ""type"": ""Button"",
                    ""id"": ""4c1bb1f5-1d58-4c8e-bc77-56124e4c3140"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Main Hand Action"",
                    ""type"": ""Button"",
                    ""id"": ""14b6e42b-c070-4205-8c97-b547cf84cb1c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Main Hand Addtitonal Action"",
                    ""type"": ""Button"",
                    ""id"": ""d73ed0da-a834-4591-b732-6c0e28cfb044"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Second Hand Action"",
                    ""type"": ""Button"",
                    ""id"": ""f08c7640-98fb-472f-8d53-e1fc7b2f50a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Second Hand Addtitonal Action"",
                    ""type"": ""Button"",
                    ""id"": ""aad4b98a-0e83-49a4-9048-f1ebf266425e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Journal"",
                    ""type"": ""Button"",
                    ""id"": ""07c17097-fb38-45a5-9456-4990085685d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Use Heal"",
                    ""type"": ""Button"",
                    ""id"": ""6cff6fbc-2a41-4ef6-812f-d6f83a1bacc5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""8acd94f7-944e-432c-8f7d-9685900434c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2ebeea5a-0a86-40c1-a99d-ee9e72069f76"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ad71470-fe5c-46aa-aebe-69540fb87cc8"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff00cd4e-c83f-4b1d-952f-8beac08a5f29"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Main Hand Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6593e66a-dd92-4c7d-90ef-c51e716e3806"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Main Hand Addtitonal Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dcc72e0b-8c98-48d7-aa30-95e08c7f1f5b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Second Hand Addtitonal Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32447e96-ffb2-4f75-8794-7112efc98049"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Second Hand Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff109b7b-b927-4dde-a34e-ca6b48da6e5c"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Journal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9b803f2-acb0-4a39-b8e3-699edb662702"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Use Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04a2ea1a-3da7-49f7-9261-d4e012b56df7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""MoveItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cef34893-88dd-4eff-97d1-5f29b6135083"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Use Item"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b11e8b9d-466d-4f1e-81b9-bdf927646d07"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""95d89746-b4b5-4cae-bca6-db27a0c9ccad"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""6c555512-178b-493e-a6cc-753f0567f22d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""71963fed-1df2-4ce1-8504-62a606439683"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""b74e26c1-b96d-42a9-864c-88a11184626e"",
                    ""path"": ""<Mouse>/delta/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""4500feb2-63bd-4914-8831-c3c13dedb2a1"",
                    ""path"": ""<Mouse>/delta/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""cdcb8ea1-d632-41f4-a6e6-fb3cc07f571d"",
                    ""path"": ""<Mouse>/delta/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""1c4cc45d-69d4-41a1-ae0c-7977d1bb1170"",
                    ""path"": ""<Mouse>/delta/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse and Keyboard"",
            ""bindingGroup"": ""Mouse and Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        m_Movement_Jump = m_Movement.FindAction("Jump", throwIfNotFound: true);
        m_Movement_Sprint = m_Movement.FindAction("Sprint", throwIfNotFound: true);
        m_Movement_Dash = m_Movement.FindAction("Dash", throwIfNotFound: true);
        m_Movement_Crouch = m_Movement.FindAction("Crouch", throwIfNotFound: true);
        m_Movement_Lay = m_Movement.FindAction("Lay", throwIfNotFound: true);
        // Interaction
        m_Interaction = asset.FindActionMap("Interaction", throwIfNotFound: true);
        m_Interaction_Inventory = m_Interaction.FindAction("Inventory", throwIfNotFound: true);
        m_Interaction_MoveItem = m_Interaction.FindAction("MoveItem", throwIfNotFound: true);
        m_Interaction_UseItem = m_Interaction.FindAction("Use Item", throwIfNotFound: true);
        m_Interaction_Use = m_Interaction.FindAction("Use", throwIfNotFound: true);
        m_Interaction_MainHandAction = m_Interaction.FindAction("Main Hand Action", throwIfNotFound: true);
        m_Interaction_MainHandAddtitonalAction = m_Interaction.FindAction("Main Hand Addtitonal Action", throwIfNotFound: true);
        m_Interaction_SecondHandAction = m_Interaction.FindAction("Second Hand Action", throwIfNotFound: true);
        m_Interaction_SecondHandAddtitonalAction = m_Interaction.FindAction("Second Hand Addtitonal Action", throwIfNotFound: true);
        m_Interaction_Journal = m_Interaction.FindAction("Journal", throwIfNotFound: true);
        m_Interaction_UseHeal = m_Interaction.FindAction("Use Heal", throwIfNotFound: true);
        m_Interaction_Reload = m_Interaction.FindAction("Reload", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_Move = m_Camera.FindAction("Move", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private List<IMovementActions> m_MovementActionsCallbackInterfaces = new List<IMovementActions>();
    private readonly InputAction m_Movement_Move;
    private readonly InputAction m_Movement_Jump;
    private readonly InputAction m_Movement_Sprint;
    private readonly InputAction m_Movement_Dash;
    private readonly InputAction m_Movement_Crouch;
    private readonly InputAction m_Movement_Lay;
    public struct MovementActions
    {
        private @PlayerControl m_Wrapper;
        public MovementActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputAction @Jump => m_Wrapper.m_Movement_Jump;
        public InputAction @Sprint => m_Wrapper.m_Movement_Sprint;
        public InputAction @Dash => m_Wrapper.m_Movement_Dash;
        public InputAction @Crouch => m_Wrapper.m_Movement_Crouch;
        public InputAction @Lay => m_Wrapper.m_Movement_Lay;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void AddCallbacks(IMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_MovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MovementActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Crouch.started += instance.OnCrouch;
            @Crouch.performed += instance.OnCrouch;
            @Crouch.canceled += instance.OnCrouch;
            @Lay.started += instance.OnLay;
            @Lay.performed += instance.OnLay;
            @Lay.canceled += instance.OnLay;
        }

        private void UnregisterCallbacks(IMovementActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Crouch.started -= instance.OnCrouch;
            @Crouch.performed -= instance.OnCrouch;
            @Crouch.canceled -= instance.OnCrouch;
            @Lay.started -= instance.OnLay;
            @Lay.performed -= instance.OnLay;
            @Lay.canceled -= instance.OnLay;
        }

        public void RemoveCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_MovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Interaction
    private readonly InputActionMap m_Interaction;
    private List<IInteractionActions> m_InteractionActionsCallbackInterfaces = new List<IInteractionActions>();
    private readonly InputAction m_Interaction_Inventory;
    private readonly InputAction m_Interaction_MoveItem;
    private readonly InputAction m_Interaction_UseItem;
    private readonly InputAction m_Interaction_Use;
    private readonly InputAction m_Interaction_MainHandAction;
    private readonly InputAction m_Interaction_MainHandAddtitonalAction;
    private readonly InputAction m_Interaction_SecondHandAction;
    private readonly InputAction m_Interaction_SecondHandAddtitonalAction;
    private readonly InputAction m_Interaction_Journal;
    private readonly InputAction m_Interaction_UseHeal;
    private readonly InputAction m_Interaction_Reload;
    public struct InteractionActions
    {
        private @PlayerControl m_Wrapper;
        public InteractionActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Inventory => m_Wrapper.m_Interaction_Inventory;
        public InputAction @MoveItem => m_Wrapper.m_Interaction_MoveItem;
        public InputAction @UseItem => m_Wrapper.m_Interaction_UseItem;
        public InputAction @Use => m_Wrapper.m_Interaction_Use;
        public InputAction @MainHandAction => m_Wrapper.m_Interaction_MainHandAction;
        public InputAction @MainHandAddtitonalAction => m_Wrapper.m_Interaction_MainHandAddtitonalAction;
        public InputAction @SecondHandAction => m_Wrapper.m_Interaction_SecondHandAction;
        public InputAction @SecondHandAddtitonalAction => m_Wrapper.m_Interaction_SecondHandAddtitonalAction;
        public InputAction @Journal => m_Wrapper.m_Interaction_Journal;
        public InputAction @UseHeal => m_Wrapper.m_Interaction_UseHeal;
        public InputAction @Reload => m_Wrapper.m_Interaction_Reload;
        public InputActionMap Get() { return m_Wrapper.m_Interaction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InteractionActions set) { return set.Get(); }
        public void AddCallbacks(IInteractionActions instance)
        {
            if (instance == null || m_Wrapper.m_InteractionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InteractionActionsCallbackInterfaces.Add(instance);
            @Inventory.started += instance.OnInventory;
            @Inventory.performed += instance.OnInventory;
            @Inventory.canceled += instance.OnInventory;
            @MoveItem.started += instance.OnMoveItem;
            @MoveItem.performed += instance.OnMoveItem;
            @MoveItem.canceled += instance.OnMoveItem;
            @UseItem.started += instance.OnUseItem;
            @UseItem.performed += instance.OnUseItem;
            @UseItem.canceled += instance.OnUseItem;
            @Use.started += instance.OnUse;
            @Use.performed += instance.OnUse;
            @Use.canceled += instance.OnUse;
            @MainHandAction.started += instance.OnMainHandAction;
            @MainHandAction.performed += instance.OnMainHandAction;
            @MainHandAction.canceled += instance.OnMainHandAction;
            @MainHandAddtitonalAction.started += instance.OnMainHandAddtitonalAction;
            @MainHandAddtitonalAction.performed += instance.OnMainHandAddtitonalAction;
            @MainHandAddtitonalAction.canceled += instance.OnMainHandAddtitonalAction;
            @SecondHandAction.started += instance.OnSecondHandAction;
            @SecondHandAction.performed += instance.OnSecondHandAction;
            @SecondHandAction.canceled += instance.OnSecondHandAction;
            @SecondHandAddtitonalAction.started += instance.OnSecondHandAddtitonalAction;
            @SecondHandAddtitonalAction.performed += instance.OnSecondHandAddtitonalAction;
            @SecondHandAddtitonalAction.canceled += instance.OnSecondHandAddtitonalAction;
            @Journal.started += instance.OnJournal;
            @Journal.performed += instance.OnJournal;
            @Journal.canceled += instance.OnJournal;
            @UseHeal.started += instance.OnUseHeal;
            @UseHeal.performed += instance.OnUseHeal;
            @UseHeal.canceled += instance.OnUseHeal;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
        }

        private void UnregisterCallbacks(IInteractionActions instance)
        {
            @Inventory.started -= instance.OnInventory;
            @Inventory.performed -= instance.OnInventory;
            @Inventory.canceled -= instance.OnInventory;
            @MoveItem.started -= instance.OnMoveItem;
            @MoveItem.performed -= instance.OnMoveItem;
            @MoveItem.canceled -= instance.OnMoveItem;
            @UseItem.started -= instance.OnUseItem;
            @UseItem.performed -= instance.OnUseItem;
            @UseItem.canceled -= instance.OnUseItem;
            @Use.started -= instance.OnUse;
            @Use.performed -= instance.OnUse;
            @Use.canceled -= instance.OnUse;
            @MainHandAction.started -= instance.OnMainHandAction;
            @MainHandAction.performed -= instance.OnMainHandAction;
            @MainHandAction.canceled -= instance.OnMainHandAction;
            @MainHandAddtitonalAction.started -= instance.OnMainHandAddtitonalAction;
            @MainHandAddtitonalAction.performed -= instance.OnMainHandAddtitonalAction;
            @MainHandAddtitonalAction.canceled -= instance.OnMainHandAddtitonalAction;
            @SecondHandAction.started -= instance.OnSecondHandAction;
            @SecondHandAction.performed -= instance.OnSecondHandAction;
            @SecondHandAction.canceled -= instance.OnSecondHandAction;
            @SecondHandAddtitonalAction.started -= instance.OnSecondHandAddtitonalAction;
            @SecondHandAddtitonalAction.performed -= instance.OnSecondHandAddtitonalAction;
            @SecondHandAddtitonalAction.canceled -= instance.OnSecondHandAddtitonalAction;
            @Journal.started -= instance.OnJournal;
            @Journal.performed -= instance.OnJournal;
            @Journal.canceled -= instance.OnJournal;
            @UseHeal.started -= instance.OnUseHeal;
            @UseHeal.performed -= instance.OnUseHeal;
            @UseHeal.canceled -= instance.OnUseHeal;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
        }

        public void RemoveCallbacks(IInteractionActions instance)
        {
            if (m_Wrapper.m_InteractionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInteractionActions instance)
        {
            foreach (var item in m_Wrapper.m_InteractionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InteractionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InteractionActions @Interaction => new InteractionActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private List<ICameraActions> m_CameraActionsCallbackInterfaces = new List<ICameraActions>();
    private readonly InputAction m_Camera_Move;
    public struct CameraActions
    {
        private @PlayerControl m_Wrapper;
        public CameraActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Camera_Move;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void AddCallbacks(ICameraActions instance)
        {
            if (instance == null || m_Wrapper.m_CameraActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CameraActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
        }

        private void UnregisterCallbacks(ICameraActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
        }

        public void RemoveCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICameraActions instance)
        {
            foreach (var item in m_Wrapper.m_CameraActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CameraActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CameraActions @Camera => new CameraActions(this);
    private int m_MouseandKeyboardSchemeIndex = -1;
    public InputControlScheme MouseandKeyboardScheme
    {
        get
        {
            if (m_MouseandKeyboardSchemeIndex == -1) m_MouseandKeyboardSchemeIndex = asset.FindControlSchemeIndex("Mouse and Keyboard");
            return asset.controlSchemes[m_MouseandKeyboardSchemeIndex];
        }
    }
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnLay(InputAction.CallbackContext context);
    }
    public interface IInteractionActions
    {
        void OnInventory(InputAction.CallbackContext context);
        void OnMoveItem(InputAction.CallbackContext context);
        void OnUseItem(InputAction.CallbackContext context);
        void OnUse(InputAction.CallbackContext context);
        void OnMainHandAction(InputAction.CallbackContext context);
        void OnMainHandAddtitonalAction(InputAction.CallbackContext context);
        void OnSecondHandAction(InputAction.CallbackContext context);
        void OnSecondHandAddtitonalAction(InputAction.CallbackContext context);
        void OnJournal(InputAction.CallbackContext context);
        void OnUseHeal(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
}
