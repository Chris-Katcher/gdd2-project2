/************************************************
 * LoadingScreen.cs
 * 
 * This file contains implementation for the LoadingScreen class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.Resources;

namespace Game.UI.Screens
{
    /// <summary>
    /// IScreen displayed whenever there are resources that need loading.
    /// </summary>
    class LoadingScreen : IScreen
    {

        #region Data Members

        /////////////////////
        // Public data fields.
        /////////////////////

        /// <summary>
        /// Determines if IScreen is initialized.
        /// </summary>
        public bool m_initialized { get; set; }

        /// <summary>
        /// Determines if IScreen's resources are loaded.
        /// </summary>
        public bool m_loaded { get; set; }

        /// <summary>
        /// IScreen resolution (x-axis).
        /// </summary>
        public int m_screenResolutionX { get; set; }

        /// <summary>
        /// IScreen resolution (y-axis).
        /// </summary>
        public int m_screenResolutionY { get; set; }


        /////////////////////
        // Private data fields.
        /////////////////////

        /// <summary>
        /// Position of the screen. (Global Coordinates).
        /// </summary>
        private Vector2 m_position;

        /// <summary>
        /// Dimensions of the screen in 2D world space.
        /// </summary>
        private Vector2 m_dimensions;

        /// <summary>
        /// Length of time the screen should be active for.
        /// </summary>
        private float m_timeToLive;

        /// <summary>
        /// Resource for the screen? // TODO: Stub.
        /// </summary>
        private Resource m_resource;

        #endregion

        #region Service Methods

        /// <summary>
        /// Initialize this IScreen object.
        /// </summary>
        public void Initialize()
        {

            // TODO: Stub.

        }

        /// <summary>
        /// Load this IScreen object's resources.
        /// </summary>
        public void Load()
        {

            // TODO: Stub.

        }

        /// <summary>
        /// Update the stage on every frame.
        /// </summary>
        public void Update(float delta)
        {

            // TODO: Stub.

        }

        #endregion

        #region Mutator Methods

        // TODO: Stub.

        #endregion

        #region Accessor Methods

        // TODO: Stub.

        #endregion

    }
}
