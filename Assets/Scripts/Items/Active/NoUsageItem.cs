using UnityEngine;

namespace Items.Active
{
    
    [CreateAssetMenu(menuName = "Items/No Usage")]
    public class NoUsageItem : Item
    {
        public override bool Use(Inventory inventory, int x, int y, GameObject player)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NoUsageItem)obj);
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }
    }
}