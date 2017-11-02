/************************************************
 * GUILabel.cs
 * 
 * Contains implementation for GUILabel.
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
    /// GUIElement that displays text.
    /// </summary>
    public class GUILabel : GUIElement
    {

        #region Data Members.

        #region Fields.

        /// <summary>
        /// Reference to the label.
        /// </summary>
        private GameObject m_label;

        /// <summary>
        /// The rect transform reference.
        /// </summary>
        private RectTransform m_rectTransform;

        /// <summary>
        /// The UnityEnigine text reference.
        /// </summary>
        private UnityEngine.UI.Text m_text;

        /// <summary>
        /// ID of the font resource to use.
        /// </summary>
        private string fontID = "";

        /// <summary>
        /// Path to the font resource to use.
        /// </summary>
        private string fontPath = "";

        /// <summary>
        /// Size of the text to display.
        /// </summary>
        private int m_fontSize = 0;

        #endregion

        #region Properties.

        /// <summary>
        /// The label to use.
        /// </summary>
        public GameObject Label
        {
            get { return this.m_label; }
            set { this.m_label = value; }
        }

        /// <summary>
        /// Reference to component on label.
        /// </summary>
        public RectTransform LabelTransform
        {
            get
            {
                if (this.m_rectTransform == null)
                {
                    this.m_rectTransform = this.Label.GetComponent<RectTransform>();
                }
                return this.m_rectTransform;
            }
        }

        /// <summary>
        /// Reference to component on label.
        /// </summary>
        public UnityEngine.UI.Text Text
        {
            get
            {
                if (this.m_text == null)
                {
                    this.m_text = this.Label.GetComponent<UnityEngine.UI.Text>();
                }
                return this.m_text;
            }
        }

        /// <summary>
        /// Reference to the label's font.
        /// </summary>
        public Font Font
        {
            get
            {
                if (this.Text != null)
                {
                    return this.Text.font;
                }
                return null;
            }

            set
            {
                this.Text.font = value;
            }
        }

        /// <summary>
        /// Reference to the font size.
        /// </summary>
        public int FontSize
        {
            get
            {
                if (this.m_fontSize == 0)
                {
                    this.m_fontSize = 110;
                }

                if (this.Text != null)
                {
                    this.m_fontSize = this.Text.fontSize;
                }

                return this.m_fontSize;
            }

            set
            {
                this.m_fontSize = Services.Clamp(value, 10, 1000);
            }
        }

        #endregion

        #endregion

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
            this.Name = "GUI Label";
        }

        #endregion

    }
}
