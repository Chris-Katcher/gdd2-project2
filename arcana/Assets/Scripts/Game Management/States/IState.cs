/************************************************
 * IState.cs
 * 
 * This file contains:
 * - The abstract StateFactory<T> factory.
 * - The abstract State class.
 * - The IState interface.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Entities.Attributes;
using Arcana.UI.Screens;
using Arcana.InputManagement;

namespace Arcana.States
{

    #region Class: StateFactory class.

    /////////////////////
    // Factory class declaration.
    /////////////////////

    /// <summary>
    /// Abstract factory that will create elements of a certain type.
    /// </summary>
    public abstract class StateFactory<T> {

        #region Static Members.

        /////////////////////
        // Static members.
        /////////////////////

        /// <summary>
        /// Instance of the factory.
        /// </summary>
        protected static StateFactory<T> instance = null;
                
        /// <summary>
        /// On creation, set this to be the instance.
        /// </summary>
        protected StateFactory()
        {
            instance = this;
        }

        #endregion

        #region Factory Methods.

        /////////////////////
        // Factory methods.
        /////////////////////

        /// <summary>
        /// Get (or create) the single instance of the factory.
        /// </summary>
        /// <returns>Returns a single factory instance.</returns>
        public abstract StateFactory<T> GetInstance();
        
        /// <summary>
        /// Create component on the parent object with default settings.
        /// </summary>
        /// <param name="parent">Parent receiving the component.</param>
        /// <returns>Returns newly created component.</returns>
        public abstract State CreateComponent(GameObject parent);
        
        #endregion
        
    }

    #endregion

    #region Class: State base class.

    /////////////////////
    // Base class declaration.
    /////////////////////

    /// <summary>
    /// An interface describing the fields and methods that will be implemented by each of the IState classes.
    /// </summary>
    public abstract class State : MonoBehaviour, IState
    {

        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Initialization flag.
        /// </summary>
        protected bool m_initialized = false;

        /// <summary>
        /// Decide if entities should be visible.
        /// </summary>
        protected bool m_visible = false;

        /// <summary>
        /// Decide if the GUI elements should be visible.
        /// </summary>
        protected bool m_display = false;

        /// <summary>
        /// The ID associated with the current state.
        /// </summary>
        protected StateID m_stateID = StateID.NULL_STATE;

        /// <summary>
        /// Input controller and handler ID associated with the state.
        /// </summary>
        protected Controller m_controller = Controller.System;

        /// <summary>
        /// The status associated with the current state.
        /// </summary>
        protected Status m_status = null;

        /// <summary>
        /// A list of game objects containing Entity components.
        /// </summary>
        protected List<GameObject> m_entities = null;

        /// <summary>
        /// A list of screen IDs containing references to IScreen implementations.
        /// </summary>
        protected List<ScreenID> m_screens = null;

        /// <summary>
        /// Index representing selected screen.
        /// </summary>
        protected int m_currentScreen = -1;

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Name of the game object.
        /// </summary>
        private string Name
        {
            get { return "State (" + StateManager.Parse(this.ID) + ")"; }
        }

        /// <summary>
        /// Returns initialization flag.
        /// </summary>
        public bool Initialized
        {
            get { return (this.m_initialized && !IsNull); }
        }

        /// <summary>
        /// Returns true if state is in null mode.
        /// </summary>
        public bool IsNull
        {
            get { return (this.m_stateID == StateID.NULL_STATE); }
        }

        /// <summary>
        /// Returns visibility flag.
        /// </summary>
        public bool Visible
        {
            get { return this.m_visible; }
        }

        /// <summary>
        /// Returns display flag.
        /// </summary>
        public bool DisplayGUI
        {
            get { return this.m_display; }
        }

        /// <summary>
        /// Returns true if collection of screens exist.
        /// </summary>
        public bool HasScreens
        {
            get { return (this.m_screens != null && this.m_screens.Count > 0); }
        }

        /// <summary>
        /// Returns the current state's ID.
        /// </summary>
        public StateID ID
        {
            get { return this.m_stateID; }
        }
        
        /// <summary>
        /// Return the status object.
        /// </summary>
        public Status Status
        {
            get { return this.m_status; }
        }

        /// <summary>
        /// Returns the list of screens.
        /// </summary>
        public List<ScreenID> Screens
        {
            get { return this.m_screens; }
        }

        /// <summary>
        /// Return the collection of game objects with Entity components.
        /// </summary>
        public virtual List<GameObject> Entities
        {
            get { return this.m_entities; }
        }

        /// <summary>
        /// Returns the current screen object.
        /// </summary>
        public abstract IScreen CurrentScreen
        {
            get;
        }

        #endregion

        #region Service Methods.

        #region UnityEngine methods.

        /// <summary>
        /// Start the state, implementing functionality that takes place before the first update.
        /// </summary>
        protected virtual void Start()
        {
            // Initialze the state.
            this.Initialize(StateID.NULL_STATE);
        }

        /// <summary>
        /// Update the state.
        /// </summary>
        protected virtual void Update()
        {
            if (Initialized)
            {
                // Handle input if initialized.
                HandleInput();

                if (this.Status.IsInactive())
                {
                    SetVisibility(false);
                    SetDisplay(false);
                }
            }
        }

        #endregion

        #region Initialization methods.

        /// <summary>
        /// General initialization of the state.
        /// </summary>
        public virtual void Initialize(StateID _state)
        {
            this.m_stateID = _state;

            // Create the status.
            this.m_status = gameObject.GetComponent<Status>();
            if (this.m_status == null)
            {
                Debugger.Print("Create the status object.");
                this.m_status = gameObject.AddComponent<Status>();
                this.m_status.Initialize();
            }

            this.m_controller = Controller.State;
            this.m_entities = new List<GameObject>();
            this.m_screens = new List<ScreenID>();
            this.m_visible = true;
            this.m_display = true;
            this.m_status.Activate();
            this.m_initialized = true;
        }
        
        #endregion

        #region Input methods.

        /// <summary>
        /// Creates a new control scheme to be used by the InputManager.
        /// </summary>
        private void BuildControlScheme()
        {
            // If a scheme doesn't exist, build a new one.
            if (GetScheme() == null)
            {
                Debugger.Print("Creating the control scheme for " + this.Name);
                InputManager.Instance.AddControlScheme(this.m_controller, new ControlScheme());
            }
        }

        /// <summary>
        /// Initialize the state's controls.
        /// </summary>
        protected abstract void InitializeControls();

        /// <summary>
        /// Handles all of the registered actions.
        /// </summary>
        protected abstract void HandleInput();

        /// <summary>
        /// Return this state's control scheme.
        /// </summary>
        /// <returns>Returns control scheme object.</returns>
        protected virtual ControlScheme GetScheme()
        {
            return InputManager.Instance.GetScheme(this.m_controller);
        }

        /// <summary>
        /// Wrapper function for handling input.
        /// </summary>
        /// <param name="_action">Action to check if trigger for.</param>
        /// <returns>Returns true if the action should be performed.</returns>
        public virtual bool GetAction(Actions _action)
        {
            return InputManager.Instance.GetAction(this.m_controller, _action);
        }

        /// <summary>
        /// Returns a value for a tracked axis.
        /// </summary>
        /// <param name="_name">Axis being tracked.</param>
        /// <returns>Returns a value.</returns>
        public virtual float GetAxis(string _name)
        {
            return InputManager.Instance.GetAxis(this.m_controller, _name);
        }

        /// <summary>
        /// Returns the raw value for a tracked axis.
        /// </summary>
        /// <param name="_name">Axis being tracked.</param>
        /// <returns>Returns a value.</returns>
        public virtual float GetAxisRaw(string _name)
        {
            return InputManager.Instance.GetAxisRaw(this.m_controller, _name);
        }

        /// <summary>
        /// Link an action to perform with a command.
        /// </summary>
        /// <param name="_action">Action to perform.</param>
        /// <param name="_command">Binding that will cause action.</param>
        /// <param name="_response">Response type that will trigger action.</param>
        public virtual void RegisterAction(Actions _action, Command _command, CommandResponseMode _response)
        {
            // Create a command response object that keeps track of the command, for triggering the action.
            CommandResponse response = new CommandResponse(_command, _response);
            CommandSequence sequence = new CommandSequence();
            sequence.Push(response);

            RegisterAction(_action, sequence);
        }

        /// <summary>
        /// Link an action to perform with a command input sequence.
        /// </summary>
        /// <param name="_action">Action to perform.</param>
        /// <param name="_sequence">Sequence of commands that will trigger the action.</param>
        public virtual void RegisterAction(Actions _action, CommandSequence _sequence)
        {
            AddControl(_action, _sequence);
        }

        /// <summary>
        /// Link an axis with a name and response trigger.
        /// </summary>
        /// <param name="_axis">Name of the axis-trigger pair.</param>
        /// <param name="_command">Axis read by trigger.</param>
        /// <param name="_response">Trigger.</param>
        protected virtual void RegisterAxis(string _axis, CommandTypes _type = CommandTypes.Axis, float _deadzone = 0.2f)
        {
            // Create a command response object that will keep track of the axis, triggering whenever it moves.
            CommandResponse response = new CommandResponse(new Command(_axis, _type, _deadzone), CommandResponseMode.NonZero);
            AddControl(_axis, response);
        }

        /// <summary>
        /// Link an action to a series of commands.
        /// </summary>
        /// <param name="_action">Action to perform.</param>
        /// <param name="_sequence">Series of responses needed to activate action.</param>
        protected void AddControl(Actions _action, CommandSequence _sequence)
        {
            // Add the control to the existing scheme.
            GetScheme().AddControl(_action, _sequence);
        }

        /// <summary>
        /// Link the axis to the command response.
        /// </summary>
        /// <param name="_axis">Axis name.</param>
        /// <param name="_response">Response triggering axis value.</param>
        protected void AddControl(string _axis, CommandResponse _response)
        {
            // Add tracking information for the axis in question.
            GetScheme().AddAxis(_axis, _response);
        }

        #endregion

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Pause the state.
        /// </summary>
        public virtual void Pause()
        {
            this.m_status.Pause();
        }
        
        /// <summary>
        /// Resume the state.
        /// </summary>
        public virtual void Resume()
        {
            this.m_status.Resume();
        }

        /// <summary>
        /// Reset the state.
        /// </summary>
        public virtual void ResetState()
        {
            this.m_status.TriggerReset();
        }

        /// <summary>
        /// Activate the state.
        /// </summary>
        public virtual void Activate()
        {
            this.m_status.Activate();
        }

        /// <summary>
        /// Deactivate the state.
        /// </summary>
        public virtual void Deactivate()
        {
            this.m_status.Deactivate();
        }

        /// <summary>
        /// Set the visibility flag.
        /// </summary>
        /// <param name="_visibility">Visibility flag.</param>
        public virtual void SetVisibility(bool _visibility)
        {
            this.m_visible = _visibility;
        }

        /// <summary>
        /// Set the display flag.
        /// </summary>
        /// <param name="_displayGUI">Display flag.</param>
        public virtual void SetDisplay(bool _displayGUI)
        {
            this.m_display = _displayGUI;
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns collection of all GameObjects containing Entity components in the scene.
        /// </summary>
        /// <returns>Returns list of GameObjects containing Entity components.</returns>
        public virtual List<GameObject> GetEntities()
        {
            return this.m_entities;
        }

        /// <summary>
        /// Return a screen object.
        /// </summary>
        /// <param name="id">Screen ID associated with requested screen.</param>
        /// <returns>Returns a screen object.</returns>
        public abstract IScreen GetScreen(ScreenID id);

        /// <summary>
        /// Check if the StateManager has been paused.
        /// </summary>
        /// <returns>Returns true if paused.</returns>
        public bool IsPaused()
        {
            return this.m_status.IsPaused();
        }
        
        #endregion

    }

    #endregion

    #region Interface: IState

    /////////////////////
    // Interface definition.
    /////////////////////

    /// <summary>
    /// IState interface that is implemented by the base State class.
    /// </summary>
    public interface IState
    {

        #region Service Methods

        #region Initialization Methods.

        /// <summary>
        /// Initialize the State and any of its elements.
        /// </summary>    
        void Initialize(StateID _state);
        
        #endregion
        
        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Pause all elements in the scene.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resume all the elements in the scene.
        /// </summary>
        void Resume();

        /// <summary>
        /// Activate all entities in the scene.
        /// </summary>
        void Activate();

        /// <summary>
        /// Deactivate all entities in the scene.
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Set visibility flag.
        /// </summary>
        /// <param name="_visibility">Visibility flag.</param>
        void SetVisibility(bool _visibility);

        /// <summary>
        /// Set display flag.
        /// </summary>
        /// <param name="_displayGUI">Display flag.</param>
        void SetDisplay(bool _displayGUI);

        #endregion

        #region Accessor Methods

        /// <summary>
        /// Returns collection of all GameObjects containing Entity components in the scene.
        /// </summary>
        /// <returns>Returns list of GameObjects containing Entity components.</returns>
        List<GameObject> GetEntities();

        /// <summary>
        /// Get IScreen returns the screen at the specified index. Since this is not the StateManager this method will not be needed.
        /// </summary>
        /// <param name="id">IScreen ID associated with desired IScreen object.</param>
        /// <returns></returns>
        IScreen GetScreen(ScreenID id);
        
        /// <summary>
        /// Returns true if the state is currently paused.
        /// </summary>
        /// <returns>Returns boolean holding value of query.</returns>
        bool IsPaused();
        
        #endregion

    }

    #endregion

}
