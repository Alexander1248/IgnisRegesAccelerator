using Player;
using UnityEngine;

namespace Items.Active
{
    
    [CreateAssetMenu(menuName = "Items/Heal")]
    public class HealItem : Item
    {
        [SerializeField] private int amountPerRound;
        [SerializeField] private int rounds;
        private int used;

        public HealItem()
        {
            used = rounds;
        }

        public override bool Use(Inventory inventory, int x, int y, GameObject player)
        {
            var health = player.GetComponent<Health>();
            health.ChangeHealth(amountPerRound);
            used--;
            return used <= 0;
        }

        public override int Charges() => used;
    }
}