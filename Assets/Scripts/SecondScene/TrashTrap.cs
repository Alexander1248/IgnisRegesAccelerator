using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTrap : MonoBehaviour
{
    [SerializeField] private float durationOpen;
    [SerializeField] private float durationClosed;
    [SerializeField] private float startDelay;
    [SerializeField] private float lightTime;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject light;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material[] materials;

    [SerializeField] private ParticleSystem[] trashParticles;
    [SerializeField] private AudioSource audioSourceOpen;
    [SerializeField] private AudioSource warning;
    [SerializeField] private AudioSource trashSound;

    void Start(){
        Invoke("OpenMe", durationClosed + startDelay);
        Invoke("LightsOn", durationClosed - lightTime + startDelay);

        foreach(ParticleSystem p in trashParticles){
            var main = p.main;
            main.duration = durationOpen - 0.5f;
        }
    }

    void LightsOn(){
        if (warning) warning.Play();
        light.SetActive(true);
        meshRenderer.material = materials[1];
    }
    void LightsOff(){
        light.SetActive(false);
        meshRenderer.material = materials[0];
    }

    void OpenMe(){
        Invoke("CloseMe", durationOpen);
        audioSourceOpen.pitch = Random.Range(0.9f, 1f);
        audioSourceOpen.Play();
        if (!animator.enabled) animator.enabled = true;
        animator.CrossFade("TrapOpen", 0.1f);
        if (trashParticles.Length > 0) {
            trashSound.Play();
            trashParticles[0].Play(true);
        }
    }

    void CloseMe(){
        LightsOff();
        audioSourceOpen.pitch = Random.Range(0.9f, 1f);
        audioSourceOpen.Play();
        Invoke("OpenMe", durationClosed);
        Invoke("LightsOn", durationClosed - lightTime);
        if (!animator.enabled) animator.enabled = true;
        animator.CrossFade("TrapClose", 0.1f);
        if (trashSound != null) trashSound.Stop();
    }
}
