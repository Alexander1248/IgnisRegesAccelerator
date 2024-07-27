using System;
using System.Linq;
using Items.Active;
using Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace Player.Interactables
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private Quaternion doorForwardRotation; 
        [SerializeField] private Animator animator; 
        public Transform doorObj;
        public Transform secondDoor; // for double doors
        private float _t;
        private float _startAngle;
        private float _endAngle;
        private bool _opened;

        [SerializeField] private bool locked = true;
        [SerializeField] private string code = "";

        private Transform _plr;

        [SerializeField] private float doorOpenAngle = 90;
        [SerializeField] private LocalizedString openTip;
        [SerializeField] private LocalizedString closeTip;
        [SerializeField] private MeshRenderer[] meshesOutline;
        public LocalizedString TipName => _opened ? closeTip : openTip;
        public MeshRenderer[] MeshesOutline => meshesOutline;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] clips;


        private InventoryManager manager;
        private void Start()
        {
            
            _plr = GameObject.FindGameObjectWithTag("Player").transform;
            if (!animator) 
                Debug.LogWarning("Door without animator!");
            
            if (!GameObject.FindGameObjectsWithTag("Player").Any(obj => obj.TryGetComponent(out manager)))
                throw new ArgumentException("Player with InventoryManager not found!");
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
                try
                {
                    var (id, pos) = manager.Find(item =>
                        item is KeyItem key && key.Code == code).First();
                    manager.RemoveItem(id, pos.x, pos.y);
                    locked = false;
                }
                catch (Exception)
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
            }

            if (animator) animator.enabled = false;
            if (audioSource)
            {
                audioSource.clip = clips[1];
                audioSource.Play();
            }

            _startAngle = doorObj.localEulerAngles.y;
            var direction = (_plr.position - doorObj.position).normalized;
            var dotProduct = Vector3.Dot(direction, doorObj.rotation * doorForwardRotation * Vector3.forward);
            if (_opened) 
                _endAngle = 0;
            else
                _endAngle = dotProduct switch
                {
                    >= 0 => -doorOpenAngle,
                    < 0 => doorOpenAngle,
                    _ => _endAngle
                };
            
            _opened = !_opened;
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

            if (secondDoor != null){
            secondDoor.localEulerAngles = new Vector3(
                    secondDoor.localEulerAngles.x,
                    Mathf.LerpAngle(-_startAngle, -_endAngle, _t),
                    secondDoor.localEulerAngles.z);
            }

            if (_t >= 1) _t = 0;
        }
    }
}