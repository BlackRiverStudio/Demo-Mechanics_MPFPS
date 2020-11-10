using UnityEngine;
namespace Claire
{
    public class InputTest : MonoBehaviour
    {
        public void OnGrabPressed(InputEventArgs _args)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = transform.position + Random.insideUnitSphere * 2f;
        }
    }
}