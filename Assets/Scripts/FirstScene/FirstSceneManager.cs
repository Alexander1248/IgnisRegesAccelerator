using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Player;
using UnityEngine.SceneManagement;
using Plugins.DialogueSystem.Scripts.DialogueGraph;
using Quests;

public class FirstSceneManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem megashipExplosion;
    [SerializeField] private Animator animatorFlash;
    [SerializeField] private GameObject holeObj;
    [SerializeField] private PlayableDirector mainCS;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject eyeBlinks;
    [SerializeField] private PlayableDirector wakeupCS;
    [SerializeField] private float wakeupSpeed;

    [SerializeField] private GameObject playertrigger;
    [SerializeField] private GameObject megaShip;
    [SerializeField] private CanonMove canonMove;
    
    [SerializeField] private GameObject WalkieTalkie;
    [SerializeField] private Coder coder;
    [SerializeField] private GameObject[] rats;

    [SerializeField] private Animator animatorFade;

    [SerializeField] private AudioSource sirena;
    [SerializeField] private AudioSource epic;
    [SerializeField] private AudioSource explosionSound;
    private bool holeExploded;
    private bool playerAwaked;
    [SerializeField] private Transform awakePos;

    [SerializeField] private Dialogue dialogue;
    [SerializeField] private AudioSource worldRadio;
    [SerializeField] private GameObject zonePC;
    private bool afterThisEnablePCZone;

    [SerializeField] private WalkieTalkieUI walkieTalkieUI;

    [SerializeField] private QuestManager questManager;
    [SerializeField] private Quest[] quests;

    private int currentDialogue = 1;

    void Awake(){
        playerController.GetComponent<Rigidbody>().isKinematic = true;

        playerController.LockPlayer();
    }

    void Update(){
        if (playerAwaked) return;
        playerController.transform.position = awakePos.position;
    }

    void Start(){
        wakeupCS.playableGraph.GetRootPlayable(0).SetSpeed(wakeupSpeed);
        megaShip.SetActive(false);
        dialogue.StartDialogueNow("Dialog1");
        questManager.Add(quests[0]);
    }

    public void playerAwake(){
        playerController.GetComponent<Rigidbody>().isKinematic = false;
        playerController.UnlockPlayer();
        playerAwaked = true;
        eyeBlinks.SetActive(false);
    }

    public void MegaShipGotHitted(){
        dialogue.StartDialogueNow("Dialog6");
        questManager.Complete(quests[3]);
        currentDialogue = 6;
        walkieTalkieUI.ActivateAnim();
        megashipExplosion.gameObject.SetActive(true);
        megashipExplosion.Play(true);
        animatorFlash.enabled = true;
        animatorFlash.Play("CanonExplosion", -1, 0);
        explosionSound.Play();
        holeObj.SetActive(false);
        mainCS.Resume();
        holeExploded = true;
        canonMove.ReleaseCanon();
    }

    public void WaitinHitMegaShip(){
        if (!holeExploded) mainCS.Pause();
    }

    public void ActivateMainCS(){
        epic.Play();
        megaShip.SetActive(true);
        mainCS.Play();
    }

    public void WaitPlayerToExit(){
        sirena.Play();
        playertrigger.SetActive(true);
        dialogue.StartDialogueNow("Dialog5");
        questManager.Complete(quests[2]);
        currentDialogue = 5;
        walkieTalkieUI.ActivateAnim();
    }

    public void PickUpWalkieTalkie(){
        WalkieTalkie.SetActive(false);
        worldRadio.Stop();
        dialogue.StopAll();
        dialogue.StartDialogueNow("Dialog2");
        currentDialogue = 2;
        questManager.Complete(quests[0]);
        walkieTalkieUI.ActivateAnim();
        afterThisEnablePCZone = true;
        // start dialogue
    }

    public void StartDialogue(string name){
        //dialogue.StopAll();
        dialogue.StartDialogueNow(name);
        char a = name[name.Length - 1];
        int.TryParse(a + "", out currentDialogue);
        walkieTalkieUI.ActivateAnim();
    }

    public void CompleteQuest(int id){
        questManager.Complete(quests[id]);
    }

    public void DialogEnded(){
        walkieTalkieUI.DisableAnim();

        if (currentDialogue == 2){
            questManager.Add(quests[1]); // подняться к пк
        }
        else if (currentDialogue == 3){
            questManager.Add(quests[2]); // найти код
        }
        else if (currentDialogue == 5){
            questManager.Add(quests[3]); // взорвать
        }
        else if (currentDialogue == 6){
            questManager.Add(quests[4]); // проникнуть
        }

        if (!afterThisEnablePCZone) return;
        afterThisEnablePCZone = false;
        zonePC.SetActive(true);
    }

    public void ZonePCActivate(){
        questManager.Complete(quests[1]);
        zonePC.SetActive(false);
        dialogue.StartDialogueNow("Dialog3");
        currentDialogue = 3;
        walkieTalkieUI.ActivateAnim();
        for(int i = 0; i < rats.Length; i++) rats[i].SetActive(true);
        coder.doNums();
    }

    void loadNextScene(){
        SceneManager.LoadScene("KOSTYAN_1");
    }

    public void EnterShip(){
        animatorFade.enabled = true;
        animatorFade.Play("FadeIn", -1, 0);
        Invoke("loadNextScene", 3);
    }
}
