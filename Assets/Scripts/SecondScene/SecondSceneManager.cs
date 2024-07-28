using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondSceneManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Animator animatorExplode;
    [SerializeField] private GameObject wall;
    [SerializeField] private Animator animatorFade;
    [SerializeField] private AudioSource audioSource;

    void Awake(){
        animatorFade.enabled = true;
        animatorFade.Play("FadeOut", -1, 0);
    }

    public void ExplodeWall(){
        wall.SetActive(false);
        explosion.gameObject.SetActive(true);
        explosion.Play(true);
        animatorExplode.enabled = true;
        animatorExplode.Play("CanonExplosion");
        audioSource.Play();
    }

    public void NextLvl(){
        animatorFade.enabled = true;
        animatorFade.Play("FadeIn", -1, 0);
        Invoke("loadNextLvl", 3);
    }

    void loadNextLvl(){
        SceneManager.LoadScene("KOSTYAN_2");
    }
}
