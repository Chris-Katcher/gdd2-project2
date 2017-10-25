using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Entities.Attributes;


namespace Arcana.Entities
{

    #region // Class: EntityManagerFactory class.

    /////////////////////
    // Factory class.
    /////////////////////

    /// <summary>
    /// Factory that fabricates EntityManager objects, and, keeps track of its instancing.
    /// </summary>
    public class EntityManagerFactory : IFactory<EntityManager> {

        #region // Static Members.

        /////////////////////
        // Static members.
        /////////////////////

        /// <summary>
        /// Instance of the factory.
        /// </summary>
        private static EntityManagerFactory instance = null;

        /// <summary>
        /// Instance of the manager.
        /// </summary>
        private static EntityManager manager = null;

        /// <summary>
        /// Returns EntityManagerFactory instance.
        /// </summary>
        /// <returns>Returns reference to manager factory instance.</returns>
        public static EntityManagerFactory Instance()
        {
            if (instance == null)
            {
                instance = new EntityManagerFactory();
            }

            return instance;
        }

        /// <summary>
        /// Get reference to the manager.
        /// </summary>
        /// <returns>Returns a single manager.</returns>
        public static EntityManager GetManagerInstance()
        {
            return manager;
        }
        
        /// <summary>
        /// On creation, set this to be the instance.
        /// </summary>
        private EntityManagerFactory()
        {
            instance = this;
        }

        #endregion

        #region // Factory Methods.

        /////////////////////
        // Factory methods.
        /////////////////////

        /// <summary>
        /// Get (or create) the single instance of the factory.
        /// </summary>
        /// <returns>Returns a single factory instance.</returns>
        public IFactory<EntityManager> GetInstance()
        {
            return Instance();
        }

        /// <summary>
        /// Create component on new empty object with default settings.
        /// </summary>
        /// <returns>Returns newly created component.</returns>
        public EntityManager CreateComponent()
        {
            if (!HasManagerInstance())
            {
                Debugger.Print("Create EntityManager on an empty game object, with the default settings.");
                manager = CreateComponent(Services.CreateEmptyObject("Entity Manager"), CreateSettings());
            }

            return manager;
        }
        
        /// <summary>
        /// Adds a new component to the parent game object, with parameters.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <param name="parameters">Settings to apply to the new Entity.</param>
        /// <returns>Return newly created component.</returns>
        public EntityManager CreateComponent(GameObject parent, Constraints parameters)
        {
            // Check if there is already an instance of the EntityManager component.
            if (!HasManagerInstance())
            {
                // Check game object.
                if (parent == null)
                {
                    // If the parent itself is null, do not return a component.
                    Debugger.Print("Tried to add a component but parent GameObject is null.", "NULL_REFERENCE");
                    return null;
                }

                // Get reference to existing script if it already exists on this parent.
                manager = parent.GetComponent<EntityManager>();

                // If the manager is null.
                if (manager == null)
                {
                    // If the manager instance is null, then create the component.
                    Debugger.Print("Create and add the EntityManager component.");
                    manager = parent.AddComponent<EntityManager>();
                }

                // Assign non-optional information.
                manager.Initialize();

                // Initialize the entity.
                foreach (string key in parameters.ValidEntries)
                {
                    manager.Initialize(key, parameters.GetEntry(key).Value);
                }
            }

            return manager;
        }

        /// <summary>
        /// Create component on the parent object with default settings.
        /// </summary>
        /// <param name="parent">Parent receiving the component.</param>
        /// <returns>Returns newly created component.</returns>
        public EntityManager CreateComponent(GameObject parent)
        {
            if (!HasManagerInstance())
            {
                manager = CreateComponent(parent, CreateSettings());
            }

            return manager;
        }
        
        /// <summary>
        /// Create the Constraints for initialization of the fabricated class.
        /// </summary>
        /// <returns>Returns one Constraints object.</returns>
        public Constraints CreateSettings()
        {
            // Create the collection.
            Debugger.Print("Creating settings for EntityManager initialization.");
            Constraints parameters = new Constraints();

            // TODO: Add non-nulllable types.
            // parameters.AddValue<T>(Constants., ); // Parameter.

            return parameters;
        }

