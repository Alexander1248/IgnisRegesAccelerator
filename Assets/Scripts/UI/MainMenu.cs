using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
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
    
    [SerializeField] private AudioSource audioSourceMusic;
    private bool fadeOut;

    [SerializeField] private AudioSource[] audioSourcesClicks;

    private int currentSelected;
    private int pressed = -1;

    void Start(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SelectButton(0);
    }


    public void SelectButton(int id){
        //if (currentSelected == id) return;
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
        Invoke("Press", 0.07f);
    }

    void loadGame(){
        SceneManager.LoadScene("KOSTYAN_NETROGAT");
    }

    void Press(){
        buttons[pressed].parent.localScale = selectedScale * Vector3.one;
        if (pressed == 0){
            // continue
            fadeOut = true;
            fadeAnim.enabled = true;
            fadeAnim.Play("FadeIn", -1, 0);
            Invoke("loadGame", 1.5f);
        }
        else if (pressed == 1){
            // new game
            fadeOut = true;
            fadeAnim.enabled = true;
            fadeAnim.Play("FadeIn", -1, 0);
            Invoke("loadGame", 1.5f);
        }
        else if (pressed == 2){
            // settings
            menus[1].SetActive(true);
        }
        else if (pressed == 3){
            // exit
            Application.Quit();
        }
        else if (pressed == 4){
            // close settings
            menus[1].SetActive(false);
            SelectButton(0);
        }
        else if (pressed == 5){
            // controls
            menus[2].SetActive(true);
        }
        else if (pressed == 6){
            // cancel controls
            menus[2].SetActive(false);
        }
        else if (pressed == 7){
            // apply controls
            menus[2].SetActive(false);
        }

        pressed = -1;
    }

    void Update(){
        if (!fadeOut) return;
        audioSourceMusic.volume -= Time.deltaTime;
    }
}
