using UnityEngine;
using UnityEngine.Events;

namespace Triggers
{
    public class ZoneTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider> enter;
        [SerializeField] private UnityEvent<Collider> stay;
        [SerializeField] private UnityEvent<Collider> exit;
        [Space]
        [SerializeField] private UnityEvent playerEnter;
        [SerializeField] private UnityEvent playerStay;
        [SerializeField] private UnityEvent playerExit;

        private void OnTriggerEnter(Collider other)
        {
            enter.Invoke(other);
            if (other.gameObject.CompareTag("Player")) playerEnter.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            stay.Invoke(other);
            if (other.gameObject.CompareTag("Player")) playerStay.Invoke();
        }
        private void OnTriggerExit(Collider other)
        {
            exit.Invoke(other);
            if (other.gameObject.CompareTag("Player")) playerExit.Invoke();
        }
    }
}