/************************************************
 * ScreenManager.cs
 * 
 * This file contains implementation for the ScreenManager class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.UI.Screens
{

    /// <summary>
    /// ID values associated with given screens.
    /// </summary>
    public enum ScreenID
    {
        SplashScreen = 0,
        LoadingScreen = 1,
        MainMenuScreen = 2,
        GameplayScreen = 3,
        GameoverScreen = 4,
        OptionsScreen = 5,
        PauseScreen = 6,
        NULL_SCREEN = 7
    }

    /// <summary>
    /// Creates and manages the IScreen objects.
    /// </summary>
    public class ScreenManager
    {
        #region Data Members

        /// <summary>
        /// Map of all IScreen instances, with their associated ID's.
        /// </summary> 
        private Dictionary<ScreenID, IScreen> m_screens { get; set; }

        #endregion

        #region Service Methods

        /// <summary>
        /// Initialize the StateManager.
        /// </summary>
        public void Initialize()
        {

            // TODO: Stub.

        }

        /// <summary>
        /// Load the current IState.
        /// </summary>
        public void Load()
        {

            // TODO: Stub.

        }

        /// <summary>
        /// Update is called every frame to update the position of game objects in the frame.
        /// </summary>
        /// <param name="delta">Elapsed time since last frame (in seconds).</param>
        public void Update(float delta)
        {

            // TODO: Stub.

        }

        #endregion

        #region Mutator Methods

        // TODO: Stub.

        #endregion

        #region Accessor Methods

        /// <summary>
        /// Get IScreen returns the screen at the specified ID.
        /// </summary>
        /// <param name="id">Desired IScreen's ID.</param>
        /// <returns>Returns IScreen associated with the input ID, if it exists.</returns>
        public IScreen GetScreen(ScreenID id)
        {
            if (m_screens != null && m_screens.ContainsKey(id))
            {
                return m_screens[id];
            }
            else
            {
                throw new ArgumentOutOfRangeException("Index was out of range of the list of screens");
            }
        }

        /// <summary>
        /// Returns the Dictionary mapping ScreenIDs to Screens.
        /// </summary>
        /// <returns>Returns <see cref="m_screens"/>.</returns>
        public Dictionary<ScreenID, IScreen> GetScreens()
        {
            return m_screens;
        }

        #endregion
    }
}
