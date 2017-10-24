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
        /// On creation, sets this to be the instance.
        /// </summary>
        private EntityFactory()
        {
            instance = this;
        }

        #endregion

        #region // Factory Methods.

        /// <summary>
        /// Return the instance of the factory.
        /// </summary>
        /// <returns>Returns a single factory.</returns>
        public IFactory<Entity> GetInstance()
        {
            return Instance();
        }
        
        /// <summary>
        /// Creates a new, empty game object, and returns the Entity component back.
        /// </summary>
        /// <returns></returns>
        public Entity CreateComponent()
        {
            // Creates a component using the default settings.
            Debugger.Print("Create an Entity on an empty game object.");
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
                Debugger.Print("Tried to add a component but parent GameObject is null.", "NULL_REFERENCE");
                return null;
            }

            // Set up the entity.
            Debugger.Print("Create the component.");
            Entity e = parent.AddComponent<Entity>();
            
            // Initialize the entity.
            foreach (string key in parameters.ValidEntries)
            {
                Debugger.Print("Initialize the component.");
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
            Debugger.Print("Building parameters for Entity component.");
            Constraints parameters = new Constraints();

            // Set the type.
            Debugger.Print("Setting type.");
            if (_type != EntityType.NULL) { parameters.AddValue<EntityType>(Constants.PARAM_ENTITY_TYPE, _type); }

            // Set the position.
            Debugger.Print("Setting position.");
            if (_position.HasValue) { parameters.AddValue<Vector2>(Constants.PARAM_POSITION, _position.Value); }

            // Dimension.
            Debugger.Print("Setting dimensions.");
            if (_dimensions != null) { parameters.AddValue<Dimension>(Constants.PARAM_DIMENSIONS, _dimensions); }

            // Health tracker.
            Debugger.Print("Setting health tracker.");
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

        /// <summary>
        /// Tracks initialization internally.
        /// </summary>
        private bool m_initialized = false;
        
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
        /// Reference to Status tracker.
        /// </summary>
        public Status Status
        {
            get { return this.m_status; }
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
        
        #endregion

        #region Service Methods

        /// <summary>
        /// Initilizes the Entity.
        /// </summary>
        protected virtual void Start()
        {
            // Run this method when initializing an entity.
            this.Initialize();
        }

        /// <summary>
        /// Update UnityEngine components.
        /// </summary>
        protected virtual void Update()
        {
            // Update method.
        }

        /// <summary>
        /// Update collision responses and physics calculations.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            // FixedUpdate method.
        }

        #region Initialization Methods.

        /// <summary>
        /// Initialize is run after the component is constructed.
        /// </summary>
        protected virtual void Initialize()
        {
            if (!this.m_initialized)
            {
                // Initialize the entity manager.
                Debugger.Print("Initializing Entity.", "Entity [" + GetEntityTypeAsString() + "]");

                // Create the status.
                this.m_status = gameObject.GetComponent<Status>();
                if (this.m_status == null)
                {
                    Debugger.Print("Create the status object.");
                    this.m_status = gameObject.AddComponent<Status>();
                    this.m_status.Initialize();
                }
                
                // Initialization flag.
                this.m_initialized = true;

                // Start the status object.
                this.m_status.Start();
            }
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
            Debugger.Print("Setting Entity data member and property values.", gameObject.name);
            this.m_entityType = _type;
            gameObject.name += " " + Entity.Parse(_type);

            // Initial location of the game object the entity belongs to.
            Debugger.Print("Setting position.", gameObject.name);
            if (_position.HasValue)
            {
                gameObject.transform.position = Services.ToVector3(_position.Value.x, _position.Value.y, Position.z);
            }

            // Create objects for data members.
            Debugger.Print("Setting dimensions.", gameObject.name);
            if (_dimensions == null)
            {
                this.m_dimensions = new Dimension(Constants.DEFAULT_DIMENSION, 0.0f);
            }
            else
            {
                this.m_dimensions = _dimensions;
            }

            // Get components.
            Debugger.Print("Setting health tracker.", gameObject.name);
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
        /// Set the value of a property via the factory.
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
        
        /// <summary>
        /// Reset the Entity.
        /// </summary>
        public virtual void Reset()
        {
            m_status.TriggerReset();
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
