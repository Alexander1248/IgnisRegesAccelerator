using UnityEngine;
using UnityEngine.Localization;

namespace Player.Interactables
{
    public interface IInteractable
    {
        void Interact(PlayerInteract playerInteract);
        LocalizedString TipName { get; }

        MeshRenderer[] MeshesOutline { get; }
        void Selected() {
            
        }
        void Deselected() {
            
        }
    }
}