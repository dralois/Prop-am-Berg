// GENERATED AUTOMATICALLY FROM 'Assets/Objects/PropControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PropControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @PropControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PropControls"",
    ""maps"": [
        {
            ""name"": ""Ingame"",
            ""id"": ""0418303a-89eb-419e-8a45-09e762b13392"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""f059060e-3413-4af8-8f6e-84e90f5c613d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""870e19b6-9455-4fd5-8909-bc8a38a2f10b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""ca143d1e-cfe2-434a-9e75-7fb9f3d0d63a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectProp"",
                    ""type"": ""Button"",
                    ""id"": ""839332d3-bbea-42df-add1-423ac8d761ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""67664105-2679-4cea-b225-64c04251a092"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BecomeProp"",
                    ""type"": ""Button"",
                    ""id"": ""16b6296d-3bd4-4711-b818-6927903b5f20"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a4a4116e-86fc-412c-9904-6ab01fc67c8a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55725b6c-1776-4936-ae4a-53562629a65d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a25660c-4004-48b8-a9fd-c6826910ddf2"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4da2315d-f904-4db5-8fc3-0dc4f28feab7"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""SelectProp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d58120b8-e0fa-4edf-b997-4da75e9e2696"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4126e146-b8b8-4dc5-b886-6b7ce6057eaa"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""BecomeProp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Ingame
        m_Ingame = asset.FindActionMap("Ingame", throwIfNotFound: true);
        m_Ingame_Look = m_Ingame.FindAction("Look", throwIfNotFound: true);
        m_Ingame_Move = m_Ingame.FindAction("Move", throwIfNotFound: true);
        m_Ingame_Jump = m_Ingame.FindAction("Jump", throwIfNotFound: true);
        m_Ingame_SelectProp = m_Ingame.FindAction("SelectProp", throwIfNotFound: true);
        m_Ingame_Pause = m_Ingame.FindAction("Pause", throwIfNotFound: true);
        m_Ingame_BecomeProp = m_Ingame.FindAction("BecomeProp", throwIfNotFound: true);
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

    // Ingame
    private readonly InputActionMap m_Ingame;
    private IIngameActions m_IngameActionsCallbackInterface;
    private readonly InputAction m_Ingame_Look;
    private readonly InputAction m_Ingame_Move;
    private readonly InputAction m_Ingame_Jump;
    private readonly InputAction m_Ingame_SelectProp;
    private readonly InputAction m_Ingame_Pause;
    private readonly InputAction m_Ingame_BecomeProp;
    public struct IngameActions
    {
        private @PropControls m_Wrapper;
        public IngameActions(@PropControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Ingame_Look;
        public InputAction @Move => m_Wrapper.m_Ingame_Move;
        public InputAction @Jump => m_Wrapper.m_Ingame_Jump;
        public InputAction @SelectProp => m_Wrapper.m_Ingame_SelectProp;
        public InputAction @Pause => m_Wrapper.m_Ingame_Pause;
        public InputAction @BecomeProp => m_Wrapper.m_Ingame_BecomeProp;
        public InputActionMap Get() { return m_Wrapper.m_Ingame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(IngameActions set) { return set.Get(); }
        public void SetCallbacks(IIngameActions instance)
        {
            if (m_Wrapper.m_IngameActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnLook;
                @Move.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnJump;
                @SelectProp.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnSelectProp;
                @SelectProp.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnSelectProp;
                @SelectProp.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnSelectProp;
                @Pause.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnPause;
                @BecomeProp.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnBecomeProp;
                @BecomeProp.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnBecomeProp;
                @BecomeProp.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnBecomeProp;
            }
            m_Wrapper.m_IngameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @SelectProp.started += instance.OnSelectProp;
                @SelectProp.performed += instance.OnSelectProp;
                @SelectProp.canceled += instance.OnSelectProp;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @BecomeProp.started += instance.OnBecomeProp;
                @BecomeProp.performed += instance.OnBecomeProp;
                @BecomeProp.canceled += instance.OnBecomeProp;
            }
        }
    }
    public IngameActions @Ingame => new IngameActions(this);
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IIngameActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSelectProp(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnBecomeProp(InputAction.CallbackContext context);
    }
}
