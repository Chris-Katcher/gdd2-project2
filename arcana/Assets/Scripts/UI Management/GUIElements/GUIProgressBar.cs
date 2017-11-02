/************************************************
 * GUIProgressBar.cs
 * 
 * Contains implementation for GUIProgressBar.
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
    /// Loading bar that is displayed on the loading screen.
    /// </summary>
    public class GUIProgressBar : GUIElement
    {

        #region UnityEngine Methods.

        /// <summary>
        /// Update the image.
        /// </summary>
        public void Update()
        {
            // Update position.
            this.transform.position = this.Offset + this.Position;
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the GUIElement.
        /// </summary>
        protected override void Initialize()
        {
            this.Offset = Vector3.zero;
            this.Position = this.transform.position;
            this.Name = "GUI Progress Bar";
        }
        
        #endregion
        
    }
}
