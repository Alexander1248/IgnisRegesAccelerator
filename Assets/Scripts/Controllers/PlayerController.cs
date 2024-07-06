using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CameraController controller;
    
        public bool canMove = true;
        [SerializeField] private float moveSpeed = 10;
    
        public bool canSprint = true;
        [SerializeField] private float sprintSpeed = 20;

    
        public bool canJump = true;
        [SerializeField] private float jumpForce = 10;
        public bool isGrounded { get; private set; }
        public bool isSprinting { get; private set; }

    
        private Rigidbody rb;
        public PlayerControl Control { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            Control = new PlayerControl();
            Control.Movement.Jump.performed += OnJump;
        
            Control.Movement.Sprint.started += OnSprint;
            Control.Movement.Sprint.canceled += OnSprint;
        
            Control.Interaction.Inventory.performed += OnInventoryStateChange;
        
            Control.Interaction.Use.performed += OnUse;
        }

        private void OnSprint(InputAction.CallbackContext obj)
        {
            if (!canSprint) return;
            isSprinting = !isSprinting;
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            if (!canJump || !isGrounded) return;
            rb.AddForce(0f, jumpForce, 0f, ForceMode.VelocityChange);
            isGrounded = false;
        }

        private void OnInventoryStateChange(InputAction.CallbackContext obj)
        {
            // TODO: Inventory open/close logic (create by ui builder)
        }
    
        private void OnUseStarted(InputAction.CallbackContext obj)
        {
            // TODO: Object use logic (after camera rotation)
        }
        private void OnUse(InputAction.CallbackContext obj)
        {
            // TODO: Object use logic (after camera rotation)
        }
        private void OnUseEnded(InputAction.CallbackContext obj)
        {
            // TODO: Object use logic (after camera rotation)
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                var speed = isSprinting && canSprint ? sprintSpeed : moveSpeed;
                var dir = Control.Movement.Move.ReadValue<Vector2>() * (speed * Time.fixedDeltaTime);
                var shift = transform.right * dir.x + transform.forward * dir.y;
                rb.MovePosition(rb.position + shift);
            }

            CheckGround();
        }

        private void OnEnable()
        {
            Control.Enable();
        }
        private void OnDisable()
        {
            Control.Disable();
        }
        private void CheckGround()
        {
            var origin = new Vector3(transform.position.x, transform.position.y - transform.localScale.y * .7f, transform.position.z);
            var direction = transform.TransformDirection(Vector3.down);
            const float distance = 0.5f;

            if (Physics.Raycast(origin, direction, out _, distance, int.MaxValue, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(origin, direction * distance, Color.red);
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
}
