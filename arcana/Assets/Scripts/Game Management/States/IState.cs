/************************************************
 * IState.cs
 * 
 * This file contains:
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
using Arcana.Entities;
using Arcana.Utilities;
using Arcana.UI.Screens;
using Arcana.InputManagement;

namespace Arcana.States
{

    #region Class: State base class.

    /////////////////////
    // Base class declaration.
    /////////////////////

    /// <summary>
    /// An interface describing the fields and methods that will be implemented by each of the IState classes.
    /// </summary>
    public abstract class State : ArcanaObject, IState
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
            return StateManager.Parse(_id);
        }

        #endregion

        #endregion

        #region Data Members

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// The ID associated with the current state.
        /// </summary>
        protected StateID m_stateID = StateID.NULL_STATE;

        /// <summary>
        /// A list of contained Entity components.
        /// </summary>
        protected List<Entity> m_entities = null;

        /// <summary>
        /// Reference to the current screen.
        /// </summary>
        protected ScreenID m_currentScreenID = ScreenID.NULL_SCREEN;
        
        /// <summary>
        /// The ID associated with the state to change to.
        /// </summary>
        protected StateID m_nextStateID = StateID.NULL_STATE;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns true if screen exists.
        /// </summary>
        public bool HasScreen
        {
            get { return (this.m_currentScreenID != ScreenID.NULL_SCREEN); }
        }

        /// <summary>
        /// Returns the current state's ID.
        /// </summary>
        public StateID ID
        {
            get { return this.m_stateID; }
        }

        /// <summary>
        /// Reference to the next state's ID.
        /// </summary>
        public StateID NextStateID
        {
            get { return this.m_nextStateID; }
        }

        /// <summary>
        /// Returns true if the next state ID isn't null.
        /// </summary>
        public bool HasNextState
        {
            get { return this.NextStateID != StateID.NULL_STATE; }
        }
        
        /// <summary>
        /// Returns the current screen object.
        /// </summary>
        public virtual IScreen CurrentScreen
        {
            get
            {
                return GetScreen(this.m_currentScreenID);
            }
        }

        #endregion

        #endregion

        #region Initialization methods.

        /// <summary>
        /// Initialize the state.
        /// </summary>
        public override void Initialize()
        {
            // Initialize as ArcanaObject.
            base.Initialize();

            // Initialize data members.
            this.m_stateID = StateID.NULL_STATE; // When just calling base class, ensure it is null.
            this.m_nextStateID = StateID.NULL_STATE; // The next state will be null until set to something else.
            this.InitializeMembers(); // Initialize the collections.    
            this.InitializeState(); // Initialize the state.
        }

        /// <summary>
        /// Set the state ID.
        /// </summary>
        public abstract void InitializeState();

        /// <summary>
        /// Initialize this class's data members.
        /// </summary>
        protected virtual void InitializeMembers()
        {
            this.m_entities = new List<Entity>();
            this.m_currentScreenID = ScreenID.NULL_SCREEN;
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns collection of all contained Entity components in the scene.
        /// </summary>
        /// <returns>Returns list of contained Entity components.</returns>
        public virtual List<Entity> GetEntities()
        {
            return this.GetChildren<Entity>();
        }

        /// <summary>
        /// Return a screen object.
        /// </summary>
        /// <param name="id">Screen ID associated with requested screen.</param>
        /// <returns>Returns a screen object.</returns>
        public virtual IScreen GetScreen(ScreenID id)
        {
            return ScreenManager.GetInstance().GetScreen(id);
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set the next state.
        /// </summary>
        /// <param name="_state">State to set next state to.</param>
        public void SetNextState(StateID _state)
        {
            this.m_nextStateID = _state;
        }

        /// <summary>
        /// Reset the state.
        /// </summary>
        public abstract void ResetState();

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

        #region Initialization Methods.

        /// <summary>
        /// Initialize the State and any of its elements.
        /// </summary>    
        void InitializeState();

        #endregion

        #region Accessor Methods

        /// <summary>
        /// Returns collection of all contained Entity components in the scene.
        /// </summary>
        /// <returns>Returns list of contained Entity components.</returns>
        List<Entity> GetEntities();

        /// <summary>
        /// Get IScreen returns the screen at the specified index. Since this is not the StateManager this method will not be needed.
        /// </summary>
        /// <param name="id">IScreen ID associated with desired IScreen object.</param>
        /// <returns></returns>
        IScreen GetScreen(ScreenID id);        

        #endregion

    }

    #endregion

}
