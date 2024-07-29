using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityHFSM;

namespace AI.Enemies
{
    public class MeleeEnemyAI : EnemyAI
    {
        [SerializeField] private float notificationRadius = 4;
        
        [SerializeField] private float viewAngle = 60;
        [SerializeField] private float viewDistance = 10;
        [SerializeField] private float sensitivityDistance = 3;
        [SerializeField] private bool retranslateNotification = true;
        [Space]
        [SerializeField] private float attackDistance = 1;
        [SerializeField] private float attackDelay = 1;
        [SerializeField] private float attackDamage = 10;
        
        [FormerlySerializedAs("eyes")] 
        [SerializeField] private Transform head;
        [SerializeField] private Transform weaponAnchor;
        [SerializeField] private Items.Weapon weapon;

        [SerializeField] private Animator animator;

        
        [SerializeField] private Transform[] patrollingPath;
        [SerializeField] private float patrollingMinTime;
        [SerializeField] private float patrollingMaxTime;

        [SerializeField] private float attentionTime = 30;
        [SerializeField] private float attentionMovementStepRadius = 10;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip[] notifySounds;

        private int patrollingIndex;
        private bool actionCompleted;
        private float viewCos;
        private Vector3 targetPosition;
        private GameObject weaponObj;

        private readonly List<EnemyAI> _nearEnemies = new();

        private void Awake()
        {
            viewCos = Mathf.Cos(viewAngle * Mathf.Deg2Rad);
            
            if (weapon == null) return;
            weapon = Instantiate(weapon);
            weaponObj = Instantiate(weapon.Prefab, weaponAnchor);
            weapon.OnEquip(gameObject, weaponObj);
            weaponObj.GetComponent<SwordHelper>().canIDamagePlayer = true;
            weaponObj.SetActive(false);
        }
        
        protected override StateMachine InitStateMachine()
        {
            var fsm = new StateMachine();
            fsm.AddState("idle", IdleState());
            fsm.AddState(
                "rapprochement",
                onLogic: _ =>
                {
                    Debug.Log("[AI]:" + name + ": rapprochement");
                    agent.enabled = true;
                    agent.SetDestination(targetPosition);
                    actionCompleted = agent.remainingDistance <= agent.stoppingDistance;
                });
            fsm.AddState(
                "searching",
                onLogic: _ =>
                {
                    Debug.Log("[AI]:" + name + ": searching");
                    agent.enabled = true;
                    agent.SetDestination(targetPosition);
                    actionCompleted = false;
                    if (agent.remainingDistance <= agent.stoppingDistance)
                        targetPosition = Target.transform.position;
                });
            fsm.AddState("attack", AttackState());
            fsm.AddState("attention", AttentionState());
            fsm.AddState("Withdrawing Sword");
            fsm.AddState("Sheathing Sword");
            
            fsm.AddTransition("idle", "Withdrawing Sword", _ => CheckView(ref Target),
                afterTransition: _=>
                {
                    weaponObj.SetActive(true);
                    animator.CrossFade("Withdrawing Sword", 0.25f, 0, 0);
                });
            fsm.AddTransition(new TransitionAfter("Withdrawing Sword", "rapprochement", 1.5f, 
                afterTransition: _ => animator.CrossFade("Walking With Shopping Bag", 0.25f, 0, 0)));
            
            fsm.AddTransition("attention", "rapprochement", _ =>
            {
                GameObject target = null;
                var b = CheckView(ref target);
                if (!b) return false;
                Target = target;
                animator.CrossFade("Walking With Shopping Bag", 0.25f, 0, 0);
                return true;

            });
            fsm.AddTransition("rapprochement", "searching",
                transition =>
                {
                    if (Target == null) return false;
                    var dir = Target.transform.position - head.transform.position;
                    RaycastHit hit;
                    if (dir.magnitude < viewDistance
                        && Vector3.Dot(dir.normalized, head.transform.forward) > viewCos
                        && Physics.Raycast(head.transform.position, dir.normalized, out hit, dir.magnitude))
                    {
                        var contains = false;
                        var t = hit.collider.transform;
                        while (t != null)
                        {
                            if (controller.Targets.Contains(t.gameObject))
                            {
                                contains = true;
                                break;
                            }
                            t = t.parent;
                        }
                        if (contains){
                            Debug.DrawRay(head.transform.position, dir, Color.green, TargetUpdateRate);
                            targetPosition = Target.transform.position;
                            actionCompleted = false;
                            return false;
                        }
                    }
                    if (dir.magnitude < sensitivityDistance)
                    {
                        Debug.DrawRay(head.transform.position, dir, Color.green, TargetUpdateRate);
                        targetPosition = Target.transform.position;
                        actionCompleted = false;
                        return false;
                    }
                    Debug.DrawRay(head.transform.position, dir, Color.red, TargetUpdateRate);
                    Debug.Log("[AI]:" + name + ": " + transition.from + "-> idle");
                    return actionCompleted;
                });
            fsm.AddTransition(new TransitionAfter("searching", "attention", 1));
            fsm.AddTransition(new TransitionAfter("attention", "Sheathing Sword", attentionTime, 
                afterTransition: _ =>
                {
                    Target = null;
                    animator.CrossFade("Sheathing Sword", 0.25f, 0, 0);
                    
                }));
            
            fsm.AddTransition(new TransitionAfter("Sheathing Sword", "idle", 1.5f, 
                afterTransition: _ => weaponObj.SetActive(false)));
            
            fsm.AddTransition(
                "rapprochement",
                "attack",
                _ =>
                {
                    if (Target == null) return false;
                    var b = Vector3.Distance(Target.transform.position, head.transform.position) > attackDistance;
                    if (b) return false;
                    Debug.Log("[AI]:" + name + ": rapprochement -> attack");
                    agent.enabled = false;
                    return true;
                });
            fsm.AddTransition(
                "attack",
                "rapprochement",
                _ =>
                {
                    if (Target == null) return false;
                    var b = Vector3.Distance(Target.transform.position, head.transform.position) < attackDistance;
                    if (b) return false;
                    Debug.Log("[AI]:" + name + ": attack -> rapprochement");
                    animator.CrossFade("Walking With Shopping Bag", 0.25f, 0, 0);
                    return true;
                });
            fsm.AddTriggerTransitionFromAny("rapprochement", "Withdrawing Sword");
            
            
            fsm.SetStartState("idle");
            return fsm;
        }

