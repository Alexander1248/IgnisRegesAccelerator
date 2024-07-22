using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class CanonMove : MonoBehaviour
{
    public Transform canon;
    public Transform canonMesh;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float rotationSpeed;
    private bool attached;

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Animator canonAnim;
    [SerializeField] private Animator animatorFlash;

    [SerializeField] private float reloadKD;
    [SerializeField] private Transform yadroObj;
    [SerializeField] private Transform yadroSpawnPoint;
    private bool readyToShoot = true;
    [SerializeField] private Yadro yadro;

    void Start(){
        //AttachCanon();
    }

    public void AttachCanon(){
        attached = true;
        playerController.useCanon();
    }

    void Update(){
        if (!attached) return;
        canon.position = transform.position;
        Vector3 newDirection = Vector3.RotateTowards(canon.forward, transform.forward, rotationSpeed * Time.deltaTime, 0.0f);
        canon.rotation = Quaternion.LookRotation(newDirection);

        if (Input.GetMouseButtonDown(0) && readyToShoot){
            readyToShoot = false;
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            playerController.releaseCanon();
            attached = false;
        }
    }

    void Shoot(){
        particles.gameObject.SetActive(true);
        particles.Play(true);
        canonAnim.enabled = true;
        canonAnim.Play("CanonShoot", -1, 0);
        animatorFlash.enabled = true;
        animatorFlash.Play("CanonExplosion", -1, 0);
        yadroObj.gameObject.SetActive(true);
        yadroObj.SetParent(null);
        yadroObj.position = yadroSpawnPoint.position;

        yadro.Activate(yadroSpawnPoint.forward);
        Invoke("resetYadro", reloadKD);
    }

    void resetYadro(){
        readyToShoot = true;
        yadro.Stop();
        yadroObj.gameObject.SetActive(false);
    }
}
