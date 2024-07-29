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
        [SerializeField] private bool changeItemLock;
        [SerializeField] private bool lockedItem;
        public LocalizedString TipName => tipName;
        public MeshRenderer[] MeshesOutline => meshesOutline;
        [SerializeField] private UnityEvent onInteract;
        
        private InventoryManager manager;
        private HandController handController;
        private void Start()
        {
            if (!GameObject.FindGameObjectsWithTag("Player").Any(obj => obj.TryGetComponent(out manager)))
                throw new ArgumentException("Player with InventoryManager not found!");
            handController = manager.GetHandController();
        }
        public void Interact(PlayerInteract playerInteract)
        {
            onInteract.Invoke();  
            var i = Instantiate(item);
            if (changeItemSecurity) i.secured = securedItem;
            if (changeItemLock) i.lockedInInventory = lockedItem;
            if (item is Items.Weapon weapon)
            {
                if (handController.IsMainHandEmpty && handController.SetMainHand(weapon))
                {
                    Destroy(gameObject);
                    return;
                }
                if (handController.IsSecondHandEmpty && handController.SetSecondHand(weapon))
                {
                    Destroy(gameObject);
                    return;
                }
            }
            if (manager.AddItem(i))
                Destroy(gameObject);
            else Destroy(item);
        }
    }
}