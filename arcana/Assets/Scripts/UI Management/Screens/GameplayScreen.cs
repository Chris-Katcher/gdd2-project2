/************************************************
 * GameplayScreen.cs
 * 
 * This file contains implementation for the GameplayScreen class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Resources;
using Arcana.UI.Elements;
using Arcana.Cameras;

namespace Arcana.UI.Screens
{
    /// <summary>
    /// IScreen displayed during the main game, with the stage, obstacles, and player entities.
    /// </summary>
    public class GameplayScreen : ScreenBackground
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
                
                GUILabel instructions = UIManager.GetInstance().GetElement<GUILabel>("INSTRUCTIONS");

                this.Position = CameraManager.GetInstance().Camera.Camera.ScreenToWorldPoint(ScreenManager.Center);
                this.Position += new Vector3(0.0f, 0.0f, 55.0f);
                this.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);

                
                // Update the renderer.
                if (this.Status.IsVisible())
                {
                    instructions.SetVisible(true);
                    this.Renderer.Show();
                }
                else
                {
                    instructions.SetVisible(false);
                    this.Renderer.Hide();
                }

                if (!this.Status.IsActive())
                {
                    instructions.Enable(false);
                    this.Renderer.Deactivate();
                }

                // While alive, display the renderer.
                if (this.Status.IsAlive())
                {
                    instructions.Enable(true);
                    this.Renderer.Activate();
                }
                else
                {
                    instructions.Enable(false);
                    this.Renderer.Deactivate();
                }

                if (instructions != null)
                {
                    instructions.SetPosition(new Vector2(ScreenManager.Center.x, 20.0f));
                    instructions.UpdatePosition();
                    instructions.FontSize = (int)(50 * ScreenManager.SafetyScale);
                }
                
            }
        }
        
        #endregion
        
        #region Abstract Class Methods.

        /// <summary>
        /// Set up the resources for the renderer.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.DeactivateTimer();
            
            this.ScreenWidth = ScreenManager.WindowBounds.max.x;
            this.ScreenHeight = ScreenManager.WindowBounds.max.y;

            // Create a label.
            GUILabel logo = UIManager.GetInstance().CreateLabel("INSTRUCTIONS", "Defeat your opponent!", new Vector2(ScreenManager.Center.x, 10.0f));
            logo.UpdatePosition();
            logo.FontSize = 48;
            logo.Text.color = Color.black;

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
            CameraManager.GetInstance().Camera.CurrentConfiguration.InitialOffset = new Vector3(0, 0, -50.0f);

        }

        /// <summary>
        /// Set up the resources for the renderer.
        /// </summary>
        public override void InitializeRendererResources()
        {
            this.DeactivateTimer(); // Get rid of the timer.

            this.BackgroundID = "BG_ARENA";
            this.BackgroundPath = "Images/Backgrounds/bg_arena";
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
            this.ScreenID = ScreenID.GameplayScreen;
        }

        /// <summary>
        /// Initialize the main menu's screen name.
        /// </summary>
        public override void InitializeScreenName()
        {
            this.Name = "Gameplay Screen";
        }

        /// <summary>
        /// Update the main menu screen's position.
        /// </summary>
        public override void UpdatePosition()
        {
            // Maintain the screen as the center of the window.
            // this.Position = CameraManager.GetInstance().Camera.Camera.ScreenToWorldPoint(ScreenManager.Center);
        }

        #endregion

    }
}
