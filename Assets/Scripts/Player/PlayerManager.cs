using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class PlayerManager : NetworkBehaviour
{
    #region variables
    [SerializeField] private CharacterController characterController;
    private static bool isDead = false;
    [SerializeField] private Camera myCamera;
    [Header("Movement")]
    private float walkSpeed = 5f;
    private float jumpSpeed = 8f;
    private float gravity = -10;
    [SerializeField] private PlayerInput controls;
    private InputAction moveAction;
    private InputAction fireAction;
    private Vector3 moveDirection;
    [SerializeField] private MouseManager mouseManager;
    [Header("Game Mode")]
    private int teamID;
    public int TeamID { get { return teamID; } }
    [Header("Weapon")]
    [SerializeField] private List<Weapon> weapons;
    private int currWeapon = 0;
    private int lastWeapon = 0;
    [SerializeField] private Vector2 dropOffset;
    #endregion
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        moveAction = controls.actions.FindAction("Movement");
        moveAction.Enable();
        myCamera = this.GetComponentInChildren<Camera>();
    }
    private void Start()
    {
        foreach (Weapon weapon in weapons) weapon.gameObject.SetActive(false);
        SwitchWeapon(currWeapon);
    }
    public override void OnStartAuthority()
    {
        myCamera.gameObject.SetActive(true);
        base.OnStartAuthority();
    }
    private void Update()
    {
        if (!isDead)
        {
            Movement();
            if (Input.GetKeyDown(KeyCode.G)) DropWeapon(currWeapon);
        }
    }
    public void Movement()
    {
        Vector2 axis = moveAction.ReadValue<Vector2>();
        Vector3 forward = transform.forward * axis.y;
        Vector3 right = transform.right * axis.x;
        moveDirection = (forward + right) * walkSpeed;
        moveDirection.y += gravity;
        characterController.Move(moveDirection * Time.deltaTime);
    }
    public void SwitchWeapon(int _weaponID, bool overrideLock = false)
    {
        if (!overrideLock && weapons[currWeapon].isWeaponLocked == true) return;
        lastWeapon = currWeapon;
        currWeapon = _weaponID;
        weapons[lastWeapon].gameObject.SetActive(false);
        weapons[currWeapon].gameObject.SetActive(true);
    }
    public void PickUpWeapon(GameObject _weaponObject, Vector2 _originalLocation, int _teamID, int _weaponID, bool _overrideLock = false)
    {
        SwitchWeapon(_weaponID, _overrideLock);
        weapons[_weaponID].SetUp(_teamID, _weaponObject, _originalLocation);
    }
    public void DropWeapon(int _weaponID)
    {
        if (weapons[_weaponID].isWeaponDropable)
        {
            Vector3 forward = transform.forward;
            forward *= dropOffset.x;
            forward.y = dropOffset.y;
            Vector3 dropLocation = transform.position + forward;
            Debug.LogError("what the fuckity fuck");
            return;
            // weapons[_weaponID].DropWeapon(characterController, dropLocation);
            // weapons[_weaponID].worldWeapon.SetActive(true);
            // SwitchWeapon(lastWeapon, true);
        }
    }
    public void ReturnWeapon(int _weaponID)
    {
        if (weapons[_weaponID].isWeaponDropable)
        {
            Vector3 returnLocation = weapons[_weaponID].originalLocation;
            weapons[_weaponID].worldWeapon.transform.position = returnLocation;
            weapons[_weaponID].worldWeapon.SetActive(true);
            SwitchWeapon(lastWeapon, true);
        }
    }
    public bool IsHolding(int _weaponID)
    {
        if (currWeapon == _weaponID) return true;
        return false;
    }
}
