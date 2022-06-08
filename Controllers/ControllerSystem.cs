using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class ControllerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent>> controllerFilter = default;
        readonly EcsPoolInject<ControllerComponent> controllerComponents = default;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in controllerFilter.Value)
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);

                if (!controllerComponent.intialized)
                {
                    // controllerComponent.controller.OnInit(world, entity);
                    controllerComponent.intialized = true;
                }

                foreach (var controller in controllerComponent.controllers)
                {
                    controller.OnUpdate(Time.deltaTime, world, entity);    
                }
            }
        }
    }
}