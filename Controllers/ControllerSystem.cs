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
            var configurations = world.GetComponents<ConfigurationComponent>();
            
            foreach (var entity in world.GetFilter<ControllerComponent>().Inc<ConfigurationComponent>().End())
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);
                var configuration = configurations.Get(entity);

                controllerComponent.onConfigurationPending =
                    controllerComponent.configurationVersion != configuration.configuredVersion;
                controllerComponent.configurationVersion = configuration.configuredVersion;
            }

            foreach (var entity in controllerFilter.Value)
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);
                
                foreach (var controller in controllerComponent.controllers)
                {
                    controller.Bind(world, entity);

                    if (!controllerComponent.intialized && controller is IInit init)
                    {
                        init.OnInit();
                    }

                    if (controllerComponent.onConfigurationPending && controller is IConfigurable configurable)
                    {
                        configurable.OnConfigured();
                    }
                    
                    controller.OnUpdate(Time.deltaTime);    
                }
                
                controllerComponent.intialized = true;
                controllerComponent.onConfigurationPending = false;
            }
        }
    }
}