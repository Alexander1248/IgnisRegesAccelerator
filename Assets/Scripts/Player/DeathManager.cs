using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private Animator animatorFade;
    
    public void PlayerDied(){
        if (animatorFade != null){
            animatorFade.enabled = true;
            animatorFade.Play("InstFade", 0, -1);
            Invoke("reloadScene", 3);
        }
        else{
            reloadScene();
        }
        // audio off
    }

    void reloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
