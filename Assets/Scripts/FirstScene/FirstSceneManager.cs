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

    void Awake(){
        playerController.LockPlayer();
    }

    void Start(){
        wakeupCS.playableGraph.GetRootPlayable(0).SetSpeed(wakeupSpeed);
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
    }

    public void WaitinHitMegaShip(){
        mainCS.Pause();
    }
}
