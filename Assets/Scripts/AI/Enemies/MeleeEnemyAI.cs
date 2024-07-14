using UnityHFSM;

namespace AI.Enemies
{
    public class MeleeEnemyAI : EnemyAI
    {
        protected override StateMachine InitStateMachine()
        {
            var fsm = new StateMachine();
            fsm.AddState(
                "idle",
                state => { },
                state => { },
                state => { });
            
            fsm.AddState(
                "patrolling",
                state => { },
                state => { },
                state => { });
            
            fsm.AddState(
                "rapprochement",
                state => { },
                state => { },
                state => { });
            fsm.AddState(
                "attack",
                state => { },
                state => { },
                state => { });
            fsm.AddState(
                "retreat",
                state => { },
                state => { },
                state => { });
            
            
            
            fsm.SetStartState("idle");
            return fsm;
        }
    }
}