using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class State
    {
        public float time;
    }
    
    public struct StatesComponent : IEntityComponent
    {
        public IDictionary<string, State> states;

        public bool HasState(string stateName)
        {
            return states.ContainsKey(stateName);
        }

        public State GetState(string stateName)
        {
            return states[stateName];
        }

        public void EnterState(string state)
        {
            states[state] = new State
            {
                
            };
        }

        public void ExitState(string state)
        {
            states.Remove(state);
        }
    }
}