using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] private float max = 100;
        [SerializeField] private float value;
        [SerializeField] private UnityEvent<float, float> onChange;
        
        [SerializeField] private float regenStartDelay = 10;
        [SerializeField] private float regenCooldown = 1;
        [SerializeField] private float regenAmount = 10;

        public float Value => value;
        private void Start()
        {
            value = max;
        }
        public void Initialize(float value)
        {
            this.value = value;
            onChange.Invoke(value, max);
        }

        public bool Use(float delta)
        {
            if (delta < 0) return false;
            if (value < delta) return false;

            CancelInvoke(nameof(Regen));
            InvokeRepeating(nameof(Regen), regenStartDelay, regenCooldown);

            Change(-delta);
            return true;
        }

        public bool Regenerate(float delta)
        {
            if (delta < 0) return false;
            Change(value);
            return true;
        }

        private void Change(float delta)
        {
            value += delta;
            if (value > max)
            {
                value = max;
                CancelInvoke(nameof(Regen));
            }
            onChange.Invoke(value, max);
        }

        private void Regen()
        {
            Change(regenAmount);
        }
        
    }
}