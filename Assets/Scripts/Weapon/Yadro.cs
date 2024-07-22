using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yadro : MonoBehaviour
{
    [SerializeField] private float speedYadro;
    [SerializeField] private float gravityYadro;
    private bool moveYadro;
    private Vector3 velocityYadro;
    public FirstSceneManager firstSceneManager;

    public void Activate(Vector3 yadroDirection) {
        transform.forward = yadroDirection;
        velocityYadro = transform.forward * speedYadro;
        moveYadro = true;
    }

    public void Stop(){
        moveYadro = false;
    }

    void Update() {
        if (!moveYadro) return;

        velocityYadro += Vector3.down * gravityYadro * Time.deltaTime;

        transform.position += velocityYadro * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other){
        if (firstSceneManager == null) return;
        if (other.name == "HOLE"){
            firstSceneManager.MegaShipGotHitted();
        }
    }
}
