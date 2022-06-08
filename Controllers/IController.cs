namespace Gemserk.Leopotam.Ecs.Controllers
{
    public interface IController
    {
        void OnUpdate(float dt, World world, Entity entity);
    }

    // public interface IAbilityEvent
    // {
    //     public enum EventType
    //     {
    //         Started,
    //         Completed,
    //         Canceled
    //     }
    //     
    //     void OnAbilityEvent(World world, Entity entity, int ability, EventType eventType);
    // }
}