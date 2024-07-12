using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

namespace Items
{
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private Vector2Int[] shape;
        [SerializeField] private LocalizedString itemName;
        [SerializeField] private LocalizedString itemDescription;

        public LocalizedString Name => itemName;
        public LocalizedString Description => itemDescription;
        
        public abstract bool Use(Inventory inventory, int x, int y, GameObject player);
        
        public bool CanBePlaced(Inventory inventory, int x, int y)
        {
            return !shape.Any(points => inventory.CellNotEmpty(points.x + x, points.y + y));
        }
        public void Place(Inventory inventory, int x, int y, int id)
        {
            foreach (var points in shape)
                inventory.SetCell(points.x + x, points.y + y, id);
        }
        public void Remove(Inventory inventory, int x, int y, int id)
        {

            foreach (var anchor in shape)
            {
                if (shape.Any(points => 
                        inventory.GetCell(points.x + anchor.x + x, points.y + anchor.y + y) != id)) continue;
                foreach (var points in shape)
                    inventory.SetCell(points.x + anchor.x + x, points.y + anchor.y + y, 0);
                break;
            }
        }
    }
}