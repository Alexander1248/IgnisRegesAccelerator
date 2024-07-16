using UnityEngine;

namespace Weapon.Utils
{
    public class ReloadManager : MonoBehaviour
    {
        public Gun gun;
        public bool IsCanShoot { get; private set; } = true;

        public void Recharge()
        {
            Debug.Log("[Gun]: Recharging...");
            IsCanShoot = false;
            Invoke(nameof(EndRecharge), gun.RechargeTime);
        }
        private void EndRecharge()
        {
            IsCanShoot = true;
            Debug.Log("[Gun]: Recharge completed!");
        }
        
        public void Reload()
        {
            Debug.Log("[Gun]: Reloading...");
            IsCanShoot = false;
            Invoke(nameof(EndReload), gun.ReloadTime);
        }
        private void EndReload()
        {
            gun.Reload();
            IsCanShoot = true;
            Debug.Log("[Gun]: Reload completed!");
        }
    }
}