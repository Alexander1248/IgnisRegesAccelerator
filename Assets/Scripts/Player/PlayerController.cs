using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Player
{
    
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Transform cameraTransform;
        
        [SerializeField] private GameObject defaultVariant;
        [SerializeField] private Transform defaultCameraAnchor;
        
        [SerializeField] private GameObject crouchVariant;
        [SerializeField] private Transform crouchCameraAnchor;
    
        [Space]
        public bool canMove = true;
        [SerializeField] private float moveSpeed = 300;
        
        [Space]
        public bool canCrouch = true;
        [SerializeField] private float crouchSpeed = 150;
    
        [Space]
        public bool canSprint = true;
        [SerializeField] private float sprintSpeed = 600;
        
        [Space]
        public bool canDash = true;
        [SerializeField] private float dashSpeed = 1000;
        [SerializeField] private float dashTime = 1;
        
        [Space]
        public bool canJump = true;
        [SerializeField] private float jumpForce = 10;
 
        [Space]
        public bool cameraCanMove = true;
        public bool lockCameraX;  
        public bool invertCameraX; 
        public bool rotateCameraX;
        public bool lockCameraY;
        public bool invertCameraY;
        [SerializeField] private float mouseSensitivity = 1;
        [SerializeField] private float maxLookAngle = 60;
        public float cameraDistance;
        
        public bool isGrounded { get; private set; }
        public bool isSprinting { get; private set; }
        public bool isCrouching { get; private set; }

        private bool scheduleSprint;
        private bool useCrouchSpeed;
         private float _dashTime = 0;
        
        private Vector2 _cameraRotation = Vector2.zero;
        
        private Rigidbody rb;
        private RaycastHit hit;
        public PlayerControl Control { get; private set; }

        public void LockPlayer()
        {
            canMove = false;
            canCrouch = false;
            canJump = false;
            canDash = false;
            canSprint = false;
            cameraCanMove = false;
        }
        public void UnlockPlayer()
        {
            canMove = true;
            canCrouch = true;
            canJump = true;
            canDash = true;
            canSprint = true;
            cameraCanMove = true;
        }
        public void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        public void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        private void Awake()
        {
            LockCursor();
            rb = GetComponent<Rigidbody>();
            Control = new PlayerControl();
            Control.Movement.Jump.performed += OnJump;
        
            Control.Movement.Sprint.performed += OnSprint;
            Control.Movement.Sprint.canceled += OnSprintCanceled;
            Control.Movement.Dash.performed += OnDash;
            
            Control.Movement.Crouch.performed += OnCrouch;
            Control.Movement.Crouch.canceled += OnCrouchCanceled;
        }

        private void OnSprint(InputAction.CallbackContext obj)
        {
            if (!canSprint) return;
            if (obj.interaction is not HoldInteraction) return;
            Debug.Log("Sprint Start!");
            scheduleSprint = true;
        }
        private void OnSprintCanceled(InputAction.CallbackContext obj)
        {
            if (!canSprint) return;
            if (obj.interaction is not HoldInteraction) return;
            Debug.Log("Sprint End!");
            scheduleSprint = false;
        }
        private void OnDash(InputAction.CallbackContext obj)
        {
            if (!canDash) return;
            if (obj.interaction is not TapInteraction) return;
            Debug.Log("Dash!");
            _dashTime = dashTime;
        }
        
        private void OnCrouch(InputAction.CallbackContext obj)
        {
            if (!canCrouch) return;
            Debug.Log("Crouch Start!");
            isCrouching = true;
            defaultVariant.SetActive(false);
            crouchVariant.SetActive(true);
            cameraTransform.parent = crouchCameraAnchor;
        }
        private void OnCrouchCanceled(InputAction.CallbackContext obj)
        {
            if (!canCrouch) return;
            Debug.Log("Crouch End!");
            isCrouching = false;
            defaultVariant.SetActive(true);
            crouchVariant.SetActive(false);
            cameraTransform.parent = defaultCameraAnchor;
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            if (!canJump || !isGrounded) return;
            rb.AddForce(0f, jumpForce, 0f, ForceMode.VelocityChange);
            isGrounded = false;
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                var speed = isSprinting && canSprint ? sprintSpeed : moveSpeed;
                speed = Mathf.Lerp(speed, dashSpeed, Mathf.Max(0, _dashTime / dashTime));
                speed = useCrouchSpeed && canCrouch ? crouchSpeed : speed;
                var dir = Control.Movement.Move.ReadValue<Vector2>() * (speed * Time.fixedDeltaTime);
                var vel = transform.right * dir.x + transform.forward * dir.y;
                vel.x -= rb.velocity.x;
                vel.z -= rb.velocity.z;
                rb.AddForce(vel, ForceMode.VelocityChange);
                _dashTime -= Time.fixedDeltaTime;
            }

            if(cameraCanMove)
            {
                var mousePos = Control.Camera.Move.ReadValue<Vector2>();
                if (!lockCameraX)
                {
                    _cameraRotation.x = rotateCameraX ? 
                        camera.transform.localEulerAngles.y : transform.localEulerAngles.y;

                    if (invertCameraX)
                        _cameraRotation.x -= mouseSensitivity * mousePos.x;
                    else
                        _cameraRotation.x += mouseSensitivity * mousePos.x;
                }

                if (!lockCameraY)
                {
                    if (invertCameraY)
                        _cameraRotation.y += mouseSensitivity * mousePos.y;
                    else
                        _cameraRotation.y -= mouseSensitivity * mousePos.y;
                }

                _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -maxLookAngle, maxLookAngle);

                if (rotateCameraX) 
                    cameraTransform.transform.localEulerAngles = new Vector3(_cameraRotation.y, _cameraRotation.x, 0);
                else
                {
                    transform.localEulerAngles = new Vector3(0, _cameraRotation.x, 0);
                    cameraTransform.transform.localEulerAngles = new Vector3(_cameraRotation.y, 0, 0);
                }
                
                if (cameraDistance > 0)
                {
                    var vec = cameraTransform.transform.localRotation * Vector3.back;
                    cameraTransform.transform.localPosition = vec * cameraDistance;
                }
                else
                    cameraTransform.transform.localPosition = Vector3.zero;
                
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
            const float distance = 0.7f;

            if (Physics.Raycast(origin, direction, out _, distance, int.MaxValue, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(origin, direction * distance, Color.red);
                isGrounded = true;
                isSprinting = scheduleSprint;
                useCrouchSpeed = isCrouching;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
}
