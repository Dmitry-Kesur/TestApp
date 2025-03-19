using System.Collections.Generic;
using Infrastructure.Enums;

namespace Infrastructure.Factories.State
{
    public interface IStatesFactory
    {
        Dictionary<StateType, StateMachine.States.State> CreateStates();
    }
}