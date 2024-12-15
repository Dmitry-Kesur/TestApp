using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.StateMachine.States;

namespace Infrastructure.Factories
{
    public interface IStatesFactory
    {
        Dictionary<StateType, State> CreateStates();
    }
}