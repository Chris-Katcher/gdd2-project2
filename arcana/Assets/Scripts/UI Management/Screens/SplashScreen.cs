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
using Arcana.Resources;

namespace Arcana.UI.Screens
{
    /// <summary>
    /// IScreen displayed at game start up. It will say, "Press any key to continue," and display a logo.
    /// </summary>
    public class SplashScreen : ScreenBackground
    {

        #region Abstract Class Methods.

        /// <summary>
        /// Set up the resources for the renderer.
        /// </summary>
        public override void InitializeRendererResources()
        {
            this.BackgroundID = "BG_SPLASH";
            this.BackgroundPath = "Images/Backgrounds/bg_splash";
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
            this.Position = ScreenManager.Center;
        }

        #endregion

    }
}
