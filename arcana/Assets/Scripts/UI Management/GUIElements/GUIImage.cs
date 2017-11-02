/************************************************
 * GUIImage.cs
 * 
 * Contains implementation for GUIImage.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.UI.Elements
{
    /// <summary>
    /// Stores information about an image in a GUI element.
    /// </summary>
    public class GUIImage : GUIElement
    {

        #region Data Members

        #region Fields.

        /////////////////////
        // Private data fields.
        /////////////////////

        /// <summary>
        /// Children that may be GUI elements.
        /// </summary>
        private List<IGUIElement> m_elements;


        #endregion

        #region Properties.

        /////////////////////
        // Public data fields.
        /////////////////////

        /// <summary>
        /// Position of the IGUIElement.
        /// </summary>
        public Vector2 m_position { get; set; }

        /// <summary>
        /// Depth level.
        /// </summary>
        public int m_depth { get; set; }

        /// <summary>
        /// Element visibility.
        /// </summary>
        public bool m_visible { get; set; }

        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool m_enabled { get; set; }

        #endregion

        #endregion

    }
}
