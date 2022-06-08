using System.Linq;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class ControllerDefinition : MonoBehaviour, IEntityDefinition
    {
        public GameObject controllerObject;
    
        public void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ControllerComponent
            {
                controllers = controllerObject.GetComponentsInChildren<IController>().ToList()
            });
        }
    }
}