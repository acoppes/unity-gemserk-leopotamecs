﻿using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

public class WeaponSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem, IEcsInitSystem
{
    public void Init(EcsSystems systems)
    {
        world.onEntityCreated += OnEntityCreated;
        world.onEntityDestroyed += OnEntityDestroyed;
    }
    
    private void OnEntityCreated(World world, int entity)
    {
        var weapons = world.GetComponents<Weapon>();
        if (weapons.Has(entity))
        {
            ref var weapon = ref weapons.Get(entity);
            weapon.gameObject = new GameObject(weapon.name);
        }
    }

    private void OnEntityDestroyed(World world, int entity)
    {
        var weapons = world.GetComponents<Weapon>();
        if (weapons.Has(entity))
        {
            ref var weapon = ref weapons.Get(entity);
            Destroy(weapon.gameObject);
            weapon.gameObject = null;
        }
    }

    public void Run(EcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<Weapon>().Exc<ToDestroy>().End();
        var weapons = systems.GetWorld().GetPool<Weapon>();

        // var sceneController = systems.GetShared<SampleSceneController>();
        // Debug.Log(sceneController.test);
        
        foreach (var entity in filter)
        {
            ref var weapon = ref weapons.Get(entity);
            weapon.cooldown -= Time.deltaTime;
            Debug.Log($"{GetType().Name}, FRAME: {Time.frameCount}, {weapon.name}, {weapon.cooldown}");

            if (weapon.cooldown < 0)
            {
                systems.AddComponent(entity, new ToDestroy
                {
                    val = 10,
                    val2 = 20
                });
                // systems.GetWorld().GetPool<ToDestroy>().Add(entity);
                // systems.GetWorld().DelEntity(entity);
            }
        }
    }

}