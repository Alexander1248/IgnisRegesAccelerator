using System;
using System.Linq;
using Items.Active;
using Managers;
using Player;
using UnityEngine;

namespace Weapon.Utils
{
    public class ReloadManager : MonoBehaviour
    {
        public Gun gun;
        public bool IsCanShoot { get; private set; } = true;
        public bool IsReloading { get; private set; } = false;
 
        private InventoryManager manager;
        private void Start()
        {
            if (!GameObject.FindGameObjectsWithTag("Player").Any(obj => obj.TryGetComponent(out manager)))
                throw new ArgumentException("Player with InventoryManager not found!");
        }
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
        public void Lock()
        {
            IsCanShoot = false;
        }
        public void Reload()
        {
            try
            {
                var (id, pos) = manager.Find(item =>
                    item is AmmoItem ammo && ammo.Type.ToLowerInvariant() == "pistol").First();
                manager.RemoveItem(id, pos.x, pos.y);
                Invoke(nameof(EndReload), gun.ReloadTime);
                IsReloading = true;
                Debug.Log("[Gun]: Reloading...");
            } catch(Exception) {}
        }
        private void EndReload()
        {
            gun.Reload();
            IsCanShoot = true;
            IsReloading = false;
            Debug.Log("[Gun]: Reload completed!");
        }
    }
}