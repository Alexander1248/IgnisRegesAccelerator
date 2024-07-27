using System;
using System.Linq;
using Items.Active;
using Managers;
using UnityEngine;

namespace Weapon.Utils
{
    public class ReloadManager : MonoBehaviour
    {
        public Gun gun;
        public bool IsCanShoot { get; private set; } = true;
 
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
        
        public void Reload()
        {
            Debug.Log("[Gun]: Reloading...");
            IsCanShoot = false;
            try
            {
                var (id, pos) = manager.Find(item =>
                    item is AmmoItem ammo && ammo.Type.ToLowerInvariant() == "pistol").First();
                manager.RemoveItem(id, pos.x, pos.y);
                Invoke(nameof(EndReload), gun.ReloadTime);
            } catch(Exception) {}
        }
        private void EndReload()
        {
            gun.Reload();
            IsCanShoot = true;
            Debug.Log("[Gun]: Reload completed!");
        }
    }
}