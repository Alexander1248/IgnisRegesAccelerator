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
        [SerializeField] private HandController handController;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private QuestManager questManager;
        [SerializeField] private InventoryManager inventory;
        
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
        [SerializeField] private TMP_Text questTask;
        [SerializeField] private TMP_Text questText;
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private GameObject divider;
        [SerializeField] private Button select;

        private int selectedQuestIndex;
        private QuestItem selectedQuestItem;
        private readonly Dictionary<Quest, GameObject> quests = new();
        private readonly Dictionary<Quest, GameObject> completed = new();
        
        // Inventory
        [Space]
        [SerializeField] private GameObject inventoryGUI;
        [SerializeField] private RectTransform[] gridContents;
        [SerializeField] private RectTransform cell;

        private Item bufferedItem;
        private GameObject bufferedObject;
        private int prevInv, prevIx, prevIy;
        private int inv, ix, iy;
        private bool inInv;
        

        private void Start()
        {
            playerController.Control.Interaction.Journal.performed += _ =>
            {
                if (journal.activeSelf)
                {
                    handController.MainHandActive(true);
                    handController.SecondHandActive(true);
                    playerController.UnlockPlayer();
                    playerController.LockCursor();
                    
                    hud.SetActive(true);
                    journal.SetActive(false);
                    inventoryGUI.SetActive(false);
                }
                else
                {
                    handController.MainHandActive(false);
                    handController.SecondHandActive(false);
                    playerController.LockPlayer();
                    playerController.UnlockCursor();
                    
                    journal.SetActive(true);
                    hud.SetActive(false);
                    inventoryGUI.SetActive(false);
                    if (selectedQuestItem) Select(selectedQuestItem.quest);
                    else
                    {
                        select.gameObject.SetActive(false);
                        questName.text = "";
                        questTask.text = "";
                        questText.text = "";
                    }
                }
            };
            playerController.Control.Interaction.Inventory.performed += _ =>
            {
                if (inventoryGUI.activeSelf)
                {
                    handController.MainHandActive(true);
                    handController.SecondHandActive(true);
                    playerController.UnlockPlayer();
                    playerController.LockCursor();
                    
                    hud.SetActive(true);
                    journal.SetActive(false);
                    inventoryGUI.SetActive(false);
                }
                else
                {
                    handController.MainHandActive(false);
                    handController.SecondHandActive(false);
                    playerController.LockPlayer();
                    playerController.UnlockCursor();
                    
                    inventoryGUI.SetActive(true);
                    journal.SetActive(false);
                    hud.SetActive(false);
                }
            };

            playerController.Control.Interaction.MoveItem.performed += _ =>
            {
                if (!inInv)
                {
                    bufferedItem = null;
                    return;
                }
                bufferedItem = inventory.RemoveItem(inv, ix, iy);
                prevInv = inv;
                prevIx = ix;
                prevIy = iy;
                
                // TODO: Get Buffered Object
                bufferedObject = null;
            };
            playerController.Control.Interaction.MoveItem.canceled += _ =>
            {
                if (bufferedItem == null) return;
                if (inInv)
                {
                    if (inventory.AddItem(inv, ix, iy, bufferedItem))
                    {
                        // TODO: Place Buffered Object
                    }
                    else
                    {
                        inventory.AddItem(prevInv, prevIx, prevIy, bufferedItem);
                        // TODO: Place Buffered Object Back
                    }
                }
                else
                {
                    bufferedItem = null;
                    Destroy(bufferedObject);
                }
            };
        }
        
        public void FixedUpdate()
        {
            if (!inventoryGUI.activeInHierarchy) return;
            inInv = false;
            var position = Input.mousePosition;
            for (var i = 0; i < gridContents.Length; i++)
            {
                var rect = gridContents[i];
                if (!RectTransformUtility.RectangleContainsScreenPoint(rect, position)) continue;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
                    position, Camera.current, out var localPoint);
                var point = (localPoint + rect.rect.size * 0.5f) / cell.sizeDelta;
                inv = i;
                ix = (int) point.x;
                iy = (int) point.y;
                inInv = true;
                Debug.Log($"[Inventory]: {inv} {ix} {iy}");
            }
        }

        public void AddQuest(Quest quest)
        {
            var obj = Instantiate(questPrefab, journalContent.transform);
            var item = obj.GetComponent<QuestItem>();
            item.quest = quest;
            item.selected.AddListener(Select);
            quests.Add(quest, obj);
            UpdateLayout();
        }

        private void Select(Quest quest)
        {
            if (selectedQuestItem) selectedQuestItem.Deselect();
            if (questManager.IsCompleted(quest))
            {
                selectedQuestIndex = -1;
                select.gameObject.SetActive(false);
                selectedQuestItem = completed[quest].GetComponent<QuestItem>();
            }
            else
            {
                selectedQuestIndex = questManager.Find(quest);
                select.gameObject.SetActive(true);
                selectedQuestItem = quests[quest].GetComponent<QuestItem>();
            }
            questName.text = quest.Name.GetLocalizedString();
            questTask.text = quest.Task.GetLocalizedString();
            questText.text = quest.Description.GetLocalizedString();
        }
        
        public void AbortQuest(Quest quest)
        {
            if (!quests.Remove(quest, out var value)) return;
            Destroy(value);
            UpdateLayout();
        }
        public void CompleteQuest(Quest quest)
        {
            if (!quests.Remove(quest, out var value)) return;
            value.GetComponent<QuestItem>().Disable();
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
                y -= t.sizeDelta.y;
            }

            if (completed.Count == 0)
            {
                divider.SetActive(false);
                return;
            }
            divider.SetActive(true);
            
            t = divider.GetComponent<RectTransform>();
            t.anchoredPosition = new Vector2(0, y);
            y -= t.sizeDelta.y;
            
            foreach (var (_, value) in completed)
            {
                t = value.GetComponent<RectTransform>();
                t.anchoredPosition = new Vector2(0, y);
                y -= t.sizeDelta.y;
            }

            t = journalContent.GetComponent<RectTransform>();
            t.sizeDelta = new Vector2(t.sizeDelta.x, -y);
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