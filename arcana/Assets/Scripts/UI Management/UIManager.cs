/************************************************
 * UIManager.cs
 * 
 * This file holds the implementation for the UIManager class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana.Utilities;
using Arcana.Resources;
using Arcana.UI.Elements;
using Arcana.UI.Screens;
using UnityEngine;

// The UI namespace contains the visual layer for all of the user-facing information.
namespace Arcana.UI
{
    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// UIManager controls visual elements that show up on the player's screen.
    /// </summary>
    [AddComponentMenu("Arcana/Managers/UI Manager")]
    public class UIManager : ArcanaObject
    {

        #region Static Methods.

        #region Instancing Methods.

        /////////////////////
        // Static methods for instancing.
        /////////////////////

        /// <summary>
        /// Static instance of the class. (We only want one).
        /// </summary>
        public static UIManager instance = null;

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        /// <returns>Returns a component.</returns>
        public static UIManager GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of UIManager.");
                instance = Services.CreateEmptyObject("UI Manager").AddComponent<UIManager>();
                instance.Initialize();
            }

            return instance;
        }

        /// <summary>
        /// Returns true if instance exists.
        /// </summary>
        /// <returns>Returns boolean indicating instance existence.</returns>
        public static bool HasInstance()
        {
            return (instance != null);
        }

        #endregion

        #region Component Factory Methods.

        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static UIManager Create(ArcanaObject _parent)
        {
            if (!HasInstance())
            {
                instance = _parent.GetComponent<UIManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<UIManager>(_parent);
            }

            return instance;
        }

        #endregion

        #endregion

        #region Data Members.

        #region Fields.

        /// <summary>
        /// Reference to the UISystem prefab.
        /// </summary>
        private UIPrefab<GameObject> m_uiSystem;

        /// <summary>
        /// Reference to the GUILabel prefab.
        /// </summary>
        private UIPrefab<GUILabel> m_label;

        /// <summary>
        /// Reference to the GUIPanel prefab.
        /// </summary>
        private UIPrefab<GUIPanel> m_panel;

        /// <summary>
        /// Reference to the GUIImage prefab.
        /// </summary>
        private UIPrefab<GUIImage> m_image;

        /// <summary>
        /// Reference to the canvas object for building UI systems.
        /// </summary>
        private GameObject m_canvas;

        /// <summary>
        /// Map of all the elements currently active.
        /// </summary>
        private Dictionary<string, IGUIElement> m_elements;
                        
        #endregion

        #region Properties.

        /// <summary>
        /// Reference to UISystem.
        /// </summary>
        public UIPrefab<GameObject> UISystem
        {
            get { return this.m_uiSystem; }
        }

        /// <summary>
        /// Reference to the label prefab.
        /// </summary>
        public UIPrefab<GUILabel> Label
        {
            get { return this.m_label; }
        }

        /// <summary>
        /// Reference to the panel prefab.
        /// </summary>
        public UIPrefab<GUIPanel> Panel
        {
            get { return this.m_panel; }
        }

        /// <summary>
        /// Reference to the image prefab.
        /// </summary>
        public UIPrefab<GUIImage> Image
        {
            get { return this.m_image; }
        }

        /// <summary>
        /// Reference to the canvas.
        /// </summary>
        public GameObject Canvas
        {
            get { return GetCanvas(); }
        }

        /// <summary>
        /// Map of all elements.
        /// </summary>
        public Dictionary<string, IGUIElement> Elements
        {
            get
            {
                if (this.m_elements == null)
                {
                    this.m_elements = new Dictionary<string, IGUIElement>();
                }
                return this.m_elements;
            }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Update the UI Manager.
        /// </summary>
        public override void Update()
        {
            // Call the base update method.
            base.Update();

            // Update the UI Manager.

            GUIImage debugImage = this.GetElement<GUIImage>("Test Image");
            GUILabel debugLabel = this.GetElement<GUILabel>("Test Label");
            GUIPanel debugPanel = this.GetElement<GUIPanel>("Test Panel");

            // Debug mode.
            if (this.Debug)
            {
                if (debugImage == null)
                {
                    debugImage = CreateImage("Test Image", "BG_ELEMENT", "Images/Backgrounds/bg_menu", ScreenManager.Center, ScreenManager.WindowBounds.max);
                }

                if (debugLabel == null)
                {
                    debugLabel = CreateLabel("Test Label", "Test Text!", ScreenManager.Center + new Vector2(0, -50));
                }

                if (debugPanel == null)
                {
                    debugPanel = CreatePanel("Test Panel", ScreenManager.Center, new Vector2(120, 120));
                }

            }

            if (debugImage != null)
            {
                debugImage.SetPosition(ScreenManager.Center);
                debugImage.SetSize(ScreenManager.WindowBounds.max);
                debugImage.Enable(this.debug_active);
                debugImage.SetVisible(this.debug_visible);
            }

            if (debugLabel != null)
            {
                debugLabel.SetPosition(ScreenManager.Center + new Vector2(0, -50));
                debugLabel.FontSize = (int)(80 * ScreenManager.SafetyScale);
                debugLabel.Enable(this.debug_active);
                debugLabel.SetVisible(this.debug_visible);
            }

            if (debugPanel != null)
            {
                debugPanel.SetPosition(ScreenManager.Center + new Vector2(0, -50));
                debugPanel.SetSize(ScreenManager.WindowBounds.max);
                debugPanel.Enable(this.debug_active);
                debugPanel.SetVisible(this.debug_visible);
            }
            
            if (this.debug_pause)
            {
                debugLabel.Message = "This is paused.";
            }
            else
            {
                debugLabel.Message = "This is not paused.";
            }

        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the prefabs for the UI.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call base function.
                base.Initialize();

                // Initialize the resources.
                this.InitializeResources();

                // Set the name of this manager.
                this.Name = "UI Manager";
                
                // Debug mode.
                this.Debug = true;

                // Set this as the UIManager instance.
                UIManager.instance = this;
            }
        }

        /// <summary>
        /// Initialize the resources used for the UIManager.
        /// </summary>
        private void InitializeResources()
        {
            // Creates and initializes the prefabs using the struct.
            this.m_uiSystem = new UIPrefab<GameObject>("PREFAB_UISYSTEM", "UISystem");
            this.m_label = new UIPrefab<GUILabel>("PREFAB_LABEL", "GUILabel");
            this.m_panel = new UIPrefab<GUIPanel>("PREFAB_PANEL", "GUIPanel");
            this.m_image = new UIPrefab<GUIImage>("PREFAB_IMAGE", "GUIImage");
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Format a key from the input.
        /// </summary>
        /// <param name="_input">Input string to convert.</param>
        /// <returns>Returns formatted string.</returns>
        public string MakeKey(string _input)
        {
            return _input.Trim().ToUpper();
        }

        /// <summary>
        /// Add the element to the map.
        /// </summary>
        /// <typeparam name="T">Type of element to use.</typeparam>
        /// <param name="key">Key of the element.</param>
        /// <param name="_element">Element itself.</param>
        public T AddElement<T>(string key, T _element) where T : Arcana.UI.Elements.GUIElement
        {
            if (!HasElement(MakeKey(key)))
            {
                this.Elements.Add(key, _element);
            }

            return this.Elements[key] as T;
        }

        /// <summary>
        /// Create a label and return it.
        /// </summary>
        /// <param name="_message">Message to give label.</param>
        /// <param name="_position">Position of the label.</param>
        /// <returns>Returns new label object.</returns>
        public GUILabel CreateLabel(string _message = "", Vector2? _position = null)
        {
            // Create the label.
            GUILabel label = GUILabel.CreateLabel(_message, _position);
            
            // Return the label.
            return label;
        }

        /// <summary>
        /// Create the panel and return a reference to it.
        /// </summary>
        /// <param name="_position">Position of the element.</param>
        /// <param name="_dimensions">Dimensions of the panel.</param>
        /// <param name="_rotation">Panel rotation.</param>
        /// <returns>Returns a GUIPanel.</returns>
        public GUIPanel CreatePanel(Vector2? _position = null, Vector2? _dimensions = null, Vector3? _rotation = null)
        {
            // Create the panel.
            GUIPanel panel = GUIPanel.CreatePanel(_position, _dimensions, _rotation);

            // Return the panel.
            return panel;
        }
        
        /// <summary>
        /// Create a image and return it.
        /// </summary>
        /// <param name="_id">ID of sprite resource.</param>
        /// <param name="_path">Path to sprite resource.</param>
        /// <param name="_position">Position of the element.</param>
        /// <param name="_dimensions">Dimensions of the element.</param>
        /// <returns>Returns new GUIElement.</returns>
        public GUIImage CreateImage(string _id, string _path, Vector2? _position = null, Vector2? _dimensions = null)
        {
            // Create the element.
            GUIImage image = GUIImage.CreateImage(_id, _path, _position, _dimensions);
            
            // Return the image.
            return image;
        }

        /// <summary>
        /// Add new label to the map, and return a reference to it.
        /// </summary>
        /// <param name="_key">Key associated with the new label.</param>
        /// <param name="_message">Message to give label.</param>
        /// <param name="_position">Position of the label.</param>
        /// <returns>Returns a GUILabel.</returns>
        public GUILabel CreateLabel(string _key, string _message = "", Vector2? _position = null)
        {
            // Check if element exists.
            string key = MakeKey(_key);
            if (HasElement(key))
            {
                // Return existing reference.
                return this.GetElement<GUILabel>(key);
            }

            // If label doesn't exist, create a new label and add it.
            return AddElement<GUILabel>(key, CreateLabel(_message, _position));
        }

        /// <summary>
        /// Create a new panel and add it to the map.
        /// </summary>
        /// <param name="_key">Key associated with the new element.</param>
        /// <param name="_position">Position of the element.</param>
        /// <param name="_dimensions">Dimensions of the panel.</param>
        /// <param name="_rotation">Rotation in Euler angle (degrees) for the panel.</param>
        /// <returns>Returns a GUIPanel.</returns>
        public GUIPanel CreatePanel(string _key, Vector2? _position = null, Vector2? _dimensions = null, Vector3? _rotation = null)
        {
            // Check if element exists.
            string key = MakeKey(_key);
            if (HasElement(key))
            {
                // Return existing reference.
                return this.GetElement<GUIPanel>(key);
            }

            // If label doesn't exist, create a new label and add it.
            return AddElement<GUIPanel>(key, CreatePanel(_position, _dimensions, _rotation));
        }

        /// <summary>
        /// Add new image to the map, and return a reference to it.
        /// </summary>
        /// <param name="_key">Key associated with the new element.</param>
        /// <param name="_id">ID of sprite resource.</param>
        /// <param name="_path">Path to sprite resource.</param>
        /// <param name="_position">Position of the element.</param>
        /// <param name="_dimensions">Dimensions of the element.</param>
        /// <returns>Returns a GUIImage.</returns>
        public GUIImage CreateImage(string _key, string _id, string _path, Vector2? _position = null, Vector2? _dimensions = null)
        {
            // Check if element exists.
            string key = MakeKey(_key);
            if (HasElement(key))
            {
                // Return existing reference.
                return this.GetElement<GUIImage>(key);
            }

            // If label doesn't exist, create a new label and add it.
            return AddElement<GUIImage>(key, CreateImage(_id, _path, _position, _dimensions));
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Determines if element is contained within the map.
        /// </summary>
        /// <param name="key">Key associated with the element.</param>
        /// <returns>Returns true if key is found in map.</returns>
        public bool HasElement(string key)
        {
            return (this.Elements.ContainsKey(MakeKey(key)));
        }

        /// <summary>
        /// Return reference to an element.
        /// </summary>
        /// <typeparam name="T">Type of GUIElement.</typeparam>
        /// <param name="_key">Key to check for.</param>
        /// <returns>Returns GUIElement descendant.</returns>
        public T GetElement<T>(string _key) where T : Arcana.UI.Elements.GUIElement
        {
            string key = MakeKey(_key);
            if (HasElement(key))
            {
                return this.Elements[key] as T;
            }
            return null;
        }

        /// <summary>
        /// Returns the Canvas game object.
        /// </summary>
        /// <returns></returns>
        public GameObject GetCanvas()
        {
            if (this.m_canvas == null)
            {
                // Returns a reference to the game object with the Canvas.
                this.m_canvas = this.UISystem.Instance.GetComponentInChildren<Canvas>().gameObject;
            }

            // Returns reference to the canvas.
            return this.m_canvas;
        }
        
        /// <summary>
        /// (Deprecated) Returns object as a reference and instantiate it..
        /// </summary>
        /// <param name="_resource">Resource to get reference to.</param>
        /// <returns>Returns GameObject reference.</returns>
        public GameObject GetPrefab(string _resourceID)
        {
            return Instantiate(ResourceManager.GetInstance().GetResource(_resourceID).Load()) as GameObject;
        }
        
        #endregion
        
    }
}
