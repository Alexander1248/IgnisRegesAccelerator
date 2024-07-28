using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkieTalkieUI : MonoBehaviour
{
    [SerializeField] private GameObject walkieTalkieUI;
    [SerializeField] private Animator animator;

    public void ActivateAnim(){
        walkieTalkieUI.SetActive(true);
        animator.enabled = true;
        animator.Play("WalkieTalkieWaves", -1, 0);
    }

    public void DisableAnim(){
        walkieTalkieUI.SetActive(false);
    }
}
