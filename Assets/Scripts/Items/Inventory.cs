using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class Inventory
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        private readonly Dictionary<Vector2Int, Item> _items = new();

        public IEnumerable<KeyValuePair<Vector2Int, Item>> Enumerator => _items;

        public bool AddItem(int x, int y, Item item)
        {
            if (!item.CanBePlaced(this, x, y)) return false;
            _items[new Vector2Int(x, y)] = item;
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

        public IEnumerable<Vector2Int> Find(Item item)
        {
            return _items.Where(pair => pair.Value == item).Select(pair => pair.Key);
        }

        public Item RemoveItem(int x, int y)
        {
            foreach (var (key, value) in _items)
                if (value.Intersects(key.x, key.y, x, y))
                {
                    _items.Remove(key);
                    return value;
                }

            return null;
        }

        public Item GetItem(int x, int y)
        {
            foreach (var (key, value) in _items)
                if (value.Intersects(key.x, key.y, x, y)) 
                    return value;
            return null;
        }
        public Vector2Int? GetItemCenter(int x, int y)
        {
            foreach (var (key, value) in _items)
                if (value.Intersects(key.x, key.y, x, y)) 
                    return key;
            return null;
        }

        public bool UseItem(int x, int y, GameObject player)
        {
            var item = GetItem(x, y);
            return item != null && item.Use(this, x, y, player);
        }
        
        public bool CellNotEmpty(int x, int y)
        {
            if (x < 0 || x >= width) return true;
            if (y < 0 || y >= height) return true;
            foreach (var (key, value) in _items)
                if (value.Intersects(key.x, key.y, x, y)) 
                    return true;
            return false;
        }
        public void ClearCell(int x, int y)
        {
            _items.Remove(new Vector2Int(x, y));
        }
    }
}