namespace Gemserk.Leopotam.Ecs.Controllers
{
    public interface IController
    {
        void Bind(World world, Entity entity);
        
        void OnUpdate(float dt);
    }

    public interface IInit
    {
        void OnInit();
    }

    public interface IConfigurable
    {
        void OnConfigured();
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