using Player;
using UnityEngine;
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
        private PlayerController playerController;


        public float RechargeTime => rechargeTime;
        public float ReloadTime => reloadTime;


        public override void OnEquip(GameObject user, GameObject weapon)
        {
            _count = cageSize;
            _manager = weapon.GetComponent<ReloadManager>();
            particleSystem = weapon.GetComponentInChildren<ParticleSystem>();
            animator = weapon.GetComponent<Animator>();
            if (user != null && user.TryGetComponent<HandController>(out HandController handController)){
                playerController = handController.getPlayer();
            }
            _manager.gun = this;
        }

        public override void OnRelease(GameObject user, GameObject weapon)
        {
            
        }

        public override void Action(GameObject user, GameObject weapon)
        {
            if (!_manager.IsCanShoot) return;

            particleSystem.Play(true);
            if (animator != null){
                if (!animator.enabled) animator.enabled = true;
                animator.CrossFade("GunShoot", 0.5f, 0, 0);
            }
            if (playerController != null) playerController.AnimateCam("Cam_Shoot");
            
            Debug.Log("[Gun]: Shoot!");
            _count--;
            if (_count <= 0)
                _manager.Reload();
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

        public void Reload()
        {
            _count = cageSize;
        }
        public override void AdditionalActionPerformed(GameObject user, GameObject weapon)
        {
            
        }

        public override void AdditionalActionCanceled(GameObject user, GameObject weapon)
        {
            
        }
    }
}