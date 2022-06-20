using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public abstract class ControllerBase : MonoBehaviour, IController
    {
        protected World world;
        protected Entity entity;

        public void Bind(World world, Entity entity)
        {
            this.world = world;
            this.entity = entity;
        }

        public abstract void OnUpdate(float dt);
    }
}