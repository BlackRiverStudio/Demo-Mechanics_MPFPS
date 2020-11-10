using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
namespace Claire
{
    [RequireComponent(typeof(Rigidbody))]
    public class InteractableObject : MonoBehaviour
    {
        public Rigidbody Rigidbody { get { return rigidbody; } }
        public Collider Collider { get { return collider; } }
        public Transform AttachPoint { get { return attachPoint; } }

        [SerializeField] private bool isGrabbable = true;
        [SerializeField] private bool isTouchable = false;
        [SerializeField] private bool isUsable = false;
        [SerializeField] private SteamVR_Input_Sources allowedSource = SteamVR_Input_Sources.Any;

        [Space]

        [SerializeField] private Transform attachPoint;

        [Space]

        public InteractionEvent onGrabbed = new InteractionEvent();
        public InteractionEvent onUngrabbed = new InteractionEvent();
        public InteractionEvent onTouched = new InteractionEvent();
        public InteractionEvent onUntouched = new InteractionEvent();
        public InteractionEvent onUsed = new InteractionEvent();
        public InteractionEvent onUnused = new InteractionEvent();

        private new Collider collider;
        private new Rigidbody rigidbody;

        private InteractionEventArgs GenerateArgs(VrController _controller)
        {
            return new InteractionEventArgs(_controller, rigidbody, collider);
        }

        private void Start()
        {
            collider = gameObject.GetComponent<Collider>();
            if (collider == null)
            {
                collider.gameObject.AddComponent<BoxCollider>();
                Debug.LogError(string.Format("Object {0} does not have a collider, adding a BoxCollider", name));
                // Debug.LogErrorFormat("Object {0} does not have a collider, adding a BoxCollider", name);
            }
            rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        public void OnObjectGrabbed(VrController _controller)
        {
            if (isGrabbable && (_controller.InputSource == allowedSource || allowedSource == SteamVR_Input_Sources.Any))
                onGrabbed.Invoke(GenerateArgs(_controller));
        }
        public void OnObjectUngrabbed(VrController _controller)
        {
            if (isGrabbable && (_controller.InputSource == allowedSource || allowedSource == SteamVR_Input_Sources.Any))
                onUngrabbed.Invoke(GenerateArgs(_controller));
        }
        public void OnObjectTouched(VrController _controller)
        {
            if (isTouchable && (_controller.InputSource == allowedSource || allowedSource == SteamVR_Input_Sources.Any))
                onTouched.Invoke(GenerateArgs(_controller));
        }
        public void OnObjectUntouched(VrController _controller)
        {
            if (isTouchable && (_controller.InputSource == allowedSource || allowedSource == SteamVR_Input_Sources.Any))
                onUntouched.Invoke(GenerateArgs(_controller));
        }
        public void OnObjectUsed(VrController _controller)
        {
            if (isUsable && (_controller.InputSource == allowedSource || allowedSource == SteamVR_Input_Sources.Any))
                onUsed.Invoke(GenerateArgs(_controller));
        }
        public void OnObjectUnused(VrController _controller)
        {
            if (isUsable && (_controller.InputSource == allowedSource || allowedSource == SteamVR_Input_Sources.Any))
                onUnused.Invoke(GenerateArgs(_controller));
        }
    }
}