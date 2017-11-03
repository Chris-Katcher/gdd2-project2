/************************************************
 * MainMenuScreen.cs
 * 
 * This file contains:
 * - The MainMenuScreen class. (Child of ScreenBackground).
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.States;
using Arcana.Cameras;
using Arcana.UI.Elements;

namespace Arcana.UI.Screens
{

    /// <summary>
    /// IScreen displayed as the main menu. It should have options for Play, Options, and Exit. Displayed after splash screen.
    /// </summary>
    public class MainMenuScreen : ScreenBackground
    {

        #region Data Members

        /// <summary>
        /// The screen ID that should this menu will be changed to.
        /// </summary>
        private StateID m_nextStateID = StateID.NULL_STATE;

        /// <summary>
        /// Returns true if next state exists.
        /// </summary>
        public bool HasNextState
        {
            get { return this.m_nextStateID != StateID.NULL_STATE; }
        }

        /// <summary>
        /// Reference to the next state.
        /// </summary>
        public StateID NextStateID
        {
            get { return this.m_nextStateID; }
        }

        #endregion

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

                GUIImage image = UIManager.GetInstance().GetElement<GUIImage>("Background");
                GUILabel logo = UIManager.GetInstance().GetElement<GUILabel>("Logo");

                // Update the renderer.
                if (this.Status.IsVisible())
                {
                    logo.SetVisible(true);
                    image.SetVisible(true);
                }
                else
                {
                    logo.SetVisible(false);
                    image.SetVisible(false);
                }

                if (!this.Status.IsActive())
                {
                    logo.Enable(false);
                    image.Enable(false);
                }

                // While alive, display the renderer.
                if (this.Status.IsAlive())
                {
                    logo.Enable(true);
                    image.Enable(true);
                }
                else
                {
                    logo.Enable(false);
                    image.Enable(false);
                }

                if (logo != null)
                {
                    logo.SetPosition(CameraManager.GetInstance().Camera.Camera.WorldToScreenPoint(this.Position) + new Vector3(0.0f, 65.0f, 0.0f));
                    logo.UpdatePosition();
                    logo.Message = "Arcana";
                    logo.FontSize = (int)(200 * ScreenManager.SafetyScale);
                }

                if (image != null)
                {
                    image.SetPosition(ScreenManager.Center);
                    image.SetSize(ScreenManager.WindowBounds.max);
                }

            }
        }

        public void OnGUI()
        {
            if (this.Status.IsAlive())
            {
                Vector2 buttonDimensions = new Vector2(200, 40);
                Vector2 center = ScreenManager.Center - buttonDimensions / 2;
                Vector2 offset = new Vector2(0.0f, 10.0f + buttonDimensions.y);

                // Button for starting the game.
                if (GUI.Button(new Rect(center + offset, buttonDimensions), "Start Game"))
                {
                    this.m_nextStateID = StateID.ArenaState;
                    this.Status.Kill();
                }

                // Button for quiting.
                if (GUI.Button(new Rect(center + (offset * 2), buttonDimensions), "Exit"))
                {
                    foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
                    {
                        if (obj != this.Self)
                        {
                            if (obj.GetComponent<ArcanaObject>() != null)
                            {
                                obj.GetComponent<ArcanaObject>().Status.Destroy();
                            }
                            else
                            {
                                Destroy(obj);
                            }
                        }
                    }
                    this.Status.Destroy();
                    Application.Quit();
                }
            }

        }

        #endregion

        #region Abstract Class Methods.

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
            CameraManager.GetInstance().Camera.CurrentConfiguration.InitialOffset = new Vector3(0, 0, -25.0f);

            // Create a background.
            GUIImage image = UIManager.GetInstance().CreateImage("Background", this.BackgroundID, this.BackgroundPath, ScreenManager.Center, ScreenManager.WindowBounds.max);
            image.transform.SetAsFirstSibling();

            // Create a label.
            GUILabel logo = UIManager.GetInstance().CreateLabel("Logo", "Arcana", this.Position + new Vector3(0.0f, 65.0f, 0.0f));
            logo.UpdatePosition();
            logo.FontSize = 120;
            logo.Text.color = Color.black;

            // Set time.
            this.Revive();

            // Maintain the screen
        }

        /// <summary>
        /// Set up the resources for the renderer.
        /// </summary>
        public override void InitializeRendererResources()
        {
            this.DeactivateTimer();

            this.BackgroundID = "BG_MENU";
            this.BackgroundPath = "Images/Backgrounds/bg_menu";
            this.MaterialID = "MAT_MENU";
            this.MaterialPath = "Materials/Screens/mat_menu";

            this.Renderer.Deactivate();
            this.Renderer.Hide(); // Hide the renderer. Use a GUIImage instead.
            this.Renderer.enabled = false;
            this.Renderer.DestroySelf();
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
            this.ScreenID = ScreenID.MainMenuScreen;
        }

        /// <summary>
        /// Initialize the main menu's screen name.
        /// </summary>
        public override void InitializeScreenName()
        {
            this.Name = "Main Menu";
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
