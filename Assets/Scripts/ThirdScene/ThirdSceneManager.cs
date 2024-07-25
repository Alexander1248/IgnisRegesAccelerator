using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Playables;

public class ThirdSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject ventTrigger;
    [SerializeField] private PlayableDirector explodeVentCS;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Animator animatorExplode;

    [SerializeField] private ParticleSystem explosionWALL;
    [SerializeField] private Animator animatorExplodeWALL;
    [SerializeField] private GameObject wall;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject triggerEnterVent;
    [SerializeField] private Transform pointInVent;
    [SerializeField] private PlayableDirector enterVentCS;
    [SerializeField] private Transform pointCS;
    private Transform cam;
    private Vector3 saveAnchor;
    private bool startCS;
    private float t;
    private Vector3 startCamPos;
    private Quaternion startCamRot;

    [SerializeField] private CanonMove[] fewCanons;
    private int k;
    private int k2;
    
    [SerializeField] private ParticleSystem[] multiexplosions;
    [SerializeField] private GameObject wallDestroyed;

    void Start(){
        cam = playerController.getCamAnchor();
    }

    public void ExplodeVent(){
        ventTrigger.SetActive(false);
        triggerEnterVent.SetActive(true);
        explodeVentCS.Play();
        explosion.gameObject.SetActive(true);
        explosion.Play(true);
        animatorExplode.enabled = true;
        animatorExplode.Play("CanonExplosion");
    }

    public void ExplodeWall(){
        wall.SetActive(false);
        explosionWALL.gameObject.SetActive(true);
        explosionWALL.Play(true);
        animatorExplodeWALL.enabled = true;
        animatorExplodeWALL.Play("CanonExplosion");
    }

    void Update(){
        if (!startCS) return;
        t += Time.deltaTime;
        cam.position = Vector3.Lerp(startCamPos, pointCS.position, t);
        cam.rotation = Quaternion.Lerp(startCamRot, pointCS.rotation, t);
        if (t >= 1){
            startCS = false;
            t = 0;
            cam.position = pointCS.position;
            startCSVent();
        }
    }

    void startCSVent(){
        playerController.GetComponent<Rigidbody>().isKinematic = true;
        playerController.transform.position = pointInVent.position;
        playerController.transform.eulerAngles = pointInVent.eulerAngles;
        enterVentCS.Play();
    }

    public void EnterVent(){
        playerController.LockPlayer();
        startCamPos = cam.position;
        startCamRot = cam.rotation;
        saveAnchor = cam.localPosition;
        startCS = true;
    }

    public void EnteredInVent(){
        playerController.UnlockPlayer();
        playerController.ForceLay();
        playerController.GetComponent<Rigidbody>().isKinematic = false;
        cam.localPosition = saveAnchor;
    }

    public void StartShooting(){
        InvokeRepeating("shootCanon", 0, 0.15f);
        InvokeRepeating("explodeDoor", 0 + .2f, 0.2f);
    }

    void explodeDoor(){
        if (k2 >= multiexplosions.Length){
            wallDestroyed.SetActive(false);
            CancelInvoke("explodeDoor");
            return;
        }
        multiexplosions[k2].gameObject.SetActive(true);
        multiexplosions[k2].Play(true);
        k2++;
    }

    void shootCanon(){
        if (k >= fewCanons.Length){
            CancelInvoke("shootCanon");
            return;
        }
        fewCanons[k].Shoot();
        k++;
    }
}
