/************************************************
 * GUIPanel.cs
 * 
 * Contains implementation for GUIPanel.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana.UI.Screens;
using UnityEngine;

namespace Arcana.UI.Elements
{
    /// <summary>
    /// Stores other GUIElements within its space.
    /// </summary>
    public class GUIPanel : GUIElement
    {

        #region Static Members.

        /// <summary>
        /// Create new GUIPanel and initialize it.
        /// </summary>
        /// <returns>Returns a GUIPanel component reference.</returns>
        public static GUIPanel CreatePanel()
        {
            // Get reference to the prefab and instantiate it.
            GameObject prefab = UIManager.GetInstance().Panel.Clone;

            // Make the label a child of the UIManager's canvas.
            prefab = Services.AddChild(UIManager.GetInstance().Canvas, prefab);

            // Create a new game object.
            GUIPanel panel = prefab.AddComponent<GUIPanel>();

            // Initialize the label.
            panel.Initialize();

            // Return the GUIPanel.
            return panel;
        }
        
        #endregion

        #region Data Members.

        #region Fields.

        /// <summary>
        /// Reference to the panel.
        /// </summary>
        private GameObject m_panel;

        /// <summary>
        /// The rect transform reference.
        /// </summary>
        private RectTransform m_rectTransform;

        /// <summary>
        /// The UnityEnigine Image reference.
        /// </summary>
        private UnityEngine.UI.Image m_image;
        
        #endregion

        #region Properties.

        /// <summary>
        /// The panel to use.
        /// </summary>
        public GameObject Panel
        {
            get { return this.gameObject; }
        }

        /// <summary>
        /// Reference to component on panel.
        /// </summary>
        public RectTransform Transform
        {
            get
            {
                if (this.m_rectTransform == null)
                {
                    this.m_rectTransform = this.Panel.GetComponent<RectTransform>();
                }
                return this.m_rectTransform;
            }
        }

        /// <summary>
        /// Reference to component on label.
        /// </summary>
        public UnityEngine.UI.Image Image
        {
            get
            {
                if (this.m_image == null)
                {
                    this.m_image = this.Panel.GetComponent<UnityEngine.UI.Image>();
                }
                return this.m_image;
            }
        }
        
        /// <summary>
        /// Width of the panel rect transform.
        /// </summary>
        public float Width
        {
            get { return this.Transform.sizeDelta.x; }
            set { this.Transform.sizeDelta = new Vector2(value, this.Height); }
        }

        /// <summary>
        /// Height of the panel rect transform.
        /// </summary>
        public float Height
        {
            get { return this.Transform.sizeDelta.y; }
            set { this.Transform.sizeDelta = new Vector2(this.Width, value); }
        }

        /// <summary>
        /// Rotation of the panel element.
        /// </summary>
        public Vector3 Rotation
        {
            get { return this.Transform.rotation.eulerAngles; }
            set { this.Transform.rotation = Quaternion.Euler(value); }
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
            this.Transform.position = this.Offset + this.Position;
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the GUIElement.
        /// </summary>
        protected override void Initialize()
        {
            this.Offset = Vector3.zero;
            this.Position = this.Transform.position;
            this.Name = "GUI Panel";
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
            this.Image.enabled = this.Enabled && this.Visible;
        }

        /// <summary>
        /// Update the visibility of the renderer.
        /// </summary>
        /// <param name="_flag">Value to set.</param>
        public override void SetVisible(bool _flag)
        {
            base.SetVisible(_flag);
            this.Image.enabled = this.Enabled && this.Visible;
        }

        #endregion
        
    }
}
