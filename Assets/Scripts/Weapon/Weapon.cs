using UnityEngine;

namespace Items
{
    public abstract class Weapon : Item
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool isTwoHanded;
        public GameObject Prefab => prefab;
        public bool IsTwoHanded => isTwoHanded;

        public abstract void OnEquip(GameObject user, GameObject weapon);
        public abstract void OnRelease(GameObject user, GameObject weapon);
        public abstract void Action(GameObject user, GameObject weapon);
        public abstract void AdditionalActionPerformed(GameObject user, GameObject weapon);
        public abstract void AdditionalActionCanceled(GameObject user, GameObject weapon);
        public abstract void AnimatorState(bool enadled);
        
        public override bool Use(Inventory inventory, int x, int y, GameObject player, AudioSource audioSource)
        {
            return false;
        }
    }
}