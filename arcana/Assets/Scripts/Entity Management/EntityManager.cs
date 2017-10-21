using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Arcana.Entities
{

    #region // Class: EntityManagerFactory class.

    /// <summary>
    /// Factory that fabricates EntityManager objects, and, keeps track of its instancing.
    /// </summary>
    public class EntityManagerFactory : IFactory<EntityManager> {

        #region // Static Members.

        /// <summary>
        /// Instance of the Factory.
        /// </summary>
        private static EntityManagerFactory instance = null;

        /// <summary>
        /// Instance of the manager.
        /// </summary>
        private static EntityManager manager = null;

        /// <summary>
        /// Returns EntityManager instance.
        /// </summary>
        /// <returns>Returns reference to manager instance.</returns>
        public static EntityManager Instance()
        {
            return manager;
        }

        /// <summary>
        /// Returns true if there is a manager instance.
        /// </summary>
        /// <returns>Returns flag defining instance state.</returns>
        public static bool HasManagerInstance()
        {
            return (Instance() != null);
        }

        #endregion

        #region // Service Methods.

        /// <summary>
        /// Get (or create) the single instance of the factory.
        /// </summary>
        /// <returns>Returns a single factory instance.</returns>
        public IFactory<EntityManager> GetInstance()
        {
            if (instance == null)
            {
                instance = new EntityManagerFactory();
            }

            return instance;
        }

        /// <summary>
        /// Create component on the parent object with supplied settings.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
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
        /// Create component on new empty object with default settings.
        /// </summary>
        /// <returns>Returns newly created component.</returns>
        public EntityManager CreateComponent()
        {
            if (!HasManagerInstance())
            {
                manager = CreateComponent(Services.CreateEmptyObject("Entity Manager"), CreateSettings());
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
            Constraints parameters = new Constraints();

            // TODO: Add non-nulllable types.
            // parameters.AddValue<T>(Constants., ); // Parameter.

            return parameters;
        }

        #endregion

    }

    #endregion

    #region // Class: EntityManager class.

    /// <summary>
    /// EntityManager contains references to all the entities in the scene.
    /// </summary>
    public class EntityManager : MonoBehaviour, IFactoryElement
    {

        #region // Static Members.

        /// <summary>
        /// Return reference to the instance of the EntityManager.
        /// </summary>
        public static EntityManager GetInstance()
        {
            return EntityManagerFactory.Instance();
        }

        /// <summary>
        /// Return true if instance exists.
        /// </summary>
        public static bool HasInstance()
        {
            return (GetInstance() != null);
        }

        #endregion

        #region // Data Members.

        // Fields.

        /// <summary>
        /// List of all the Entity objects handled by the manager.
        /// </summary>
        private List<Entity> m_entities;

        /// <summary>
        /// Stores cache of GameObjects with entity components. Only updates when requested.
        /// </summary>
        private List<GameObject> m_objects;

        /// <summary>
        /// Pause flag will pause all other entities, while paused.
        /// </summary>
        private bool m_pause;

        // Properties.

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
        /// Determines if the manager has been paused.
        /// </summary>
        public bool IsPaused
        {
            get { return this.m_pause; }
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

        #region // // UnityEngine methods.

        /// <summary>
        /// Run when the EntityManager is created for the very first time.
        /// </summary>
        public void Start()
        {
            // Stub;
        }

        /// <summary>
        /// Update references to entities in this manager.
        /// </summary>
        public void Update()
        {
            // Handle pausing all the entities.
            if (IsPaused)
            {
                Pause(this.m_entities);
            }
            else
            {
                Resume(this.m_entities);


            }
        }

        #endregion

        #region // // Initialization methods.

        /// <summary>
        /// Initialize is run after the component is constructed.
        /// </summary>
        internal void Initialize()
        {
            // Stub;
        }

        /// <summary>
        /// Initialize individual properties, assigned by select cases.
        /// </summary>
        /// <param name="parameter">Parameter to assign value to.</param>
        /// <param name="value">Value to assign.</param>
        public void Initialize(string parameter, object value)
        {
            switch (parameter)
            {
                // TODO.
            }
        }

        #endregion

        #region // // Other methods.

        /// <summary>
        /// Pauses the entire manager.
        /// </summary>
        public void Pause()
        {
            this.m_pause = true;
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
            this.m_pause = false;
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

        #endregion

        // Accessor Methods.

        // Mutator Methods.

    }

    #endregion

}
