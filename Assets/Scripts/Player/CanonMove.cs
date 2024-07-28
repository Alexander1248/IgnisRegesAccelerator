using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class CanonMove : MonoBehaviour, IChecker
{
    public Transform canon;
    private PlayerController playerController;
    private Transform player;
    [SerializeField] private float rotationSpeed;
    public bool attached;

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Animator canonAnim;
    [SerializeField] private Animator animatorFlash;

    [SerializeField] private float reloadKD;
    [SerializeField] private Transform yadroObj;
    [SerializeField] private Transform yadroSpawnPoint;
    private bool readyToShoot = true;
    private bool releasedCanonByLastFrame;
    [SerializeField] private Yadro yadro;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    void Start(){
        //AttachCanon();
        playerController = GameObject.Find("GamePlayer").GetComponent<PlayerController>();
        player = playerController.transform;
        resetYadro();
    }

    public void AttachCanon(){
        attached = true;
        playerController.useCanon();
    }

    public void ReleaseCanon(){
        playerController.releaseCanon();
        attached = false;
    }

    void Update(){
        releasedCanonByLastFrame = false;
        if (!attached) return;
        canon.position = player.position;
        Vector3 newDirection = Vector3.RotateTowards(canon.forward, player.forward, rotationSpeed * Time.deltaTime, 0.0f);
        canon.rotation = Quaternion.LookRotation(newDirection);

        if (Input.GetMouseButtonDown(0) && readyToShoot){
            readyToShoot = false;
            Shoot();
        }
        if (Input.GetKeyUp(KeyCode.Escape)){
            releasedCanonByLastFrame = true;
            ReleaseCanon();
        }
    }

    public void Shoot(){
        audioSource.clip = clips[0];
        audioSource.Play();
        readyToShoot = false;
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
        Invoke("reloadAudio", reloadKD - 1f);
    }

    void reloadAudio(){
        audioSource.clip = clips[1];
        audioSource.Play();
    }

    void resetYadro(){
        readyToShoot = true;
        yadro.Stop();
        yadroObj.gameObject.SetActive(false);
    }

    public bool boolMethod(){
        return attached || releasedCanonByLastFrame;
    }
}
