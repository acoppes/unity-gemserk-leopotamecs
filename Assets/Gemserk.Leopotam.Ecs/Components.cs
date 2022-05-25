using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{ 
    public struct LookingDirection : IEntityComponent
    {
        public Vector2 value;
        public bool disableIndicator;
    }
    
    public struct PositionComponent : IEntityComponent
    {
        public Vector2 value;
    }
    
    public struct NameComponent : IEntityComponent
    {
        public string name;
        public bool singleton;
    }
    
    public struct PlayerComponent : IEntityComponent
    {
        public int player;
    }

    public struct Target
    {
        public int entity;
        public int player;
        public Vector2 position;
        
        public object extra;
    }

    // public struct TargetPosition
    // {
    //     public Target target;
    // }

    public struct TargetComponent : IEntityComponent
    {
        public Target target;
    }
    
    public class Ability
    {
        public enum StartType
        {
            Loaded = 0,
            Unloaded = 1
        }
        
        public string name;
        
        public float duration;
        
        public float cooldownTotal;
        public float cooldownCurrent;
        
        public float runningTime;
        
        public bool isReady => isCooldownReady && !isRunning;
        public bool isCooldownReady => cooldownCurrent > cooldownTotal;

        public bool isComplete;

        public bool isRunning;

        public Vector2 position;
        public Vector2 direction;

        public IEntityDefinition projectileDefinition;
        
        public Ability.StartType startType;

        public void StartRunning()
        {
            runningTime = 0;
            isRunning = true;
            // isComplete = false;
        }

        public void Stop()
        {
            cooldownCurrent = 0;
            isRunning = false;
            // isComplete = false;
        }

        public void Cancel()
        {
            isRunning = false;
            // isComplete = false;
        }
    }

    public class Targeting
    {
        public string name;

        public TargetingParameters parameters;

        public List<Target> targets = new List<Target>();
    }
    
    public struct AbilitiesComponent : IEntityComponent
    {
        public List<Ability> abilities;
        public List<Targeting> targetings;

        public Ability GetAbility(string name)
        {
            return abilities.FirstOrDefault(a => a.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
        
        public Targeting GetTargeting(string name)
        {
            return targetings.FirstOrDefault(a => a.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
    
    public struct ProjectileComponent : IEntityComponent
    {
        public Vector2 startPosition;
        public Vector2 startDirection;

        public bool started;
    }
}