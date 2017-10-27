﻿/************************************************
 * CameraManager.cs
 * 
 * This file contains:
 * - The CameraManager class. (Child of Arcana Object).
 * - The CameraMode enum.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Utilities;

namespace Arcana.Cameras
{

    #region Class: CameraManager class.

    /////////////////////
    // Manager declaration.
    /////////////////////

    /// <summary>
    /// Handles all functionality related to the camera users see from.
    /// </summary>
    [AddComponentMenu("Arcana/Managers/CameraManager")]
    public class CameraManager : ArcanaObject
    {

        #region Static Methods.

        #region Enum Parsing Method.

        /// <summary>
        /// Parse the type of the enum as a string.
        /// </summary>
        /// <param name="_mode">Enum value to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(CameraMode _mode)
        {
            string result = "";

            switch (_mode)
            {
                case CameraMode.Free:
                    result = "(Free)";
                    break;
                case CameraMode.Fixed:
                    result = "(Fixed)";
                    break;
                case CameraMode.TargetAll:
                    result = "(Target All)";
                    break;
                case CameraMode.TargetOne:
                    result = "(Target One)";
                    break;
                default:
                    result = "(Unknown Camera Mode)";
                    break;
            }

            return result;
        }

        #endregion
        
        #region Instancing Methods.

        /////////////////////
        // Static methods for instancing.
        /////////////////////

        /// <summary>
        /// Static instance of the factory. (We only want one).
        /// </summary>
        public static CameraManager instance = null;

        /// <summary>
        /// Returns the single instance of the factory.
        /// </summary>
        /// <returns>Returns a ComponentFactory MonoBehaviour component.</returns>
        public static CameraManager GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of ComponentFactory.");
                instance = Services.CreateEmptyObject("Camera Manager").AddComponent<CameraManager>();
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
        public static CameraManager Create(ArcanaObject _parent)
        {
            if (!HasInstance())
            {
                instance = _parent.GetComponent<CameraManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<CameraManager>(_parent);
            }

            return instance;
        }
        
        #endregion

        #endregion

        #region Constructor.

        /////////////////////
        // Private constructor.
        /////////////////////

        /// <summary>
        /// Private constructor that creates the manager when requested for the very first time.
        /// </summary>
        private CameraManager()
        {
            CameraManager.instance = this;
        }

        #endregion
        
        #region Data Members

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Wrapping object that will be shook to get the camera shake effect.
        /// </summary>
        private ArcanaObject m_cameraWrap = null;

        /// <summary>
        /// Reference to the camera.
        /// </summary>
        private CameraSettings m_camera = null;

        /// <summary>
        /// Position of the camera when not shaking.
        /// </summary>
        private Vector3 m_noShakePosition;

        /// <summary>
        /// Position of the camera when shaking it.
        /// </summary>
        private Vector3 m_currentShakePosition;

        /// <summary>
        /// Current position of the camera shaker.
        /// </summary>
        private Vector3 m_currentPosition;

        /// <summary>
        /// Decay tracker allowing timer for the entire shake.
        /// </summary>
        private DecayTracker m_cameraShake = null;

        /// <summary>
        /// Decay tracker allowing strength to decline over time.
        /// </summary>
        private DecayTracker m_cameraShakeStrength = null;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns true if the shaker exists.
        /// </summary>
        public bool HasShaker
        {
            get { return (this.m_cameraWrap != null); }
        }

        /// <summary>
        /// Returns true if a reference to a UnityEngine.Camera exists.
        /// </summary>
        public bool HasCamera
        {
            get { return (this.m_camera != null && this.m_camera.HasCamera); }
        }

        /// <summary>
        /// Returns true if a reference to the camera wrapper exists.
        /// </summary>
        public bool HasCameraWrapper
        {
            get { return (this.m_cameraWrap != null); }
        }

        /// <summary>
        /// Returns reference to camera wrapper's game object.
        /// </summary>
        public GameObject CameraWrapper
        {
            get
            {
                return this.m_cameraWrap.Self;
            }
        }

        /// <summary>
        /// Returns shaker 
        /// </summary>
        public DecayTracker CameraShaker
        {
            get
            {
                if (!HasShaker)
                {
                    this.m_cameraShake = this.gameObject.GetComponent<DecayTracker>();
                }

                if (!HasShaker)
                {
                    this.m_cameraShake = this.gameObject.AddComponent<DecayTracker>();
                    this.m_cameraShake.Initialize();
                }

                return this.m_cameraShake;
            }
        }


        #endregion

        #endregion

        #region UniyEngine

        /// <summary>
        /// Update the camera manager.
        /// </summary>
        public override void Update()
        {
            // The base update is called here.
            base.Update();

            // If active:
            if (this.m_camera.Status.IsActive())
            {
                // Update the target lists based on those in the scene.
                this.AddTargets(this.Self.GetComponentsInChildren<CameraTarget>().ToList());
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the various components and place them in the proper locations.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Base initialization.
                base.Initialize();

                // Set this name.
                this.Name = "Camera Manager";

                // Initialize the camera manager.
                Debugger.Print("Initializing camera manager.", this.Self.name);
                
                // Set up parenting.
                this.m_cameraWrap = Services.CreateEmptyObject("Camera Shaker").AddComponent<ArcanaObject>();
                this.m_cameraWrap.Name = "Camera Shaker";
                
                this.m_camera = CameraSettings.Create(this.m_cameraWrap);
                this.m_cameraShake = this.m_cameraWrap.Self.AddComponent<DecayTracker>();
                this.m_cameraShakeStrength = this.m_cameraWrap.Self.AddComponent<DecayTracker>();

                // Add children.
                // this.m_cameraWrap.AddChild(this.m_camera);
                this.m_cameraWrap.AddChild(this.m_cameraShake);
                this.m_cameraWrap.AddChild(this.m_cameraShakeStrength);
                // Services.AddChild(this.m_cameraWrap.Self, this.m_camera.Self);
                Services.AddChild(this.m_cameraWrap.Self, this.m_cameraShake.Self);
                Services.AddChild(this.m_cameraWrap.Self, this.m_cameraShakeStrength.Self);

                // Make the wrapper object a child of this manager's GameObject.
                Services.AddChild(this.Self, this.m_cameraWrap.Self);
            }
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns the time left in the shaker.
        /// </summary>
        /// <returns>Returns time in seconds.</returns>
        public float ShakeTimeLeft()
        {
            if (HasShaker)
            {
                return this.m_cameraShake.Value;
            }

            return 0.0f;
        }

        /// <summary>
        /// Returns true if time left to shake is greater than zero.
        /// </summary>
        /// <returns>Returns </returns>
        public bool IsShaking()
        {
            return (ShakeTimeLeft() > 0.0f);
        }

        #endregion

        #region Mutator Methods.
        
        /// <summary>
        /// Change the color of the camera.
        /// </summary>
        /// <param name="_color">Color to set the background of the viewport to.</param>
        public void ChangeBackground(Color _color)
        {
            if (HasCamera)
            {
                this.m_camera.CurrentConfiguration.Background = _color;
            }
        }

        /// <summary>
        /// Set the camera free.
        /// </summary>
        public void SetCameraFree()
        {
            CameraConfiguration config = null;
            this.m_camera.SetMode(CameraMode.Free);
            this.m_camera.ResetCamera();

            // Check if configuration has been made.
            if (!this.m_camera.HasConfiguration(this.m_camera.Mode))
            {
                // Build the configuration for this mode.
                config = this.m_camera.CurrentConfiguration;
            }

            config = this.m_camera.CurrentConfiguration;

            // Set up initial values.
            config.Orthographic = false;
            config.InitialBackground = Constants.CORNFLOWER_BLUE;
            config.InitialPosition = this.m_camera.OriginalPosition;

            // Initialize the config again.
            config.Initialize();

            // Set up target values.
            config.TargetFOV = 12.5f;
            config.SetOffsetRange(config.GetZOffset(), -50.0f);
            config.TargetBackground = Constants.CORNFLOWER_BLUE;

        }

        /// <summary>
        /// Set the Camera as fixed.
        /// </summary>
        public void SetCameraFixed()
        {
            CameraConfiguration config = null;
            CameraSettings cam = this.m_camera;
            cam.SetMode(CameraMode.Fixed);
            cam.ResetCamera();

            // Check if configuration has been made.
            if (!cam.HasConfiguration(cam.Mode))
            {
                // Build the configuration for this mode.
                config = cam.CurrentConfiguration;

            }

            config = cam.CurrentConfiguration;

            // Set up initial values.
            config.Orthographic = true;
            config.InitialBackground = Constants.CORNFLOWER_BLUE;
            config.InitialPosition = cam.OriginalPosition;

            // Initialize the config again.
            config.Initialize();

            // Set up target values.
            config.TargetFOV = 12.5f;
            config.SetOffsetRange(0.0f, -10.0f);
            config.TargetBackground = Color.red;
        }

        /// <summary>
        /// Set the camera into one of the target modes.
        /// </summary>
        public void SetCameraTargetOne()
        {
            CameraConfiguration config = null;
            CameraSettings cam = this.m_camera;
            cam.SetMode(CameraMode.TargetOne);

            // Check if configuration has been made.
            if (!cam.HasConfiguration(cam.Mode))
            {
                // Build the configuration for this mode.
                config = cam.CurrentConfiguration;

            }

            config = cam.CurrentConfiguration;

            // Set up initial values.
            config.Orthographic = false;
            config.InitialBackground = Color.red;
            config.InitialPosition = cam.OriginalPosition;

            // Initialize the config again.
            config.Initialize();

            // Set up target values.
            config.TargetFOV = 20f;
            config.SetOffsetRange(config.GetZOffset(), -50.0f);
            config.TargetBackground = Color.white;
            config.SelectedTargetIndex = 0;
                        
        }


        /// <summary>
        /// Set the camera into one of the target modes.
        /// </summary>
        public void SetCameraTargetAll()
        {
            CameraConfiguration config = null;
            CameraSettings cam = this.m_camera;
            cam.SetMode(CameraMode.TargetAll);

            // Check if configuration has been made.
            if (!cam.HasConfiguration(cam.Mode))
            {
                // Build the configuration for this mode.
                config = cam.CurrentConfiguration;

            }

            config = cam.CurrentConfiguration;

            // Set up initial values.
            config.Orthographic = false;
            config.InitialBackground = Color.white;
            config.InitialPosition = cam.OriginalPosition;

            // Initialize the config again.
            config.Initialize();

            // Set up target values.
            config.TargetFOV = 10.0f;
            config.SetOffsetRange(config.GetZOffset(), -65.0f);
            config.TargetBackground = Color.green;
            config.SelectedTargetIndex = -1;
                        
        }


        /// <summary>
        /// Add targets to the camera.
        /// </summary>
        /// <param name="_targets">Targets to add.</param>
        public void AddTargets(List<CameraTarget> _targets)
        {
            foreach (CameraTarget target in _targets)
            {
                if (target.Status.IsActive())
                {
                    this.m_camera.AddTarget(target, CameraMode.TargetOne, CameraMode.TargetAll);
                }
            }
        }

        #endregion

    }

    #endregion

    #region Enum: CameraMode

    /// <summary>
    /// Determines the mode of the camera. Defaults to free.
    /// </summary>
    public enum CameraMode
    {
        TargetAll,
        TargetOne,
        Fixed,
        Free
    }

    #endregion

}
