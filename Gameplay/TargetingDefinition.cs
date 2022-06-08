using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Gameplay
{
    public class TargetingDefinition : MonoBehaviour, IEntityDefinition
    {
        public float range;

        public HealthComponent.State state;
    
        public void Apply(World world, int entity)
        {
            ref var abilitiesComponent = ref world.GetComponent<AbilitiesComponent>(entity);
            abilitiesComponent.targetings.Add(new Targeting
            {
                name = gameObject.name,
                parameters = GetTargetingParameters()
            });
        }

        protected virtual TargetingParameters GetTargetingParameters()
        {
            return new TargetingParameters
            {
                range = range,
                state = state
            };
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}