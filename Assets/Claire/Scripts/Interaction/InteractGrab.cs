using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Claire
{
    [RequireComponent(typeof(VrControllerInput))]
    public class InteractGrab : MonoBehaviour
    {
        private VrControllerInput input;
        public InteractionEvent grabbed = new InteractionEvent();
        public InteractionEvent unGrabbed = new InteractionEvent();
        private InteractableObject collidingObject;
        private InteractableObject heldObject;
        private void Start()
        {
            input = gameObject.GetComponent<VrControllerInput>();
            input.onGrabPressed.AddListener(OnGrabPressed);
            input.onGrabReleased.AddListener(OnGrabReleased);
        }
        private void OnGrabPressed(InputEventArgs _args)
        {
            if (collidingObject != null) GrabObject();
        }
        private void OnGrabReleased(InputEventArgs _args)
        {
            if (heldObject != null) UngrabObject();
        }
        private void SetCollidingObject(Collider _collider)
        {
            InteractableObject interactable = _collider.GetComponent<InteractableObject>();
            if (collidingObject != null || interactable == null) return;
            collidingObject = interactable;
        }
        private void OnTriggerEnter(Collider _collider) => SetCollidingObject(_collider);
        private void OnTriggerExit(Collider _collider)
        {
            // if the object we're colliding with already is the existing object, reset collidingObject
            if (collidingObject == _collider.GetComponent<InteractableObject>()) collidingObject = null;
        }
        private void GrabObject()
        {
            heldObject = collidingObject;
            collidingObject = null;
            FixedJoint joint = AddJoint();
            joint.connectedBody = heldObject.Rigidbody;
            if (heldObject.AttachPoint != null)
            {
                heldObject.transform.position = transform.position - (heldObject.AttachPoint.position - heldObject.transform.position);
                heldObject.transform.rotation = transform.rotation * Quaternion.Euler(heldObject.AttachPoint.localEulerAngles);
            }
            else heldObject.transform.position = transform.position;
            grabbed.Invoke(new InteractionEventArgs(input.Controller, heldObject.Rigidbody, heldObject.Collider));
            heldObject.OnObjectGrabbed(input.Controller);
        }
        private void UngrabObject()
        {
            FixedJoint joint = gameObject.GetComponent<FixedJoint>();
            if (joint != null)
            {
                joint.connectedBody = null;
                Destroy(joint);
                heldObject.Rigidbody.velocity = input.Controller.Velocity;
                heldObject.Rigidbody.angularVelocity = input.Controller.AngularVelocity;
            }
            unGrabbed.Invoke(new InteractionEventArgs(input.Controller, heldObject.Rigidbody, heldObject.Collider));
            heldObject.OnObjectUngrabbed(input.Controller);
            heldObject = null;
        }
        private FixedJoint AddJoint()
        {
            FixedJoint fx = gameObject.AddComponent<FixedJoint>();
            fx.breakForce = 20000;
            fx.breakTorque = 20000;
            return fx;
        }
    }
}
// FixedUpdate is Update's sister - wouldn't say shes much better but at least she consistent ¯\_(ツ)_/¯