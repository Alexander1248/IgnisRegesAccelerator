using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject UIPause;
    private bool paused;

    [System.Serializable]
    public class UIButton{
        public Transform parent;
        public Image[] imgs;
        public TMP_Text text;
    }

    [SerializeField] public UIButton[] buttons;

    [SerializeField] private float selectedScale;
    [SerializeField] private float unselectedScale;
    [SerializeField] private float aplhaUnselected;
    [SerializeField] private Sprite[] filledNnotThings;

    [SerializeField] private GameObject[] menus;

    [SerializeField] private Animator fadeAnim;

    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject journal;
    [SerializeField] private GameObject Ichecker;
    private IChecker checker;

    private int currentSelected;
    private bool inControls;
    private int pressed = -1;
    private float timeScale;

    [SerializeField] private AudioSource[] audioSourcesClicks;

    void Start(){
        checker = Ichecker.GetComponent<IChecker>();
    }

    void Update(){
        if (Input.GetKeyUp(KeyCode.Escape)){
            if (checker != null && checker.boolMethod() || inventory.activeSelf || journal.activeSelf) return;
            PauseGame();
        }
    }

    void PauseGame(){
        if (inControls){
            // apply controls
            inControls = false;
            menus[1].SetActive(false);
            return;
        }
        paused = !paused;
        if (paused){
            playerController.UnlockCursor();
            playerController.LockPlayer();
            settingsMenu.UpdateSlider();
            menus[0].SetActive(true);
            timeScale = 0.1f;
        }
        else{
            playerController.CheckSettings();
            playerController.LockCursor();
            playerController.UnlockPlayer();
            menus[0].SetActive(false);
            timeScale = 1f;
        }
        Time.timeScale = timeScale;
    }

    public void SelectButton(int id){
        if (currentSelected != id) audioSourcesClicks[0].Play();
        Deselect(currentSelected);
        currentSelected = id;

        buttons[id].parent.localScale = selectedScale * Vector3.one;
        if (buttons[id].imgs.Length != 3) return;
        for(int i = 0; i < buttons[id].imgs.Length; i++){
            buttons[id].imgs[i].color = new Color(buttons[id].imgs[i].color.r, 
                buttons[id].imgs[i].color.g, buttons[id].imgs[i].color.b, 1); 
            if (i != 0) buttons[id].imgs[i].sprite = filledNnotThings[0];
        }
        buttons[id].text.color = new Color(buttons[id].text.color.r, 
            buttons[id].text.color.g, buttons[id].text.color.b, 1); 
    }

    public void Deselect(int id){
        buttons[id].parent.localScale = unselectedScale * Vector3.one;
        if (buttons[id].imgs.Length != 3) return;
        for(int i = 0; i < buttons[id].imgs.Length; i++){
            buttons[id].imgs[i].color = new Color(buttons[id].imgs[i].color.r, 
                buttons[id].imgs[i].color.g, buttons[id].imgs[i].color.b, aplhaUnselected); 
            if (i != 0) buttons[id].imgs[i].sprite = filledNnotThings[1];
        }
        buttons[id].text.color = new Color(buttons[id].text.color.r, 
            buttons[id].text.color.g, buttons[id].text.color.b, aplhaUnselected); 
    }

    public void PressButton(int id){
        if (pressed != -1) return;
        audioSourcesClicks[1].Play();
        pressed = id;
        buttons[pressed].parent.localScale = selectedScale * 1.1f * Vector3.one;
        Invoke("Press", 0.07f * timeScale);
    }

    void loadMenu(){
        SceneManager.LoadScene("MENU");
    }

    void Press(){
        buttons[pressed].parent.localScale = selectedScale * Vector3.one;
        if (pressed == 0){
            // continue
            PauseGame();
        }
        else if (pressed == 1){
            // controls
            menus[1].SetActive(true);
            inControls = true;
        }
        else if (pressed == 2){
            // exit
            Time.timeScale = 1f;
            fadeAnim.enabled = true;
            fadeAnim.Play("FadeIn", -1, 0);
            Invoke("loadMenu", 1.5f);
        }
        else if (pressed == 3){
            // cancel controls
            inControls = false;
            menus[1].SetActive(false);
        }
        else if (pressed == 4){
            // apply controls
            inControls = false;
            menus[1].SetActive(false);
        }

        pressed = -1;
    }
}
