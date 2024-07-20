using System;
using System.Collections.Generic;
using Items;
using Player;
using Quests;
using TMPro;
using UnityEngine;
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
        [SerializeField] private GameObject hud;
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
        [SerializeField] private GameObject journal;
        [SerializeField] private GameObject journalContent;
        [SerializeField] private TMP_Text questName;
        [SerializeField] private TMP_Text questText;
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private GameObject divider;

        private int selectedQuestIndex;
        private Dictionary<Quest, GameObject> quests = new();
        private Dictionary<Quest, GameObject> completed = new();
        
        // Inventory
        [Space]
        [SerializeField] private GameObject inventoryGUI;


        private void Start()
        {
            playerController.Control.Interaction.Journal.performed += _ =>
            {
                if (journal.activeSelf)
                {
                    playerController.LockCursor();
                    hud.SetActive(true);
                    
                    journal.SetActive(false);
                    inventoryGUI.SetActive(false);
                }
                else
                {
                    playerController.UnlockCursor();
                    journal.SetActive(true);
                    
                    hud.SetActive(false);
                    inventoryGUI.SetActive(false);
                }
            };
            playerController.Control.Interaction.Inventory.performed += _ =>
            {
                if (inventoryGUI.activeSelf)
                {
                    playerController.LockCursor();
                    hud.SetActive(true);
                    
                    journal.SetActive(false);
                    inventoryGUI.SetActive(false);
                }
                else
                {
                    playerController.UnlockCursor();
                    inventoryGUI.SetActive(true);
                    
                    journal.SetActive(false);
                    hud.SetActive(false);
                }
            };
        }

        public void AddQuest(Quest quest)
        {
            var obj = Instantiate(questPrefab, journalContent.transform);
            var item = obj.GetComponent<QuestItem>();
            item.index = quests.Count;
            item.quest = quest;
            item.selected.AddListener(Select);
            quests.Add(quest, obj);
            UpdateLayout();
        }

        private void Select(int index)
        {
            selectedQuestIndex = index;
        }
        
        public void AbortQuest(Quest quest)
        {
            quests.Remove(quest);
            UpdateLayout();
        }
        public void CompleteQuest(Quest quest)
        {
            if (!quests.Remove(quest, out var value)) return;
            completed.Add(quest, value);
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            RectTransform t;
            float y = 0;
            foreach (var (_, value) in quests)
            {
                t = value.GetComponent<RectTransform>();
                t.anchoredPosition = new Vector2(0, y);
                y += t.sizeDelta.y;
            }

            if (completed.Count == 0)
            {
                divider.SetActive(false);
                return;
            }
            divider.SetActive(true);
            
            t = divider.GetComponent<RectTransform>();
            t.anchoredPosition = new Vector2(0, y);
            y += t.sizeDelta.y;
            
            foreach (var (_, value) in completed)
            {
                t = value.GetComponent<RectTransform>();
                t.anchoredPosition = new Vector2(0, y);
                y += t.sizeDelta.y;
            }

            t = journalContent.GetComponent<RectTransform>();
            t.sizeDelta = new Vector2(t.sizeDelta.x, y);
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