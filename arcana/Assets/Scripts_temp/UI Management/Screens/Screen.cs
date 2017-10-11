/************************************************
 * IScreen.cs
 * 
 * Contains implementation for the IScreen interface.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.UI.Screens
{

    /// <summary>
    /// Defines the public data members and class references for each IScreen type classes.
    /// </summary>
    public interface IScreen
    {

        #region Data Members
        
        /////////////////////
        // Public data fields.
        /////////////////////

        /// <summary>
        /// Determine if IScreen object is initialized.
        /// </summary>
        bool m_initialized { get; set; }
        
        /// <summary>
        /// Determine if IScreen object resources are loaded.
        /// </summary>
        bool m_loaded { get; set; }

        /// <summary>
        /// IScreen resolution (x-axis).
        /// </summary>
        int m_screenResolutionX { get; set; }

        /// <summary>
        /// IScreen resolution (y-axis).
        /// </summary>
        int m_screenResolutionY { get; set; }

        #endregion

        #region Service Methods
        
        /// <summary>
        /// Initialize this IScreen object.
        /// </summary>
        void Initialize();
        
        /// <summary>
        /// Load this IScreen object's resources.
        /// </summary>
        void Load();
        
        /// <summary>
        /// Update the stage on every frame.
        /// </summary>
        void Update(float delta);

        #endregion

        #region Mutator Methods

        // TODO: Stub.

        #endregion

        #region Accessors Methods

        // TODO: Stub.

        #endregion

    }
}
