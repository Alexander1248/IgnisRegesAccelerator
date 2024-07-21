using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace Player.Interactables
{
    public class WorldButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private Animator animator;
        [SerializeField] private string animatorState;
        [SerializeField] private float transitionDuration = 0.1f;

        [SerializeField] private LocalizedString tipName;
        [SerializeField] private MeshRenderer[] meshesOutline;
        public LocalizedString TipName => tipName;
        public MeshRenderer[] MeshesOutline => meshesOutline;
        [SerializeField] private AudioSource audioSource;
        public void Interact(PlayerInteract playerInteract)
        {
            onClick.Invoke();  
            if (!animator) return;
            if (!animator.enabled) animator.enabled = true;
            animator.CrossFade(animatorState, transitionDuration, 0, 0);
        }
    }
}