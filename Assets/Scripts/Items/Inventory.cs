using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        private int[,] cells;
        private Dictionary<int, Item> _items = new();

        private void Awake()
        {
            cells = new int[width, height];
        }

        public bool AddItem(int x, int y, Item item)
        {
            if (!item.CanBePlaced(this, x, y)) return false;
            int id;
            do
            { 
                id = Random.Range(1, int.MaxValue);
            } while (_items.ContainsKey(id));
            _items.Add(id, item);
            item.Place(this, x, y, id);
            return true;
        }
        public bool AddItem(Item item)
        {
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    if (AddItem(x, y, item))
                        return true;
            return false;
        }

        public void RemoveItem(int x, int y)
        {
            var id = cells[x, y];
            if (!_items.Remove(id, out var item)) return;
            item.Remove(this, x, y, id);
        }

        public Item GetItem(int x, int y)
        {
            return _items.GetValueOrDefault(cells[x, y]);
        }

        public bool UseItem(int x, int y, GameObject player)
        {
            return _items.TryGetValue(cells[x, y], out var item) 
                   && item.Use(this, x, y, player);
        }
        
        public bool CellNotEmpty(int x, int y)
        {
            return _items.ContainsKey(cells[x, y]);
        }
        public int GetCell(int x, int y)
        {
            return cells[x, y];
        }
        public void SetCell(int x, int y, int id)
        {
            cells[x, y] = id;
        }
    }
}