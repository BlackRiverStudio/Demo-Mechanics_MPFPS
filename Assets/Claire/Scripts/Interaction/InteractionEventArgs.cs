using UnityEngine;
using UnityEngine.Events;
using Serializable = System.SerializableAttribute;
namespace Claire
{
    [Serializable] public class InteractionEvent : UnityEvent<InteractionEventArgs> { }
    [Serializable] public class InteractionEventArgs
    {
        /// <summary>The controller that is sending the event assoociated with these args.</summary>
        public VrController controller;
        
        /// <summary>The rigidbody of the object that is being interacted with.</summary>
        public Rigidbody rigidbody;
        
        /// <summary>The collider of the object being interacted with.</summary>
        public Collider collider;

        public InteractionEventArgs(VrController _controller, Rigidbody _rigidbody, Collider _collider)
        {
            controller = _controller;
            rigidbody = _rigidbody;
            collider = _collider;
        }
    }
}