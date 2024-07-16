using System;
using System.Collections.Generic;
using Controllers;
using Math;
using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "Weapons/Sword")]
    public class Sword : Items.Weapon
    {
        [SerializeField] private float damageCurveThickness;
        [SerializeField] private float damage = 10;
        [SerializeField] private float delay = 1;
        
        
        private RaycastHit[] hits;
        private HashSet<GameObject> objects;
        private CatmullRomSpline damageCurve;
        private float _time;

        public override void OnEquip(GameObject user, GameObject weapon)
        {
            _time = Time.realtimeSinceStartup - delay;
            hits = new RaycastHit[16];
            objects = new HashSet<GameObject>();
            damageCurve = weapon.GetComponentInChildren<CatmullRomSpline>();
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
            for (var i = 0; i < damageCurve.Count; i++)
            {
                var previousPoint = damageCurve.Get(i - 1);
                for (var j = 1; j <= damageCurve.Segments; j++)
                {
                    var t = j / (float)damageCurve.Segments;
                    var point = damageCurve.Compute(i, t);
                    var dir = point - previousPoint;
                    var count = Physics.SphereCastNonAlloc(
                        previousPoint,
                        damageCurveThickness,
                        dir.normalized,
                        hits,
                        dir.magnitude);
                    for (var c = 0; c <= count; c++)
                    {
                        if (hits[c].collider == null) continue;
                        objects.Add(hits[c].collider.gameObject);
                    }

                    previousPoint = point;
                }
            }
            
            foreach (var obj in objects)
            {
                Debug.Log("[Sword]:" + obj.name + " hit!");
                if (obj.TryGetComponent(out Health health))
                    health.DealDamage(damage, user.transform.forward, 0);
            
                if (obj.TryGetComponent(out HealthUpdater updater))
                    updater.DealDamage(damage, user.transform.forward, 0);
            }
            _time = Time.realtimeSinceStartup;
        }

        public override void AdditionalActionPerformed(GameObject user, GameObject weapon)
        {
            
        }

        public override void AdditionalActionCanceled(GameObject user, GameObject weapon)
        {
            
        }
    }
}