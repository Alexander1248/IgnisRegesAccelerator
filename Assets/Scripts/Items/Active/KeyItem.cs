using UnityEngine;
using UnityEngine.Serialization;

namespace Items.Active
{
    
    [CreateAssetMenu(menuName = "Items/Key")]
    public class KeyItem : Item
    {
        [FormerlySerializedAs("key")] [SerializeField] private string code;
        public string Code => code;
        public override bool Use(Inventory inventory, int x, int y, GameObject player)
        {
            return false;
        }
    }
}