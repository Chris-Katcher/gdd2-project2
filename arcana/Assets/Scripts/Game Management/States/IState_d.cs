/*
 * Has control scheme template.

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



}
*/