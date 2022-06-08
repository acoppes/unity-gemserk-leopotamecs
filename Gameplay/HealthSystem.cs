using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Gameplay
{
    public class HealthSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler
    {
        public void OnEntityCreated(World world, int entity)
        {
            if (world.HasComponent<HealthComponent>(entity))
            {
                ref var healthComponent = ref world.GetComponent<HealthComponent>(entity);
                healthComponent.pendingDamages = new List<Damage>();
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var healthComponents = world.GetComponents<HealthComponent>();
            
            foreach (var entity in world.GetFilter<HealthComponent>().End())
            {
                ref var healthComponent = ref healthComponents.Get(entity);
                
                foreach (var damage in healthComponent.pendingDamages)
                {
                    healthComponent.current = Mathf.Clamp(healthComponent.current- damage.value, 
                        0, healthComponent.total);
                }
                
                healthComponent.pendingDamages.Clear();

                if (healthComponent.deathRequest)
                {
                    healthComponent.current = 0;
                }

                if (healthComponent.state == HealthComponent.State.Death)
                {
                    if (healthComponent.autoDestroyOnDeath)
                    {
                        world.DestroyEntity(entity);
                    }
                }
            }
        }


    }
}