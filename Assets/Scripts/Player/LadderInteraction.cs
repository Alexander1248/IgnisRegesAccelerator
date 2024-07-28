using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class LadderInteraction : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    private Transform startPoint;
    private Transform endPoint;
    private int endIndex;
    [SerializeField] private Transform[] playerExitPoints;
    [SerializeField] private float duration;
    [SerializeField] private AnimatorController animatorController;
    private PlayerController playerController;
    private Vector3 cameraPositionInPlayer;
    private Transform camAnchor;
    private Transform cam;
    private Animator camAnimator;
    private state currentState;
    private enum state{
        none, cameraSet, walkingUp, cameraBack
    };
    private float t;

    private Vector3 startingPos;
    private Quaternion startingAngle;
    private Quaternion startingAngle2;

    void Start(){
        playerController = GameObject.Find("GamePlayer").GetComponent<PlayerController>();
        camAnchor = playerController.getCamAnchor();
        camAnimator = camAnchor.GetComponent<Animator>();
        cam = camAnchor.GetChild(0).GetChild(0);
    }

    void Update(){
        if (currentState == state.none) return;

        if (currentState == state.cameraSet){
            t += Time.deltaTime / 1;
            camAnchor.position = Vector3.Lerp(startingPos, startPoint.position, t);
            camAnchor.rotation = Quaternion.Lerp(startingAngle, startPoint.rotation, t);
            cam.localRotation = Quaternion.Lerp(startingAngle2, Quaternion.Euler(0, 0, 0), t);
            if (t >= 1){
                currentState = state.walkingUp;
                startingPos = startPoint.position;
                t = 0;
            }
        }
        else if (currentState == state.walkingUp){
            t += Time.deltaTime / duration;
            camAnchor.position = Vector3.Lerp(startingPos, endPoint.position, t);
            if (t >= 1){
                camAnimator.enabled = false;
                currentState = state.cameraBack;
                startingPos = endPoint.position;
                startingAngle = camAnchor.rotation;
                t = 0;

                playerController.gameObject.transform.position = playerExitPoints[endIndex].position;
                playerController.gameObject.transform.eulerAngles = playerExitPoints[endIndex].eulerAngles;
                camAnchor.position = startingPos;
                camAnchor.rotation = startingAngle;
            }
        }
        else if (currentState == state.cameraBack){
            t += Time.deltaTime / 1;
            camAnchor.position = Vector3.Lerp(startingPos, playerController.transform.position + cameraPositionInPlayer, t);
            camAnchor.rotation = Quaternion.Lerp(startingAngle, playerController.transform.rotation, t);
            if (t >= 1){
                currentState = state.none;
                camAnimator.transform.GetChild(0).localPosition = Vector3.zero;
                camAnimator.transform.GetChild(0).localEulerAngles = Vector3.zero;
                camAnchor.localPosition = cameraPositionInPlayer;
                camAnchor.localEulerAngles = Vector3.zero;
                playerController.UnlockPlayer();
                playerController.ShowHands();
                t = 0;
            }
        }
    }

    public void UseLadder(){
        if (playerController.isLayingOrCrouch()) return;
        playerController.hideHands();
        playerController.LockPlayer();
        if (playerController.transform.position.y <= transform.position.y){
            startPoint = points[0];
            endPoint = points[1];
            endIndex = 0;
        }
        else{
            startPoint = points[1];
            endPoint = points[0];
            endIndex = 1;
        }

        cameraPositionInPlayer = camAnchor.localPosition;
        startingPos = camAnchor.position;
        startingAngle = camAnchor.rotation;
        startingAngle2 = cam.localRotation;
        camAnimator.enabled = true;
        camAnimator.runtimeAnimatorController = animatorController;
        camAnimator.CrossFade("LadderAnim", 0.5f);
        currentState = state.cameraSet;
        playerController.ResetCamRotation();
    }
}
