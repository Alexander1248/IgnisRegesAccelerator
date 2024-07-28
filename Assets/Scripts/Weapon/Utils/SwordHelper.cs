using System;
using System.Collections.Generic;
using Math;
using Player;
using UnityEngine;

public class SwordHelper : MonoBehaviour
{
    [SerializeField] private float damageCurveThickness;
    [SerializeField] private float damage = 10;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioClip[] hitClips;

    [SerializeField] private bool canIDamagePlayer;

    [SerializeField] private Transform swordSpline;
    
    private RaycastHit[] hits;
    private HashSet<GameObject> objects;
    private CatmullRomSpline damageCurve;

    void Start(){
        hits = new RaycastHit[16];
        objects = new HashSet<GameObject>();
        damageCurve = GetComponentInChildren<CatmullRomSpline>();
        if (damageCurve == null && damageCurve.Count < 1)
            throw new NullReferenceException("Damage curve error!");
        swordSpline.SetParent(transform.parent);
    }


    public void Attack(){
        audioSource.Play();
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
        
        bool hittedSomeone = false;
        foreach (var obj in objects)
        {
            if (!canIDamagePlayer && obj.CompareTag("Player")) continue;
            Debug.Log("[Sword]:" + obj.name + " hit!");
            if (obj.TryGetComponent(out Health health))
            {
                health.DealDamage(damage, transform.forward, 0);
                hittedSomeone = true;
            }
        
            if (obj.TryGetComponent(out HealthUpdater updater))
            {
                updater.DealDamage(damage, transform.forward, 0);
                hittedSomeone = true;
            }
        }
        if (hittedSomeone){
            hitSound.clip = hitClips[0];
            hitSound.Play();
        }
        else if (objects.Count != 0){
            hitSound.clip = hitClips[1];
            hitSound.Play();
        }

        hits = new RaycastHit[16];
        objects = new HashSet<GameObject>();
    }
}
