using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Gameplay
{
    public class AbilitiesSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler
    {
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            if (world.HasComponent<AbilitiesComponent>(entity))
            {
                var abilitiesComponent = world.GetComponent<AbilitiesComponent>(entity);
                foreach (var ability in abilitiesComponent.abilities)
                {
                    if (ability.startType == Ability.StartType.Loaded)
                    {
                        ability.cooldownCurrent = ability.cooldownTotal;
                    }
                    else if (ability.startType == Ability.StartType.Unloaded)
                    {
                        ability.cooldownCurrent = 0;
                    }
                }
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var filter = world.GetFilter<AbilitiesComponent>().End();
            var positions = world.GetComponents<PositionComponent>();
            var lookingDirections = world.GetComponents<LookingDirection>();

            var abilitiesComponents = world.GetComponents<AbilitiesComponent>();

            // update ability position
            foreach (var entity in world.GetFilter<AbilitiesComponent>().Inc<PositionComponent>().End())
            {
                var abilitiesComponent = abilitiesComponents.Get(entity);
                var position = positions.Get(entity);
                foreach (var ability in abilitiesComponent.abilities)
                {
                    // + fire position attach point
                    ability.position = position.value;
                }
            }
            
            // Update ability direction
            foreach (var entity in world.GetFilter<AbilitiesComponent>().Inc<LookingDirection>().End())
            {
                var abilitiesComponent = abilitiesComponents.Get(entity);
                var lookingDirection = lookingDirections.Get(entity);
                foreach (var ability in abilitiesComponent.abilities)
                {
                    ability.direction = lookingDirection.value;
                }
            }

            foreach (var entity in filter)
            {
                ref var abilitiesComponent = ref abilitiesComponents.Get(entity);
                foreach (var ability in abilitiesComponent.abilities)
                {
                    if (!ability.isRunning)
                    {
                        ability.cooldownCurrent += Time.deltaTime;
                    }
                    else
                    {
                        ability.runningTime += Time.deltaTime;
                        ability.isComplete = false;
                    }

                    if (!ability.isComplete && ability.runningTime > ability.duration)
                    {
                        // // fire projectile?
                        //
                        // if (ability.projectileDefinition != null)
                        // {
                        //     var projectileEntity = world.CreateEntity(ability.projectileDefinition);
                        //     ref var projectileComponent = ref world.GetComponent<ProjectileComponent>(projectileEntity);
                        //
                        //     projectileComponent.startPosition = ability.position;
                        //     projectileComponent.startDirection = ability.direction;
                        //
                        //     if (world.HasComponent<PlayerComponent>(projectileEntity))
                        //     {
                        //         var player = world.GetComponent<PlayerComponent>(projectileEntity);
                        //         
                        //     }
                        //     
                        //     // instantiate target effects in projectile
                        //     
                        //     // Copy player to for damage later
                        //     // projectileController.entity.player.player = e.player.player;
                        //     
                        //     // Copy damage
                        //     // projectileEntity.projectile.damage = weaponData.damage + e.attack.extraDamage;
                        //     
                        //     // this was for collisions
                        //     // if (e.player.player == 0)
                        //     // {
                        //     //     projectileController.gameObject.layer = playerProjectilesLayer;
                        //     // }
                        //     // else
                        //     // {
                        //     //     projectileController.gameObject.layer = enemyProjectilesLayer;
                        //     // }
                        // }

                        ability.isComplete = true;
                    }
                }
            }
        }


    }
}