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
        GameOverState = 3,
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

        /// <summary>
        /// Get the name of the state.
        /// </summary>
        /// <param name="_id">ID of the state.</param>
        /// <returns>Returns string containing the name of the state.</returns>
        public static string Parse(StateID _id)
        {
            switch (_id)
            {
                case StateID.ArenaState:
                    return "Arena State";
                case StateID.GameOverState:
                    return "Gameover State";
                case StateID.LoadingState:
                    return "Loading State";
                case StateID.MainMenuState:
                    return "Main Menu State";
                case StateID.ScoreState:
                    return "Score State";
                case StateID.NULL_STATE:
                    return "NULL State";
            }

            return "ID cannot be parsed.";
        }

        #endregion

        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Flag tracks if class has been initialized.
        /// </summary>
        private bool m_initialized = false;

        /// <summary>
        /// Status of the StateManager.
        /// </summary>
        private Status m_status = null;

        /// <summary>
        /// Holds the states as components.
        /// </summary>
        private GameObject m_container = null;
        
        /// <summary>
        /// Reference map of all IState instances, paired with their associated ID's.
        /// </summary>
        private Dictionary<StateID, State> m_states = null;

        /// <summary>
        /// Tracks the current state.
        /// </summary>
        private StateID m_currentStateID = StateID.NULL_STATE;
        
        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns initialization flag.
        /// </summary>
        public bool Initialized
        {
            get { return (this.m_initialized); }
        }

        /// <summary>
        /// Returns active flag.
        /// </summary>
        public bool Active
        {
            get { return (this.Status.IsActive()); }
        }

        /// <summary>
        /// Map of all IState instances, with their associated ID's.
        /// </summary>
        public Dictionary<StateID, State> States
        {
            get { return this.m_states; }
        }

        /// <summary>
        /// Reference to the currently active state.
        /// </summary>
        public StateID CurrentStateID
        {
            get { return this.m_currentStateID; }
        }

        /// <summary>
        /// Tracks the active state.
        /// </summary>
        public State CurrentState
        {
            get { return this.m_states[this.m_currentStateID]; }
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
            if (!Initialized)
            {
                this.Initialize();
            }
        }

        /// <summary>
        /// Update meta information about the states. States are updated separately.
        /// </summary>
        public void Update()
        {
            if (Initialized && Active)
            {
                // If initialized.
                // TODO: StateManager update functionality.
            }
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

                // Ensure no movement.
                this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

                // Create the status.
                BuildStatus();

                // Create the states.
                BuildStates();

                // Set current state.
                this.m_currentStateID = StateID.MainMenuState;

                // Initialization flag.
                this.m_initialized = true;
            }
        }

        #region Build Property Methods.

        /// <summary>
        /// Creates the Status object.
        /// </summary>
        private void BuildStatus()
        {
            // Create the status.
            this.m_status = gameObject.GetComponent<Status>();
            if (this.m_status == null)
            {
                this.m_status = gameObject.AddComponent<Status>();
                this.m_status.Initialize();
            }

            // Activate the status object.
            this.m_status.Activate();
        }

        /// <summary>
        /// Builds the states.
        /// </summary>
        private void BuildStates()
        {
            // Initialize data members.
            this.m_states = new Dictionary<StateID, State>();

            // Initialize other properties.
            this.m_currentStateID = StateID.NULL_STATE;

            // Create the container.
            this.m_container = Services.AddChild(gameObject, Services.CreateEmptyObject("States"));

            // Create the individual states.
            this.CreateState(StateID.MainMenuState, MainMenuFactory.Instance().CreateComponent(this.m_container));
            this.CreateState(StateID.ArenaState, ArenaFactory.Instance().CreateComponent(this.m_container));
            this.CreateState(StateID.GameOverState, GameOverFactory.Instance().CreateComponent(this.m_container));
        }

        /// <summary>
        /// Create and add state to reference map.
        /// </summary>
        /// <param name="_stateID">ID of the state being added.</param>
        /// <param name="_state">State component.</param>
        private void CreateState(StateID _stateID, State _state)
        {
            if (!this.m_states.ContainsKey(_stateID))
            {
                // Deactivate newly added states.
                _state.Deactivate();
                this.m_states.Add(_stateID, _state);
            }
        }

        #endregion

        /// <summary>
        /// Initialize individual properties, assigned by select cases.
        /// </summary>
        /// <param name="parameter">Parameter to assign value to.</param>
        /// <param name="value">Value to assign.</param>
        public void Initialize(string parameter, object value)
        {
            // TODO: Set up initialization pathway.
            return; // No parameters actually get passed in to the state.
        }

        #endregion

        #region State Methods.
        
        /// <summary>
        /// Pause the current state.
        /// </summary>
        public void Pause()
        {
            Debugger.Print("Pausing current state.");
            this.CurrentState.Pause();
        }

        /// <summary>
        /// Resume the current state.
        /// </summary>
        public void Resume()
        {
            Debugger.Print("Resuming current state.");
            this.CurrentState.Resume();
        }

        /// <summary>
        /// Reset the current state.
        /// </summary>
        public void ResetState()
        {
            Debugger.Print("Resetting the state.");
            this.CurrentState.ResetState();
        }

        /// <summary>
        /// Assign current state to input state.
        /// </summary>
        /// <param name="_state">State to change to.</param>
        public void ChangeStates(StateID _stateID)
        {
            if (Active)
            {
                if (this.m_currentStateID != _stateID)
                {
                    // Deactivate the previous state.
                    this.CurrentState.Deactivate();

                    // Activate the current state.
                    this.m_currentStateID = _stateID;
                    this.CurrentState.Activate();

                }
            }
        }

        #endregion
        
        #endregion
        
    }

    #endregion

}