        private bool CheckView(ref GameObject target)
        {
            target = null;
            var dst = float.MaxValue; 
            foreach (var tObj in controller.Targets)
            {
                var dir = tObj.transform.position - head.transform.position;
                if (dir.magnitude < viewDistance
                    && Vector3.Dot(dir.normalized, head.transform.forward) > viewCos
                    && Physics.Raycast(head.transform.position, dir.normalized, out var hit, dir.magnitude))
                {
                    var contains = false;
                    var t = hit.collider.transform;
                    while (t != null)
                    {
                        if (controller.Targets.Contains(t.gameObject))
                        {
                            contains = true;
                            break;
                        }
                        t = t.parent;
                    }
                    if (contains && dir.magnitude < dst)
                    {
                        target = tObj;
                        dst = dir.magnitude;
                    }
                    Debug.DrawRay(head.transform.position, dir, Color.green, TargetUpdateRate);
                }
                else if (dir.magnitude < sensitivityDistance)
                {
                    if (dir.magnitude < dst)
                    {
                        target = tObj;
                        dst = dir.magnitude;
                    }
                    Debug.DrawRay(head.transform.position, dir, Color.green, TargetUpdateRate);
                }
                else Debug.DrawRay(head.transform.position, dir, Color.red, TargetUpdateRate);
            }

            if (target == null) return false;
            targetPosition = target.transform.position;
            actionCompleted = false;
            // Notify nearest
            if (source)
            {
                source.clip = notifySounds[Random.Range(0, notifySounds.Length)];
                source.Play();
            }
            _nearEnemies.Clear();
            Location.FindNearestBwd(head.transform.position, notificationRadius, _nearEnemies);
            foreach (var enemyAI in _nearEnemies)
            {
                enemyAI.Target = target;
                Debug.DrawLine(head.transform.position, enemyAI.transform.position, Color.blue, 1);
                enemyAI.Notify(new HashSet<EnemyAI>(),"rapprochement");
            }
            Debug.Log("[AI]:" + name + ": idle -> rapprochement");
            return true;
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
        
        public override void Notify(HashSet<EnemyAI> notified, string type)
        {
            notified.Add(this);
            switch (type)
            {
                case "rapprochement":
                    actionCompleted = false;
                    targetPosition = Target.transform.position;
                    break;
            }
            FSM.TriggerLocally(type);
            if (!retranslateNotification) return;
            
            _nearEnemies.Clear();
            Location.FindNearestBwd(transform.position, notificationRadius, _nearEnemies);
            foreach (var enemyAI in _nearEnemies.Where(enemyAI => !notified.Contains(enemyAI)))
            {
                enemyAI.Target = Target;
                Debug.DrawLine(head.transform.position, enemyAI.transform.position, Color.blue, TargetUpdateRate);
                enemyAI.Notify(notified, type);
            }
        }

        private StateBase<string> AttackState()
        {
            var fsm = new StateMachine();

            fsm.AddState("attack",
                onEnter: _ =>
                {
                    weaponObj.SetActive(true);
                    animator.CrossFade("Stable Sword Inward Slash", 0.25f, 0, 0)
                },
                onLogic: _ =>
                {
                    Debug.Log("[AI]:" + name + ": attack");
                    weapon.Action(gameObject, weaponObj);
                });
            
            fsm.AddState("wait",
                onEnter: _ => animator.CrossFade("Stable Sword Idle", 0.25f, 0, 0),
                onLogic: _ => Debug.Log("[AI]:" + name + ": wait"));
            fsm.AddTransition(new TransitionAfter("wait", "attack", attackDelay * 0.5f));
            fsm.AddTransition(new TransitionAfter("attack", "wait", attackDelay * 0.5f));
            
            fsm.SetStartState("attack");
            return fsm;
        }
        
        private StateBase<string> AttentionState()
        {
            var fsm = new StateMachine();
            
            fsm.AddState("searching target",
                onEnter: _ =>
                {
                    animator.CrossFade("Walking With Shopping Bag", 0.25f, 0, 0);
                    agent.enabled = true;
                    actionCompleted = false;
                    if (Random.value > 0.2 || Target == null)
                    {
                        for (var i = 0; i < 10; i++)
                        {
                            var newPos = RandomNavSphere(transform.position, attentionMovementStepRadius, -1);
                            if (!IsReachable(newPos)) continue;
                            agent.SetDestination(newPos);
                            break;
                        }
                    }
                    else
                    {
                        var path = new NavMeshPath();
                        agent.CalculatePath(Target.transform.position, path);
                        var remained = attentionMovementStepRadius;
                        var prev = path.corners[0];
                        for (var i = 1; i < path.corners.Length; i++)
                        {
                            var dst = Vector3.Distance(prev, path.corners[i]);
                            if (dst < remained) remained -= dst;
                            else
                            {
                                agent.SetDestination(Vector3.Lerp(prev, path.corners[i], remained / dst));
                                break;
                            }

                            prev = path.corners[i];
                        }
                    }

                    Debug.Log("[AI]:" + name + ": searching target");
                });
            fsm.AddState("look around",
                onEnter: _ =>
                {
                    Debug.Log("[AI]:" + name + ": look around");
                    agent.enabled = false;
                    animator.CrossFade("Look Around", 0.25f, 0, 0);
                });
            fsm.AddTransition(
                "searching target", 
                "look around",
                condition: _ => agent.remainingDistance <= agent.stoppingDistance);
            fsm.AddTransition(new TransitionAfter("look around", "searching target", 4.467f));
            
            fsm.SetStartState("look around");
            return fsm;
        }
        private StateBase<string> IdleState()
        {
            var fsm = new StateMachine();
            
            fsm.AddState("patrolling",
                onEnter: _ =>
                {
                    if (patrollingPath.Length <= 0) return;
                    patrollingIndex++;
                    if (patrollingIndex >= patrollingPath.Length) 
                        patrollingIndex -= patrollingPath.Length;
                    targetPosition = patrollingPath[patrollingIndex].position;
                    animator.CrossFade("Walking", 0.25f, 0, 0);
                    agent.enabled = true;
                    agent.SetDestination(targetPosition);
                    Debug.Log("[AI]:" + name + ": patrolling");
                });
            fsm.AddState("look around",
                onEnter: _ =>
                {
                    Debug.Log("[AI]:" + name + ": look around");
                    agent.enabled = false;
                    animator.CrossFade("Look Around", 0.25f, 0, 0);
                });
            fsm.AddTransition(
                "patrolling", 
                "look around",
                condition: _ => patrollingPath.Length <= 0 || agent.remainingDistance <= agent.stoppingDistance);
            fsm.AddTransition(new TransitionAfter("look around", "patrolling", 4.467f));
            
            fsm.SetStartState("patrolling");
            return fsm;
        }


        public void Death()
        {
            // FSM.RequestExit(true);
            // animator.CrossFade("Dying", 0.25f, 0, 0);
            //Invoke(nameof(Destroy), 1f);
            
            Destroy(gameObject);
        }
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}