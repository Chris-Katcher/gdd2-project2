/************************************************
 * Entity.cs
 * 
 * This file contains implementation for the Entity class,
 * EntityType enum, and EntityFactory class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System.Collections;
using System.Collections.Generic;
using Arcana;
using UnityEngine;
using Arcana.Entities.Attributes;

namespace Arcana.Entities
{

    #region // Enum: EntityType.

    /////////////////////
    // Enum declaration for entity types.
    /////////////////////

    /// <summary>
    /// Entity type should reflect all its different implementations, 
    /// so it can be easily referenced.
    /// </summary>
    public enum EntityType
    {
        NULL,
        Entity,
        Projectile,
        Player,
        Platform,
        Wall
    }

    #endregion

    #region // Class: EntityFactory class.

    /////////////////////
    // Factory Declaration.
    /////////////////////

    /// <summary>
    /// The Entity factory will create an entity component and add it to the appropriate game object.
    /// </summary>
    public class EntityFactory : IFactory<Entity>
    {

        #region // Static Members.

        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static EntityFactory instance = null;

        /// <summary>
        /// Get reference to EntityFactory.
        /// </summary>
        /// <returns>Returns a single factory.</returns>
        public static EntityFactory Instance()
        {
            if (instance == null)
            {
                instance = new EntityFactory();
            }

            return instance;
        }

        /// <summary>
        /// Return the instance of the factory.
        /// </summary>
        /// <returns>Returns a single factory.</returns>
        public IFactory<Entity> GetInstance()
        {
            return Instance();
        }

        #endregion

        #region // Service Methods. 

        /// <summary>
        /// Creates a new, empty game object, and returns the Entity component back.
        /// </summary>
        /// <returns></returns>
        public Entity CreateComponent()
        {
            // Creates a component using the default settings.
            return CreateComponent(Services.CreateEmptyObject("Entity"), CreateSettings());
        }

        /// <summary>
        /// Adds a new component to the parent game object, with parameters.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <param name="parameters">Settings to apply to the new Entity.</param>
        /// <returns>Return newly created component.</returns>
        public Entity CreateComponent(GameObject parent, Constraints parameters)
        {
            // Check game object.
            if (parent == null)
            {
                // If the parent itself is null, do not return a component.
                Debugger.Print("Tried to add a Arcanas.Cameras.Camera component but parent GameObject is null.", "NULL_REFERENCE");
                return null;
            }

            // Set up the entity.
            Entity e = parent.AddComponent<Entity>();
            
            // Initialize the entity.
            foreach (string key in parameters.ValidEntries)
            {
                e.Initialize(key, parameters.GetEntry(key).Value);
            }

            return e;
        }

        /// <summary>
        /// Adds a new, default component, to the parent game object.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <returns>Return newly created component.</returns>
        public Entity CreateComponent(GameObject parent)
        {
            // Creates a component using the default settings.
            return CreateComponent(parent, CreateSettings());
        }

        /// <summary>
        /// Set up the parameters associated with this factory's IFactoryElement.
        /// </summary>
        /// <returns></returns>
        public virtual Constraints CreateSettings(
            EntityType _type = EntityType.NULL, 
            Vector2? _position = null,
            Dimension _dimensions = null,
            HealthTracker _tracker = null)
        {
            // Create the collection.
            Constraints parameters = new Constraints();

            // Set the type.
            if (_type != EntityType.NULL) { parameters.AddValue<EntityType>(Constants.PARAM_ENTITY_TYPE, _type); }
            
            // Set the position.
            if (_position.HasValue) { parameters.AddValue<Vector2>(Constants.PARAM_POSITION, _position.Value); }

            // Dimension.
            if (_dimensions != null) { parameters.AddValue<Dimension>(Constants.PARAM_DIMENSIONS, _dimensions); }

            // Health tracker.
            if (_tracker != null) { parameters.AddValue<HealthTracker>(Constants.PARAM_HEALTH_TRACKER, _tracker); }

            return parameters;
        }

        #endregion

    }

    #endregion

    #region // Class: Entity class.

    /////////////////////
    // Entity class.
    /////////////////////

    /// <summary>
    /// Entity helps utilize the <see cref="GameObject"/> in ways that align with our program.
    /// </summary>
    public class Entity : MonoBehaviour, IFactoryElement
    {

        #region Static Methods.

        /// <summary>
        /// Parse the type as a string using the EntityType.
        /// </summary>
        /// <param name="_type">Type of Entity to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(EntityType _type)
        {
            string result = "";

            switch (_type)
            {
                case EntityType.Projectile:
                    result = "(Projectile)";
                    break;
                case EntityType.Wall:
                    result = "(Wall)";
                    break;
                case EntityType.Platform:
                    result = "(Platform)";
                    break;
                case EntityType.Player:             
                    result = "(Player)";
                    break;
                case EntityType.Entity:
                    result = "(Entity)";
                    break;
                case EntityType.NULL:
                default:
                    result = "";
                    break;
            }

            return result;
        }

        #endregion

        #region Data Members

        /////////////////////
        // Data members.
        /////////////////////

        /// <summary>
        /// The entity type.
        /// </summary>
        private EntityType m_entityType;

        /// <summary>
        /// Entity dimension manager. (Usually for calculations).
        /// </summary>
        private Dimension m_dimensions;

        /// <summary>
        /// Entity's health tracker.
        /// </summary>
        private HealthTracker m_health;

        /// <summary>
        /// Tracks the status of an entity.
        /// </summary>
        private Status m_status;
        
        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Return a reference to this entity's containing body, to refer to other components.
        /// </summary>
        public GameObject Self
        {
            get { return gameObject; }
        }
        
        /// <summary>
        /// The entity type belonging to the current entity. (Read-only).
        /// </summary>
        public EntityType EntityType
        {
            get { return this.m_entityType; }
        }

        /// <summary>
        /// Reference to this <see cref="GameObject"/>'s <see cref="Transform"/>. (Read-only).
        /// </summary>
        public Vector3 Position
        {
            get { return gameObject.transform.position; }
        }

        /// <summary>
        /// Reference to Entity width.
        /// </summary>
        public float Width
        {
            get { return m_dimensions.Width; }
        }
        
        /// <summary>
        /// Reference to Entity width.
        /// </summary>
        public float Height
        {
            get { return m_dimensions.Height; }
        }

        /// <summary>
        /// Reference to Entity depth level.
        /// </summary>
        public float Depth
        {
            get { return m_dimensions.Depth; }
        }

        /// <summary>
        /// Property tracking whether or not the entity is alive.
        /// </summary>
        public bool IsAlive
        {
            get { return !m_status.IsDead(); }
        }

        #endregion

        #region Service Methods

        /// <summary>
        /// Initilizes the Entity.
        /// </summary>
        protected virtual void Start()
        {
            if (m_status == null)
            {
                m_status = gameObject.AddComponent<Status>();
            }

            if (m_status.IsNull)
            {
                m_status.Run();
            }
        }

        /// <summary>
        /// Update UnityEngine components.
        /// </summary>
        protected virtual void Update()
        {
            if (m_status.IsRunning())
            {
                // Call update entity for inherited classes.
                UpdateEntity();

                if (this.IsAlive)
                {
                    // Call update life for inherited classes.
                    UpdateLife();
                }
                else if (m_status.JustKilled())
                {
                    // Call on death for inherited classes.
                    OnDeath();
                }
                else if (m_status.IsDead())
                {
                    // Call while dead.
                }
            }
        }

        /// <summary>
        /// Update the Entity's status.
        /// </summary>
        protected virtual void UpdateEntity()
        {
            if (m_health.IsVulnerable())
            {
                if (!m_health.IsAlive())
                {
                    m_status.Kill();
                }
            }
        }

        /// <summary>
        /// Updates that occur while alive.
        /// </summary>
        protected virtual void UpdateLife()
        {
            // Stub.
        }

        /// <summary>
        /// Event triggered upon death.
        /// </summary>
        protected virtual void OnDeath()
        {
            // Release on death.
            m_status.Release();
        }

        /// <summary>
        /// Reset the Entity.
        /// </summary>
        public virtual void Reset()
        {
            m_status.TriggerReset();
            m_status.Run();
        }

        /// <summary>
        /// Pause the Entity updates.
        /// </summary>
        public virtual void Pause()
        {
            m_status.Pause();
        }

        /// <summary>
        /// Resume the Entity updates.
        /// </summary>
        public virtual void Resume()
        {
            m_status.Resume();
        }

        /// <summary>
        /// Initialize the Entity.
        /// </summary>
        /// <param name="_type">The <see cref="EntityType"/> associated with an object.</param>
        /// <param name="_position">Position of the transform. If left null, the entity will inherit this value from its gameObject.</param>
        /// <param name="_dimensions">Dimensions reflect the boundaries of the entity.</param>
        /// <param name="_health">An entity may be able to be destroyed.</param>
        protected virtual void Initialize(EntityType _type = EntityType.Entity, Vector2? _position = null, Dimension _dimensions = null, HealthTracker _health = null)
        {
            // Set the name and type.
            this.m_entityType = _type;
            gameObject.name += " " + Entity.Parse(_type);

            // Initial location of the game object the entity belongs to.
            if (_position.HasValue)
            {
                gameObject.transform.position = Services.ToVector3(_position.Value.x, _position.Value.y, Position.z);
            }

            // Create objects for data members.
            if (_dimensions == null)
            {
                this.m_dimensions = new Dimension(Constants.DEFAULT_DIMENSION, 0.0f);
            }
            else
            {
                this.m_dimensions = _dimensions;
            }

            // Get components.
            if (_health == null)
            {
                this.m_health = HealthTrackerFactory.Instance().CreateComponent(gameObject);
            }
            else
            {
                this.m_health = _health;
            }
            
        }

        /// <summary>
        /// Set the value of a property.
        /// </summary>
        /// <param name="parameter">Switch trigger that determines which property is set.</param>
        public virtual void Initialize(string parameter, object value)
        {
            switch (parameter)
            {
                case Constants.PARAM_ENTITY_TYPE:
                    this.m_entityType = (EntityType)value;
                    break;
                case Constants.PARAM_POSITION:
                    Vector2 _position = (Vector2)value;
                    gameObject.transform.position = Services.ToVector3(_position.x, _position.y, Position.z);
                    break;
                case Constants.PARAM_DIMENSIONS:
                    this.m_dimensions = (Dimension)value;
                    break;
                case Constants.PARAM_HEALTH_TRACKER:
                    this.m_health = (HealthTracker)value;
                    break;                
            }
        }

        #endregion

        #region Accessor Methods

        /// <summary>
        /// Return this Entity's EntityType as a string.
        /// </summary>
        /// <returns>Return the string of the EntityType.</returns>
        public string GetEntityTypeAsString()
        {
            return Entity.Parse(this.m_entityType);
        }

        /// <summary>
        /// Return this Entity's EntityType.
        /// </summary>
        /// <returns>Return the EntityType.</returns>
        public EntityType GetEntityType()
        {
            return this.m_entityType;
        }

        #endregion

        #region Mutator Methods

        /// <summary>
        /// Set the vertical dimension of the Entity.
        /// </summary>
        /// <param name="height">Height of the Entity.</param>
        public void SetHeight(float height)
        {
            this.m_dimensions.SetHeight(Services.Max(height, 0.1f));
        }

        /// <summary>
        /// Set the horizontal dimension of the Entity.
        /// </summary>
        /// <param name="width">Width of the Entity.</param>
        public void SetWidth(float width)
        {
            this.m_dimensions.SetWidth(Services.Max(width, 0.1f));
        }

        /// <summary>
        /// Set the depth level of the Entity.
        /// </summary>
        /// <param name="depth">Depth level of the Entity.</param>
        public void SetDepth(int depth)
        {
            this.m_dimensions.SetDepth(Services.Max(depth, 0.1f));
        }

        /// <summary>
        /// Null objects can have their type reset. In this case, null just means empty.
        /// </summary>
        /// <param name="type">Type to set a Null Entity to.</param>
        public void SetEntityType(EntityType type)
        {
            if(this.m_entityType == EntityType.NULL)
            {
                this.m_entityType = type;
            }
        }

        #endregion

    }

    #endregion

}
