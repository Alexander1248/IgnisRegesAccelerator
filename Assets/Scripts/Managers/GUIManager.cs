using System.Collections.Generic;
using Items;
using Player;
using Quests;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Managers
{
    public class GUIManager : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private QuestManager questManager;
        [SerializeField] private Inventory inventory;
        
        // HUD
        [Space]
        [SerializeField] private GameObject selectedQuest;
        [SerializeField] private TMP_Text selectedQuestText;
        
        [SerializeField] private GameObject hpBarObject;
        [SerializeField] private Image hpBar;
        [SerializeField] private GameObject staminaBarObject;
        [SerializeField] private Image staminaBar;
        [SerializeField] private GameObject ammoTextObject;
        [SerializeField] private TMP_Text ammoText;
        
        // Journal
        [Space]
        [SerializeField] private GameObject journalContent;
        [SerializeField] private TMP_Text questName;
        [SerializeField] private TMP_Text questText;
        [SerializeField] private GameObject questPrefab;

        private int selectedQuestIndex = -1;
        private List<GameObject> quests = new();
        private List<GameObject> completed = new();
        
        // Inventory


        public void AddQuestToUI(Quest quest)
        {
            var obj = Instantiate(questPrefab);
            var item = obj.GetComponent<QuestItem>();
            item.index = quests.Count;
            item.quest = quest;
            item.selected.AddListener(Select);
            quests.Add(obj);
        }

        private void Select(int index)
        {
            selectedQuestIndex = index;
        }
        
        public void AbortQuest(Quest quest)
        {
            
        }
        public void CompleteQuest(Quest quest)
        {
            
        }
        
        public void SetCurrentQuest()
        {
            questManager.Select(selectedQuestIndex);
        }
        public void UpdateCurrentQuest(Quest quest)
        {
            if (quest == null)
            {
                selectedQuest.SetActive(false);
                return;
            }
            selectedQuest.SetActive(true);
            selectedQuestText.text = quest.Task.GetLocalizedString();
        }

        public void UpdateHP(float hp, float max)
        {
            var amount = hp / max;
            if (amount >= 1)
            {
                hpBarObject.SetActive(false);
                return;
            }
            hpBarObject.SetActive(true);
            hpBar.fillAmount = amount;
        }

        public void UpdateStamina(float stamina, float max)
        {
            var amount = stamina / max;
            if (amount >= 1)
            {
                staminaBarObject.SetActive(false);
                return;
            }
            staminaBarObject.SetActive(true);
            staminaBar.fillAmount = amount;
        }

        public void SetAmmoActive(bool active)
        {
            staminaBarObject.SetActive(active);
        }
        public void UpdateAmmo(int charged, int other)
        {
            ammoText.text = charged + "/" + other;
        }
    }
}