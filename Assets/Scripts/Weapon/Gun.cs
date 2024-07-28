using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.Utils;

namespace Weapon
{
    [CreateAssetMenu(menuName = "Weapons/Gun")]
    public class Gun : Items.Weapon
    {
        [SerializeField] private float maxDistance = 100;
        [SerializeField] private float damage = 10;
        [SerializeField] private float rechargeTime = 0.3f;
        
        [SerializeField] private int cageSize = 3;
        [SerializeField] private float reloadTime = 10;
        private RaycastHit _hit;
        private ReloadManager _manager;
        private int _count;

        private ParticleSystem particleSystem;
        private Animator animator;
        private Animator animatorLight;
        private PlayerController playerController;
        private AudioSource audioSource;


        public float RechargeTime => rechargeTime;
        public float ReloadTime => reloadTime;


        public override void OnEquip(GameObject user, GameObject weapon)
        {
            _count = cageSize;
            _manager = weapon.GetComponent<ReloadManager>();
            particleSystem = weapon.GetComponentInChildren<ParticleSystem>();
            animator = weapon.GetComponent<Animator>();
            animatorLight = weapon.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>();
            audioSource = weapon.GetComponentInChildren<AudioSource>();
            if (user != null && user.TryGetComponent(out HandController handController)){
                playerController = handController.getPlayer();
                playerController.Control.Interaction.Reload.performed += StartReload;
            }
            _manager.gun = this;
        }

        public override void OnRelease(GameObject user, GameObject weapon)
        {
            if (playerController != null)
                playerController.Control.Interaction.Reload.performed -= StartReload;
                
        }

        private void StartReload(InputAction.CallbackContext callbackContext)
        {
            _manager.Reload();
        }

        public override void Action(GameObject user, GameObject weapon)
        {
            if (!_manager.IsCanShoot) return;

            particleSystem.Play(true);
            if (animator != null){
                if (!animator.enabled) animator.enabled = true;
                animator.CrossFade("GunShoot", 0.5f, 0, 0);
                animatorLight.Play("CanonExplosion", 0, 0);
            }
            if (playerController != null) playerController.AnimateCam("Cam_Shoot");
            if (audioSource != null) audioSource.Play();
            
            Debug.Log("[Gun]: Shoot!");
            _count--;
            if (_count <= 0)
                _manager.Lock();
            else
                _manager.Recharge();
            
            Debug.DrawRay(user.transform.position, user.transform.forward * maxDistance, Color.red);
            if (!Physics.Raycast(user.transform.position, user.transform.forward, 
                    out _hit, maxDistance)) return;
            
            Debug.Log("[Gun]:" + _hit.collider.name + " hit!");
            if (_hit.collider.gameObject.TryGetComponent(out Health health))
                health.DealDamage(damage, user.transform.forward, 0);
            
            if (_hit.collider.gameObject.TryGetComponent(out HealthUpdater updater))
                updater.DealDamage(damage, user.transform.forward, 0);


            // var o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // o.transform.position = hit.point;
            // var c = o.AddComponent<DestroyAfterDelay>();
            // c.delay = 10;
        }

        public void AnimReload(){
            if (animator != null){
                if (!animator.enabled) animator.enabled = true;
                animator.CrossFade("GunReload", 0.1f, 0, 0);
            }
        }

        public void Reload()
        {
            _count = cageSize;
            if (animator != null){
                if (!animator.enabled) animator.enabled = true;
                animator.CrossFade("GunRelaodBack", 0.1f, 0, 0);
            }
        }
        public override void AdditionalActionPerformed(GameObject user, GameObject weapon)
        {
            
        }

        public override void AdditionalActionCanceled(GameObject user, GameObject weapon)
        {
            
        }

        public override void AnimatorState(bool _enadled)
        {
            if (animator != null){
                animator.enabled = _enadled;
            }
        }
    }
}