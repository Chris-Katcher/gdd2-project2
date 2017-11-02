/************************************************
 * GUIButton.cs
 * 
 * Contains implementation for GUIButton.
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
    /// Button that can be interacted with by players.
    /// </summary>
    public class GUIButton : GUIElement
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
            this.Name = "GUI Button";
        }

        #endregion
        
    }
}
