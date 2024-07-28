using System.Collections;
using System.Collections.Generic;
using Plugins.DialogueSystem.Scripts.DialogueGraph;
using Quests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondSceneManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Animator animatorExplode;
    [SerializeField] private GameObject wall;
    [SerializeField] private Animator animatorFade;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private WalkieTalkieUI walkieTalkieUI;
    [SerializeField] private Dialogue dialogue;

    [SerializeField] private QuestManager questManager;
    [SerializeField] private Quest[] quests;
    private bool exitQuestAdded;
    private bool gunTaken;

    private int currentDialogue = 1;

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

    public void DialogueEnd(){
        walkieTalkieUI.DisableAnim();

        if (currentDialogue == 7 && !gunTaken) questManager.Add(quests[0]);
        else if (currentDialogue == 8) questManager.Add(quests[1]);
    }

    public void CompleteQuest(int id){
        if (id == 0){
            gunTaken = true;
            return;
        }
        questManager.Complete(quests[id]);
    }
    public void AddQuestExit(){
        if (exitQuestAdded) return;
        exitQuestAdded = true;
        questManager.Add(quests[2]);
    }


    public void StartDialogue(string name){
        //dialogue.StopAll();
        dialogue.StartDialogueNow(name);
        char a = name[name.Length - 1];
        int.TryParse(a + "", out currentDialogue);
        walkieTalkieUI.ActivateAnim();
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
