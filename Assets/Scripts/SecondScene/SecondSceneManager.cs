using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondSceneManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Animator animatorExplode;
    [SerializeField] private GameObject wall;

    public void ExplodeWall(){
        wall.SetActive(false);
        explosion.gameObject.SetActive(true);
        explosion.Play(true);
        animatorExplode.enabled = true;
        animatorExplode.Play("CanonExplosion");
    }
}
