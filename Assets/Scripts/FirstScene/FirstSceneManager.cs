using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Player;

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

    void Awake(){
        playerController.LockPlayer();
    }

    void Start(){
        wakeupCS.playableGraph.GetRootPlayable(0).SetSpeed(wakeupSpeed);
        megaShip.SetActive(false);
    }

    public void playerAwake(){
        playerController.UnlockPlayer();
        eyeBlinks.SetActive(false);
    }

    public void MegaShipGotHitted(){
        megashipExplosion.gameObject.SetActive(true);
        megashipExplosion.Play(true);
        animatorFlash.enabled = true;
        animatorFlash.Play("CanonExplosion", -1, 0);
        holeObj.SetActive(false);
        mainCS.Resume();
        canonMove.ReleaseCanon();
    }

    public void WaitinHitMegaShip(){
        mainCS.Pause();
    }

    public void ActivateMainCS(){
        megaShip.SetActive(true);
        mainCS.Play();
    }

    public void WaitPlayerToExit(){
        playertrigger.SetActive(true);
    }
}
