using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

namespace Items
{
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private Vector2Int[] shape;
        [SerializeField] private GameObject uiPrefab;
        [SerializeField] private LocalizedString itemName;
        [SerializeField] private LocalizedString itemDescription;

        public GameObject UIPrefab => uiPrefab;
        public LocalizedString Name => itemName;
        public LocalizedString Description => itemDescription;
        
        public abstract bool Use(Inventory inventory, int x, int y, GameObject player);

        public virtual float Durability() => 1;
        public virtual int Charges() => 0;
        
        public bool CanBePlaced(Inventory inventory, int x, int y)
        {
            return !shape.Any(points => inventory.CellNotEmpty(points.x + x, points.y + y));
        }
        public bool Intersects(int cx, int cy, int x, int y)
        {
            return shape.Any(points => points.x + cx == x && points.y + cy == y);
        }
    }
}