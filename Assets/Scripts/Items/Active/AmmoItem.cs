using System;
using UnityEngine;

namespace Items.Active
{
    
    [CreateAssetMenu(menuName = "Items/Ammo")]
    public class AmmoItem : Item
    {
        [SerializeField] private string type;
        public string Type => type;
        public override bool Use(Inventory inventory, int x, int y, GameObject player, AudioSource audioSource)
        {
            return false;
        }

        private bool Equals(AmmoItem other)
        {
            return base.Equals(other) && type == other.type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AmmoItem)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), type);
        }
        // TODO: Частичное использование патронов и отображение в инвентаре
    }
}