using System;
using System.Text;
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

        public override byte[] SaveState()
        {
            return Encoding.UTF8.GetBytes(type);
        }

        public override void LoadState(byte[] data)
        {
            type = Encoding.UTF8.GetString(data);
        }
        // TODO: Частичное использование патронов и отображение в инвентаре
    }
}