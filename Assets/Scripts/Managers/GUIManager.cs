using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Items.Active;
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
        [SerializeField] private InventoryUpdater updater;
        [SerializeField] private RectTransform leftHand;
        [SerializeField] private RectTransform rightHand;

        private Item bufferedItem;
        private RectTransform bufferedObject;
        private int prevInv, prevIx, prevIy, prevLocation;
        private int inv, ix, iy, location;
        

        private void Start()
        {
            playerController.Control.Interaction.UseHeal.performed += _ =>
            {
                try {
                    var (id, pos) = inventory.Find(item => item is HealItem).First();
                    if (inventory.UseItem(id, pos.x, pos.y, playerController.gameObject))
                    {
                        Destroy(inventory.RemoveItem(id, pos.x, pos.y));
                        Destroy(gridContents[id].Find($"Inv_Item_{id}_{pos.x}_{pos.y}").gameObject);
                    }
                    else
                    {
                        inventory.DrawItem(id, pos.x, pos.y, gridContents[id]
                            .Find($"Inv_Item_{id}_{pos.x}_{pos.y}").GetComponent<RectTransform>());
                    }
                } catch(Exception) {} 
            };
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

            playerController.Control.Interaction.UseItem.performed += _ =>
            {
                if (!inventoryGUI.activeInHierarchy) return;
                if (location != 0) return;
                Debug.Log($"[Inventory]: Use {inv} {ix} {iy}");
                if (inventory.UseItem(inv, ix, iy, playerController.gameObject))
                {
                    Destroy(inventory.RemoveItem(inv, ix, iy));
                    Destroy(gridContents[inv].Find($"Inv_Item_{inv}_{ix}_{iy}").gameObject);
                }
                else
                {
                    var center = inventory.GetItemCenter(inv, ix, iy);
                    if (center == null) return;
                    inventory.DrawItem(inv, ix, iy, gridContents[inv]
                        .Find($"Inv_Item_{inv}_{center.Value.x}_{center.Value.y}").GetComponent<RectTransform>());
                }

            };
            playerController.Control.Interaction.MoveItem.performed += _ =>
            {
                if (!inventoryGUI.activeInHierarchy) return;
                prevLocation = location;
                Debug.Log($"[Inventory]: Select {location} {inv} {ix} {iy}");
                switch (location)
                {
                    case 0:
                    {
                        prevInv = inv;
                        prevIx = ix;
                        prevIy = iy;
                        var center = inventory.GetItemCenter(inv, ix, iy);
                        bufferedItem = inventory.RemoveItem(inv, ix, iy);
                        if (bufferedItem == null) break;

                        bufferedObject = center.HasValue
                            ? gridContents[inv]
                                .Find($"Inv_Item_{inv}_{center.Value.x}_{center.Value.y}").GetComponent<RectTransform>()
                            : null;
                        break;
                    }
                    case 1:
                        bufferedItem = handController.ClearSecondHand();
                        if (bufferedItem == null) break;
                        bufferedObject = leftHand.Find("Left_Hand_Item").GetComponent<RectTransform>();
                        break;
                    case 2:
                        bufferedItem = handController.ClearMainHand();
                        if (bufferedItem == null) break;
                        bufferedObject = rightHand.Find("Right_Hand_Item").GetComponent<RectTransform>();
                        break;
                    default:
                        bufferedItem = null;
                        break;
                }
            };
            playerController.Control.Interaction.MoveItem.canceled += _ =>
            {
                if (!inventoryGUI.activeInHierarchy) return;
                if (bufferedItem == null) return;
                Debug.Log($"[Inventory]: Place {location} {inv} {ix} {iy}");
                switch (location)
                {
                    case 0 when inventory.AddItem(inv, ix, iy, bufferedItem):
                        bufferedObject.name = $"Inv_Item_{inv}_{ix}_{iy}";
                        bufferedObject.SetParent(gridContents[inv]);
                        bufferedObject.anchorMax = bufferedObject.anchorMin = new Vector2(0f, 0f);
                        bufferedObject.anchoredPosition = new Vector2(ix, iy) * cell.rect.size
                                                          + bufferedObject.rect.size / 2;
                        break;
                    case 0:
                        ReturnBack();
                        break;
                    case 1 when bufferedItem is Items.Weapon weapon && handController.SetSecondHand(weapon):
                    {
                        bufferedObject.name = "Left_Hand_Item";
                        bufferedObject.SetParent(leftHand);
                        bufferedObject.anchorMax = bufferedObject.anchorMin = new Vector2(0.5f, 0.5f);
                        bufferedObject.anchoredPosition = Vector2.zero;
                        break;
                    }
                    case 1:
                        ReturnBack();
                        break;
                    case 2 when bufferedItem is Items.Weapon weapon && handController.SetMainHand(weapon):
                    {
                        bufferedObject.name = "Right_Hand_Item";
                        bufferedObject.SetParent(rightHand);
                        bufferedObject.anchorMax = bufferedObject.anchorMin = new Vector2(0.5f, 0.5f);
                        bufferedObject.anchoredPosition = Vector2.zero;
                        break;
                    }
                    case 2:
                        ReturnBack();
                        break;
                    default:
                        if (bufferedItem.secured) ReturnBack();
                        else
                        {
                            Destroy(bufferedItem);
                            Destroy(bufferedObject.gameObject);
                        }
                        break;
                }
                bufferedItem = null;
                bufferedObject = null;
            };

            updater.manager = inventory;
            updater.gridContents = gridContents;
            updater.cell = cell;
        }

        private void ReturnBack()
        {
            if (!inventoryGUI.activeInHierarchy) return;
            Debug.Log($"[Inventory]: Return {prevLocation} {prevInv} {prevIx} {prevIy}");
            switch (prevLocation)
            {
                case 0:
                    inventory.AddItem(prevInv, prevIx, prevIy, bufferedItem);
                    bufferedObject.name = $"Inv_Item_{prevInv}_{prevIx}_{prevIy}";
                    bufferedObject.SetParent(gridContents[prevInv]);
                    bufferedObject.anchorMax = bufferedObject.anchorMin = new Vector2(0f, 0f);
                    bufferedObject.anchoredPosition = new Vector2(prevIx, prevIy) * cell.rect.size
                                                      + bufferedObject.rect.size / 2;
                    break;
                case 1:
                {
                    handController.SetSecondHand(bufferedItem as Items.Weapon);
                        
                    bufferedObject.name = "Left_Hand_Item";
                    bufferedObject.SetParent(leftHand);
                    bufferedObject.anchorMax = bufferedObject.anchorMin = new Vector2(0.5f, 0.5f);
                    bufferedObject.anchoredPosition = Vector2.zero;
                    break;
                }
                case 2:
                {
                    handController.SetMainHand(bufferedItem as Items.Weapon);
                    
                    bufferedObject.name = "Right_Hand_Item";
                    bufferedObject.SetParent(rightHand);
                    bufferedObject.anchorMax = bufferedObject.anchorMin = new Vector2(0.5f, 0.5f);
                    bufferedObject.anchoredPosition = Vector2.zero;
                    break;
                }
            }
        }

        public void FixedUpdate()
        {
            if (!inventoryGUI.activeInHierarchy) return;
            location = -1;
            var position = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(leftHand, position))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(leftHand,
                    position, Camera.current, out var localPoint);
                var len = new Vector2(localPoint.x / leftHand.rect.x, localPoint.y / leftHand.rect.y).magnitude;
                if (len <= 1) location = 1;
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(rightHand, position))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rightHand,
                    position, Camera.current, out var localPoint);
                var len = new Vector2(localPoint.x / rightHand.rect.x, localPoint.y / rightHand.rect.y).magnitude;
                if (len <= 1) location = 2;
            }
            else
            {
                for (var i = 0; i < gridContents.Length; i++)
                {
                    var rect = gridContents[i];
                    if (!RectTransformUtility.RectangleContainsScreenPoint(rect, position)) continue;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
                        position, Camera.current, out var localPoint);
                    var point = (localPoint + rect.rect.size * 0.5f) / cell.sizeDelta;
                    location = 0;
                    inv = i;
                    ix = (int)point.x;
                    iy = (int)point.y;
                    break;
                }
            }

            if (bufferedItem == null) return;
            Debug.Log($"[Inventory]: Move {location} {inv} {ix} {iy}");
            switch (location)
            {
                case 0 when inventory.CanPlace(inv, ix, iy, bufferedItem):
                    bufferedObject.name = $"Inv_Item_{inv}_{ix}_{iy}";
                    bufferedObject.SetParent(gridContents[inv]);
                    bufferedObject.anchorMax = bufferedObject.anchorMin = new Vector2(0f, 0f);
                    bufferedObject.anchoredPosition = new Vector2(ix, iy) * cell.rect.size
                                                      + bufferedObject.rect.size / 2;
                    break;
                case 1 when bufferedItem is Items.Weapon:
                {
                    bufferedObject.name = "Left_Hand_Item";
                    bufferedObject.SetParent(leftHand);
                    bufferedObject.anchorMax = bufferedObject.anchorMin = new Vector2(0.5f, 0.5f);
                    bufferedObject.anchoredPosition = Vector2.zero;
                    break;
                }
                case 2when bufferedItem is Items.Weapon:
                {
                    bufferedObject.name = "Right_Hand_Item";
                    bufferedObject.SetParent(rightHand);
                    bufferedObject.anchorMax = bufferedObject.anchorMin = new Vector2(0.5f, 0.5f);
                    bufferedObject.anchoredPosition = Vector2.zero;
                    break;
                }
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