using UnityEngine;

namespace TestUtils
{
    public class DestroyAfterDelay : MonoBehaviour
    {
        public float delay;

        private void Start()
        {
            Invoke(nameof(Destroy), delay);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}