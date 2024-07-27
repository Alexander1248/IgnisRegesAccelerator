using System.Collections;
using System.Collections.Generic;
using Math;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class Rat : MonoBehaviour
{
    [SerializeField] private Transform RatParent;
    [SerializeField] float wanderRadius = 10f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float speedTransferKoeff;

    [SerializeField] private float chanceAttak;
    [SerializeField] private float radiusAttack;
    [Space]
    [SerializeField] private CatmullRomSpline damageCurve;
    [SerializeField] private float damage;
    [SerializeField] private float damageCurveThickness;
    [SerializeField] private float damageDelay;
    
    private RaycastHit[] hits;
    private HashSet<GameObject> objects;
    private bool canDamage = true;
    private bool iSeePlayer;
    private bool attacking;
    private Transform player;

    [SerializeField] private float jumpTime;
    private bool jumping;
    private Vector3 jumpStart;
    private Vector3 jumpEnd;
    private float jumpT;


    void Start(){
        player = GameObject.Find("GamePlayer").transform;
        walkToRandPos();
        objects = new HashSet<GameObject>();
        hits = new RaycastHit[2];
    }

    void Update(){
        if (!attacking) animator.speed = agent.velocity.magnitude * speedTransferKoeff;
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

    public void Reload()
    {
        canDamage = true;
    }

    void tryAttck(){
        if (Random.value > chanceAttak || attacking) return;

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
        
        if (!canDamage) return;
        objects.Clear();
        for (var i = 0; i < damageCurve.Count; i++)
        {
            var previousPoint = damageCurve.Get(i - 1);
            for (var j = 1; j <= damageCurve.Segments; j++)
            {
                var t = j / (float)damageCurve.Segments;
                var point = damageCurve.Compute(i, t);
                var dir = point - previousPoint;
                var count = Physics.SphereCastNonAlloc(
                    previousPoint,
                    damageCurveThickness,
                    dir.normalized,
                    hits,
                    dir.magnitude);
                for (var c = 0; c <= count; c++)
                {
                    if (hits[c].collider == null) continue;
                    objects.Add(hits[c].collider.gameObject);
                }

                previousPoint = point;
            }
        }

        foreach (var obj in objects)
        {
            Debug.Log("[Rat]:" + obj.name + " hit!");
            if (obj.TryGetComponent(out Health health))
                health.DealDamage(damage, transform.forward, 0);

            if (obj.TryGetComponent(out HealthUpdater updater))
                updater.DealDamage(damage, transform.forward, 0);
            canDamage = false;
            Invoke(nameof(Reload), damageDelay);
        }
    }
    public void EndAttack_anim(){
        RatParent.position = AdjustPositionToNavMesh(jumpEnd);
        jumping = false;
        attacking = false;
        walkToRandPos();
    }

    void walkToRandPos(){
        if (attacking) return;
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
