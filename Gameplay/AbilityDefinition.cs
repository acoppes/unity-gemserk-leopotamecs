using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Gameplay
{
    public class AbilityDefinition : MonoBehaviour, IEntityDefinition
    {
        public float cooldown;
        public float duration;

        public GameObject projectileDefinitionPrefab;

        public Ability.StartType startType;

        public void Apply(World world, int entity)
        {
            ref var abilitiesComponent = ref world.GetComponent<AbilitiesComponent>(entity);
        
            IEntityDefinition projectileDefinition = null;

            if (projectileDefinitionPrefab != null)
            {
                projectileDefinition = projectileDefinitionPrefab
                    .GetComponentInChildren<IEntityDefinition>();
            }
            
            abilitiesComponent.abilities.Add(new Ability
            {
                name = gameObject.name,
                duration = duration,
                cooldownTotal = cooldown,
                projectileDefinition = projectileDefinition,
                startType = startType
            });
        }
    }
}