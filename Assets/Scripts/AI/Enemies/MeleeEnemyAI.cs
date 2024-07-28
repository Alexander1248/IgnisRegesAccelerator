using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityHFSM;

namespace AI.Enemies
{
    public class MeleeEnemyAI : EnemyAI
    {
        [SerializeField] private float notificationRadius = 4;
        
        [SerializeField] private float viewAngle = 60;
        [SerializeField] private float viewDistance = 10;
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
            fsm.AddState("attack", AttackState());
            fsm.SetStartState("idle");
            
            fsm.AddTransition(
                "idle",
                "rapprochement",
                _ => {
                    Target = null;
                    var dst = float.MaxValue; 
                    foreach (var tObj in controller.Targets)
                    {
                        var dir = tObj.transform.position - head.transform.position;
                        if (dir.magnitude < viewDistance
                            && Vector3.Dot(dir.normalized, transform.forward) > viewCos
                            && Physics.Raycast(head.transform.position, dir.normalized, dir.magnitude))
                        {
                            if (dir.magnitude < dst)
                            {
                                Target = tObj;
                                dst = dir.magnitude;
                            }
                            Debug.DrawRay(head.transform.position, dir, Color.green, TargetUpdateRate);
                        }
                        else Debug.DrawRay(head.transform.position, dir, Color.red, TargetUpdateRate);
                    }

                    if (Target == null) return false;
                    targetPosition = Target.transform.position;
                    transform.forward = (targetPosition - head.transform.position).normalized;
                    actionCompleted = false;
                    // Notify nearest
                    _nearEnemies.Clear();
                    Location.FindNearestBwd(head.transform.position, notificationRadius, _nearEnemies);
                    foreach (var enemyAI in _nearEnemies)
                    {
                        enemyAI.Target = Target;
                        Debug.DrawLine(head.transform.position, enemyAI.transform.position, Color.blue, 1);
                        enemyAI.Notify(new HashSet<EnemyAI>(),"rapprochement");
                    }
                    Debug.Log("[AI]:" + name + ": idle -> rapprochement");
                    animator.CrossFade("Walking With Shopping Bag", 0.25f, 0, 0);
                    return true;
                }
            );
            fsm.AddTransitionFromAny("idle",
                transition =>
                {
                    if (Target == null) return actionCompleted;
                    var dir = Target.transform.position - head.transform.position;
                    if (dir.magnitude < viewDistance
                        && Vector3.Dot(dir.normalized, transform.forward) > viewCos
                        && Physics.Raycast(head.transform.position, dir.normalized, dir.magnitude))
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
            var oldARState = false;
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
            fsm.AddTriggerTransitionFromAny("rapprochement", "rapprochement");
            return fsm;
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
                    animator.CrossFade("Stable Sword Inward Slash", 0.25f, 0, 0),
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
                condition: _ => agent.remainingDistance <= agent.stoppingDistance);
            fsm.AddTransition(new TransitionAfter("look around", "patrolling", 4.467f));
            
            fsm.SetStartState("patrolling");
            return fsm;
        }


        public void Death()
        {
            FSM.RequestExit(true);
            animator.CrossFade("Dying", 0.25f, 0, 0);
            Invoke(nameof(Destroy), 10f);
        }
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}