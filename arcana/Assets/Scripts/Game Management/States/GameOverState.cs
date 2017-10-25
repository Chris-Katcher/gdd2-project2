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

        #region Static Members.

        // TODO: Create component maker.

        #endregion

        #region Data Members

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
        /// The main menu should handle some input functionality.
        /// </summary>
        public override void Update()
        {
            // Call base method.
            base.Update();

            // Update when running.
            if (this.Status.IsRunning())
            {
                // Handle input.
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

            // Set the state ID.
            this.InitializeState(StateID.GameOverState);

            // TODO: Add the screen IDs.
        }

        #endregion

        /*
        #region Input Methods.

        protected override void HandleInput()
        {
            // TODO: Implement action handling.
            throw new NotImplementedException();
        }

        protected override void InitializeControls()
        {
            // TODO: Implement controls for the main menu controls.
            throw new NotImplementedException();
        }

        #endregion
        */

        #region Accessor Methods

        /// <summary>
        /// Return the requested Screen object.
        /// </summary>
        /// <param name="id">Screen ID associated with requested screen.</param>
        /// <returns>Returns a screen object.</returns>
        public sealed override IScreen GetScreen(ScreenID id)
        {
            // TODO: Implement wrapper function.
            throw new NotImplementedException();
        }

        #endregion

    }

    #endregion

}
