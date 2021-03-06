using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityCreatedHandler
    {
        void OnEntityCreated(World world, Entity entity);
    }

    public interface IEntityDestroyedHandler
    {
        void OnEntityDestroyed(World world, Entity entity);
    }
    
    public class World : SingletonBehaviour<World>
    {
        [SerializeField]
        private Transform fixedUpdateParent, updateParent, lateUpdateParent;
        
        private EcsWorld world;

        public readonly WorldSharedData sharedData = new WorldSharedData();

        private EcsSystems fixedUpdateSystems, updateSystems, lateUpdateSystems;

        private bool initialized;

        // public Action<World, int> onEntityCreated, onEntityDestroyed;

        private readonly IList<IEntityCreatedHandler> entityCreatedHandlers = new List<IEntityCreatedHandler>();
        private readonly IList<IEntityDestroyedHandler> entityDestroyedHandlers = new List<IEntityDestroyedHandler>();

        public Entity CreateEntity(IEntityDefinition definition, IEnumerable<IEntityInstanceParameter> parametersList = null)
        {
            var entity = world.NewEntity();
            
            AddComponent(entity, new EntityDefinitionComponent
            {
                definition = definition,
                parameters = parametersList
            });
            
            definition.Apply(this, entity);
            
            if (parametersList != null)
            {
                foreach (var parameters in parametersList)
                {
                    parameters.Apply(this, entity);
                }
            }
            
            OnEntityCreated(entity);
            
            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            OnEntityDestroyed(entity);
            world.DelEntity(entity);
        }

        public void AddComponent<T>(Entity entity) where T : struct
        {
            world.GetPool<T>().Add(entity);
        }
        
        public void AddComponent<T>(Entity entity, T t) where T : struct
        {
            ref var newT = ref world.GetPool<T>().Add(entity);
            newT = t;
        }
        
        public ref T GetComponent<T>(Entity entity) where T : struct
        {
            return ref world.GetPool<T>().Get(entity);
        }
        
        public bool HasComponent<T>(Entity entity) where T : struct
        {
            return world.GetPool<T>().Has(entity);
        }
        
        public new EcsPool<T> GetComponents<T>() where T : struct
        {
            return world.GetPool<T>();
        }
        
        public EcsWorld.Mask GetFilter<T>() where T : struct
        {
            return world.Filter<T>();
        }

        public EcsFilter Filter<T1>() where T1 : struct
        {
            return world.Filter<T1>().End();
        }
        
        public EcsFilter Filter<T1, T2>() 
            where T1 : struct 
            where T2 : struct
        {
            return world.Filter<T1>().Inc<T2>().End();
        }
        
        public EcsFilter Filter<T1, T2, T3>() 
            where T1 : struct 
            where T2 : struct
            where T3 : struct
        {
            return world.Filter<T1>().Inc<T2>().Inc<T3>().End();
        }
        
        public EcsFilter Filter<T1, T2, T3, T4>() 
            where T1 : struct 
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            return world.Filter<T1>().Inc<T2>().Inc<T3>().Inc<T4>().End();
        }

        private void Register(Component systemsParent, EcsSystems ecsSystems)
        {
            var systems = systemsParent.GetComponentsInChildren<IEcsSystem>();
            foreach (var system in systems)
            {
                if (system is BaseSystem baseSystem)
                {
                    baseSystem.world = this;
                }
                ecsSystems.Add(system);

                if (system is IEntityCreatedHandler entityCreatedHandler)
                {
                    entityCreatedHandlers.Add(entityCreatedHandler);
                }

                if (system is IEntityDestroyedHandler entityDestroyedHandler)
                {
                    entityDestroyedHandlers.Add(entityDestroyedHandler);
                }
            }
        }

        private void Awake()
        {
            Init();
        }

        public void Init() {
            
            if (initialized)
            {
                return;
            }
            
            world = new EcsWorld ();

            fixedUpdateSystems = new EcsSystems(world, sharedData);
            updateSystems = new EcsSystems(world, sharedData);
            lateUpdateSystems = new EcsSystems(world, sharedData);
            
            Register(fixedUpdateParent, fixedUpdateSystems);
            Register(updateParent, updateSystems);
            Register(lateUpdateParent, lateUpdateSystems);
            
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                updateSystems.Add(new EcsWorldDebugSystem());
#endif

            fixedUpdateSystems.Inject();
            updateSystems.Inject();
            lateUpdateSystems.Inject();
            
            fixedUpdateSystems.Init ();
            updateSystems.Init();
            lateUpdateSystems.Init();

            initialized = true;
        }
    
        private void FixedUpdate()
        {
            // time.deltaTime = UnityEngine.Time.deltaTime;
            fixedUpdateSystems.Run ();
        }

        private void Update () {
            // time.deltaTime = UnityEngine.Time.deltaTime;
            updateSystems.Run ();
        }

        private void LateUpdate()
        {
            // time.deltaTime = UnityEngine.Time.deltaTime;
            lateUpdateSystems.Run ();
        }

        protected override void OnDestroy () {
            base.OnDestroy();
            
            if (fixedUpdateSystems != null) {
                fixedUpdateSystems.Destroy ();
                fixedUpdateSystems = null;
            }
        
            if (updateSystems != null) {
                updateSystems.Destroy ();
                updateSystems = null;
            }
        
            if (lateUpdateSystems != null) {
                lateUpdateSystems.Destroy ();
                lateUpdateSystems = null;
            }
        
            // destroy world.
            if (world != null) {
                world.Destroy ();
                world = null;
            }
        }

        private void OnEntityCreated(int entity)
        {
            foreach (var entityCreatedHandler in entityCreatedHandlers)
            {
                entityCreatedHandler.OnEntityCreated(this, entity);
            }
            // onEntityCreated?.Invoke(this, entity);
        }
        
        private void OnEntityDestroyed(int entity)
        {
            foreach (var entityDestroyedHandler in entityDestroyedHandlers)
            {
                entityDestroyedHandler.OnEntityDestroyed(this, entity);
            }
            // onEntityDestroyed?.Invoke(this, entity);
        }

        public Entity GetEntityByName(string entityName)
        {
            
            if (sharedData.singletonByNameEntities.ContainsKey(entityName))
            {
                return sharedData.singletonByNameEntities[entityName];
            }

            return Entity.NullEntity;
        }
    }
}
