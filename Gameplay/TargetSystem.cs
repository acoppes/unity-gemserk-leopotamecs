using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs.Gameplay
{
    public class TargetSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var targets = world.GetComponents<TargetComponent>();
            var positions = world.GetComponents<PositionComponent>();
            var players = world.GetComponents<PlayerComponent>();
            var healthComponents = world.GetComponents<HealthComponent>();
            
            foreach (var entity in world.GetFilter<TargetComponent>().Inc<PositionComponent>().End())
            {
                ref var targetComponent = ref targets.Get(entity);
                var positionComponent = positions.Get(entity);

                ref var target = ref targetComponent.target;
                target.entity = entity;
                target.position = positionComponent.value;
            }
            
            foreach (var entity in world.GetFilter<TargetComponent>().Inc<PlayerComponent>().End())
            {
                ref var targetComponent = ref targets.Get(entity);
                var playerComponent = players.Get(entity);

                ref var target = ref targetComponent.target;
                target.entity = entity;
                target.player = playerComponent.player;
            }
            
            foreach (var entity in world.GetFilter<TargetComponent>().Inc<HealthComponent>().End())
            {
                ref var targetComponent = ref targets.Get(entity);
                var healthComponent = healthComponents.Get(entity);

                ref var target = ref targetComponent.target;
                target.entity = entity;
                target.state = healthComponent.state;
            }
        }
    }
}