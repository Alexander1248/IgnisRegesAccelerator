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
        
        // TODO: Частичное использование патронов и отображение в инвентаре
    }
}