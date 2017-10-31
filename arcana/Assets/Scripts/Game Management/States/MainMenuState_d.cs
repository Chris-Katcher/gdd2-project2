/*

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

namespace Arcana.States
{

    #region Factory: MainMenuFactory declaration.

    /////////////////////
    // Factory declaration.
    /////////////////////

    /// <summary>
    /// Create the main menu state.
    /// </summary>
    public class MainMenuFactory : StateFactory<MainMenuState>
    {

        #region Static Methods.

        public static MainMenuFactory Instance()
        {
            if (instance == null)
            {
                instance = new MainMenuFactory();
            }

            return (MainMenuFactory)instance;
        }

        #endregion

        #region Factory methods.

        /// <summary>
        /// Get the instance of this factory.
        /// </summary>
        /// <returns>Return the MainMenuFactory.</returns>
        public override StateFactory<MainMenuState> GetInstance()
        {
            return Instance();
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
            State mainMenu = parent.GetComponent<MainMenuState>();

            // If the state is still null.
            if (mainMenu == null)
            {
                // If it doesn't exist, create a new one.
                Debugger.Print("Create and add the Main Menu state.");
                mainMenu = Services.AddChild(parent, Services.CreateEmptyObject("State (Main Menu)")).AddComponent<MainMenuState>();
            }

            // Assign non-optional information.
            mainMenu.Initialize(StateID.MainMenuState);

            return mainMenu;
        }

        #endregion

    }

    #endregion

    #region Class: MainMenuState class.

    /////////////////////
    // State declaration.
    /////////////////////

    /// <summary>
    /// <para>Implements the state run at the start of the game.</para>
    /// It will display a screen object called "MainMenuScreen" and update frames as needed. When inputs are triggered to change the state of the StageManager this screen will stop being displayed.
    /// </summary>
    public class MainMenuState_d : State
    {

        #region Data Members


        #endregion

        #region Service Methods.

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

        #region Accesor Methods.

        
        #endregion

    }

    #endregion

}
*/