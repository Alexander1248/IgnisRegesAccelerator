using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private Inventory[] inventories;

        public int Count => inventories.Length;
        
        public bool UseItem(int id, int x, int y, GameObject player)
        {
            return inventories[id].UseItem(x, y, player);
        }
        
        public bool AddItem(int id, int x, int y, Item item)
        {
            return inventories[id].AddItem(x, y, item);
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
    }
}