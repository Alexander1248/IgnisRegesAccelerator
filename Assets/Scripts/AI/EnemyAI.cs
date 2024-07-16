using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace AI
{
    public abstract class EnemyAI : TrackableBehaviour<EnemyAI>
    {
        [SerializeField] protected EnemyCoordinationController controller;
        [SerializeField] protected NavMeshAgent agent;
        public GameObject Target;
        private StateMachine fsm;
        protected StateMachine FSM => fsm;

        [SerializeField] private float targetUpdateRate = 1;

        public float TargetUpdateRate => targetUpdateRate;
        
        protected virtual void Start()
        {
            controller.Add(this);
            fsm = InitStateMachine();
            fsm.Init();
            
            InvokeRepeating(nameof(UpdateNavigationTarget), 0, targetUpdateRate);
        }

        protected abstract StateMachine InitStateMachine();

        private void UpdateNavigationTarget()
        {
            fsm.OnLogic();
            Debug.DrawRay(transform.position, transform.forward * 3, Color.yellow, TargetUpdateRate);
        }

        public abstract void Notify(HashSet<EnemyAI> enemyAis, string type);

    }
}