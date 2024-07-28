using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Player;
using UnityEngine.SceneManagement;

public class FirstSceneManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem megashipExplosion;
    [SerializeField] private Animator animatorFlash;
    [SerializeField] private GameObject holeObj;
    [SerializeField] private PlayableDirector mainCS;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject eyeBlinks;
    [SerializeField] private PlayableDirector wakeupCS;
    [SerializeField] private float wakeupSpeed;

    [SerializeField] private GameObject playertrigger;
    [SerializeField] private GameObject megaShip;
    [SerializeField] private CanonMove canonMove;
    
    [SerializeField] private GameObject WalkieTalkie;
    [SerializeField] private Coder coder;
    [SerializeField] private GameObject[] rats;

    [SerializeField] private Animator animatorFade;

    [SerializeField] private AudioSource sirena;
    [SerializeField] private AudioSource epic;
    [SerializeField] private AudioSource explosionSound;
    private bool holeExploded;
    private bool playerAwaked;
    [SerializeField] private Transform awakePos;

    void Awake(){
        playerController.GetComponent<Rigidbody>().isKinematic = true;

        playerController.LockPlayer();
    }

    void Update(){
        if (playerAwaked) return;
        playerController.transform.position = awakePos.position;
    }

    void Start(){
        wakeupCS.playableGraph.GetRootPlayable(0).SetSpeed(wakeupSpeed);
        megaShip.SetActive(false);
    }

    public void playerAwake(){
        playerController.GetComponent<Rigidbody>().isKinematic = false;
        playerController.UnlockPlayer();
        playerAwaked = true;
        eyeBlinks.SetActive(false);
    }

    public void MegaShipGotHitted(){
        megashipExplosion.gameObject.SetActive(true);
        megashipExplosion.Play(true);
        animatorFlash.enabled = true;
        animatorFlash.Play("CanonExplosion", -1, 0);
        explosionSound.Play();
        holeObj.SetActive(false);
        mainCS.Resume();
        holeExploded = true;
        canonMove.ReleaseCanon();
    }

    public void WaitinHitMegaShip(){
        if (!holeExploded) mainCS.Pause();
    }

    public void ActivateMainCS(){
        epic.Play();
        megaShip.SetActive(true);
        mainCS.Play();
    }

    public void WaitPlayerToExit(){
        sirena.Play();
        playertrigger.SetActive(true);
    }

    public void PickUpWalkieTalkie(){
        for(int i = 0; i < rats.Length; i++) rats[i].SetActive(true);
        WalkieTalkie.SetActive(false);
        coder.doNums();
        // start dialogue
    }

    void loadNextScene(){
        SceneManager.LoadScene("KOSTYAN_1");
    }

    public void EnterShip(){
        animatorFade.enabled = true;
        animatorFade.Play("FadeIn", -1, 0);
        Invoke("loadNextScene", 3);
    }
}
