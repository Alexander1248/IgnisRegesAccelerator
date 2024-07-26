using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Playables;

public class PalubaManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    private Transform cam;
    [SerializeField] private  PlayerController playerController;
    [SerializeField] private GameObject merc;

    void Start(){
        cam = playerController.getCamAnchor();
        merc.SetActive(false);
    }

    public void StartCS(){
        playerController.LockPlayer();
        cam.GetChild(0).localEulerAngles = Vector3.zero;
        cam.SetParent(playableDirector.transform);
        playableDirector.Play();
    }
}
