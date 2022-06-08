using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Gameplay
{
    public class CollisionSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var filter = world.GetFilter<ColliderComponent>().Inc<PositionComponent>().End();
            var positions = world.GetComponents<PositionComponent>();
            var colliderComponents = world.GetComponents<ColliderComponent>();

            foreach (var entity in filter)
            {
                var positionComponent = positions.Get(entity);
                ref var colliderComponent = ref colliderComponents.Get(entity);

                colliderComponent.collisionCount = Physics2D.OverlapCircleNonAlloc(positionComponent.value, 
                    colliderComponent.radius, colliderComponent.collisions);

                // could be interesting to have contacts cached too
                // Physics2D.GetContacts()
            }
        }
    }
}