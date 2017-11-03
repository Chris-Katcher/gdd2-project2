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
    public class GameplayScreen : ScreenBase
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

                GUIImage image = UIManager.GetInstance().GetElement<GUIImage>("BG_ARENA");
                GUILabel instructions = UIManager.GetInstance().GetElement<GUILabel>("INSTRUCTIONS");

                // Update the renderer.
                if (this.Status.IsVisible())
                {
                    instructions.SetVisible(true);
                    image.SetVisible(true);
                }
                else
                {
                    instructions.SetVisible(false);
                    image.SetVisible(false);
                }

                if (!this.Status.IsActive())
                {
                    instructions.Enable(false);
                    image.Enable(false);
                }

                // While alive, display the renderer.
                if (this.Status.IsAlive())
                {
                    instructions.Enable(true);
                    image.Enable(true);
                }
                else
                {
                    instructions.Enable(false);
                    image.Enable(false);
                }

                if (instructions != null)
                {
                    instructions.SetPosition(new Vector2(ScreenManager.Center.x, 20.0f));
                    instructions.UpdatePosition();
                    instructions.FontSize = (int)(50 * ScreenManager.SafetyScale);
                }

                if (image != null)
                {
                    image.SetPosition(ScreenManager.Center);
                    image.SetSize(ScreenManager.WindowBounds.max);
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

            GUIImage image = UIManager.GetInstance().CreateImage("BG_ARENA", "BG_ARENA", "Images/Backgrounds/bg_arena", ScreenManager.Center, ScreenManager.WindowBounds.max);
            image.transform.SetAsFirstSibling();

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
            this.Position = CameraManager.GetInstance().Camera.Camera.ScreenToWorldPoint(ScreenManager.Center);
        }

        #endregion

    }
}
