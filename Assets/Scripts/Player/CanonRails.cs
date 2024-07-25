using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.Splines;
using Unity.Mathematics;

public class CanonRails : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private int splineIndex = 0;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed;
    private Spline spline;


    [SerializeField] private Transform canon;
    [SerializeField] private PlayerController playerController;
    private Transform player;
    private bool attached;
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

    private bool stationary;

    void Start()
    {
        spline = splineContainer.Splines[0];
        player = playerController.transform;
        resetYadro();
    }

    public void AttachCanon(){
        attached = true;
        playerController.UseRailCanon(false);
    }

    public void ReleaseCanon(){
        playerController.UseRailCanon(true);
        attached = false;
    }

    void Shoot(){
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
    }

    void resetYadro(){
        readyToShoot = true;
        yadro.Stop();
        yadroObj.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!attached) return;
        player.position = canon.position;

        if (Input.GetMouseButtonDown(0) && readyToShoot){
            readyToShoot = false;
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            ReleaseCanon();
        }
        
        HandleInput();
    }

    void HandleInput()
    {
        if (stationary) {
            if (transform.position != StationaryPos) transform.position = Vector3.MoveTowards(transform.position, StationaryPos, 5 * Time.deltaTime);
            Quaternion targetRotation = Quaternion.Euler(player.eulerAngles - new Vector3(0, 180, 0));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, StaticRotationSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, player.rotation, StaticRotationSpeed * Time.deltaTime);
            //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            return;
        }
        var native = new NativeSpline(spline);

        Vector3 localPosition = splineContainer.transform.InverseTransformPoint(transform.position);
        float3 nearestPoint = SplineUtility.GetNearestPoint(native, localPosition, out float3 nearest, out float t);

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
        }
        else if (Input.GetKey(KeyCode.S)){
            //rb.AddForce(transform.forward * moveSpeed, ForceMode.VelocityChange);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        if (splineIndex == 1 && t >= 0.9f) {
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
}
