using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PalubaManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    private Transform cam;
    [SerializeField] private  PlayerController playerController;
    [SerializeField] private GameObject merc;
    [SerializeField] private Collider trigegrInteract;

    [SerializeField] private Animator animatorFade;

    void Start(){
        animatorFade.enabled = true;
        animatorFade.Play("FadeOut", 0, 0);
        cam = playerController.getCamAnchor();
        merc.SetActive(false);
    }

    public void StartCS(){
        trigegrInteract.enabled = false;
        playerController.LockPlayer();
        playerController.hideHands();
        cam.GetChild(0).GetChild(0).localEulerAngles = Vector3.zero;
        cam.SetParent(playableDirector.transform);
        playableDirector.Play();
    }

    public void EndGame(){
        animatorFade.Play("InstFade", 0, 0);
        Invoke("loadMenu", 9);
    }

    void loadMenu(){
        SceneManager.LoadScene("MENU");
    }
}
