using Zenject;

namespace Infrastructure.StateMachine.States
{
    public abstract class State
    {
        [Inject] protected StateMachineService StateMachineService;

        public abstract void Enter();

        public virtual void Exit()
        {
            
        }
    }
}