using UnityEngine;

namespace Controllers
{
    public class HealthUpdater : MonoBehaviour
    {
        [SerializeField] private Health health;

        public void DealDamage(float damage, Vector3 direction, float kickForce, Vector3? point = null)
            => health.DealDamage(damage, direction, kickForce, point);
    }
}