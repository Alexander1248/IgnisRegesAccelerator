using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Player
{
    
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Stamina stamina;
        [SerializeField] private Camera camera;
        [SerializeField] private Transform cameraTransform;
        
        [SerializeField] private GameObject defaultVariant;
        [SerializeField] private Transform defaultCameraAnchor;
        [SerializeField] private Transform defaultCameraAnchorSAVE;

        [SerializeField] private GameObject crouchVariant;
        [SerializeField] private Transform crouchCameraAnchor;

        [SerializeField] private GameObject layVariant;
        [SerializeField] private Transform layCameraAnchor;

        [SerializeField] private float standHeight;
    
        [Space]
        public bool canMove = true;
        [SerializeField] private float moveSpeed = 300;

        [Space]
        private bool usingHeavyObj = false;
        [SerializeField] private float slowSpeed = 50;
        
        [Space]
        public bool canCrouch = true;
        [SerializeField] private float crouchSpeed = 150;

        [Space]
        public bool canLay = true;
        [SerializeField] private float laySpeed = 50;
    
        [Space]
        public bool canSprint = true;
        [SerializeField] private float sprintSpeed = 600;
        [SerializeField] private float sprintStaminaUsage = 1;
        
        [Space]
        public bool canDash = true;
        [SerializeField] private float dashSpeed = 1000;
        [SerializeField] private float dashTime = 1;
        [SerializeField] private float dashStaminaUsage = 30;
        
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
        public bool isLaying { get; private set; }

        private bool wantToStand;
        private bool scheduleSprint;
        private bool useCrouchSpeed;
        private float _dashTime;
        
        private Vector2 _cameraRotation = Vector2.zero;
        
        private Rigidbody rb;
        private RaycastHit hit;
        public PlayerControl Control { get; private set; }

        [SerializeField] private Transform GUIObj;

        [SerializeField] private AudioSource[] audioSource;
        private int footK;
        private float tStep;
        [SerializeField] private float[] pitchK;
        [SerializeField] private float SoundStepMultiplier;
        
        [SerializeField] private float SoundStepMultiplierVENT;
        private float SoundStepMultiplierSAVE;
        [SerializeField] private AudioClip[] ventClips;
        [SerializeField] private AudioClip woodClip;
        private bool inVent;

        [SerializeField] private Animator camAnimator;
        [SerializeField] private AnimatorController animatorController;
        [SerializeField] private float layingAnimKoeff;

        public Transform getCamAnchor(){
            return defaultCameraAnchor;
        }

        public bool isLayingOrCrouch() => isLaying || isCrouching;

        public void LockPlayer()
        {
            canMove = false;
            canCrouch = false;
            canJump = false;
            canLay = false;
            canDash = false;
            canSprint = false;
            cameraCanMove = false;
        }
        public void UnlockPlayer()
        {
            canMove = true;
            canCrouch = true;
            canJump = true;
            canLay = true;
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
            Cursor.lockState = CursorLockMode.None;
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
            
            Control.Movement.Lay.performed += OnLay;
            Control.Movement.Lay.canceled += OnLayCanceled;
        }

        void Start(){
            SoundStepMultiplierSAVE = SoundStepMultiplier;
            GUIObj.SetParent(null);
            mouseSensitivity =  PlayerPrefs.GetFloat("Sens", 0.15f);
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
            if (stamina.Use(dashStaminaUsage))
                _dashTime = dashTime;
        }
        
        private void OnCrouch(InputAction.CallbackContext obj)
        {
            if (!canCrouch) return;
            Debug.Log("Crouch Start!");

            wantToStand = false;
            isCrouching = true;

            crouchVariant.SetActive(true);
            cameraTransform.parent.parent.SetParent(crouchCameraAnchor); // currentAnchor.SetParent(other anchor)
            cameraTransform.parent.parent.localPosition = Vector3.zero;
            defaultVariant.SetActive(false);
            layVariant.SetActive(false);
        }
        private void OnCrouchCanceled(InputAction.CallbackContext obj)
        {
            if (!canCrouch) return;
            Debug.Log("Crouch End!");
            wantToStand = true;
        }

        void Stand(){
            Debug.Log("Stand!");
            wantToStand = false;
            isCrouching = false;
            isLaying = false;

            defaultVariant.SetActive(true);
            cameraTransform.parent.parent.SetParent(defaultCameraAnchorSAVE);
            cameraTransform.parent.parent.localPosition = Vector3.zero;
            crouchVariant.SetActive(false);
            layVariant.SetActive(false);

            camAnimator.runtimeAnimatorController = null;
            camAnimator.speed = 1;
            cameraTransform.position = Vector3.zero;
        }

        private void OnLay(InputAction.CallbackContext obj)
        {
            _onLay();
        }
        void _onLay(){
            if (!canLay) return;
            Debug.Log("Lay Start!");
            wantToStand = false;
            isLaying = true;
            layVariant.SetActive(true);
            cameraTransform.parent.parent.SetParent(layCameraAnchor);
            cameraTransform.parent.parent.localPosition = Vector3.zero;
            Debug.Log(cameraTransform.parent.parent);
            Debug.Log(cameraTransform.parent.parent.localPosition);
            defaultVariant.SetActive(false);
            crouchVariant.SetActive(false);
            camAnimator.enabled = true;
            camAnimator.runtimeAnimatorController = animatorController;
            camAnimator.Play("LayingAnim", 0, 0);
        }
        private void OnLayCanceled(InputAction.CallbackContext obj)
        {
             _onLayCanceled();
        }
        void _onLayCanceled(){
            if (!canLay) return;
            Debug.Log("Lay End!");
            wantToStand = true;
        }

        public void UseRailCanon(bool active){
            rb.velocity = Vector3.zero;
            canSprint = active;
            canDash = active;
            canCrouch = active;
            canJump = active;
            canMove = active;
            canLay = active;
        }

        public void useCanon(){
            canSprint = false;
            canDash = false;
            canCrouch = false;
            canJump = false;
            canLay = false;
            usingHeavyObj = true;
            mouseSensitivity /= 3;
        }
        public void releaseCanon(){
            rb.velocity = Vector3.zero;
            canSprint = true;
            canDash = true;
            canCrouch = true;
            canJump = true;
            canLay = true;
            usingHeavyObj = false;
            mouseSensitivity *= 3;
        }

        public void AnimateCam(string animName){
            if (!camAnimator.enabled)
                camAnimator.enabled = true;
            if (camAnimator.runtimeAnimatorController == null)
                camAnimator.runtimeAnimatorController = animatorController;
            camAnimator.speed = 1;
            camAnimator.CrossFade(animName, 0.5f, 0, 0);
        }

        public void ResetCamRotation(){
            _cameraRotation = Vector3.zero;
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            if (!canJump || !isGrounded) return;
            rb.AddForce(0f, jumpForce, 0f, ForceMode.VelocityChange);
            isGrounded = false;
        }

        public void ForceLay(){
            SoundStepMultiplier = SoundStepMultiplierVENT;
            inVent = true;
             _onLay();
             _onLayCanceled();
        }

        void checkHeight(){
            var origin = new Vector3(transform.position.x, isCrouching ? crouchCameraAnchor.position.y : layCameraAnchor.position.y, transform.position.z);
            float distance = standHeight;

            Debug.DrawRay(origin, Vector3.up * distance, Color.red);
            if (Physics.Raycast(origin, Vector3.up, out _, distance, int.MaxValue, QueryTriggerInteraction.Ignore))
            {
                
            }
            else
            {
                Stand();
            }
        }

        public void CheckSettings(){
            mouseSensitivity =  PlayerPrefs.GetFloat("Sens", 0.15f);
        }

        public void ExitVent(){
            SoundStepMultiplier = SoundStepMultiplierSAVE;
            inVent = false;
        }

        void playStep(){
            if (!isGrounded) return;
            footK++;
            if (footK >= 2) footK = 0;
            audioSource[footK].pitch = Random.Range(pitchK[0], pitchK[1]);
            if (inVent) audioSource[footK].clip = ventClips[Random.Range(0, ventClips.Length)];
            else audioSource[footK].clip = woodClip;
            audioSource[footK].Play();
        }

        private void FixedUpdate()
        {
            if (isLaying){
                camAnimator.speed = rb.velocity.magnitude * layingAnimKoeff;
            }

            tStep += rb.velocity.magnitude * SoundStepMultiplier * Time.deltaTime;
            if (tStep >= 1){
                playStep();
                tStep = 0;
            }

            if (wantToStand){
                checkHeight();
            }

            if (canMove)
            {
                var speed = isSprinting && canSprint 
                                        && stamina.Use(sprintStaminaUsage * Time.fixedDeltaTime) ? sprintSpeed : moveSpeed;
                speed = Mathf.Lerp(speed, dashSpeed, Mathf.Max(0, _dashTime / dashTime));
                speed = useCrouchSpeed && canCrouch ? crouchSpeed : speed;
                speed = isLaying ? laySpeed : speed;
                if (usingHeavyObj) speed = slowSpeed;
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

        private void OnSprint()
        {
            stamina.Use(sprintStaminaUsage);
        }
    }
}
