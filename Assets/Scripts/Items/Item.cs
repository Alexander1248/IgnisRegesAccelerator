using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace Items
{
    
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private Vector2Int[] shape;
        [SerializeField] private GameObject uiPrefab;
        [SerializeField] private LocalizedString itemName;
        [SerializeField] private LocalizedString itemDescription;
        public bool secured;
        [FormerlySerializedAs("lockInInventory")] public bool lockedInInventory = false;

        public string ID => id;
        public GameObject UIPrefab => uiPrefab;
        public LocalizedString Name => itemName;
        public LocalizedString Description => itemDescription;


        public abstract bool Use(Inventory inventory, int x, int y, GameObject player, AudioSource audioSource);

        public virtual void Draw(RectTransform rect) { }

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