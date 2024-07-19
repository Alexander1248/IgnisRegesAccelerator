﻿using System;
using Interactable;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace Interactables
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private Quaternion doorForwardRotation; 
        [SerializeField] private Animator animator; 
        public Transform doorObj;
        private float _t;
        private float _startAngle;
        private float _endAngle;
        private bool _opened;

        [SerializeField] private bool locked = true;

        private Transform _plr;

        [SerializeField] private string lockedMessage = "LOCKED";

        [SerializeField] private LocalizedString tipName;
        [SerializeField] private MeshRenderer[] meshesOutline;
        public LocalizedString TipName => tipName;
        public MeshRenderer[] MeshesOutline => meshesOutline;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] clips;


        private void Start()
        {
            _plr = GameObject.FindGameObjectWithTag("Player").transform;
            if (!animator) 
                Debug.LogWarning("Door without animator!");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(doorObj.position,doorObj.rotation * doorForwardRotation * Vector3.forward);
            Gizmos.color = Color.white;
        }

        public void Interact(PlayerInteract playerInteract)
        {
            if (locked)
            {
                if (audioSource)
                {
                    audioSource.clip = clips[0];
                    audioSource.Play();
                }

                if (!animator) return;
                if (!animator.enabled) animator.enabled = true;
                animator.CrossFade("LockedDoor", 0.1f, 0, 0);
                return;
            }

            if (animator) animator.enabled = false;
            if (audioSource)
            {
                audioSource.clip = clips[1];
                audioSource.Play();
            }

            _opened = !_opened;

            _startAngle = doorObj.localEulerAngles.y;
            var direction = (_plr.position - doorObj.position).normalized;
            var dotProduct = Vector3.Dot(direction, doorObj.rotation * doorForwardRotation * Vector3.forward);
            if (_opened) 
                _endAngle = 0;
            else
                _endAngle = dotProduct switch
                {
                    >= 0 => -90,
                    < 0 => 90,
                    _ => _endAngle
                };

            _t = 0.01f;
        }

        private void Update()
        {
            if (_t == 0) return;
            _t += Time.deltaTime;

            doorObj.localEulerAngles = new Vector3(
                doorObj.localEulerAngles.x,
                Mathf.LerpAngle(_startAngle, _endAngle, _t),
                doorObj.localEulerAngles.z);

            if (_t >= 1) _t = 0;
        }
    }
}