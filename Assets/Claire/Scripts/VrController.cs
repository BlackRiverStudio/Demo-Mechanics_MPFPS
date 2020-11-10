using UnityEngine;
using Valve.VR;
namespace Claire
{
    [RequireComponent(typeof(SteamVR_Behaviour_Pose))]
    [RequireComponent(typeof(VrControllerInput))]
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))] // physics
    public class VrController : MonoBehaviour
    {
        public Vector3 Velocity { get { return pose.GetVelocity(); } }
        public Vector3 AngularVelocity { get { return pose.GetAngularVelocity(); } }
        public SteamVR_Input_Sources InputSource { get { return inputSource; } }
        [SerializeField] private SteamVR_Input_Sources inputSource;
        private SteamVR_Behaviour_Pose pose;
        private VrControllerInput input;
        private new SphereCollider collider;
        private new Rigidbody rigidbody;
        public void SetUp()
        {
            pose = gameObject.GetComponent<SteamVR_Behaviour_Pose>();
            input = gameObject.GetComponent<VrControllerInput>();
            collider = gameObject.GetComponent<SphereCollider>();
            rigidbody = gameObject.GetComponent<Rigidbody>();
            input.SetUp(this);
        }
    }
}