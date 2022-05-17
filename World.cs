﻿using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class World : MonoBehaviour
    {
        public EcsWorld world;

        public object sharedData;
        
        public bool autoInitializationOnStart;
    
        private EcsSystems fixedUpdateSystems, updateSystems, lateUpdateSystems;

        private bool initialized;

        public int NewEntity()
        {
            return world.NewEntity();
        }
        
        public void AddComponent<T>(int entity) where T : struct
        {
            world.GetPool<T>().Add(entity);
        }
        
        public void AddComponent<T>(int entity, T t) where T : struct
        {
            ref var newT = ref world.GetPool<T>().Add(entity);
            newT = t;
        }
        
        public ref T GetComponent<T>(int entity) where T : struct
        {
            return ref world.GetPool<T>().Get(entity);
        }
        
        public new EcsPool<T> GetComponents<T>() where T : struct
        {
            return world.GetPool<T>();
        }
        
        public EcsWorld.Mask GetFilter<T>() where T : struct
        {
            return world.Filter<T>();
        }

        private void Register<T>(EcsSystems ecsSystems) where T: IEcsSystem
        {
            var systems = GetComponentsInChildren<T>();
            foreach (var system in systems)
            {
                if (system is BaseSystem baseSystem)
                {
                    baseSystem.world = this;
                }
                ecsSystems.Add(system);
            }
        }

        private void Start()
        {
            if (autoInitializationOnStart)
            {
                Init();
            }
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
            
            Register<IFixedUpdateSystem>(fixedUpdateSystems);
            Register<IUpdateSystem>(updateSystems);
            Register<ILateUpdateSystem>(lateUpdateSystems);

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

        private void OnDestroy () {
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

        public void OnEntityCreated(int entity)
        {
            // throw new System.NotImplementedException();
        }
    }
}