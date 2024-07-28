using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat : MonoBehaviour
{
    [SerializeField] private Transform RatParent;
    [SerializeField] float wanderRadius = 10f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float speedTransferKoeff;

    [SerializeField] private float chanceAttak;
    [SerializeField] private float radiusAttack;
    private bool iSeePlayer;
    private bool attacking;
    private Transform player;

    [SerializeField] private float jumpTime;
    private bool jumping;
    private Vector3 jumpStart;
    private Vector3 jumpEnd;
    private float jumpT;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip hittedClip;
    [SerializeField] private AudioSource walkSound;

    private bool hitted;


    void Start(){
        player = GameObject.Find("GamePlayer").transform;
        walkToRandPos();
    }

    void Update(){
        if (!attacking){
            if (agent.velocity.magnitude < 1) walkSound.Pause();
            else if (!walkSound.isPlaying) walkSound.Play();
            if (!hitted)
                animator.speed = agent.velocity.magnitude * speedTransferKoeff;
        }
        else{
            RatParent.forward = (new Vector3(player.position.x, RatParent.position.y, player.position.z) - RatParent.position).normalized;
        }

        if (HasReachedDestination() && !attacking) walkToRandPos();

        if (Mathf.Abs(RatParent.position.y - player.position.y) <= 2.5f && 
        Vector3.Distance(RatParent.position, player.position) <= radiusAttack && !iSeePlayer){
            iSeePlayer = true;
            InvokeRepeating("tryAttck", 0, 0.5f);
        }
        else if ((Vector3.Distance(RatParent.position, player.position) > radiusAttack || 
        Mathf.Abs(RatParent.position.y - player.position.y) > 2.5f) && iSeePlayer){
            iSeePlayer = false;
            CancelInvoke("tryAttck");
            if (!attacking && !jumping){
                jumping = false;
                attacking = false;
            }
        }

        if (jumping){
            jumpT += Time.deltaTime;
            Vector3 currentPos = Vector3.Lerp(jumpStart, jumpEnd, jumpT / jumpTime);
            currentPos = AdjustPositionToNavMesh(currentPos);
            RatParent.position = currentPos;
        }
    }

    void PlaySound(AudioClip clip){
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    public void getHitted(){
        PlaySound(hittedClip);
        if (!jumping){
            hitted = true;
            CancelInvoke("Unstun");
            agent.ResetPath();
            Invoke("Unstun", 1);
            animator.speed = 1;
            animator.CrossFade("RatHitted", 0.1f, 0, 0);
            if (attacking){
                jumping = false;
                attacking = false;
            }
        }
    }
    void Unstun(){
        hitted = false;
        walkToRandPos();
    }

    void tryAttck(){
        if (Random.value > chanceAttak || attacking || hitted) return;

        PlaySound(attackClips[Random.Range(0, attackClips.Length)]);
        walkSound.Pause();

        attacking = true;
        agent.ResetPath();
        animator.speed = 1;
        animator.CrossFade("RatJump", 0.15f);
    }

    public void Jump_anim(){
        jumpStart = RatParent.position;
        jumpEnd = new Vector3(player.position.x, RatParent.position.y, player.position.z);
        jumpT = 0;
        jumping = true;
    }
    public void EndAttack_anim(){
        RatParent.position = AdjustPositionToNavMesh(jumpEnd);
        jumping = false;
        attacking = false;
        walkToRandPos();
    }

    void walkToRandPos(){
        if (attacking || hitted) return;
        animator.CrossFade("RatWalk", 0.15f);

        for(int i = 0; i < 10; i++){
            Vector3 newPos = RandomNavSphere(RatParent.position, wanderRadius, -1);
            if (IsReachable(newPos))
            {
                agent.SetDestination(newPos);
                break;
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    private bool IsReachable(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    private Vector3 AdjustPositionToNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }

    private bool HasReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
