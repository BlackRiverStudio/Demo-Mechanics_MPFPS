using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Serializable = System.SerializableAttribute;
namespace Claire
{
    public class VrControllerInput : MonoBehaviour
    {
        [Serializable] public class InputEvent : UnityEvent<InputEventArgs> { }
        public VrController Controller { get { return controller; } }
        #region Steam_Actions
        [SerializeField] private SteamVR_Action_Boolean pointer;
        [SerializeField] private SteamVR_Action_Boolean teleport;
        [SerializeField] private SteamVR_Action_Boolean use;
        [SerializeField] private SteamVR_Action_Boolean grab;
        [SerializeField] private SteamVR_Action_Vector2 touchpadAxis;
        #endregion
        #region InputEvents
        public InputEvent onPointerPressed = new InputEvent();
        public InputEvent onPointerReleased = new InputEvent();
        public InputEvent onGrabPressed = new InputEvent();
        public InputEvent onGrabReleased = new InputEvent();
        public InputEvent onUsePressed = new InputEvent();
        public InputEvent onUseReleased = new InputEvent();
        public InputEvent onTeleportPressed = new InputEvent();
        public InputEvent onTeleportReleased = new InputEvent();
        public InputEvent onTouchpadAxisChanged = new InputEvent();
        #endregion
        private VrController controller;
        private InputEventArgs GenerateArgs()
        {
            return new InputEventArgs(controller, controller.InputSource, touchpadAxis.axis);
        }
        #region steam vr input callbacks
        private void OnPointerDown(SteamVR_Action_Boolean _action, SteamVR_Input_Sources _source) => onPointerPressed.Invoke(GenerateArgs());
        private void OnPointerUp(SteamVR_Action_Boolean _action, SteamVR_Input_Sources _source) => onPointerReleased.Invoke(GenerateArgs());
        private void OnGrabDown(SteamVR_Action_Boolean _action, SteamVR_Input_Sources _source) => onGrabPressed.Invoke(GenerateArgs());
        private void OnGrabUp(SteamVR_Action_Boolean _action, SteamVR_Input_Sources _source) => onGrabReleased.Invoke(GenerateArgs());
        private void OnUseDown(SteamVR_Action_Boolean _action, SteamVR_Input_Sources _source) => onUsePressed.Invoke(GenerateArgs());
        private void OnUseUP(SteamVR_Action_Boolean _action, SteamVR_Input_Sources _source) => onUseReleased.Invoke(GenerateArgs());
        private void OnTeleportDown(SteamVR_Action_Boolean _action, SteamVR_Input_Sources _source) => onTeleportPressed.Invoke(GenerateArgs());
        private void OnTeleportUp(SteamVR_Action_Boolean _action, SteamVR_Input_Sources _source) => onTeleportReleased.Invoke(GenerateArgs());
        private void OnTouchpadAxisChanged(SteamVR_Action_Vector2 _action, SteamVR_Input_Sources _source, Vector2 _axis, Vector2 _delta) => onTouchpadAxisChanged.Invoke(GenerateArgs());
        #endregion
        public void SetUp(VrController _controller)
        {
            controller = _controller;
            pointer.AddOnStateDownListener(OnPointerDown, controller.InputSource);
            pointer.AddOnStateDownListener(OnPointerUp, controller.InputSource);
            teleport.AddOnStateDownListener(OnTeleportDown, controller.InputSource);
            teleport.AddOnStateDownListener(OnTeleportUp, controller.InputSource);
            use.AddOnStateDownListener(OnUseDown, controller.InputSource);
            use.AddOnStateDownListener(OnUseUP, controller.InputSource);
            grab.AddOnStateDownListener(OnGrabDown, controller.InputSource);
            grab.AddOnStateDownListener(OnGrabUp, controller.InputSource);
            touchpadAxis.AddOnChangeListener(OnTouchpadAxisChanged, controller.InputSource);
        }
    }
}