using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Player
{
    public class HandController : MonoBehaviour
    {
        [SerializeField] private PlayerController controller;
        [SerializeField] private Transform mainHandAnchor;
        [SerializeField] private Transform secondHandAnchor;
        
        [SerializeField] private Items.Weapon mainHand;
        [SerializeField] private Items.Weapon secondHand;

        private GameObject _mainHandObj;
        private bool _mainHandAdditionalPerformed;
        private GameObject _secondHandObj;
        private bool _secondHandAdditionalPerformed;

        public void MainHandActive(bool active)
        {
            _mainHandObj.SetActive(active);
        }
        public void SecondHandActive(bool active)
        {
            _secondHandObj.SetActive(active);
        }
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
            ClearMainHand();
            mainHand = Instantiate(weapon);
            _mainHandObj = Instantiate(weapon.Prefab, mainHandAnchor);
            if (mainHand) mainHand.OnEquip(gameObject,_mainHandObj);
            return true;
        }

        public void ClearMainHand()
        {
            if (_mainHandObj) Destroy(_mainHandObj);
            if (mainHand)
            {
                mainHand.OnRelease(gameObject,_mainHandObj);
                Destroy(mainHand);
            }
            mainHand = null;
        }
        public bool SetSecondHand(Items.Weapon weapon)
        {
            if (weapon.IsTwoHanded && mainHand) return false;
            ClearSecondHand();
            secondHand = Instantiate(weapon);
            _secondHandObj = Instantiate(weapon.Prefab, secondHandAnchor);
            if (secondHand) secondHand.OnEquip(gameObject,_secondHandObj);
            return true;
        }
        public void ClearSecondHand()
        {
            if (_secondHandObj) Destroy(_secondHandObj);
            if (secondHand) Destroy(secondHand);
            secondHand = null;
        }

        private void OnMainHandActionPerformed(InputAction.CallbackContext obj)
        {
            if (!mainHand) return;
            if (obj.interaction is not TapInteraction) return;
            if (!_mainHandObj.activeInHierarchy) return;
            mainHand.Action(gameObject,_mainHandObj);
        }
        private void OnMainHandAddtitonalActionPerformed(InputAction.CallbackContext obj)
        {
            if (!mainHand) return;
            if (obj.interaction is not HoldInteraction) return;
            if (!_mainHandObj.activeInHierarchy) return;
            mainHand.AdditionalActionPerformed(gameObject,_mainHandObj);
            _mainHandAdditionalPerformed = true;
        }
        private void OnMainHandAddtitonalActionCanceled(InputAction.CallbackContext obj)
        {
            if (!mainHand) return;
            if (!_mainHandAdditionalPerformed) return;
            if (!_mainHandObj.activeInHierarchy) return;
            mainHand.AdditionalActionCanceled(gameObject,_mainHandObj);
            _mainHandAdditionalPerformed = false;
        }

        
        private void OnSecondHandActionPerformed(InputAction.CallbackContext obj)
        {
            if (!secondHand) return;
            if (obj.interaction is not TapInteraction) return;
            if (!_secondHandObj.activeInHierarchy) return;
            secondHand.Action(gameObject,_secondHandObj);
        }
        private void OnSecondHandAddtitonalActionPerformed(InputAction.CallbackContext obj)
        {
            if (!secondHand) return;
            if (obj.interaction is not HoldInteraction) return;
            if (!_secondHandObj.activeInHierarchy) return;
            secondHand.AdditionalActionPerformed(gameObject,_secondHandObj);
            _secondHandAdditionalPerformed = true;
        }
        private void OnSecondHandAddtitonalActionCanceled(InputAction.CallbackContext obj)
        {
            if (!secondHand) return;
            if (!_secondHandAdditionalPerformed) return;
            if (!_secondHandObj.activeInHierarchy) return;
            secondHand.AdditionalActionCanceled(gameObject,_secondHandObj);
            _mainHandAdditionalPerformed = false;
        }
    }
}