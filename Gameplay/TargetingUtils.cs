using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Gameplay
{
    public struct TargetingParameters
    {
        // player looking for targets
        public int player;
        
        public Vector2 position;
        
        public float range;

        public HealthComponent.State state;

        public object extra;

        public Func<TargetingParameters, Target, bool> extraValidation;
        public Comparison<Target> sorter;
    }
    
    public static class TargetingUtils
    {
        public static bool FindTargets(World world, TargetingParameters targeting, List<Target> targets)
        {
            var targetComponents = world.GetComponents<TargetComponent>();
            var found = false;

            foreach (var entity in world.GetFilter<TargetComponent>().End())
            {
                var targetComponent = targetComponents.Get(entity);
                var target = targetComponent.target;

                if (ValidateTarget(targeting, target))
                {
                    targets.Add(target);
                    found = true;
                }
            }
            
            // sorting?
            var sorter = targeting.sorter;

            if (sorter == null)
            {
                sorter = (a, b) =>
                {
                    var distanceA = Vector2.Distance(a.position, targeting.position);
                    var distanceB = Vector2.Distance(b.position, targeting.position);

                    if (distanceA > distanceB)
                        return 1;

                    if (distanceA < distanceB)
                        return -1;

                    return 0;
                };
            }
            
            targets.Sort(sorter);
            
            return found;
        }

        public static bool ValidateTarget(TargetingParameters targeting, Target target)
        {
            // TODO: we should add more player options, like alliances, etc
            if (target.player == targeting.player)
            {
                return false;
            }
                
            if (Vector2.Distance(target.position, targeting.position) > targeting.range)
            {
                return false;
            }
            
            if ((targeting.state & target.state) == 0)
            {
                return false;
            }

            if (targeting.extraValidation != null && !targeting.extraValidation(targeting, target))
            {
                return false;
            }

            return true;
        }
        
    }
}