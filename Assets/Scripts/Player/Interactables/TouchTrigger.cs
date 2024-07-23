using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent eventOnTouch;
    [SerializeField] private bool destroyAfterTouch = true;

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")) {
            eventOnTouch.Invoke();
            if (destroyAfterTouch) Destroy(gameObject);
        }
    }
}