        #endregion

        #region // Service Methods.

        /////////////////////
        // Service methods.
        /////////////////////

        /// <summary>
        /// Returns true if there is a manager instance.
        /// </summary>
        /// <returns>Returns flag defining instance state.</returns>
        public static bool HasManagerInstance()
        {
            return (GetManagerInstance() != null);
        }

        /// <summary>
        /// Delete the instance of the EntityManager.
        /// </summary>
        public static void Release()
        {
            if (HasManagerInstance())
            {
                UnityEngine.Object.Destroy(manager);
            }
        }

        #endregion

    }

    #endregion

    #region // Class: EntityManager class.

    /////////////////////
    // Blueprint class.
    /////////////////////

    /// <summary>
    /// EntityManager contains references to all the entities in the scene.
    /// </summary>
    public class EntityManager : MonoBehaviour, IFactoryElement
    {

        #region // Static Members.

        /////////////////////
        // Static members.
        /////////////////////

        /// <summary>
        /// Return reference to the instance of the EntityManager.
        /// </summary>
        public static EntityManager Instance
        {
            get { return EntityManagerFactory.GetManagerInstance(); }
        }

        /// <summary>
        /// Return true if instance exists.
        /// </summary>
        /// <returns>Returns a boolean value.</returns>
        public static bool HasInstance()
        {
            return (EntityManager.Instance != null);
        }

        #endregion

        #region // Data Members.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Manager status tracking for all entities.
        /// </summary>
        private Status m_status;

        /// <summary>
        /// Reference to all entities in the environment.
        /// </summary>
        private List<Entity> m_entities;
        
        /// <summary>
        /// Stores cache of GameObjects with entity components. Only updates when requested.
        /// </summary>
        private List<GameObject> m_objects;

        /// <summary>
        /// Tracks initialization internally.
        /// </summary>
        private bool m_initialized = false;

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns a list of all the GameObjects with Entity components, handled by this manager.
        /// </summary>
        public List<GameObject> Entities
        {
            get
            {
                if (m_objects == null || m_objects.Count != m_entities.Count)
                {
                    m_objects = new List<GameObject>();
                    
                    if (m_entities.Count > 0)
                    {
                        foreach (Entity e in m_entities)
                        {
                            m_objects.Add(e.Self);
                        }
                    }
                }
                
                return m_objects;
            }
        }

        /// <summary>
        /// Reference to component's current state.
        /// </summary>
        public Status Status
        {
            get { return this.m_status; }
        }
        
