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
using Arcana.Resources;
using Arcana.UI.Screens;

namespace Arcana.UI.Elements
{
    /// <summary>
    /// Stores information about an image in a GUI element.
    /// </summary>
    public class GUIImage : GUIElement
    {

        #region Static Members.

        /// <summary>
        /// Create new GUIElement and initialize it.
        /// </summary>
        /// <returns>Returns a GUIElement component reference.</returns>
        public static GUIImage CreateImage()
        {
            // Get reference to the prefab and instantiate it.
            GameObject prefab = UIManager.GetInstance().Image.Clone;

            // Make the label a child of the UIManager's canvas.
            prefab = Services.AddChild(UIManager.GetInstance().Canvas, prefab);

            // Create a new game object.
            GUIImage image = prefab.AddComponent<GUIImage>();

            // Initialize the image.
            image.Initialize();

            // Return the GUIImage.
            return image;
        }
        
        /// <summary>
        /// Creates the image element.
        /// </summary>
        /// <param name="_id">ID of art asset.</param>
        /// <param name="_path">Path to art asset.</param>
        /// <param name="_position">Position of the image.</param>
        /// <param name="_dimensions">Dimensions to give the image.</param>
        /// <param name="_rotation">Rotation to apply to the image.</param>
        /// <returns></returns>
        public static GUIImage CreateImage(string _id, string _path, Vector2? _position = null, Vector2? _dimensions = null, Vector2? _rotation = null)
        {
            GUIImage image = CreateImage();

            ResourceManager.GetInstance().AddResource(_id, _path, ResourceType.Art);
            Sprite sprite = ResourceManager.GetInstance().GetArtResource(_id).GetSprite();

            if (sprite != null)
            {
                image.Background = sprite;
            }

            if (_position.HasValue)
            {
                image.SetPosition(_position.Value);
            }
            else
            {
                image.SetPosition(ScreenManager.Center);
            }
            
            if (_dimensions.HasValue)
            {
                image.Dimensions = _dimensions.Value;
            }
            else
            {
                // Defaults.
                image.SetSize(sprite.rect.max);
            }

            if (_rotation.HasValue)
            {
                image.Rotation = _rotation.Value;
            }
            else
            {
                // Defaults.
                image.Rotation = Vector3.zero;
            }
            
            return image;
        }

        #endregion

        #region Data Members.

        #region Fields.

        /// <summary>
        /// The rect transform reference.
        /// </summary>
        private RectTransform m_rectTransform;

        /// <summary>
        /// The UnityEngine Image reference.
        /// </summary>
        private UnityEngine.UI.Image m_image;

        #endregion

        #region Properties.

        /// <summary>
        /// The panel to use.
        /// </summary>
        public GameObject Self
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
                    this.m_rectTransform = this.Self.GetComponent<RectTransform>();
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
                    this.m_image = this.Self.GetComponent<UnityEngine.UI.Image>();
                }
                return this.m_image;
            }
        }

        /// <summary>
        /// Reference to the image's sprite.
        /// </summary>
        public Sprite Background
        {
            get { return this.Image.sprite; }
            set { this.Image.sprite = value; }
        }

        /// <summary>
        /// Width of the image's rect transform.
        /// </summary>
        public float Width
        {
            get { return this.Transform.sizeDelta.x; }
            set { this.Transform.sizeDelta = new Vector2(value, this.Height); }
        }

        /// <summary>
        /// Height of the image's rect transform.
        /// </summary>
        public float Height
        {
            get { return this.Transform.sizeDelta.y; }
            set { this.Transform.sizeDelta = new Vector2(this.Width, value); }
        }

        /// <summary>
        /// Rotation of the image element.
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
            this.transform.position = this.Offset + this.Position;
            this.Width = this.Dimensions.x;
            this.Height = this.Dimensions.y;
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
            this.Name = "GUI Image";
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
