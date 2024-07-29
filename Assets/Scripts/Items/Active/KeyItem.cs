using System;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.Active
{
    
    [CreateAssetMenu(menuName = "Items/Key")]
    public class KeyItem : Item
    {
        [FormerlySerializedAs("key")] [SerializeField] private string code;
        public string Code => code;
        public override bool Use(Inventory inventory, int x, int y, GameObject player, AudioSource audioSource)
        {
            return false;
        }

        protected bool Equals(KeyItem other)
        {
            return base.Equals(other) && code == other.code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeyItem)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), code);
        }
        public override byte[] SaveState()
        {
            return Encoding.UTF8.GetBytes(code);
        }

        public override void LoadState(byte[] data)
        {
            code = Encoding.UTF8.GetString(data);
        }
    }
}