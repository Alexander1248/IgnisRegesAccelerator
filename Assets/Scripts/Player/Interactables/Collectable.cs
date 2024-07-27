using Items;
using Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace Player.Interactables
{
    public class Collectable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Item item;
        [SerializeField] private LocalizedString tipName;
        [SerializeField] private MeshRenderer[] meshesOutline;
        [SerializeField] private bool changeItemSecure;
        [SerializeField] private bool securedItem;
        public LocalizedString TipName => tipName;
        public MeshRenderer[] MeshesOutline => meshesOutline;
        
        private InventoryManager manager;
        private void Start()
        {
            manager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        }
        public void Interact(PlayerInteract playerInteract)
        {
            var i = Instantiate(item);
            if (changeItemSecure) i.secured = securedItem;
            if (manager.AddItem(i))
                Destroy(gameObject);
            else Destroy(item);
        }
    }
}