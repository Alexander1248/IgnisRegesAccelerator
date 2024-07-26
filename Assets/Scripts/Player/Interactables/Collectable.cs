using Items;
using Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace Player.Interactables
{
    public class Collectable : MonoBehaviour, IInteractable
    {
        [SerializeField] private InventoryManager manager;
        [Space]
        [SerializeField] private Item item;
        [SerializeField] private LocalizedString tipName;
        [SerializeField] private MeshRenderer[] meshesOutline;
        public LocalizedString TipName => tipName;
        public MeshRenderer[] MeshesOutline => meshesOutline;
        public void Interact(PlayerInteract playerInteract)
        {
            manager.AddItem(Instantiate(item));
            Destroy(gameObject);
        }
    }
}