using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        
        [SerializeField] private Transform weaponAnchor;
        [SerializeField] private Items.Weapon weapon;

        
        
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
                        var dir = tObj.transform.position - transform.position;
                        if (dir.magnitude < viewDistance
                            && Vector3.Dot(dir.normalized, transform.forward) > viewCos
                            && Physics.Raycast(transform.position, dir.normalized, dir.magnitude))
                        {
                            if (dir.magnitude < dst)
                            {
                                Target = tObj;
                                dst = dir.magnitude;
                            }
                            Debug.DrawRay(transform.position, dir, Color.green, TargetUpdateRate);
                        }
                        else Debug.DrawRay(transform.position, dir, Color.red, TargetUpdateRate);
                    }

                    if (Target == null) return false;
                    targetPosition = Target.transform.position;
                    transform.forward = (targetPosition - transform.position).normalized;
                    actionCompleted = false;
                    // Notify nearest
                    _nearEnemies.Clear();
                    Location.FindNearestBwd(transform.position, notificationRadius, _nearEnemies);
                    foreach (var enemyAI in _nearEnemies)
                    {
                        enemyAI.Target = Target;
                        Debug.DrawLine(transform.position, enemyAI.transform.position, Color.blue, 1);
                        enemyAI.Notify(new HashSet<EnemyAI>(),"rapprochement");
                    }
                    Debug.Log("[AI]:" + name + ": idle -> rapprochement");
                    return true;
                }
            );
            fsm.AddTransitionFromAny("idle",
                transition =>
                {
                    if (Target == null) return actionCompleted;
                    Debug.Log("[AI]:" + name + ": " + transition.from + "-> idle");
                    var dir = Target.transform.position - transform.position;
                    if (dir.magnitude < viewDistance
                        && Vector3.Dot(dir.normalized, transform.forward) > viewCos
                        && Physics.Raycast(transform.position, dir.normalized, dir.magnitude))
                    {
                        Debug.DrawRay(transform.position, dir, Color.green, TargetUpdateRate);
                        targetPosition = Target.transform.position;
                        actionCompleted = false;
                        return false;
                    }
                    Debug.DrawRay(transform.position, dir, Color.red, TargetUpdateRate);
                    return actionCompleted;
                });
            fsm.AddTwoWayTransition(
                "rapprochement",
                "attack",
                _ =>
                {
                    if (Target == null) return false;
                    var b = Vector3.Distance(Target.transform.position, transform.position) < attackDistance;
                    if (b) Debug.Log("[AI]:" + name + ": rapprochement -> attack");
                    else Debug.Log("[AI]:" + name + ": attack -> rapprochement");
                    return b;
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
                Debug.DrawLine(transform.position, enemyAI.transform.position, Color.blue, TargetUpdateRate);
                enemyAI.Notify(notified, type);
            }
        }

        private StateBase<string> AttackState()
        {
            var fsm = new StateMachine();
            
            fsm.AddState("attack", 
                onLogic: _ =>
                {
                    weapon.Action(gameObject, weaponObj);
                    fsm.RequestStateChange("wait");
                });
            fsm.AddState("wait");
            
            fsm.AddTransition(new TransitionAfter("wait", "attack", attackDelay));
            
            fsm.SetStartState("attack");
            return fsm;
        }
        private StateBase<string> IdleState()
        {
            return new State(
                onLogic: _ =>
                {
                    Debug.Log("[AI]:" + name + ": idle");
                });
        }
    }
}