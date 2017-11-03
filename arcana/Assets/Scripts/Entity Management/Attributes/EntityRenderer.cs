using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Resources;
using Arcana.Resources.Art;
using Arcana.Utilities;


namespace Arcana.Entities.Attributes
{

    #region Class: EntityRenderer class.

    /// <summary>
    /// Displays a 2D sprite on the ArcanaObject this belongs to.
    /// </summary>
    public class EntityRenderer : ArcanaObject
    {

        #region Data Members

        #region Fields.
        
        /// <summary>
        /// Resource ID for the sprite this renderer should display.
        /// </summary>
        private string m_spriteID = "";

        /// <summary>
        /// Resource ID for the material this renderer should display.
        /// </summary>
        private string m_materialID = "";

        /// <summary>
        /// Renderer that will display the sprite.
        /// </summary>
        private SpriteRenderer m_spriteRenderer;

        #endregion

        #region Properties.
        
        /// <summary>
        /// Renderer reference.
        /// </summary>
        public SpriteRenderer Renderer
        {
            get
            {
                if (this.m_spriteRenderer == null)
                {
                    this.m_spriteRenderer = InitializeRenderer();
                }
                return this.m_spriteRenderer;
            }
        }

        /// <summary>
        /// Reference to renderer sprite property.
        /// </summary>
        public Sprite Sprite
        {
            get { return this.Renderer.sprite; }
            set { this.Renderer.sprite = value; }
        }

        /// <summary>
        /// Reference to renderer material property.
        /// </summary>
        public Material Material
        {
            get { return this.Renderer.material; }
            set { this.Renderer.material = value; }
        }

        /// <summary>
        /// Returns true if art resource reference is not null.
        /// </summary>
        public bool HasSprite
        {
            get
            {
                return (Sprite != null);
            }
        }
        
        /// <summary>
        /// Reference to renderer flipX property.
        /// </summary>
        public bool FlipX
        {
            get { return this.Renderer.flipX; }
            set { this.Renderer.flipX = value; }
        }

        /// <summary>
        /// Reference to renderer flipY property.
        /// </summary>
        public bool FlipY
        {
            get { return this.Renderer.flipY; }
            set { this.Renderer.flipY = value; }
        }

        /// <summary>
        /// Reference to renderer draw mode property.
        /// </summary>
        public SpriteDrawMode DrawMode
        {
            get { return this.Renderer.drawMode; }
            set { this.Renderer.drawMode = value; }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Update the renderer properties.
        /// </summary>
        public override void Update()
        {
            if (!this.Initialized)
            {
                this.Initialize();
            }
            else
            {
                // Call the base method.
                base.Update();

                // Show what's on the renderer when:
                // Active.
                // Not Inactive.
                // Not Paused.
                // Visible.

                // Must follow above:
                this.Renderer.enabled = this.Status.IsActive() && this.Status.IsVisible();                             
            }
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Create the renderer for the player controller.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the base class' method.
                base.Initialize();

                // Initialize the data members.
                this.m_spriteID = "";
                this.m_materialID = "";
                this.m_spriteRenderer = this.Renderer;
            }
        }

        /// <summary>
        /// Initialize resources for the renderer.
        /// </summary>
        /// <param name="_spriteID">Sprite resource ID.</param>
        /// <param name="_matID">Material resource ID.</param>
        /// <param name="_spritePath">Path to sprite asset. (Don't include extensions).</param>
        /// <param name="_materialPath">Path to material asset. (Don't include extensions).</param>
        public void InitializeResources(string _spriteID, string _matID, string _spritePath, string _materialPath)
        {
            this.InitializeResourceIDs(_spriteID, _matID);
            this.InitializeResources(_spritePath, _materialPath);
        }

        /// <summary>
        /// Initialize the renderer's resource IDs.
        /// </summary>
        /// <param name="_spriteID">Sprite resource ID.</param>
        /// <param name="_matID">Material resource ID.</param>
        private void InitializeResourceIDs(string _spriteID = "", string _matID = "")
        {
            this.m_spriteID = _spriteID;
            this.m_materialID = _matID;
        }

        /// <summary>
        /// Initialize the resources.
        /// </summary>
        /// <param name="_spritePath">Path to sprite asset. (Don't include extensions).</param>
        /// <param name="_materialPath">Path to material asset. (Don't include extensions).</param>
        private void InitializeResources(string _spritePath = "", string _materialPath = "")
        {
            if (this.m_spriteID.Length > 0 && _spritePath.Length > 0)
            {
                ResourceManager.GetInstance().AddResource(this.m_spriteID, _spritePath, ResourceType.Art);
            }

            if (this.m_materialID.Length > 0 && _materialPath.Length > 0)
            {
                ResourceManager.GetInstance().AddResource(this.m_materialID, _materialPath, ResourceType.Art);
            }
        }

        /// <summary>
        /// Initializes the player's sprite renderer.
        /// </summary>
        /// <returns>Returns the initialized sprite renderer.</returns>
        public SpriteRenderer InitializeRenderer()
        {
            this.m_spriteRenderer = this.Self.GetComponent<SpriteRenderer>();

            if (this.m_spriteRenderer == null)
            {
                this.m_spriteRenderer = this.Self.AddComponent<SpriteRenderer>();
            }

            if (this.m_spriteID.Length > 0)
            {
                // Create the image.
                this.Sprite = ResourceManager.GetInstance().GetArtResource(this.m_spriteID).GetSprite();
            }

            if (this.m_materialID.Length > 0)
            {
                // Set the material.
                this.Material = ResourceManager.GetInstance().GetArtResource(this.m_materialID).Get() as Material; ;
            }

            // Set default flips.
            this.FlipX = false;
            this.FlipY = false;

            // Set default properties.
            this.DrawMode = SpriteDrawMode.Simple;

            return m_spriteRenderer;
        }

        #endregion

        #region ArcanaObject Methods.

        /// <summary>
        /// Destroy the sprite renderer.
        /// </summary>
        public override void DestroySelf()
        {
            Destroy(this.Renderer);
            Destroy(this);
        }

        #endregion

    }

    #endregion

}
