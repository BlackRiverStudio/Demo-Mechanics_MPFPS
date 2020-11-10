using Mirror;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [Header("Weapon")]
    public int teamID;
    public bool isWeaponLocked = false;
    public bool isWeaponDropable = false;
    public GameObject worldWeapon;
    public Vector3 originalLocation;
    public void SetUp(int _teamID, GameObject _worldObject, Vector3 _originalPos)
    {
        this.teamID = _teamID;
        if (_worldObject != null) worldWeapon = _worldObject;
        this.originalLocation = _originalPos;
    }
    public void DropLocation(Vector3 _dropLocation)
    {
        Vector3 direction2Drop = _dropLocation - Camera.main.transform.position;
        Ray ray2DropLocation = new Ray(Camera.main.transform.position, direction2Drop);
        if (Physics.Raycast(ray2DropLocation, out RaycastHit hit, direction2Drop.magnitude)) _dropLocation = hit.point;
        worldWeapon.transform.position = _dropLocation;
        Renderer rend = worldWeapon.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Vector3 topPoint = rend.bounds.center;
            topPoint.y += rend.bounds.extents.y;
            float height = rend.bounds.extents.y * 2;
            Ray rayDown = new Ray(topPoint, Vector3.down);
            if (Physics.Raycast(rayDown, out RaycastHit downHit, height * 1.1f))
            {
                _dropLocation = downHit.point;
                _dropLocation.y += height * 1.1f;
            }
            worldWeapon.transform.position = _dropLocation;
        }
        else Debug.LogError("Renderer not found for drop location.");
    }
}
