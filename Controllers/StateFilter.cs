using System.Collections.Generic;
using System.Linq;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StateFilter
    {
        private readonly List<string> withStateList = new List<string>();
        private readonly List<string> withoutStateList = new List<string>();

        public static StateFilter EmptyFilter = new StateFilter();
        
        public StateFilter(string withState, string withoutState = null)
        {
            if (!string.IsNullOrEmpty(withState))
            {
                withStateList.Add(withState);
            }
            
            if (!string.IsNullOrEmpty(withoutState))
            {
                withoutStateList.Add(withoutState);
            }
        }

        public StateFilter(string[] withStates = null, string[] withoutStates = null)
        {
            if (withStates != null)
            {
                withStateList.AddRange(withStates);
            }
            
            if (withoutStates != null)
            {
                withoutStateList.AddRange(withoutStates);
            }
        }
        
        public bool Match(List<string> states)
        {
            return withStateList.All(states.Contains) 
                   && !withoutStateList.Any(states.Contains);
        }

        public override string ToString()
        {
            var withString = "";
            var withoutString = "";

            if (withStateList.Count > 0)
            {
                withString = $"WITH:{string.Join("|", withStateList)}";
            }
            
            if (withoutStateList.Count > 0)
            {
                withoutString = $"WITHOUT:{string.Join("|", withoutStateList)}";
            }
            
            return $"{withString}, {withoutString}";
        }
    }
}