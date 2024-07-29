using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.Splines;
using Unity.Mathematics;

public class CanonRails : MonoBehaviour, IChecker
{
    [SerializeField] private SplineContainer splineContainer;
    private int splineIndex = 0;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed;
    private Spline spline;


    [SerializeField] private Transform canon;
    private PlayerController playerController;
    private Transform player;
    private bool attached;
    private bool releasedCanonByLastFrame;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Animator canonAnim;
    [SerializeField] private Animator animatorFlash;
    [SerializeField] private float StaticRotationSpeed;

    [SerializeField] private float reloadKD;
    [SerializeField] private Transform yadroObj;
    [SerializeField] private Transform yadroSpawnPoint;
    private bool readyToShoot = true;
    [SerializeField] private Yadro yadro;
    [SerializeField] private Vector3 StationaryPos;

    [SerializeField] private AudioSource audioSourceRails;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    [SerializeField] private AudioSource fallAudio;

    [SerializeField] private Collider triggerInteract;
    [SerializeField] private ThirdSceneManager thirdSceneManager;

    private bool stationary;

    void Start()
    {
        playerController = GameObject.Find("GamePlayer").GetComponent<PlayerController>();
        spline = splineContainer.Splines[0];
        player = playerController.transform;
        resetYadro();
        audioSourceRails.Play();
        audioSourceRails.Pause();
    }

    public void AttachCanon(){
        triggerInteract.enabled = false;
        attached = true;
        playerController.UseRailCanon(false);
        playerController.hideHands();
    }

    public void ReleaseCanon(){
        triggerInteract.enabled = true;
        playerController.UseRailCanon(true);
        playerController.ShowHands();
        attached = false;
    }

    void Shoot(){
        audioSource.clip = clips[0];
        audioSource.Play();
        particles.gameObject.SetActive(true);
        particles.Play(true);
        canonAnim.enabled = true;
        canonAnim.Play("CanonShoot", -1, 0);
        animatorFlash.enabled = true;
        animatorFlash.Play("CanonExplosion", -1, 0);
        yadroObj.gameObject.SetActive(true);
        yadroObj.SetParent(null);
        yadroObj.position = yadroSpawnPoint.position;

        yadro.Activate(yadroSpawnPoint.forward);
        Invoke("resetYadro", reloadKD);
        Invoke("reloadAudio", reloadKD - 1f);
    }

    void reloadAudio(){
        audioSource.clip = clips[1];
        audioSource.Play();
    }

    void resetYadro(){
        readyToShoot = true;
        yadro.Stop();
        yadroObj.gameObject.SetActive(false);
    }

    void clearFlag(){
        releasedCanonByLastFrame = false;
    }

    void Update()
    {
        if (!attached){
            if (audioSourceRails.isPlaying) audioSourceRails.Pause();
            return;
        }
        player.position = canon.position;

        if (Input.GetMouseButtonDown(0) && readyToShoot){
            readyToShoot = false;
            Shoot();
        }
        
        HandleInput();
        if (Input.GetKeyDown(KeyCode.Escape)){
            releasedCanonByLastFrame = true;
            Invoke("clearFlag", 0.2f);
            ReleaseCanon();
        }
    }

    void HandleInput()
    {
        if (stationary) {
            if (transform.position != StationaryPos) transform.position = Vector3.MoveTowards(transform.position, StationaryPos, 5 * Time.deltaTime);
            Quaternion targetRotation = Quaternion.Euler(player.eulerAngles - new Vector3(0, 180, 0));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, StaticRotationSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, player.rotation, StaticRotationSpeed * Time.deltaTime);
            //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            if (audioSourceRails.isPlaying) audioSourceRails.Pause();
            return;
        }
        var native = new NativeSpline(spline);

        Vector3 localPosition = splineContainer.transform.InverseTransformPoint(transform.position);
        float3 nearestPoint = SplineUtility.GetNearestPoint(native, localPosition, out float3 nearest, out float t); // 0.42

        if (splineIndex == 1 && t > 0.3f && !thirdSceneManager.wallIsBroken){
            SplineUtility.Evaluate(spline, 0.3f, out float3 lp, out float3 lf, out float3 uv);
            nearest = lp;
        }

        transform.position = splineContainer.transform.TransformPoint(nearest);

        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(t));
        Vector3 up = spline.EvaluateUpVector(t);

        Quaternion rot = Quaternion.LookRotation(forward, up) * splineContainer.transform.rotation;
        if (Quaternion.Angle(rot, transform.rotation) > 60){
            rot = Quaternion.LookRotation(-forward, up) * splineContainer.transform.rotation;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (Input.GetKey(KeyCode.W)){
            //rb.AddForce(-transform.forward * moveSpeed, ForceMode.VelocityChange);
            transform.position += -transform.forward * moveSpeed * Time.deltaTime;
            if (!audioSourceRails.isPlaying) audioSourceRails.UnPause();
        }
        else if (Input.GetKey(KeyCode.S)){
            //rb.AddForce(transform.forward * moveSpeed, ForceMode.VelocityChange);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            if (!audioSourceRails.isPlaying) audioSourceRails.UnPause();
        }
        else{
            audioSourceRails.Pause();
        }

        if (splineIndex == 1 && t >= 0.9f) {
            fallAudio.Play();
            stationary = true;
            return;
        }
        if (t >= .99f || t <= 0f) ChangeSpline(t);
    }

    public void ChangeSpline(float t)
    {
        if (splineIndex == 1) splineIndex = 0;
        else splineIndex = 1;

        spline = splineContainer.Splines[splineIndex];


        Vector3 pos = splineContainer.transform.InverseTransformPoint(transform.position);
        float3 nearestPoint = SplineUtility.GetNearestPoint(spline, pos, out float3 nearest, out float tt);
        if (tt == 0){
            SplineUtility.Evaluate(spline, 0.01f, out float3 localPosition, out float3 localForward, out float3 upVector);
            transform.position = splineContainer.transform.TransformPoint(localPosition);
        }
        else {
            transform.position = splineContainer.transform.TransformPoint(nearest);
        }
    }

    public bool boolMethod(){
        return attached || releasedCanonByLastFrame;
    }
}
