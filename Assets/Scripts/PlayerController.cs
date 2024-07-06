using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CameraController controller;

    [SerializeField] private float playerSpeed = 10;
    
    private Rigidbody rb;
    public PlayerControl Control { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Control = new PlayerControl();
        Control.Movement.Jump.performed += OnJump;
        Control.Interaction.Inventory.performed += OnInventoryStateChange;
        Control.Interaction.Use.performed += OnUse;
    }
    private void OnJump(InputAction.CallbackContext obj)
    {
        // TODO: Jump logic
    }
    private void OnInventoryStateChange(InputAction.CallbackContext obj)
    {
        // TODO: Inventory open/close logic
    }
    private void OnUse(InputAction.CallbackContext obj)
    {
        // TODO: Object use logic
    }

    private void FixedUpdate()
    {
        var dir = Control.Movement.Move.ReadValue<Vector2>() * (playerSpeed * Time.fixedDeltaTime);
        
        rb.MovePosition(rb.position + new Vector3(dir.x, 0, dir.y));
    }

    private void OnEnable()
    {
        Control.Enable();
    }
    private void OnDisable()
    {
        Control.Disable();
    }
}
