using System;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace AI
{
    public abstract class EnemyAI : TrackableBehaviour<EnemyAI>
    {
        [SerializeField] protected EnemyCoordinationController controller;
        [SerializeField] protected NavMeshAgent agent;
        private StateMachine fsm;

        [SerializeField] private float targetUpdateRate = 1;
        
        private void Start()
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
        }
    }
}