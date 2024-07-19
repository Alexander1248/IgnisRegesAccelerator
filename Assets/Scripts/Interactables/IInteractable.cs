using Interactables;
using UnityEngine;
using UnityEngine.Localization;

namespace Interactable
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