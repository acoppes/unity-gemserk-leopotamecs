using System.Collections.Generic;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public struct ControllerComponent : IEntityComponent
    {
        public List<IController> controllers;
        public bool intialized;

        public bool onConfigurationPending;
        public int configurationVersion;
    }
}