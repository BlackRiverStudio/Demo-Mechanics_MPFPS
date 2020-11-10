using Mirror;
using UnityEngine;

public class MouseManager : NetworkBehaviour
{
    [SerializeField] private Camera myCamera;
    private Transform cameraTransform;
    private float xRotation = 0f;
    private float range = 30f;
    private Controls controls;
    private Controls Controls
    {
        get
        {
            if (controls != null) return controls;
            return controls = new Controls();
        }
    }
    [ClientCallback] private void OnEnable() => Controls.Enable();
    [ClientCallback] private void OnDisable() => Controls.Disable();
    public override void OnStartAuthority()
    {
        cameraTransform = myCamera.transform;
        cameraTransform.gameObject.SetActive(true);
        enabled = true;
        Controls.Player.Look.performed += ctx => MouseLook(ctx.ReadValue<Vector2>());
        Controls.Player.Fire.performed += ctx => Fire();
    }
    private void MouseLook(Vector2 _lookAxis)
    {
        xRotation -= _lookAxis.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * _lookAxis.x);
    }
    private void Fire() => CmdFire(cameraTransform.position, cameraTransform.forward);
    [Command] private void CmdFire(Vector3 _cameraPos, Vector3 _cameraForward)
    {
        Ray ray = new Ray(_cameraPos, _cameraForward * range);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                var thatPlayer = hit.collider.GetComponent<NetworkIdentity>();
                Debug.Log(thatPlayer.netId + " was shot.");
            }
        }
    }
}
