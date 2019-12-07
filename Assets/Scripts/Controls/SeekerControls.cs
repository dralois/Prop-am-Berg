// GENERATED AUTOMATICALLY FROM 'Assets/Objects/SeekerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @SeekerControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @SeekerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""SeekerControls"",
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
                    ""name"": ""ShowProps"",
                    ""type"": ""Button"",
                    ""id"": ""839332d3-bbea-42df-add1-423ac8d761ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""6a7a870c-6b95-45a1-b8d0-b63f13eb4e3a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7626a8c0-ffc0-4a7a-bc16-61aa916e0068"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""KB"",
                    ""id"": ""c66e2b3a-841c-4d0e-b9e0-e38f18b4c101"",
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
                    ""id"": ""50d30ad9-8a94-4734-8b86-de0137cc99e7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""103b28ce-442d-4592-86c5-d2a482a92ce1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d643e437-444e-4946-87ba-a21b18da2808"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""55d1ec6c-c8d9-4166-b41f-ff59cb009fb5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f20c9f13-62be-4309-87f1-319fe1ca1c87"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""ShowProps"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7dfe1194-537a-4666-8aa7-2e43282d1f86"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KB&M"",
            ""bindingGroup"": ""KB&M"",
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
        // Ingame
        m_Ingame = asset.FindActionMap("Ingame", throwIfNotFound: true);
        m_Ingame_Look = m_Ingame.FindAction("Look", throwIfNotFound: true);
        m_Ingame_Move = m_Ingame.FindAction("Move", throwIfNotFound: true);
        m_Ingame_ShowProps = m_Ingame.FindAction("ShowProps", throwIfNotFound: true);
        m_Ingame_Shoot = m_Ingame.FindAction("Shoot", throwIfNotFound: true);
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
    private readonly InputAction m_Ingame_ShowProps;
    private readonly InputAction m_Ingame_Shoot;
    public struct IngameActions
    {
        private @SeekerControls m_Wrapper;
        public IngameActions(@SeekerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Ingame_Look;
        public InputAction @Move => m_Wrapper.m_Ingame_Move;
        public InputAction @ShowProps => m_Wrapper.m_Ingame_ShowProps;
        public InputAction @Shoot => m_Wrapper.m_Ingame_Shoot;
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
                @ShowProps.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnShowProps;
                @ShowProps.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnShowProps;
                @ShowProps.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnShowProps;
                @Shoot.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnShoot;
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
                @ShowProps.started += instance.OnShowProps;
                @ShowProps.performed += instance.OnShowProps;
                @ShowProps.canceled += instance.OnShowProps;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
            }
        }
    }
    public IngameActions @Ingame => new IngameActions(this);
    private int m_KBMSchemeIndex = -1;
    public InputControlScheme KBMScheme
    {
        get
        {
            if (m_KBMSchemeIndex == -1) m_KBMSchemeIndex = asset.FindControlSchemeIndex("KB&M");
            return asset.controlSchemes[m_KBMSchemeIndex];
        }
    }
    public interface IIngameActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnShowProps(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
}
