using System.Collections;
using System.Collections.Generic;
using Managers;
using Player;
using Plugins.DialogueSystem.Scripts.DialogueGraph;
using Quests;
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

    [SerializeField] private WalkieTalkieUI walkieTalkieUI;
    [SerializeField] private Dialogue dialogue;
    private bool lastDialog;

    [SerializeField] private QuestManager questManager;
    [SerializeField] private Quest[] quests;
    private bool foundChest;

    [SerializeField] private GameObject[] enemies;

    [SerializeField] private SaveManager saveManager;

     void Start(){
        animatorFade.enabled = true;
        animatorFade.Play("FadeOut", 0, 0);
        cam = playerController.getCamAnchor();
        merc.SetActive(false);
        questManager.Add(quests[0]);
    }
    public void FindChest(){
        if (foundChest) return;
        foundChest = true;
        questManager.Complete(quests[0]);
    }

    public void StartCS(){
        trigegrInteract.enabled = false;
        playerController.LockPlayer();
        playerController.hideHands();
        cam.GetChild(0).GetChild(0).localEulerAngles = Vector3.zero;
        cam.SetParent(playableDirector.transform);
        playableDirector.Play();

        for(int i = 0; i < enemies.Length; i++) if(enemies[i] != null) Destroy(enemies[i]);
    }

    public void DialogueEnd(){
        if (lastDialog){
            Invoke("loadMenu", 2);
        }
        walkieTalkieUI.DisableAnim();
    }

    public void StartDialogue(string name){
        //dialogue.StopAll();
        dialogue.StartDialogueNow(name);
        walkieTalkieUI.ActivateAnim();
    }

    public void EndGame(){
        animatorFade.Play("InstFade", 0, 0);
        StartDialogue("Dialog12");
        lastDialog = true;
    }

    void loadMenu(){
        saveManager.Save();
        SceneManager.LoadScene("MENU");
    }
}
