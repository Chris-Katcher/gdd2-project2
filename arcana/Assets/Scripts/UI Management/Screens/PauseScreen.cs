/************************************************
 * PauseScreen.cs
 * 
 * This file contains implementation for the PauseScreen class.
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
    /// IScreen displayed when the game presses the pause button.
    /// </summary>
    public class PauseScreen : ScreenBase
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

                GUIPanel panel = UIManager.GetInstance().GetElement<GUIPanel>("BG_PAUSE");
                GUILabel instructions = UIManager.GetInstance().GetElement<GUILabel>("PAUSE");

                // Update the renderer.
                if (this.Status.IsVisible())
                {
                    instructions.SetVisible(true);
                    panel.SetVisible(true);
                }
                else
                {
                    instructions.SetVisible(false);
                    panel.SetVisible(false);
                }

                if (!this.Status.IsActive())
                {
                    instructions.Enable(false);
                    panel.Enable(false);
                }

                // While alive, display the renderer.
                if (this.Status.IsAlive())
                {
                    instructions.Enable(true);
                    panel.Enable(true);
                }
                else
                {
                    instructions.Enable(false);
                    panel.Enable(false);
                }

                if (instructions != null)
                {
                    instructions.SetPosition(ScreenManager.Center);
                    instructions.UpdatePosition();
                    instructions.FontSize = (int)(50 * ScreenManager.SafetyScale);
                }

                if (panel != null)
                {
                    panel.SetPosition(ScreenManager.Center);
                    panel.SetSize(ScreenManager.WindowBounds.max);
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
                        
            GUIPanel panel = UIManager.GetInstance().CreatePanel("BG_PAUSE", ScreenManager.Center, ScreenManager.WindowBounds.max);
            panel.transform.SetAsFirstSibling();

            // Create a label.
            GUILabel pauselabel = UIManager.GetInstance().CreateLabel("PAUSE", "Press Start to Resume.", ScreenManager.Center);
            pauselabel.UpdatePosition();
            pauselabel.FontSize = 50;
            pauselabel.Text.color = Color.white;

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
            this.ScreenID = ScreenID.PauseScreen;
        }

        /// <summary>
        /// Initialize the main menu's screen name.
        /// </summary>
        public override void InitializeScreenName()
        {
            this.Name = "Pause Screen";
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