        /// <summary>
        /// Determine if the manager is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return((m_entities == null) || (m_entities.Count == 0));
            }
        }

        #endregion

        #region // Service Methods.

        /////////////////////
        // Service methods.
        /////////////////////

        #region // // UnityEngine methods.

        /// <summary>
        /// Run when the EntityManager is created for the very first time.
        /// </summary>
        public void Start()
        { 
            // Start method.
        }

        /// <summary>
        /// Update references to entities in this manager.
        /// </summary>
        public void Update()
        {
            // Update method.
        }

        #endregion

        #region // // Initialization methods.

        /// <summary>
        /// Initialize is run after the component is constructed.
        /// </summary>
        internal void Initialize()
        {
            if (!this.m_initialized)
            {
                // Initialize the entity manager.
                Debugger.Print("Initializing entity manager.", gameObject.name);

                // Ensure no movement.
                this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

                // Create the status.
                this.m_status = gameObject.GetComponent<Status>();
                if (this.m_status == null)
                {
                    this.m_status = gameObject.AddComponent<Status>();
                    this.m_status.Initialize();
                }

                // Create the lists.
                this.m_objects = new List<GameObject>();
                this.m_entities = new List<Entity>();

                // Initialization flag.
                this.m_initialized = true;

                // Start the status object.
                this.m_status.Start();
            }
        }

        /// <summary>
        /// Initialize individual properties, assigned by select cases.
        /// </summary>
        /// <param name="parameter">Parameter to assign value to.</param>
        /// <param name="value">Value to assign.</param>
        public void Initialize(string parameter, object value)
        {
            return; // No implementation necessary for this interface method.
        }

        #endregion

        #endregion

        #region // Accessor Methods.

        /// <summary>
        /// Checks if entity already exists in list.
        /// </summary>
        /// <param name="e">Entity to search for.</param>
        /// <returns>Returns true if it is contained within the list.</returns>
        public bool HasEntity(Entity e)
        {
            return this.m_entities.Contains(e);
        }

        /// <summary>
        /// Checks if a given entity is available to be reused.
        /// </summary>
        /// <param name="e">Entity to check.</param>
        /// <returns>Returns true if entity is inactive.</returns>
        public bool IsAvailable(Entity e)
        {
            // When an element is marked as inactive, it's available to be reused.
            return (e.IsAvailable());
        }

        /// <summary>
        /// Get next available will return the first inactive Entity in the list it can find.
        /// If it is unavailable, it will create a new Entity.
        /// </summary>
        /// <param name="_type">Type of Entity to request.</param>
        /// <returns>Returns entity of given type if it is available.</returns>
        public Entity GetNextAvailable(EntityType _type)
        {
            // Temp value.
            Entity available = null;

            // Find the first available entity in the list.
            foreach (Entity e in this.m_entities)
            {
                if (IsAvailable(e))
                {
                    available = e;
                    break; // Break out the for loop.
                }
            }

            // If available is still null, make a new Entity.
            if (available == null)
            {
                // Creates default entity.
                available = MakeEntity(_type, null);
            }

            // Return available.
            return available;
        }

        #endregion

        #region // Mutator methods.

        /// <summary>
        /// Creates a new Entity and adds it to the list.
        /// </summary>
        /// <returns>Returns the newly created Entity.</returns>
        public Entity MakeEntity(EntityType _type = EntityType.NULL, GameObject _parent = null, Constraints _parameters = null)
        {
            Entity e = null;
            GameObject p = _parent;
            Constraints c = _parameters;

            if(p == null)
            {
                p = Services.CreateEmptyObject("Entity");
            }

            if (c == null)
            {
                c = EntityFactory.Instance().CreateSettings(_type);
            }

            // Create and add the entity.
            e = EntityFactory.Instance().CreateComponent(p, c);
            this.m_entities.Add(e);

            return e;
        }

        /// <summary>
        /// Make available will call the entity's MakeAvailable function.
        /// </summary>
        /// <param name="e">Entity to make available.</param>
        private void MakeAvailable(Entity e)
        {
            e.MakeAvailable();
        }

        /// <summary>
        /// Entity is added to the list.
        /// </summary>
        /// <param name="e">Entity to add.</param>
        private void AddEntity(Entity e)
        {
            this.m_entities.Add(e);
        }

        /// <summary>
        /// Entity is removed from the list.
        /// </summary>
        /// <param name="e">Entity to remove.</param>
        private void RemoveEntity(Entity e)
        {
            if (HasEntity(e))
            {
                this.m_entities.Remove(e);
            }
        }

        /// <summary>
        /// Pauses the entire manager.
        /// </summary>
        public void Pause()
        {
            this.m_status.Pause();
        }

        /// <summary>
        /// Pauses a selection of entities.
        /// </summary>
        /// <param name="entities">List of entities to pause.</param>
        public void Pause(List<Entity> entities)
        {
            if (!IsEmpty)
            {
                foreach (Entity e in entities)
                {
                    e.Pause();
                }
            }
        }

        /// <summary>
        /// Unpauses the entire manager.
        /// </summary>
        public void Resume()
        {
            this.m_status.Resume();
        }

        /// <summary>
        /// Unpauses a selection of entities.
        /// </summary>
        /// <param name="entities">List of entities to unpause.</param>
        public void Resume(List<Entity> entities)
        {
            if (!IsEmpty)
            {
                foreach (Entity e in entities)
                {
                    e.Resume();
                }
            }
        }

        #endregion

    }

    #endregion

}
