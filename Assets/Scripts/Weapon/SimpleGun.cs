using Controllers;
using TestUtils;
using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "Weapons/Simple Gun")]
    public class SimpleGun : Items.Weapon
    {
        private RaycastHit hit;

        public override void OnEquip(GameObject user, GameObject weapon)
        {
            
        }

        public override void OnRelease(GameObject user, GameObject weapon)
        {
            
        }

        public override void Action(GameObject user, GameObject weapon)
        {
            Debug.DrawRay(user.transform.position, user.transform.forward * 10, Color.red);
            if (!Physics.Raycast(user.transform.position, user.transform.forward, 
                    out hit, float.MaxValue)) return;
            var o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            o.transform.position = hit.point;
            var c = o.AddComponent<DestroyAfterDelay>();
            c.delay = 10;
        }

        public override void AdditionalActionPerformed(GameObject user, GameObject weapon)
        {
            
        }

        public override void AdditionalActionCanceled(GameObject user, GameObject weapon)
        {
            
        }
    }
}