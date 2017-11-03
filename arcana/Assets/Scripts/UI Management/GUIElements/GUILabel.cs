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
using Arcana.UI.Screens;

namespace Arcana.UI.Elements
{
    /// <summary>
    /// GUIElement that displays text.
    /// </summary>
    public class GUILabel : GUIElement
    {

        #region Static Members.

        /// <summary>
        /// Create new GUILabel and initialize it.
        /// </summary>
        /// <returns>Returns a GUILabel component reference.</returns>
        public static GUILabel CreateLabel()
        {
            // Get reference to the prefab and instantiate it.
            GameObject prefab = UIManager.GetInstance().Label.Clone;

            // Make the label a child of the UIManager's canvas.
            prefab = Services.AddChild(UIManager.GetInstance().Canvas, prefab);

            // Create a new game object.
            GUILabel label = prefab.AddComponent<GUILabel>();

            // Initialize the label.
            label.Initialize();

            // Return the GUILabel.
            return label;
        }

        /// <summary>
        /// Create a label while specifying its message and position.
        /// </summary>
        /// <param name="message">Message to set label to.</param>
        /// <param name="position">Position to place label.</param>
        /// <returns>Returns a GUILabel.</returns>
        public static GUILabel CreateLabel(string _message = "", Vector2? _position = null)
        {
            GUILabel label = CreateLabel();

            label.Message = _message;

            if (_position.HasValue)
            {
                label.SetPosition(_position.Value);
            }
            else
            {
                label.SetPosition(ScreenManager.Center);
            }

            return label;
        }

        #endregion

        #region Data Members.

        #region Fields.
        
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
            get { return this.gameObject; }
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
        /// Reference to the actual content of the label.
        /// </summary>
        public string Message
        {
            get { return this.Text.text; }
            set { this.Text.text = value; }
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
            if (this.m_fontSize != 0)
            {
                this.Text.fontSize = this.m_fontSize;
            }
        }

        /// <summary>
        /// Update position.
        /// </summary>
        public void UpdatePosition()
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

        #region GUIElement Methods.

        /// <summary>
        /// Update the enabled state of the renderer.
        /// </summary>
        /// <param name="_flag">Value to set.</param>
        public override void Enable(bool _flag)
        {
            base.Enable(_flag);
            this.Text.enabled = this.Enabled && this.Visible;
        }

        /// <summary>
        /// Update the visibility of the renderer.
        /// </summary>
        /// <param name="_flag">Value to set.</param>
        public override void SetVisible(bool _flag)
        {
            base.SetVisible(_flag);
            this.Text.enabled = this.Enabled && this.Visible;
        }

        #endregion
        
    }
}
