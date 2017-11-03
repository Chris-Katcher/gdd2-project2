/************************************************
 * MainMenuState.cs
 * 
 * This file contains:
 * - The MainMenuState class. (Child of State).
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
using Arcana.Cameras;

namespace Arcana.States
{

    #region Class: MainMenuState class.

    /////////////////////
    // State declaration.
    /////////////////////

    /// <summary>
    /// Main Menu state displays screens for the main menu of the game.
    /// </summary>
    [AddComponentMenu("Arcana/States/Main Menu")]
    public class MainMenuState : State
    {

        #region Data Members

        #region Fields.

        /// <summary>
        /// Time to switch states.
        /// </summary>
        private float timeToLive;
        
        #endregion
        
        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// The main menu should handle some input functionality.
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
                    // Based on the screen.
                    switch (this.m_currentScreenID)
                    {
                        case ScreenID.SplashScreen:
                            CameraManager.GetInstance().ChangeBackground(Services.GetColor(0, 0, 0));
                            SplashScreen screen1 = this.CurrentScreen as SplashScreen;
                            if (screen1.Status.IsDead())
                            {
                                SwitchScreen(ScreenID.MainMenuScreen);
                            }
                            break;
                        case ScreenID.MainMenuScreen:
                            MainMenuScreen screen2 = this.CurrentScreen as MainMenuScreen;
                            screen2.DeactivateTimer();
                            if (screen2.HasNextState)
                            {
                                this.m_nextStateID = screen2.NextStateID;
                            }
                            break;
                    }
                    
                    #region Debug Functionality.

                    if (this.Debug)
                    {
                        Debugger.Print("Running main menu.", this.Self.name, this.Debug);

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
                            this.SetNextState(StateID.ArenaState);
                        }
                    }

                    #endregion
                }
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Add (and request) the screens for the Main Menu.
        /// </summary>
        public sealed override void Initialize()
        {
            // Initialize base data members and collection.
            base.Initialize();

            // Initialize members.                  
            this.Name = "Arcana (Main Menu State)"; // Set the object name.
            this.Revive();
            this.ResetState();
        }

        /// <summary>
        /// Set the state to the menu state.
        /// </summary>
        public sealed override void InitializeState()
        {
            this.m_stateID = StateID.MainMenuState;
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
            this.SwitchScreen(ScreenID.SplashScreen);
        }

        /// <summary>
        /// Switch screens.
        /// </summary>
        /// <param name="_id">Screen to switch to.</param>
        public void SwitchScreen(ScreenID _id)
        {
            if (this.m_currentScreenID != _id)
            {
                ScreenBase screen = this.CurrentScreen as ScreenBase;
                if (screen != null)
                {
                    screen.Deactivate();
                    screen.Kill();
                    screen.Hide();
                    screen.HideGUI();
                }

                this.m_currentScreenID = _id;
                screen = this.CurrentScreen as ScreenBase;
                screen.Revive();
                screen.Activate();
                screen.Show();
                screen.ShowGUI();
            }
        }

        #endregion

    }

    #endregion

}
