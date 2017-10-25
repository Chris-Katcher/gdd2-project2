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
        /// A list of screen IDs containing references to IScreen implementations.
        /// </summary>
        protected List<ScreenID> m_screens = null;

        /// <summary>
        /// Index representing selected screen.
        /// </summary>
        protected int m_currentScreen = -1;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////
        
        /// <summary>
        /// Returns true if state is in null mode.
        /// </summary>
        public override bool IsNull
        {
            get { return (base.IsNull || this.m_stateID == StateID.NULL_STATE); }
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
        /// Returns the list of screens.
        /// </summary>
        public List<ScreenID> Screens
        {
            get { return this.m_screens; }
        }
        
        /// <summary>
        /// Returns the current screen object.
        /// </summary>
        public abstract IScreen CurrentScreen
        {
            get;
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
            this.InitializeMembers();

            // Initialize the state.
            this.InitializeState(StateID.NULL_STATE);
        }

        /// <summary>
        /// Set the state ID.
        /// </summary>
        public virtual void InitializeState(StateID _state)
        {
            this.m_stateID = _state;
        }

        /// <summary>
        /// Initialize this class's data members.
        /// </summary>
        protected virtual void InitializeMembers()
        {
            this.m_entities = new List<Entity>();
            this.m_screens = new List<ScreenID>();
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
        public abstract IScreen GetScreen(ScreenID id);
        
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
        void InitializeState(StateID _state);

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
