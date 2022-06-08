using System.Collections.Generic;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public struct ControllerComponent : IEntityComponent
    {
        public List<IController> controllers;
        public bool intialized;
    }
}