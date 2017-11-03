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
        /// UI System ID.
        /// </summary>
        private string m_uiSystemID = "PREFAB_UISYSTEM";

        /// <summary>
        /// UI System Path
        /// </summary>
        private string m_uiSystemPath = "UISystem";

        /// <summary>
        /// Reference to the ui system prefab for building UI systems.
        /// </summary>
        private GameObject m_uiSystem;

        /// <summary>
        /// Reference to the canvas object for building UI systems.
        /// </summary>
        private GameObject m_canvas;

        /// <summary>
        /// Label resource ID.
        /// </summary>
        private string m_labelID = "PREFAB_LABEL";

        /// <summary>
        /// Label resource Path.
        /// </summary>
        private string m_labelPath = "GUILabel";

        /// <summary>
        /// Reference to the label prefab for building the GUI labels.
        /// </summary>
        private GameObject m_label;

        #endregion

        #region Properties.

        /// <summary>
        /// Reference to UISystem.
        /// </summary>
        public GameObject UISystem
        {
            get { return GetUISystem(); }
        }

        /// <summary>
        /// Reference to the canvas.
        /// </summary>
        public GameObject Canvas
        {
            get { return GetCanvas(); }
        }

        /// <summary>
        /// Reference to the label prefab.
        /// </summary>
        public GameObject Label
        {
            get { return GetLabel(); }
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

            // Debug mode.
            if (this.Debug)
            {
                // We want to create a label.
                GUILabel label = GetCanvas().gameObject.GetComponentInChildren<GUILabel>();

                if (label == null)
                {
                    label = GUILabel.CreateLabel("Test text", ScreenManager.Center + new Vector2(100, 100));
                    label.FontSize = 120;
                }

                label.Enable(this.debug_active);
                label.SetVisible(this.debug_visible);
            }
            else
            {
                // We want to create a label.
                GUILabel label = GetCanvas().gameObject.GetComponentInChildren<GUILabel>();

                if (label != null)
                {
                    label.Enable(this.debug_active);
                    label.SetVisible(this.debug_visible);
                }
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
            // Add resources.
            ResourceManager.GetInstance().AddResource(this.m_uiSystemID, this.m_uiSystemPath, ResourceType.Prefab);
            ResourceManager.GetInstance().AddResource(this.m_labelID, this.m_labelPath, ResourceType.Prefab);
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns the UI system.
        /// </summary>
        /// <returns></returns>
        public GameObject GetUISystem()
        {
            if(this.m_uiSystem == null)
            {
                // Load the UISystem resource (and instantiate it).
                this.m_uiSystem = GetPrefab(this.m_uiSystemID);
            }

            // Returns reference to the UI System.
            return this.m_uiSystem;
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
                this.m_canvas = this.GetUISystem().GetComponentInChildren<Canvas>().gameObject;
            }

            // Returns reference to the canvas.
            return this.m_canvas;
        }

        /// <summary>
        /// Returns the GUILabel prefab reference (without instantiating it).
        /// </summary>
        /// <returns></returns>
        public GameObject GetLabel()
        {
            if (this.m_label == null)
            {
                // Load the label resource (and instantiate it).
                this.m_label = GetPrefab(this.m_labelID);
            }

            // Returns reference to the label prefab.
            return this.m_label;
        }
        
        /// <summary>
        /// Returns object as a reference and instantiate it..
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
