using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FirstSceneManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem megashipExplosion;
    [SerializeField] private Animator animatorFlash;
    [SerializeField] private GameObject holeObj;
    [SerializeField] private PlayableDirector mainCS;

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
