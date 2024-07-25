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

    void Start(){
        playerController.ForceLay();
    }

    public void ExplodeVent(){
        ventTrigger.SetActive(false);
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
}
