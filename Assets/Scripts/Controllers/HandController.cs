using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Controllers
{
    public class HandController : MonoBehaviour
    {
        [SerializeField] private PlayerController controller;
        [SerializeField] private Transform mainHandAnchor;
        [SerializeField] private Transform secondHandAnchor;
        
        [SerializeField] private Items.Weapon mainHand;
        [SerializeField] private Items.Weapon secondHand;
        
        private GameObject mainHandObj;
        private bool mainHandAdditionalPerformed;
        private GameObject secondHandObj;
        private bool secondHandAdditionalPerformed;

        private void Start()
        {
            controller.Control.Interaction.MainHandAction.performed += OnMainHandActionPerformed;
            controller.Control.Interaction.MainHandAddtitonalAction.performed += OnMainHandAddtitonalActionPerformed;
            controller.Control.Interaction.MainHandAddtitonalAction.canceled += OnMainHandAddtitonalActionCanceled;
            
            controller.Control.Interaction.SecondHandAction.performed += OnSecondHandActionPerformed;
            controller.Control.Interaction.SecondHandAddtitonalAction.performed += OnSecondHandAddtitonalActionPerformed;
            controller.Control.Interaction.SecondHandAddtitonalAction.canceled += OnSecondHandAddtitonalActionCanceled;

            var mainHandBuff = mainHand;
            var secondHandBuff = secondHand;
            
            mainHand = null;
            secondHand = null;
            
            if (mainHandBuff) SetMainHand(mainHandBuff);
            if (secondHandBuff) SetSecondHand(secondHandBuff);
        }

        public bool SetMainHand(Items.Weapon weapon)
        {
            if (weapon.IsTwoHanded && secondHand) return false;
            if (mainHandObj) Destroy(mainHandObj);
            mainHand = Instantiate(weapon);
            mainHandObj = Instantiate(weapon.Prefab, mainHandAnchor);
            return true;
        }

        public void ClearMainHand()
        {
            if (mainHandObj) Destroy(mainHandObj);
            if (mainHand) Destroy(mainHand);
            mainHand = null;
        }
        public bool SetSecondHand(Items.Weapon weapon)
        {
            if (weapon.IsTwoHanded && mainHand) return false;
            if (secondHandObj) Destroy(secondHandObj);
            secondHand = Instantiate(weapon);
            secondHandObj = Instantiate(weapon.Prefab, secondHandAnchor);
            return true;
        }
        public void ClearSecondHand()
        {
            if (secondHandObj) Destroy(secondHandObj);
            if (secondHand) Destroy(secondHand);
            secondHand = null;
        }

        private void OnMainHandActionPerformed(InputAction.CallbackContext obj)
        {
            if (!mainHand) return;
            if (obj.interaction is not TapInteraction) return;
            mainHand.Action(gameObject,mainHandObj);
        }
        private void OnMainHandAddtitonalActionPerformed(InputAction.CallbackContext obj)
        {
            if (!mainHand) return;
            if (obj.interaction is not HoldInteraction) return;
            mainHand.AdditionalActionPerformed(gameObject,mainHandObj);
            mainHandAdditionalPerformed = true;
        }
        private void OnMainHandAddtitonalActionCanceled(InputAction.CallbackContext obj)
        {
            if (!mainHand) return;
            if (!mainHandAdditionalPerformed) return;
            mainHand.AdditionalActionCanceled(gameObject,mainHandObj);
            mainHandAdditionalPerformed = false;
        }

        
        private void OnSecondHandActionPerformed(InputAction.CallbackContext obj)
        {
            if (!secondHand) return;
            if (obj.interaction is not TapInteraction) return;
            secondHand.Action(gameObject,secondHandObj);
        }
        private void OnSecondHandAddtitonalActionPerformed(InputAction.CallbackContext obj)
        {
            if (!secondHand) return;
            if (obj.interaction is not HoldInteraction) return;
            secondHand.AdditionalActionPerformed(gameObject,secondHandObj);
            secondHandAdditionalPerformed = true;
        }
        private void OnSecondHandAddtitonalActionCanceled(InputAction.CallbackContext obj)
        {
            if (!secondHand) return;
            if (!secondHandAdditionalPerformed) return;
            secondHand.AdditionalActionCanceled(gameObject,secondHandObj);
            mainHandAdditionalPerformed = false;
        }
    }
}