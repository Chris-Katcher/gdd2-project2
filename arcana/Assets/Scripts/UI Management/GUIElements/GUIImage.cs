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
using Arcana.Cameras;
using Arcana.Entities.Attributes;

namespace Arcana.UI.Elements
{
    /// <summary>
    /// Stores information about an image in a GUI element.
    /// </summary>
    public class GUIImage : GUIElement
    {

        #region Data Members

        #region Fields.

        /// <summary>
        /// Background ID of the screen.
        /// </summary>
        private string m_backgroundID;

        /// <summary>
        /// Location of the background sprite.
        /// </summary>
        private string m_backgroundPath;

        /// <summary>
        /// Material ID.
        /// </summary>
        private string m_materialID;

        /// <summary>
        /// Location of the background material.
        /// </summary>
        private string m_materialPath;

        /// <summary>
        /// SpriteRenderer for the background.
        /// </summary>
        private EntityRenderer m_renderer = null;

        /// <summary>
        /// Size of the element.
        /// </summary>
        private Rect m_dimensions;

        #endregion

        #region Properties.

        /// <summary>
        /// Background ID.
        /// </summary>
        public string BackgroundID
        {
            get { return this.m_backgroundID; }
            protected set { this.m_backgroundID = value; }
        }

        /// <summary>
        /// Location of the background sprite.
        /// </summary>
        public string BackgroundPath
        {
            get { return this.m_backgroundPath; }
            protected set { this.m_backgroundPath = value; }
        }

        /// <summary>
        /// Material ID.
        /// </summary>
        public string MaterialID
        {
            get { return this.m_materialID; }
            protected set { this.m_materialID = value; }
        }

        /// <summary>
        /// Location of the background material.
        /// </summary>
        public string MaterialPath
        {
            get { return this.m_materialPath; }
            protected set { this.m_materialPath = value; }
        }

        /// <summary>
        /// Reference to the screen's sprite renderer.
        /// </summary>
        public EntityRenderer Renderer
        {
            get
            {
                if (this.m_renderer == null)
                {
                    this.m_renderer = this.InitializeRenderer();
                }

                return this.m_renderer;
            }
        }

        /// <summary>
        /// Size of the image.
        /// </summary>
        public Vector2 Dimensions
        {
            get { return this.m_dimensions.max; }
            set { SetSize(value); }
        }

        /// <summary>
        /// Returns the boundaries in world-space.
        /// </summary>
        public Rect Bounds
        {
            get { return new Rect(this.Position.x, this.Position.y, this.Position.x + this.Dimensions.x, this.Position.y + this.Dimensions.y); }
            set
            {
                this.Position = value.min;
                this.Dimensions = value.max - value.min;
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
            if (this.m_renderer == null)
            {
                // If it's been given a background.
                if (this.BackgroundID.Length > 0 && this.BackgroundPath.Length > 0)
                {
                    this.m_renderer = this.InitializeRenderer();
                }
            }
            
            // Update position.
            this.transform.position = this.Offset + this.Position;
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Load the background resource for the main menu.
        /// </summary>
        protected override void Initialize()
        {
            this.Offset = Vector3.zero;
            this.Position = this.transform.position;
            InitializeRendererResources("", "");
            this.Name = "GUI Image";
        }

        /// <summary>
        /// Set up the resources for the renderer.
        /// </summary>
        public void InitializeRendererResources(string _backgroundID, string _backgroundPath)
        {
            this.BackgroundID = "";
            this.BackgroundPath = "";
            this.MaterialID = "MAT_MENU";
            this.MaterialPath = "Materials/Screens/mat_menu";
        }

        /// <summary>
        /// Iniitalize the renderer.
        /// </summary>
        /// <returns>Returns a SpriteRenderer component.</returns>
        public EntityRenderer InitializeRenderer()
        {
            if (this.BackgroundID.Length > 0 && this.BackgroundPath.Length > 0)
            {

                EntityRenderer renderer = this.gameObject.GetComponent<EntityRenderer>();

                if (renderer == null)
                {
                    renderer = this.gameObject.AddComponent<EntityRenderer>();
                    renderer.Initialize();
                    renderer.InitializeResources(
                        BackgroundID,
                        MaterialID,
                        BackgroundPath,
                        MaterialPath);
                    renderer.InitializeRenderer();
                }

                return renderer;
            }

            return null;
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
            this.m_renderer.enabled = this.Enabled && this.Visible;
        }

        /// <summary>
        /// Update the visibility of the renderer.
        /// </summary>
        /// <param name="_flag">Value to set.</param>
        public override void SetVisible(bool _flag)
        {
            base.SetVisible(_flag);
            this.m_renderer.enabled = this.Enabled && this.Visible;
        }

        #endregion

    }
}
