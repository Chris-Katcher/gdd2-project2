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

namespace Arcana.UI.Screens
{

    /// <summary>
    /// IScreen displayed as the main menu. It should have options for Play, Options, and Exit. Displayed after splash screen.
    /// </summary>
    public class MainMenuScreen : ScreenBackground
    {

        #region Abstract Class Methods.

        /// <summary>
        /// Set up the resources for the renderer.
        /// </summary>
        public override void InitializeRendererResources()
        {
            this.BackgroundID = "BG_MENU";
            this.BackgroundPath = "Images/Backgrounds/bg_menu";
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
            this.Position = ScreenManager.Center;
        }

        #endregion
        
    }
}
