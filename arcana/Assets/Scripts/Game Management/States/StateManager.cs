/************************************************
 * StateManager.cs
 * 
 * This file contains implementation for the StateManager class, 
 * as well as the enum definitions for StateIDs and ScreenIDs.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.UI.Screens;
using Arcana.Entities.Attributes;

namespace Arcana.States
{

    #region Enum: State ID

    /// <summary>
    /// ID values associated with given states.
    /// </summary>
    public enum StateID
    {
        LoadingState = 0,
        MainMenuState = 1,
        ArenaState = 2,
        GameoverState = 3,
        ScoreState = 4,
        NULL_STATE = 5
    }

    #endregion

    #region Class: StateManagerFactory class.

    /////////////////////
    // Factory class.
    /////////////////////

    /// <summary>
    /// Factory that returns StateManager components.
    /// </summary>
    public class StateManagerFactory : IFactory<StateManager> {

        #region // Static Members.

        /////////////////////
        // Static members.
        /////////////////////

        /// <summary>
        /// Instance of the factory.
        /// </summary>
        private static StateManagerFactory instance = null;

        /// <summary>
        /// Instance of the manager.
        /// </summary>
        private static StateManager manager = null;

        /// <summary>
        /// Returns factory instance.
        /// </summary>
        /// <returns>Returns reference to manager factory instance.</returns>
        public static StateManagerFactory Instance()
        {
            if (instance == null)
            {
                instance = new StateManagerFactory();
            }

            return instance;
        }

        /// <summary>
        /// Get reference to the manager.
        /// </summary>
        /// <returns>Returns a single manager.</returns>
        public static StateManager GetManagerInstance()
        {
            return manager;
        }

        /// <summary>
        /// On creation, set this to be the instance.
        /// </summary>
        private StateManagerFactory()
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
        public IFactory<StateManager> GetInstance()
        {
            return Instance();
        }

        /// <summary>
        /// Create component on new empty object with default settings.
        /// </summary>
        /// <returns>Returns newly created component.</returns>
        public StateManager CreateComponent()
        {
            if (!HasManagerInstance())
            {
                Debugger.Print("Create StateManager on an empty game object, with default parameters.");
                manager = CreateComponent(Services.CreateEmptyObject("State Manager"), CreateSettings());
            }

            return manager;
        }

        /// <summary>
        /// Adds a new component to the parent game object, with parameters.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <param name="parameters">Settings to apply to the new manager.</param>
        /// <returns>Return newly created component.</returns>
        public StateManager CreateComponent(GameObject parent, Constraints parameters)
        {
            // Check if there is already an instance of the manager component.
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
                manager = parent.GetComponent<StateManager>();

                // If the manager is null.
                if (manager == null)
                {
                    // If the manager instance is null, then create the component.
                    Debugger.Print("Create and add the StateManager component.");
                    manager = parent.AddComponent<StateManager>();
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
        public StateManager CreateComponent(GameObject parent)
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
            Debugger.Print("Creating settings for StateManager initialization.");
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
        /// Delete the instance of the StateManager.
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

    #region Class: StateManager class.

    /////////////////////
    // Blueprint class.
    /////////////////////

    /// <summary>
    /// Manager responsible for the IState items.
    /// </summary>
    public class StateManager : MonoBehaviour, IFactoryElement
    {

        #region Static Members

        /////////////////////
        // Static members.
        /////////////////////

        /// <summary>
        /// Returns the instance of the manager.
        /// </summary>
        public static StateManager Instance
        {
            get { return StateManagerFactory.GetManagerInstance(); }
        }

        /// <summary>
        /// Return true if instance exists.
        /// </summary>
        /// <returns>Returns a boolean value.</returns>
        public static bool HasInstance()
        {
            return (StateManager.Instance != null);
        }

        #endregion

        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Status component holds informationa about the class's state.
        /// </summary>
        private Status m_status = null;

        /// <summary>
        /// Map of all IState instances, with their associated ID's.
        /// </summary>
        private Dictionary<StateID, IState> m_states = null;

        /// <summary>
        /// Tracks the active state.
        /// </summary>
        private IState m_currentState = null;

        /// <summary>
        /// Flag tracks if class has been initialized.
        /// </summary>
        private bool m_initialized = false;

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Map of all IState instances, with their associated ID's.
        /// </summary>
        public Dictionary<StateID, IState> States
        {
            get { return this.m_states; }
        }

        /// <summary>
        /// Tracks the active state.
        /// </summary>
        public IState CurrentState
        {
            get { return this.m_currentState; }
        }

        /// <summary>
        /// Keeps track of the loading state, when loading needs to occur.
        /// </summary>
        public IState LoadingState
        {
            get;
            set;
        }

        /// <summary>
        /// Reference to component's current state.
        /// </summary>
        public Status Status
        {
            get { return this.m_status; }
        }

        #endregion

        #region Service Methods

        /////////////////////
        // Service methods.
        /////////////////////

        #region UnityEngine Methods

        /// <summary>
        /// Run when the component is created for the very first time.
        /// </summary>
        public void Start()
        {
            // Start method.
        }

        /// <summary>
        /// Update meta information about the states. States are updated separately.
        /// </summary>
        public void Update()
        {
            // Update.
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the StateManager.
        /// </summary>
        internal void Initialize()
        {
            if (!this.m_initialized)
            {
                // Initialize the entity manager.
                Debugger.Print("Initializing state manager.", gameObject.name);

                // Create the status.
                this.m_status = gameObject.GetComponent<Status>();
                if (this.m_status == null)
                {
                    this.m_status = gameObject.AddComponent<Status>();
                    this.m_status.Initialize();
                }

                // Initialize data members.
                this.m_states = new Dictionary<StateID, IState>();
                
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
            switch (parameter)
            {
                // TODO.
            }
        }

        #endregion

        #region State Methods.

        /// <summary>
        /// Load resources for the current state.
        /// </summary>
        public void LoadState()
        {
            // TODO: Stub.
        }

        /// <summary>
        /// Load resources for the input state.
        /// </summary>
        /// <param name="_state"></param>
        public void LoadState(IState _state)
        {
            // TODO: Stub.
        }

        /// <summary>
        /// Pause the current state.
        /// </summary>
        public void Pause()
        {
            // TODO: Stub.
        }

        /// <summary>
        /// Resume the current state.
        /// </summary>
        public void Resume()
        {
            // TODO: Stub.
        }

        /// <summary>
        /// Reset the current state.
        /// </summary>
        public void ResetState()
        {
            // TODO: Stub.
        }

        /// <summary>
        /// Assign current state to input state.
        /// </summary>
        /// <param name="_state">State to change to.</param>
        public void ChangeStates(IState _state)
        {
            if (this.m_currentState != _state)
            {
                this.m_currentState = _state;
            }
        }

        #endregion
        
        #endregion

        #region Mutator Methods

        // TODO: Stub.

        #endregion

        #region Accessor Methods
        
        // TODO: Stub.

        #endregion

    }

    #endregion

}
