## Intro

This is a small project with wrapper utilities built over Leopotam ecs to be used for personal projects like game jams, etc. And in the future, if it useful for someone, to share and collaborate to improve.

### World

World is a wrapper over the Leopotam EcsWorld, and it has a prefab with predefined systems in there. As this is our Unity adaptation of the ecs, we treat systems as game objects with a BaseSystem MonoBehaviours.

World contains three objects, FixedUpdate, Update and LateUpdate, systems inside those objects are updated using the corresponding Untiy update type. 

### Entity Definitions & instance parameters

Entity definitions are like a Prototype, Template or Archetype concepts. It provides a customizable way of configuring an entity (add components with data) after instantiation. They are normally implemented in stateless prefabs.

Entity instance is a GameObject meant to instantiate an entity and apply a definition. It allows customizable instance configurations by using the instance parameters, applied after the entity was instantiated and definition was applied (so the components might be already there). They can override data or even add components.  

World wrapper has some methods to create entities by using a definition and parameters.

Entities spawned using the definition and parameters have a component named EntityDefinitionComponent with a reference to those values used to instantiate the entity. It could be used to debug and/or to clone entities (by spawning another with the same values). 

### Named entities & Singletons

Entities can use the NameComponent to identify them by name, to be used in any way by the game but they can also be marked as singleton which make them unique so there can't be two entities with the same name marked as singletons. Singleton entities are cached in shared object dictionary and can be accessed by name from there. 

### Entity wrapper

It is just a way to have references to entities but with a custom struct which could be extended in the future, for now it is just an int and a static declaration of NullEntity (where null is the entity -1).

## Behaviours

### Controllers

Controllers are a basic way of adding custom behaviors to entities, they receive an OnUpdate method with the delta time, world and entity to act on. They are stateless and are normally implemented in prefabs (no instantiation) and reused among multiple entities using the same controller.

### States

States are just a list of states the entity might be on right now, to be used for example for a stun. For now they are just strings in a list and they can be added for some time. A bit basic right now but useful anyway.

### Target & Targeting

It provides an extensible base to have targets and be able to target them matching some targeting filters.

## Roadmap

* ~Separate in two projects, core and extras.~
* ~Read from IEntityDefinition if not PrefabEntityDefiniton in prefab.~
* ~Change API to use Entity instead of int.~
* World simplified Filter<T1, T2, T3>
* ~Base system should be IEcsSystem~

* Systems ordering
  - ~~For now, just the gameobjects ordering.~~
  - Could add attributes for update after/before like Unity ecs 

* Delayed creation /destruction?
  - Maybe have a way to lightweight create, like Flyweight pattern, quickly create the object but initialize later on real usage. If entity destroyed before, then it was never initialized.
  - For destruction, we could mark the entity with special component and exclude it in systems if necessary, and have a System running for last to complete destroy.

* Tuples
  - ~~Dependency Injection of tuples? (there is an extension for that, have to test it yet)~~
  - A concept between the filter and the pools? When we programmed Clash of the Olympians we had something like that. 
  - Also, could be super useful for Controllers logic.

* Entity Queries?
    - This is for quickly searching entities matching some criteria, but not only having or not a component but more like checking if some specific value conditions apply. 

* Database join?

* ~~Spawn entities while processing?~~
    - For example, for firing bullets or for spawning enemies.

* Controllers
  - A way to create custom specific logic for entities.
  - Would be great to have an extensible way of adding custom events, for example, OnDamage(), game could add a system to call that event on all controllers from that entity implementing the OnDamaged interface.

* Add or Remove entity during update?
  - Not sure if common, normally we would delegate the destruction to another system, like mark it has no more health or the effect is completed and then there is a system to process that.

* ~~Maybe use gameobjects to identify fixed update, update and late update systems and remove that from code.~~

* ~Improve PrefabInstanceParameters system, it is using FindObjectsOfType right now and that sucks in terms of performance.~

* Delayed or triggerable instantiation of PrefabInstance objects. 
    - Right now they are auto instantiated when the world starts but could be useful to have them there but activate/instantiate during gameplay.

* Inject controllers with components?

* Abilities system? (duration, cooldown, is active)

* Coordinates? 2d, 3d or customizable
  - One idea is to have a way to select which type to use, like 2d isometric or 2d or 3d, and always use 3d coordinates but in a proper way depending the type of coordinates system. 
  - Another idea is to have like a static conversion type functions (from world and to world) and register that in a static place and all the engine uses that in its functions. The idea is to treat everything as 3d inside the engine but there is a way to convert from/to the game coordinates, if isometric game, then maybe treat z coordinate as the height for example. Maybe provide with the engine some convert functions as examples.

* Target Effects
  - A custom way of performing different logics for projectiles. Effects are entities.
  - It could be an ability or a controller/behaviour attached to the projectile to be performed on collision detection (on impact), while traveling (on tick), when destination reached (on destination) or on time to live expired (on ttl).  For example:

```
class DamageEffect : IController {
  
  float damage;

  onImpact() {
    if (impact.target) {
      impact.target.health.Add(damage);
    }
  }
  
}
```

  - The workflow would be something like creating the projectile with other effect entities attached already or maybe attaching them in our custom behaviour spawning the projectile. For example:

```
projectile = createProjectile();
effect1 = createEffect1();
effect1.damage = 1;
projectile.childs.attach(effect1);
```

* Create "disabled" entities to be activated later
  - The idea behind this is to be able to create an entity, configure it but keep it disabled for future activation (for example, just a delay or maybe for firing projectiles with some effects to be activated later).

* How to not process entities in some state (like being death)? or to optionally incorporate that in core systems? For example the abilities could process or not while death, but should be an option for that, but that means the concept should be integrated in core.
  - There are some core states that could be interesting for all systems, like being spawning or death even though they sound like specific game concepts.
  - It could be delegated to game specific controllers, like onDeath() then disable canMove, etc, but might not escalate well if it is too common more like a feature.
  - Another way is to delegate to game specific systems, like before but more or less scalable, like having a system to disable movement while death (if entity defined or configured as cant move while death).
  - In the case of abilities, they could declare something similar to a statesMask and make it a bit more generic when to not execute. They start to sound similar to state controllers (controllers that execute only while some states).

## Examples

TODO: 

* Clone entity by using the definition and parameters.
* Targeting

### SpawnEntitiesExample scene

Shows how to dynamically create entities while iterating in a System and to configure stuff. In this case it is a weapon with a cooldown spawning bullets each time that cooldown is ready (no visual stuff)

### ReferenceToGameObjects scene

Shows how to dynamically create and destroy a Unity GameObject and keep data in sync. It is normally useful in the case of having to use a GameObject to use Unity stuff like SpriteRenderer, etc.

### SingletonEntitiesExample

It shows having multiple entities with the same name but not marked as singletons and then it has two entities marked as singletons which will fail on construction time.