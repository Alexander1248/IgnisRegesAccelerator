using System;
using System.Collections.Generic;
using Math;
using Player;
using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "Weapons/Sword")]
    public class Sword : Items.Weapon
    {
        [SerializeField] private float damageCurveThickness;
        [SerializeField] private float damage = 10;
        [SerializeField] private float delay = 1;
        
        private Animator animator;
        private PlayerController playerController;
        
        private RaycastHit[] hits;
        private HashSet<GameObject> objects;
        private CatmullRomSpline damageCurve;
        private float _time;

        public override void OnEquip(GameObject user, GameObject weapon)
        {
            _time = Time.realtimeSinceStartup - delay;
            hits = new RaycastHit[16];
            objects = new HashSet<GameObject>();
            animator = weapon.GetComponent<Animator>();
            damageCurve = weapon.GetComponentInChildren<CatmullRomSpline>();
            if (user != null && user.TryGetComponent<HandController>(out HandController handController)){
                playerController = handController.getPlayer();
            }
            if (damageCurve == null && damageCurve.Count < 1)
                throw new NullReferenceException("Damage curve error!");
        }

        public override void OnRelease(GameObject user, GameObject weapon)
        {
            hits = null;
            objects.Clear();
            objects = null;
        }

        public override void Action(GameObject user, GameObject weapon)
        {
            if (Time.realtimeSinceStartup - _time < delay) return;
            objects.Clear();
            if (animator != null){
                if (!animator.enabled) animator.enabled = true;
                animator.CrossFade("Sword", 0.5f, 0, 0);
            }
            if (playerController != null) playerController.AnimateCam("Cam_Sword");
            
            //swordHelper.Attack();

            _time = Time.realtimeSinceStartup;
        }

        public override void AdditionalActionPerformed(GameObject user, GameObject weapon)
        {
            
        }

        public override void AdditionalActionCanceled(GameObject user, GameObject weapon)
        {
            
        }

        public override void AnimatorState(bool enabled)
        {
            if (animator != null){
                animator.enabled = enabled;
            }
        }
    }
}