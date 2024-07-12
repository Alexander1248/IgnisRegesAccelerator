using Interactables;
using Plugins.DialogueSystem.Scripts.Utils;
using UnityEngine;
using UnityEngine.Localization;

namespace Interactable
{
    public interface IInteractable
    {
        void Interact(PlayerInteract playerInteract);
        KeyCode TipButton { get; }
        LocalizedString TipName { get; }

        MeshRenderer[] MeshesOutline { get; }
        void Selected() {
            
        }
        void Deselected() {
            
        }
    }
}