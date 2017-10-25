/************************************************
 * GameOverState.cs
 * 
 * This file contains:
 * - The GameOverFactory factory.
 * - The GameOverState class (State sub-class).
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

namespace Arcana.States
{

    #region Factory: GameOverFactory declaration.

    /////////////////////
    // Factory declaration.
    /////////////////////

    /// <summary>
    /// Create the Game Over state.
    /// </summary>
    public class GameOverFactory : StateFactory<GameOverState>
    {

        #region Static Methods.

        public static GameOverFactory Instance()
        {
            if (instance == null)
            {
                instance = new GameOverFactory();
            }

            return (GameOverFactory)instance;
        }

        #endregion

        #region Factory methods.

        /// <summary>
        /// Get the instance of this factory.
        /// </summary>
        /// <returns>Return the factory.</returns>
        public override StateFactory<GameOverState> GetInstance()
        {
            if (instance == null)
            {
                instance = new GameOverFactory();
            }

            return instance;
        }

        /// <summary>
        /// Adds a new component to the parent game objec.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <returns>Return newly created component.</returns>
        public override State CreateComponent(GameObject parent)
        {
            // Check game object.
            if (parent == null)
            {
                // If the parent itself is null, do not return a component.
                Debugger.Print("Tried to add a component but parent GameObject is null.", "NULL_REFERENCE");
                return null;
            }

            // Get reference to it from the existing script if it already exists.
            State state = parent.GetComponent<GameOverState>();

            // If the state is still null.
            if (state == null)
            {
                // If it doesn't exist, create a new one.
                Debugger.Print("Create and add the Game Over state.");
                state = Services.AddChild(parent, Services.CreateEmptyObject("State (Game Over)")).AddComponent<GameOverState>();
            }

            // Assign non-optional information.
            state.Initialize(StateID.GameOverState);

            return state;
        }

        #endregion

    }

    #endregion

    #region Class: GameOverState class.

    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// <para>Implements the end of game state.</para>
    ///  It will display a screen object called "GameoverScreen" and update frames as needed. When inputs are triggered to change the state of the StageManager this screen will stop being displayed.
    /// </summary>
    public class GameOverState : State
    {

        #region Data Members

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

        #region Service Methods

        #region Initialization Methods.

        /// <summary>
        /// General initialization of the state.
        /// </summary>
        public sealed override void Initialize(StateID _state)
        {
            base.Initialize(_state);
            // TODO: Initialize all the state's properties.
            // throw new NotImplementedException();
        }

        #endregion

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

        #endregion

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
