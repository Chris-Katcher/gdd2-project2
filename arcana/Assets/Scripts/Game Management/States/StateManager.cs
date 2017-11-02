/************************************************
 * StateManager.cs
 * 
 * This file contains:
 * - The StateManager class. (Child of ArcanaObject).
 * - The StateID enum.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Utilities;
using Arcana.UI.Screens;
using Arcana.Entities.Attributes;

namespace Arcana.States
{
    
    #region Class: StateManager class.

    /////////////////////
    // Manager class.
    /////////////////////

    /// <summary>
    /// Manager responsible for the IState items.
    /// </summary>
    [AddComponentMenu("Arcana/Managers/State Manager")]
    public class StateManager : ArcanaObject
    {

        #region Static Methods.

        #region Enum Parsing Method.

        /////////////////////
        // Enum parsing.
        /////////////////////

        /// <summary>
        /// Get the name of the state.
        /// </summary>
        /// <param name="_id">ID of the state.</param>
        /// <returns>Returns string containing the name of the state.</returns>
        public static string Parse(StateID _id)
        {
            string result = "";

            switch (_id)
            {
                case StateID.ArenaState:
                    result = "(Arena State)";
                    break;
                case StateID.GameOverState:
                    result = "(Gameover State)";
                    break;
                case StateID.MainMenuState:
                    result = "(Main Menu State)";
                    break;
                case StateID.NULL_STATE:
                    result = "(NULL State)";
                    break;
                default:
                    result = "(Unknown State)";
                    break;
            }

            return result;
        }

        #endregion

        #region Instancing Methods.

        /////////////////////
        // Static methods for instancing.
        /////////////////////

        /// <summary>
        /// Static instance of the class. (We only want one).
        /// </summary>
        public static StateManager instance = null;

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        /// <returns>Returns a component.</returns>
        public static StateManager GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of StateManager.");
                instance = Services.CreateEmptyObject("State Manager").AddComponent<StateManager>();
            }

            return instance;
        }

        /// <summary>
        /// Returns true if instance exists.
        /// </summary>
        /// <returns>Returns boolean indicating instance existence.</returns>
        public static bool HasInstance()
        {
            return (instance != null);
        }


        #endregion

        #region Component Factory Methods.
        
        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static StateManager Create(ArcanaObject _parent)
        {
            if (!HasInstance())
            {
                instance = _parent.GetComponent<StateManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<StateManager>(_parent);
            }

            return instance;
        }

        #endregion

        #endregion

        #region Data Members

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Holds the states as components.
        /// </summary>
        private ArcanaObject m_container = null;

        /// <summary>
        /// Reference map of all IState instances, paired with their associated ID's.
        /// </summary>
        private Dictionary<StateID, State> m_states = null;

        /// <summary>
        /// Tracks the current state.
        /// </summary>
        private StateID m_currentStateID = StateID.NULL_STATE;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

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
            get { return this.GetState(this.m_currentStateID); }
        }

        #endregion

        #endregion

        #region UnityEngine Methods

        /// <summary>
        /// Run when the component is created for the very first time.
        /// </summary>
        public override void Start()
        {
            this.Initialize();
        }

        /// <summary>
        /// Update meta information about the states. States are updated separately.
        /// </summary>
        public override void Update()
        {
            if (!this.Initialized)
            {
                this.Initialize();
            }
            else
            {
                // Run base update.
                base.Update();                
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Create the data members for the StateManager.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Initialize the base values.
                base.Initialize();

                // Set this name.
                this.Name = "State Manager";

                // Initialize the state manager.
                Debugger.Print("Initializing state manager.", this.Self.name);

                // Create the container, while also adding an ArcanaObject component and making it a child of this component's gameObject.
                this.m_container = Services.AddChild(this.Self, Services.CreateEmptyObject("State Container")).AddComponent<ArcanaObject>();

                // Create the dictionary for state management.
                this.m_states = new Dictionary<StateID, State>();

                // This isn't a poolable element.
                this.IsPoolable = false;
            }
        }

        /// <summary>
        /// Builds a state (and adds it to the dictionary) when requested.
        /// </summary>
        /// <param name="_state">ID belonging to state that will be built.</param>
        private State GetState(StateID _state)
        {
            switch (_state)
            {
                case StateID.MainMenuState:
                    return GetState<MainMenuState>(_state);
                case StateID.ArenaState:
                    return GetState<ArenaState>(_state);
                case StateID.GameOverState:
                    return GetState<GameOverState>(_state);
                case StateID.NULL_STATE:
                default:
                    return null;
            }            
        }

        /// <summary>
        /// Builds the state of the particular type and initializes it.
        /// </summary>
        /// <typeparam name="T">Generic where T is a State.</typeparam>
        /// <param name="_stateID">ID of state to build, initialize, and retrieve.</param>
        /// <returns>Returns state object.</returns>
        private T GetState<T>(StateID _stateID) where T: State
        {
            // If the state already exists, return it.
            if (HasState(_stateID))
            {
                return this.States[_stateID] as T;
            }
            else
            {
                // If the state hasn't been built yet.
                // Put it on the container.
                T state = this.m_container.Self.GetComponent<T>();

                // If it is null.
                if (state == null)
                {
                    // Create the new state.
                    state = ComponentFactory.Create<T>(this.m_container);

                    // Initialize the state.
                    state.Initialize();
                }

                // Add the state to the states dictionary.
                this.States.Add(_stateID, state);

                // Return the state.
                return state;
            }
        }

        /*
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
        }*/

        #endregion

        #region Status Methods.

        /// <summary>
        /// Pause the current state.
        /// </summary>
        public override void Pause()
        {
            base.Pause();
            Debugger.Print("Pausing current state.");
            this.CurrentState.Pause();
        }

        /// <summary>
        /// Resume the current state.
        /// </summary>
        public override void Resume()
        {
            base.Resume();
            Debugger.Print("Resuming current state.");
            this.CurrentState.Resume();
        }

        #endregion

        #region State Methods.

        /// <summary>
        /// Checks to see if state ID exists as a key for the states map.
        /// </summary>
        /// <param name="_stateID">ID of state to check for.</param>
        /// <returns>Returns true if entry exists</returns>
        public bool HasStateID(StateID _stateID)
        {
            return (this.States.ContainsKey(_stateID));
        }

        /// <summary>
        /// Checks to see if state exists.
        /// </summary>
        /// <param name="_stateID">ID of state to check for.</param>
        /// <returns>Returns true if entry exists and is not null.</returns>
        public bool HasState(StateID _stateID)
        {
            return (HasStateID(_stateID) && (this.States[_stateID] != null));
        }

        /// <summary>
        /// Reset the current state.
        /// </summary>
        public void ResetState()
        {
            Debugger.Print("Resetting the state.");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Assign current state to input state.
        /// </summary>
        /// <param name="_state">State to change to.</param>
        public void ChangeStates(StateID _stateID)
        {
            if (this.Status.IsActive())
            {
                if (this.m_currentStateID != _stateID)
                {
                    // Deactivate the previous state.
                    this.CurrentState.Status.Deactivate();

                    // Activate the current state.
                    this.m_currentStateID = _stateID;
                    this.CurrentState.Status.Activate();
                }
            }
        }

        #endregion
        
    }

    #endregion

    #region Enum: State ID

    /// <summary>
    /// ID values associated with given states.
    /// </summary>
    public enum StateID
    {
        NULL_STATE,
        MainMenuState,
        ArenaState,
        GameOverState,
    }

    #endregion

}
