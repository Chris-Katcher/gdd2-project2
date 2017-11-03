/************************************************
 * SplashScreen.cs
 * 
 * This file contains implementation for the SplashScreen class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.UI.Elements;
using Arcana.Resources;
using Arcana.Cameras;

namespace Arcana.UI.Screens
{
    /// <summary>
    /// IScreen displayed at game start up. It will say, "Press any key to continue," and display a logo.
    /// </summary>
    public class SplashScreen : ScreenBackground
    {

        #region Update Methods.

        /// <summary>
        /// Update the splash screen.
        /// </summary>
        public override void Update()
        {
            if (!this.Initialized)
            {
                this.Initialize();
            }
            else
            {
                // Call the base function.
                base.Update();

                GUILabel logo = UIManager.GetInstance().GetElement<GUILabel>("Logo");
                
                if(logo != null)
                {
                    logo.SetPosition(CameraManager.GetInstance().Camera.Camera.WorldToScreenPoint(this.Position) + new Vector3(-5.0f, -50.0f, 0.0f));
                    logo.UpdatePosition();
                    logo.Message = "Arcana";
                    logo.FontSize = (int)(120 * ScreenManager.SafetyScale);
                }

                // Update the renderer.
                if (this.Status.IsVisible())
                {
                    this.Renderer.Show();
                    logo.SetVisible(true);
                }
                else
                {
                    this.Renderer.Hide();
                    logo.SetVisible(false);
                }

                if (!this.Status.IsActive())
                {
                    this.Renderer.Deactivate();
                    logo.Enable(false);
                }

                // While alive, display the renderer.
                if (this.Status.IsAlive())
                {
                    Debugger.Print("Time left to live: " + this.TimeToLive + " seconds.", this.Self.name, this.Debug);
                    this.UpdateTime();
                    this.Renderer.Activate();
                    logo.Enable(true);
                }
            }
        }

        #endregion

        #region Abstract Class Methods.

        /// <summary>
        /// Initialize the splash screen.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.Debug = true;

            foreach (CameraTarget t in ScreenManager.Targets)
            {
                CameraManager.GetInstance().Camera.RemoveTarget(t);
            }

            // Add a camera target.
            CameraTarget target = CameraTarget.Create(this);
            target.Initialize();
            target.Activate();
            ScreenManager.Targets.Add(target);

            // Add the target to the camera manager.
            CameraManager.GetInstance().AddTarget(target);
            CameraManager.GetInstance().SetCameraTargetOne();
            CameraManager.GetInstance().Camera.CurrentConfiguration.InitialOffset = new Vector3(0, 0, -30.0f);

            // Create a label.
            GUILabel logo = UIManager.GetInstance().CreateLabel("Logo", "Arcana", this.Position + new Vector3(-5.0f, -50.0f, 0.0f));
            logo.UpdatePosition();
            logo.FontSize = 80;

            // Set time.
            this.Revive();
            
            // Maintain the screen as the center of the window.
            UpdatePosition();
        }

        /// <summary>
        /// Set up the resources for the renderer.
        /// </summary>
        public override void InitializeRendererResources()
        {
            this.SetTimer(15.0f); // Give it a time limit of 10 seconds.

            this.BackgroundID = "BG_SPLASH";
            this.BackgroundPath = "Images/Backgrounds/bg_splash_logo";
            this.MaterialID = "MAT_MENU";
            this.MaterialPath = "Materials/Screens/mat_menu";
        }

        /// <summary>
        /// Initialize the main menu's screen's dimensions.
        /// </summary>
        public override void InitializeDimensions()
        {
            this.ScreenWidth = ScreenManager.WindowWidth * ScreenManager.Safety.x;
            this.ScreenHeight = ScreenManager.WindowHeight * ScreenManager.Safety.y;            
        }

        /// <summary>
        /// Initialize the main menu's screen ID.
        /// </summary>
        public override void InitializeScreenID()
        {
            this.ScreenID = ScreenID.SplashScreen;
        }

        /// <summary>
        /// Initialize the main menu's screen name.
        /// </summary>
        public override void InitializeScreenName()
        {
            this.Name = "Splash Screen";
        }

        /// <summary>
        /// Update the main menu screen's position.
        /// </summary>
        public override void UpdatePosition()
        {
            // Maintain the screen as the center of the window.
            this.Position = CameraManager.GetInstance().Camera.Camera.ScreenToWorldPoint(ScreenManager.Center);
        }

        #endregion

    }
}
