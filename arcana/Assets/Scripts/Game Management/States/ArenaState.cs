/************************************************
 * ArenaState.cs
 * 
 * This file contains:
 * - The ArenaState class. (Child of State).
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
using Arcana.Environment;

namespace Arcana.States
{

    #region Class: ArenaState class.

    /////////////////////
    // State declaration.
    /////////////////////

    /// <summary>
    /// <para>Implements the main gameplay state.</para>
    /// It will display a screen object called "GamePlayScreen" and update frames as needed. When inputs are triggered to change the state of the StageManager this screen will stop being displayed.
    /// </summary>
    [AddComponentMenu("Arcana/States/Arena")]
    public class ArenaState : State
    {

        #region Data Members

        #region Fields.

        /// <summary>
        /// Time to switch states.
        /// </summary>
        private float timeToLive;

        private GameObject environment;

        #endregion
        
        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// The arena should handle some input functionality.
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

                if (this.Status.IsActive())
                {
                    if (!this.Status.IsPaused())
                    {
                        environment.SetActive(true);
                        CameraManager.GetInstance().SetCameraTargetAll();
                        CameraManager.GetInstance().ChangeBackground(Color.gray);
                        this.SwitchScreen(ScreenID.GameplayScreen);
                    }
                    else
                    {
                        environment.SetActive(false);
                        CameraManager.GetInstance().ChangeBackground(Color.black);
                        this.SwitchScreen(ScreenID.PauseScreen);
                    }

                    if (this.m_currentScreenID == ScreenID.GameplayScreen)
                    {
                        // Create the platforms.




                    }
                }

                #region Debug Functionality.

                if (this.Debug)
                {
                    // Update when running.
                    if (this.Status.IsActive())
                    {
                        Debugger.Print("Running arena state.", this.Self.name, this.Debug);

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
                            this.SetNextState(StateID.GameOverState);
                        }
                    }
                }

                #endregion
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Add (and request) the screens for the Arena.
        /// </summary>
        public sealed override void Initialize()
        {
            // Initialize base data members.
            base.Initialize();

            // Initialize members.                  
            this.Name = "Arcana (Arena State)"; // Set the object name.
            this.Revive();

            // Initialize the platforms.
            environment = Instantiate(UnityEngine.Resources.Load("Environment")) as GameObject;
            
            this.ResetState();
        }
        
        /// <summary>
        /// Set the state to the arena state.
        /// </summary>
        public sealed override void InitializeState()
        {
            this.m_stateID = StateID.ArenaState;
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
            this.SwitchScreen(ScreenID.GameplayScreen);
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
