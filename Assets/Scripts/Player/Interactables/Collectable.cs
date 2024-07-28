using System;
using System.Linq;
using Items;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace Player.Interactables
{
    public class Collectable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Item item;
        [SerializeField] private LocalizedString tipName;
        [SerializeField] private MeshRenderer[] meshesOutline;
        [FormerlySerializedAs("changeItemSecure")]
        [SerializeField] private bool changeItemSecurity;
        [SerializeField] private bool securedItem;
        public LocalizedString TipName => tipName;
        public MeshRenderer[] MeshesOutline => meshesOutline;
        [SerializeField] private UnityEvent onInteract;
        
        private InventoryManager manager;
        private void Start()
        {
            if (!GameObject.FindGameObjectsWithTag("Player").Any(obj => obj.TryGetComponent(out manager)))
                throw new ArgumentException("Player with InventoryManager not found!");
        }
        public void Interact(PlayerInteract playerInteract)
        {
            onInteract.Invoke();  
            var i = Instantiate(item);
            if (changeItemSecurity) i.secured = securedItem;
            if (manager.AddItem(i))
                Destroy(gameObject);
            else Destroy(item);
        }
    }
}