using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Yadro : MonoBehaviour
{
    [SerializeField] private float speedYadro;
    [SerializeField] private float gravityYadro;
    private bool moveYadro;
    private Vector3 velocityYadro;

    [SerializeField] private UnityEvent EventHit;
    [SerializeField] private UnityEvent AdditionalEventHit;
    private MeshRenderer meshRenderer;

    void Awake(){
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Activate(Vector3 yadroDirection) {
        if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        transform.forward = yadroDirection;
        velocityYadro = transform.forward * speedYadro;
        moveYadro = true;
    }

    public void Stop(){
        if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        moveYadro = false;
    }

    void Update() {
        if (!moveYadro) return;

        velocityYadro += Vector3.down * gravityYadro * Time.deltaTime;

        transform.position += velocityYadro * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other){
        if (other.name == "HOLE"){
            EventHit.Invoke();
            Stop();
        }
        else if (other.name == "HITABLE"){
            AdditionalEventHit.Invoke();
            Stop();
        }
    }
}
