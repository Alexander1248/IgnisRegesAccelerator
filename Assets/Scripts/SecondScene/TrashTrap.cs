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

    void Start(){
        Invoke("OpenMe", durationClosed + startDelay);
        Invoke("LightsOn", durationClosed - lightTime + startDelay);

        foreach(ParticleSystem p in trashParticles){
            var main = p.main;
            main.duration = durationOpen - 0.5f;
        }
    }

    void LightsOn(){
        light.SetActive(true);
        meshRenderer.material = materials[1];
    }
    void LightsOff(){
        light.SetActive(false);
        meshRenderer.material = materials[0];
    }

    void OpenMe(){
        Invoke("CloseMe", durationOpen);
        if (!animator.enabled) animator.enabled = true;
        animator.CrossFade("TrapOpen", 0.1f);
        if (trashParticles.Length > 0) trashParticles[0].Play(true);
    }

    void CloseMe(){
        LightsOff();
        Invoke("OpenMe", durationClosed);
        Invoke("LightsOn", durationClosed - lightTime);
        if (!animator.enabled) animator.enabled = true;
        animator.CrossFade("TrapClose", 0.1f);
    }
}
