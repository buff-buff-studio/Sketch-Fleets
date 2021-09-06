// GENERATED AUTOMATICALLY FROM 'Assets/Input Actions/PlayerControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControl : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""667e7e09-1f96-4b67-8a93-9d934fd3613f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""c3e8bb9d-d6f5-4743-8e8b-2a4fae4f3a5b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""37486250-f384-4ca0-9f07-954cc49dce50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""SummonShips"",
                    ""type"": ""Button"",
                    ""id"": ""9328ae32-1dca-4eeb-9044-84789f690212"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Regen"",
                    ""type"": ""Button"",
                    ""id"": ""62b903b9-b69a-4daa-95a6-bc395b03d7dc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""51c383a1-243b-40cd-af0e-511508b6944b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""5e40e799-201f-4690-92e8-259b726ad04f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cd716097-b8ad-4076-8078-aa414373abe4"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""a8d10ff9-0320-4f59-a094-4badeace9153"",
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
                    ""id"": ""5e47b9bd-08e1-4ec8-a977-7ccef0b2c6fb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""69c93c65-0473-4603-9650-64e61a2beb8c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""10044b8b-0741-4e7d-bd7e-5c732a61217b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f49cc1b5-6754-46c8-bb07-7f41ea8cf831"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1cee869d-8e4f-4e97-9d53-3911dd5ce0c9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""102ab674-2814-4305-8f57-0a844c33b3eb"",
                    ""path"": ""<Touchscreen>/touch1/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2621cc1e-3ac4-4eca-9b2d-84e2afaec9fc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""SummonShips"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a4b4428-df2e-4835-ab20-8b9baea4a1ff"",
                    ""path"": ""<Touchscreen>/touch1/tap"",
                    ""interactions"": ""MultiTap"",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""SummonShips"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87430454-a391-4710-847d-1d661177c843"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Regen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7df11f4-3522-475d-8aaf-51afd8f58889"",
                    ""path"": ""<Touchscreen>/touch2/tap"",
                    ""interactions"": ""MultiTap(tapCount=3)"",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Regen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cccc3303-d212-499a-872f-7cd17dddb0b2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69682d58-e06b-4aed-8315-bca454d1002a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false,invertY=false)"",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Draw"",
            ""id"": ""eca2833c-b067-4228-b8c1-24666bad70cc"",
            ""actions"": [
                {
                    ""name"": ""Start Draw"",
                    ""type"": ""Button"",
                    ""id"": ""3c4561cf-b294-45dd-9288-1a891777219d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Draw"",
                    ""type"": ""Value"",
                    ""id"": ""cdc3da72-d2ba-4c0b-a2fa-2136b92be823"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dca88d01-81c7-447d-8cf4-a89b0cc229c4"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Start Draw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03611810-4ead-4b5c-bda1-c4b4367dd138"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme"",
                    ""action"": ""Draw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Control Scheme"",
            ""bindingGroup"": ""Control Scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_SummonShips = m_Player.FindAction("SummonShips", throwIfNotFound: true);
        m_Player_Regen = m_Player.FindAction("Regen", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        // Draw
        m_Draw = asset.FindActionMap("Draw", throwIfNotFound: true);
        m_Draw_StartDraw = m_Draw.FindAction("Start Draw", throwIfNotFound: true);
        m_Draw_Draw = m_Draw.FindAction("Draw", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_SummonShips;
    private readonly InputAction m_Player_Regen;
    private readonly InputAction m_Player_Pause;
    private readonly InputAction m_Player_Look;
    public struct PlayerActions
    {
        private @PlayerControl m_Wrapper;
        public PlayerActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @SummonShips => m_Wrapper.m_Player_SummonShips;
        public InputAction @Regen => m_Wrapper.m_Player_Regen;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
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
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @SummonShips.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSummonShips;
                @SummonShips.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSummonShips;
                @SummonShips.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSummonShips;
                @Regen.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRegen;
                @Regen.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRegen;
                @Regen.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRegen;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @SummonShips.started += instance.OnSummonShips;
                @SummonShips.performed += instance.OnSummonShips;
                @SummonShips.canceled += instance.OnSummonShips;
                @Regen.started += instance.OnRegen;
                @Regen.performed += instance.OnRegen;
                @Regen.canceled += instance.OnRegen;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Draw
    private readonly InputActionMap m_Draw;
    private IDrawActions m_DrawActionsCallbackInterface;
    private readonly InputAction m_Draw_StartDraw;
    private readonly InputAction m_Draw_Draw;
    public struct DrawActions
    {
        private @PlayerControl m_Wrapper;
        public DrawActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @StartDraw => m_Wrapper.m_Draw_StartDraw;
        public InputAction @Draw => m_Wrapper.m_Draw_Draw;
        public InputActionMap Get() { return m_Wrapper.m_Draw; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DrawActions set) { return set.Get(); }
        public void SetCallbacks(IDrawActions instance)
        {
            if (m_Wrapper.m_DrawActionsCallbackInterface != null)
            {
                @StartDraw.started -= m_Wrapper.m_DrawActionsCallbackInterface.OnStartDraw;
                @StartDraw.performed -= m_Wrapper.m_DrawActionsCallbackInterface.OnStartDraw;
                @StartDraw.canceled -= m_Wrapper.m_DrawActionsCallbackInterface.OnStartDraw;
                @Draw.started -= m_Wrapper.m_DrawActionsCallbackInterface.OnDraw;
                @Draw.performed -= m_Wrapper.m_DrawActionsCallbackInterface.OnDraw;
                @Draw.canceled -= m_Wrapper.m_DrawActionsCallbackInterface.OnDraw;
            }
            m_Wrapper.m_DrawActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StartDraw.started += instance.OnStartDraw;
                @StartDraw.performed += instance.OnStartDraw;
                @StartDraw.canceled += instance.OnStartDraw;
                @Draw.started += instance.OnDraw;
                @Draw.performed += instance.OnDraw;
                @Draw.canceled += instance.OnDraw;
            }
        }
    }
    public DrawActions @Draw => new DrawActions(this);
    private int m_ControlSchemeSchemeIndex = -1;
    public InputControlScheme ControlSchemeScheme
    {
        get
        {
            if (m_ControlSchemeSchemeIndex == -1) m_ControlSchemeSchemeIndex = asset.FindControlSchemeIndex("Control Scheme");
            return asset.controlSchemes[m_ControlSchemeSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnSummonShips(InputAction.CallbackContext context);
        void OnRegen(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
    public interface IDrawActions
    {
        void OnStartDraw(InputAction.CallbackContext context);
        void OnDraw(InputAction.CallbackContext context);
    }
}
