using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Items;
using Player;
using Quests;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private static GameState _state;
        [SerializeField] private string path;
        [SerializeField] private string[] supportedVersions;
        [SerializeField] private List<Item> items;
        [SerializeField] private List<Quest> quests;
        [FormerlySerializedAs("manager")] [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private QuestManager questManager;
        [SerializeField] private Health health;
        [SerializeField] private Stamina stamina;

        [SerializeField] private bool saveToFile;
        
        private void Start()
        {
            GameState state;
            if (_state != null) state = _state;
            else
            {
                if (!File.Exists(path)) return;
                state = JsonUtility.FromJson<GameState>(File.ReadAllText(path));
                if (state.supportedVersions.All(version => Application.version != version)) return;
            }
            inventoryManager.Initialize(state.inventories.Select(inventory => inventory.items.ToDictionary(
                locations => new Vector2Int(locations.x, locations.y),
                locations =>
                {
                    var item = Instantiate(items.First(item => item.ID == locations.id));
                    item.secured = locations.secured;
                    item.lockedInInventory = locations.lockedInInventory;
                    item.LoadState(Convert.FromBase64String(locations.data));
                    return item;
                })).ToArray());
            
            var handController = inventoryManager.GetHandController();
            if (state.mainHand != null)
            {
                var item = Instantiate(items.First(item => item.ID == state.mainHand.id));
                item.secured = state.mainHand.secured;
                item.lockedInInventory = state.mainHand.lockedInInventory;
                item.LoadState(Convert.FromBase64String(state.mainHand.data));
                handController.SetMainHand(item as Items.Weapon);
            }
            if (state.secondHand != null)
            {
                var item = Instantiate(items.First(item => item.ID == state.secondHand.id));
                item.secured = state.secondHand.secured;
                item.lockedInInventory = state.secondHand.lockedInInventory;
                item.LoadState(Convert.FromBase64String(state.secondHand.data));
                handController.SetMainHand(item as Items.Weapon);
            }

            foreach (var quest in state.quests)
            {
                var q = quests.First(item => item.ID == quest.id);
                questManager.Add(q);
                if (quest.completed) 
                    questManager.Complete(q);
            }
            questManager.Select(state.selectedQuest);
            
            health.Initialize(state.hp);
            stamina.Initialize(state.stamina);
        }

        public void Save()
        {
            var inventories = new Inventory[inventoryManager.Count];
            for (var i = 0; i < inventories.Length; i++)
            {
                inventories[i] = new Inventory
                {
                    items = new List<ItemData>()
                };
                foreach (var (key, value) in inventoryManager[i])
                    inventories[i].items.Add(new ItemData(
                        key.x, key.y, value.ID, value.secured, 
                        value.lockedInInventory, Convert.ToBase64String(value.SaveState())
                    ));
            }

            var handController = inventoryManager.GetHandController();
            var mainHand = handController.GetMainHand();
            var secondHand = handController.GetSecondHand();
            _state = new GameState
            {
                supportedVersions = supportedVersions.Append(Application.version).ToArray(),
                inventories = inventories,
                quests = questManager.Quests.Select(tuple => new QuestData(tuple.Item1, tuple.Item2)).ToList(),
                hp = health.HP,
                stamina = stamina.Value,
                selectedQuest = questManager.Find(questManager.SelectedQuest)
            };

            if (mainHand)
                _state.mainHand = new ItemData(0, 0, mainHand.ID, mainHand.secured,
                    mainHand.lockedInInventory, Convert.ToBase64String(mainHand.SaveState()));
            else _state.mainHand = null;
            if (secondHand)
                _state.secondHand = new ItemData(0, 0, secondHand.ID, secondHand.secured, 
                    secondHand.lockedInInventory, Convert.ToBase64String(secondHand.SaveState()));
            else _state.secondHand = null;
            
            if (!saveToFile) return;
            File.WriteAllText(path, JsonUtility.ToJson(_state));
        }

        public void Clear()
        {
            _state = null;
            File.Delete(path);
        }
        
        
        [Serializable]
        private class GameState
        {
            public string[] supportedVersions;
            public Inventory[] inventories;
            public ItemData mainHand;
            public ItemData secondHand;
            public List<QuestData> quests;
            public int selectedQuest;
            public float hp;
            public float stamina;
        }
        
        [Serializable]
        private class Inventory
        {
            public List<ItemData> items;
        }
        [Serializable]
        private class ItemData
        {
            public int x;
            public int y;
            public string id;
            public bool secured;
            public bool lockedInInventory;
            public string data;
            public ItemData(int x, int y, string id, bool secured, bool lockedInInventory, string data)
            {
                this.x = x;
                this.y = y;
                this.id = id;
                this.secured = secured;
                this.lockedInInventory = lockedInInventory;
                this.data = data;
            }

            public ItemData()
            {
            }
        }
        [Serializable]
        private class QuestData
        {
            public string id;
            public bool completed;

            public QuestData(string id, bool completed)
            {
                this.id = id;
                this.completed = completed;
            }
            public QuestData() { }
        }
    }
}