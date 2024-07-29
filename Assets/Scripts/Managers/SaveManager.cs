using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    var item = items.First(item => item.ID == locations.id);
                    item.secured = locations.secured;
                    item.lockedInInventory = locations.lockedInInventory;
                    return item;
                })).ToArray());
            
            foreach (var quest in state.quests)
            {
                var q = quests.First(item => item.ID == quest.id);
                questManager.Add(q);
                if (quest.completed) 
                    questManager.Complete(q);
            }
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
                        key.x, key.y, value.ID, value.secured, value.lockedInInventory
                    ));
            }
            

            _state = new GameState
            {
                supportedVersions = supportedVersions.Append(Application.version).ToArray(),
                inventories = inventories,
                quests = questManager.Quests.Select(tuple => new QuestData(tuple.Item1, tuple.Item2)).ToList(),
                hp = health.HP,
                stamina = stamina.Value
            };
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
            public List<QuestData> quests;
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
            public ItemData(int x, int y, string id, bool secured, bool lockedInInventory)
            {
                this.x = x;
                this.y = y;
                this.id = id;
                this.secured = secured;
                this.lockedInInventory = lockedInInventory;
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