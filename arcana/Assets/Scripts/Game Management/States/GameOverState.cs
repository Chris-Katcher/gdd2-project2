/************************************************
 * GameOverState.cs
 * 
 * This file contains:
 * - The GameOverState class. (Child of State).
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

namespace Arcana.States
{

    #region Class: GameOverState class.

    /////////////////////
    // State declaration.
    /////////////////////

    /// <summary>
    /// <para>Implements the end of game state.</para>
    ///  It will display a screen object called "GameoverScreen" and update frames as needed. When inputs are triggered to change the state of the StageManager this screen will stop being displayed.
    /// </summary>
    [AddComponentMenu("Arcana/States/Game Over")]
    public class GameOverState : State
    {

        #region Data Members

        #region Fields.

        /// <summary>
        /// Time to switch states.
        /// </summary>
        private float timeToLive;

        #endregion

        #region Properties

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Return the current screen.
        /// </summary>
        public sealed override IScreen CurrentScreen
        {
            get
            {
                // TODO: Fix this implementation in the base class.
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// The game over state should handle some input functionality.
        /// </summary>
        public override void Update()
        {
            if (!this.Initialized)
            {
                this.Initialize();
            }
            else
            {
                // Call base method.
                base.Update();

                // Update when running.
                if (this.Status.IsActive())
                {
                    Debugger.Print("Running game over state.", this.Self.name, this.Debug);

                    if (this.Status.IsPaused())
                    {
                        Debugger.Print("State is paused.", this.Self.name, this.Debug);
                    }
                    else
                    {
                        this.timeToLive = Services.Max(this.timeToLive - Time.fixedDeltaTime, 0.0f);
                        Debugger.Print("Time until state switch: " + this.timeToLive + " seconds.", this.Self.name, this.Debug);
                    }

                    if (timeToLive == 0.0f)
                    {
                        this.SetNextState(StateID.MainMenuState);
                    }
                }
            }
        }

        #endregion
        
        #region Initialization Methods.

        /// <summary>
        /// Add (and request) the screens for the GameOver state.
        /// </summary>
        public sealed override void Initialize()
        {
            // Initialize base data members.
            base.Initialize();

            // Initialize members.                  
            this.Name = "Arcana (Game Over State)"; // Set the object name.
            this.Revive();
            this.ResetState();
        }

        /// <summary>
        /// Set the state to the arena state.
        /// </summary>
        public sealed override void InitializeState()
        {
            this.m_stateID = StateID.GameOverState;
        }

        #endregion
        
        #region Mutator Methods.

        /// <summary>
        /// Reset the state.
        /// </summary>
        public override void ResetState()
        {
            this.timeToLive = 5.0f; // 5 seconds to live.
            this.SetNextState(StateID.NULL_STATE);
            this.m_currentScreenID = ScreenID.GameoverScreen;
        }

        #endregion
    }

    #endregion

}
