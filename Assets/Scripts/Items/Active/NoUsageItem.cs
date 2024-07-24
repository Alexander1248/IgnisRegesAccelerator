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
    }
}