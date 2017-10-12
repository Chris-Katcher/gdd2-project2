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
using Game.UI.Screens;

namespace Game.States
{
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
    
    /// <summary>
    /// Manager responsible for the IState items.
    /// </summary>
    public class StateManager
    {

        #region Data Members

        /// <summary>
        /// Map of all IState instances, with their associated ID's.
        /// </summary>
        public Dictionary<StateID, IState> m_states { get; set; }
        
        /// <summary>
        /// Active state.
        /// </summary>
        public IState m_currentState { get; set; }

        #endregion

        #region Service Methods

        /// <summary>
        /// Initialize the StateManager.
        /// </summary>
        public void Initialize()
        {

            // TODO: Stub.

        }
        
        /// <summary>
        /// Load the current IState.
        /// </summary>
        public void Load()
        {

            // TODO: Stub.

        }

        /// <summary>
        /// Update is called every frame to update the position of game objects in the frame.
        /// </summary>
        /// <param name="delta">Elapsed time since last frame (in seconds).</param>
        public void Update(float delta)
        {

            // TODO: Stub.

        }

        #endregion

        #region Mutator Methods

        // TODO: Stub.

        #endregion

        #region Accessor Methods
        
        // TODO: Stub.

        #endregion

    }
}
