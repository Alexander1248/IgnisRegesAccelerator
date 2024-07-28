using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace Player
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private Inventory[] inventories;

        public void Initialize(Dictionary<Vector2Int, Item>[] items)
        {
            for (var i = 0; i < items.Length; i++)
                foreach (var (pos, item) in items[i])
                    inventories[i].AddItem(pos.x, pos.y, item);
            
        }

        public int Count => inventories.Length;

        public IEnumerable<KeyValuePair<Vector2Int, Item>> this[int index] => inventories[index].Enumerator;
        
        public bool UseItem(int id, int x, int y, GameObject player)
        {
            return inventories[id].UseItem(x, y, player);
        }
        public void DrawItem(int id, int x, int y, RectTransform transform)
        {
            inventories[id].DrawItem(x, y, transform);
        }
        
        public bool AddItem(int id, int x, int y, Item item)
        {
            return inventories[id].AddItem(x, y, item);
        }
        public bool CanPlace(int id, int x, int y, Item item)
        {
            return inventories[id].CanPlace(x, y, item);
        }
        public bool AddItem(int id, Item item)
        {
            return inventories[id].AddItem(item);
        }
        public bool AddItem(Item item)
        {
            return inventories.Any(inventory => inventory.AddItem(item));
        }
        
        public Item RemoveItem(int id, int x, int y)
        {
            return inventories[id].RemoveItem(x, y);
        }
        public Item GetItem(int id, int x, int y)
        {
            return inventories[id].GetItem(x, y);
        }
        public Vector2Int? GetItemCenter(int id, int x, int y)
        {
            return inventories[id].GetItemCenter(x, y);
        }
        
        public IEnumerable<Vector2Int> Find(int id, Item item)
        {
            return inventories[id].Find(item);
        }
        public IEnumerable<Vector2Int> Find(Item item)
        {
            var points = new List<Vector2Int>();
            foreach (var inventory in inventories)
                points.AddRange(inventory.Find(item));
            return points;
        }
        
        public IEnumerable<Vector2Int> Find(int id, Predicate<Item> condition)
        {
            return inventories[id].Find(condition);
        }
        public IEnumerable<(int, Vector2Int)> Find(Predicate<Item> condition)
        {
            var points = new List<(int, Vector2Int)>();
            for (var i = 0; i < inventories.Length; i++)
            {
                var inventory = inventories[i];
                points.AddRange(inventory.Find(condition).Select(v => (i, v)));
            }

            return points;
        }
    }
}