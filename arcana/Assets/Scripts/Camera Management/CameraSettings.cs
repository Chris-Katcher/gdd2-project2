/************************************************
 * CameraSettings.cs
 * 
 * This file contains:
 * - The CameraSettings class. (Child of Arcana Object).
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityCamera = UnityEngine.Camera;
using Arcana.Utilities;

namespace Arcana.Cameras
{

    #region Class: CameraSettings class.

    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// A camera setting component enables for on-the-fly modification of Camera properties.
    /// </summary>
    [AddComponentMenu("Arcana/Cameras/CameraSettings")]
    public class CameraSettings : ArcanaObject
    {

        #region Static Methods.

        #region Enum Parsing Methods.

        /// <summary>
        /// Parse the name of the Enum.
        /// </summary>
        /// <param name="_type">Enum value to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(CameraMode _type)
        {
            string result = "";

            switch (_type)
            {
                case CameraMode.Free:
                    result = "(Free Camera Mode)";
                    break;
                case CameraMode.Fixed:
                    result = "(Fixed Camera Mode)";
                    break;
                case CameraMode.TargetOne:
                    result = "(Target One Mode)";
                    break;
                case CameraMode.TargetAll:
                    result = "(Target All Mode)";
                    break;
                default:
                    result = "(Unknown Camera Mode)";
                    break;
            }

            return result;
        }

        #endregion

        #region Component Factory Methods.

        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static CameraSettings Create(ArcanaObject _parent)
        {
            // Check if there's a camera on the parent.
            if (_parent == null || _parent.GetComponent<UnityCamera>() == null)
            {
                // Check to see if there are any cameras.
                if (UnityCamera.allCamerasCount > 0)
                {
                    // Loop through, but, do it for the first available.
                    foreach (UnityCamera cameraObject in UnityCamera.allCameras)
                    {
                        // Get the arcana object on the camera, if it exists.
                        ArcanaObject camera = cameraObject.gameObject.GetComponent<ArcanaObject>();

                        // If the component doesn't exist, add an arcana object and camera settings.
                        if (camera == null)
                        {
                            // Add the arcana object to the game object.
                            camera = UnityCamera.allCameras[0].gameObject.AddComponent<ArcanaObject>();
                            camera.Name = "Camera (Managed)";
                        }

                        if (camera.gameObject.GetComponent<CameraSettings>() == null)
                        {
                            if (_parent != null) { Services.AddChild(_parent.Self, camera.Self); }
                            return Create(camera);
                        }
                    }
                }

                // If there are no cameras that can be found,
                // OR if all cameras have an associated CameraSettings object already:

                // Create a new parent with a new camera if neither can be found.
                ArcanaObject parent = (new GameObject()).AddComponent<ArcanaObject>();
                parent.Name = "Camera (New) (Managed)";
                parent.Self.AddComponent<UnityCamera>();
                return ComponentFactory.Create<CameraSettings>(parent);
            }

            return ComponentFactory.Create<CameraSettings>(_parent);
        }

        /// <summary>
        /// Clone a component and set it equal to another.
        /// </summary>
        /// <param name="_parent">Parent to add clone to.</param>
        /// <param name="_template">Component to clone.</param>
        /// <returns>Returns a new component that has been cloned.</returns>
        public static CameraSettings Clone(ArcanaObject _parent, CameraSettings _template)
        {
            return (CameraSettings)(CameraSettings.Create(_parent)).Clone(_template);
        }

        #endregion

        #endregion

        #region Data Members

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Reference to <see cref="UnityEngine.Camera"/> component.
        /// </summary>
        private UnityCamera m_camera = null;

        /// <summary>
        /// Configurations created for each mode.
        /// </summary>
        private Dictionary<CameraMode, CameraConfiguration> m_settings = null;

        /// <summary>
        /// The <see cref="Arcana.Cameras.CameraMode"/> this component is in.
        /// </summary>
        private CameraMode m_mode = CameraMode.Free;

        /// <summary>
        /// The base position all cameras should be relative to.
        /// </summary>
        private Vector3 m_basePosition = Vector3.zero;

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if there is a <see cref="UnityCamera"/> component.
        /// </summary>
        public bool HasCamera
        {
            get { return (this.Camera != null); }
        }

        /// <summary>
        /// Reference to the camera mode.
        /// </summary>
        public CameraMode Mode
        {
            get { return this.m_mode; }
        }

        /// <summary>
        /// Reference to the base position.
        /// </summary>
        public Vector3 OriginalPosition
        {
            get { return this.m_basePosition; }
            set { this.m_basePosition = value; }
        }

        /// <summary>
        /// Reference to the <see cref="UnityCamera"/> component.
        /// </summary>
        public UnityCamera Camera
        {
            get
            {
                if (this.m_camera == null)
                {
                    // Get the component camera on the game object if it's currently null.
                    this.m_camera = gameObject.GetComponent<UnityCamera>();
                }

                if (this.m_camera == null && UnityCamera.allCamerasCount > 0)
                {
                    // If still null, see if there are any cameras in the scene.
                    this.m_camera = UnityCamera.allCameras[0]; // Get the main camera in the scene.
                }
                
                if (this.m_camera == null)
                {
                    // If camera is still null, add a new component.
                    this.m_camera = gameObject.AddComponent<UnityCamera>();
                }

                return this.m_camera;
            }
        }
        
        /// <summary>
        /// Collection of configurations for each mode.
        /// </summary>
        public Dictionary<CameraMode, CameraConfiguration> Settings
        {
            get
            {
                if (this.m_settings == null)
                {
                    this.m_settings = new Dictionary<CameraMode, CameraConfiguration>();
                }

                return this.m_settings;
            }
        }

        /// <summary>
        /// Get the configuration related with this mode. If none exists, it creates one.
        /// </summary>
        public CameraConfiguration CurrentConfiguration
        {
            get
            {
                if (!HasConfiguration(m_mode))
                {
                    Settings.Add(m_mode, null);
                }

                if (Settings[m_mode] == null)
                {
                    // Create a configuration file.
                    Debugger.Print("Creating a configuration for current mode: " + Parse(m_mode) + ".", this.Name);
                    Settings[m_mode] = CameraConfiguration.Create();
                }

                return Settings[m_mode];
            }
        }
        
        #endregion

        #endregion

        #region UnityEngine Methods.
        
        /// <summary>
        /// Toggle the current camera mode.
        /// </summary>
        public void ToggleMode()
        {
            switch (this.m_mode)
            {
                case CameraMode.Free:
                    CameraManager.GetInstance().SetCameraFixed();
                    break;

                case CameraMode.Fixed:
                    CameraManager.GetInstance().SetCameraTargetOne();
                    break;

                case CameraMode.TargetOne:
                    CameraManager.GetInstance().SetCameraTargetAll();
                    break;

                case CameraMode.TargetAll:
                    CameraManager.GetInstance().SetCameraFree();
                    break;
            }
        }

        /// <summary>
        /// Update camera every cycle.
        /// </summary>
        public override void Update()
        {
            // Call the base update.
            base.Update();
            
            // Update the camera based on its mode.
            UpdateCamera();
        }

        #endregion

        #region Initialization Method.

        /// <summary>
        /// Initialize the component.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Create the status, set the name, and children collection.
                base.Initialize();

                // Set the camera.
                this.m_camera = this.Camera;

                // Set the mode.
                this.m_mode = CameraMode.Free;

                // Set up the configurations.
                this.m_settings = new Dictionary<CameraMode, CameraConfiguration>();
                
                // Set the copyable flag to false.
                this.SetCopyable(false);

                // Change name of the object.
                this.Name = "Arcana Camera";

                Debugger.Print("Reset camera.", this.Self.name, this.Debug);

                // Reset camera when in free mode.
                ResetCamera();

                // Initialize the camera settings.
                Debugger.Print("Initializing camera settings.", this.Self.name);
            }
        }
        
        #endregion

        #region Service Methods.

        /// <summary>
        /// Check to see if a configuration exists for this mode.
        /// </summary>
        /// <param name="_mode">Mode to check.</param>
        /// <returns>Return true if there is a CameraConfiguration object associated with this mode.</returns>
        public bool HasConfiguration(CameraMode _mode)
        {
            return (Settings.ContainsKey(_mode));
        }
        
        /// <summary>
        /// Get's a message declaring the state of the Status component in human readable form.
        /// </summary>
        /// <returns>Returns a report of all active states.</returns>
        public override string ToString()
        {
            // Default message.
            string report = "Camera \"" + gameObject.name + "\" is not initialized.";

            if (this.Status.IsInitialized())
            {
                // Return all currently active status names as comma separated list.
                report = "Camera \"" + gameObject.name + "\" Mode: [" + Parse(this.m_mode) + "]";
            }

            // Return the report.
            return report;
        }

        /// <summary>
        /// Increment index selector to next target, if possible.
        /// </summary>
        public void NextTarget()
        { 
            if (CurrentConfiguration.HasTargets && CurrentConfiguration.Targets.Count > 1)
            {
                int targetCount = CurrentConfiguration.Targets.Count;
                CurrentConfiguration.SelectedTargetIndex++;

                if (CurrentConfiguration.SelectedTargetIndex >= targetCount)
                {
                    CurrentConfiguration.SelectedTargetIndex = 0;
                }                
            }
        }

        /// <summary>
        /// Decrement index selector to previous target, if possible.
        /// </summary>
        public void PreviousTarget()
        {
            if (CurrentConfiguration.HasTargets && CurrentConfiguration.Targets.Count > 1)
            {
                int targetCount = CurrentConfiguration.Targets.Count;
                CurrentConfiguration.SelectedTargetIndex--;

                if (CurrentConfiguration.SelectedTargetIndex < 0)
                {
                    CurrentConfiguration.SelectedTargetIndex = (targetCount - 1);
                }
            }
        }

        /// <summary>
        /// Update the camera's movement based on its CameraMode.
        /// </summary>
        public void UpdateCamera()
        {
            // Print out the camera mode.
            Debugger.Print("Current camera mode: " + Parse(this.m_mode), this.Name, this.Debug);
            
            // When in free mode.
            if (!IsFreeCamera())
            {
                // When in target one mode.
                if (IsTargetOne())
                {
                    // Console statement. // Update the target cameras on the target index.
                    Debugger.Print("Target camera one.", this.Self.name, this.Debug);
                    UpdateTargetCamera(CurrentConfiguration.SelectedTargetIndex);
                }

                // Set index to -1 if target all is enabled.
                if (IsTargetAll())
                {
                    // Console statement.
                    Debugger.Print("Target camera all.", this.Self.name, this.Debug);
                    UpdateTargetCamera(-1);
                }


                // When in fixed camera mode.
                if (IsFixedCamera())
                {
                    // Update the fixed camera.
                    Debugger.Print("Fixed camera.", this.Self.name, this.Debug);
                    UpdateFixedCamera();
                }
            }

            // Update these qualities for every camera.
            CurrentConfiguration.Update(this);
        }

        /// <summary>
        /// Apply settings when in fixed camera mode.
        /// </summary>
        public void UpdateFixedCamera()
        {
            CurrentConfiguration.Orthographic = true;
            CurrentConfiguration.TargetPosition = Vector3.zero;
            CurrentConfiguration.TargetOffsetZ = -10.0f;
            CurrentConfiguration.OrthographicSize = 5.0f;
        }

        /// <summary>
        /// Apply settings when in target camera mode.
        /// </summary>
        /// <param name="_index">Index of the selected target.</param>
        public void UpdateTargetCamera(int _index = -1)
        {
            if (CurrentConfiguration.HasTargets)
            {
                if (Services.InRange<int>(_index, 0, CurrentConfiguration.Targets.Count))
                {
                    // When index is in range, target only a single target.
                    CurrentConfiguration.TargetPosition = CurrentConfiguration.Targets[_index].Location;

                    Debugger.Print("Targeting index: " + _index + " " + CurrentConfiguration.Targets[_index].Name + " || May be " + CurrentConfiguration.SelectedTarget.Name + ".");

                    // Keep track of the target.
                    CurrentConfiguration.SetOffsetRange(CurrentConfiguration.InitialOffset.z, -CurrentConfiguration.InitialOffset.z + CurrentConfiguration.Targets[_index].Radius);
                }
                else
                {
                    // When index is out of range, this means there is no single target selected.
                    Vector3 centerOfTargets = Vector3.zero;
                    float radii = 0.0f;

                    foreach (CameraTarget target in CurrentConfiguration.Targets)
                    {
                        centerOfTargets += target.Location;
                        radii += target.Radius;
                    }

                    // Get the average.
                    centerOfTargets /= CurrentConfiguration.Targets.Count;
                    radii /= CurrentConfiguration.Targets.Count;

                    // Get the largest distance.
                    float largestDistance = Services.Clamp(Math.Abs(GetLargestDistance()), 15.0f, 100.0f);

                    // Set the target position.
                    CurrentConfiguration.TargetPosition = centerOfTargets;
                    
                    // Multiple targets end up with this offset amount.
                    CurrentConfiguration.TargetOffsetZ = -((largestDistance + radii) * (CurrentConfiguration.Targets.Count + 1));
                }
            }
            else
            {
                Debugger.Print("No targets at all.");

                // If there are no targets, set the center of the screen as the target.
                CurrentConfiguration.TargetPosition = new Vector3(0.0f, 0.0f, 0.0f);

                // Set the offset to the default.
                CurrentConfiguration.TargetOffsetZ = -Constants.DEFAULT_OFFSET;
            }
        }

        /// <summary>
        /// Returns the largest distance between all elements in the set.
        /// </summary>
        /// <returns>Returns the largest distance.</returns>
        public float GetLargestDistance()
        {
            float minDistance = CurrentConfiguration.Offset.z;
            float maxDistance = minDistance;
            float currentDistance = 0.0f;

            if (CurrentConfiguration.HasTargets)
            {
                for (int i = 0; i < CurrentConfiguration.Targets.Count; i++)
                {
                    for (int j = CurrentConfiguration.Targets.Count - 1; j >= 0; j--)
                    {
                        if (i != j)
                        {
                            currentDistance = CurrentConfiguration.Targets[i].CalculateDistance(CurrentConfiguration.Targets[j]);
                            maxDistance = Services.Max(currentDistance, maxDistance);
                        }
                    }
                }
            }

            return maxDistance;
        }

        /// <summary>
        /// Reset the camera.
        /// </summary>
        public void ResetCamera()
        {
            Settings[m_mode] = CameraConfiguration.Create();
            Settings[m_mode].InitialFOV = Constants.DEFAULT_FIELD_OF_VIEW;
            Settings[m_mode].InitialBackground = Constants.CORNFLOWER_BLUE;
            Settings[m_mode].InitialPosition = m_basePosition;
            Settings[m_mode].InitialOffset = new Vector3(0.0f, 0.0f, -Constants.DEFAULT_OFFSET);
            Settings[m_mode].OrthographicSize = 5.0f;
            Settings[m_mode].Initialize();
            Settings[m_mode].TargetPosition = Vector3.zero;  
        }
        
        /// <summary>
        /// Returns true if in specified camera mode.
        /// </summary>
        /// <returns>Returns true if match.</returns>
        public bool IsFreeCamera()
        {
            return (this.m_mode == CameraMode.Free);
        }

        /// <summary>
        /// Returns true if in specified camera mode.
        /// </summary>
        /// <returns>Returns true if match.</returns>
        public bool IsFixedCamera()
        {
            return (this.m_mode == CameraMode.Fixed);
        }
        
        /// <summary>
        /// Returns true if in specified camera mode.
        /// </summary>
        /// <returns>Returns true if match.</returns>
        public bool IsTargetOne()
        {
            return (this.m_mode == CameraMode.TargetOne);
        }

        /// <summary>
        /// Returns true if in specified camera mode.
        /// </summary>
        /// <returns>Returns true if match.</returns>
        public bool IsTargetAll()
        {
            return (this.m_mode == CameraMode.TargetAll);
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Get target index for input CameraTarget.
        /// </summary>
        /// <param name="_query">CameraTarget to get index for.</param>
        /// <returns>Returns -1 if invalid.</returns>
        private int GetTargetIndex(CameraTarget _query)
        {
            if (CurrentConfiguration.HasTargets)
            {
                return CurrentConfiguration.Targets.IndexOf(_query);
            }

            return -1;
        }

        /// <summary>
        /// Select the CameraTarget.
        /// </summary>
        /// <param name="_query">CameraTarget to select.</param>
        public void SelectTarget(CameraTarget _query)
        {
            int index = GetTargetIndex(_query);

            if (index != -1)
            {
                CurrentConfiguration.SelectedTargetIndex = index;
            }
        }

        /// <summary>
        /// Returns the current target if the index allows. Returns null if empty or index not set.
        /// </summary>
        /// <returns></returns>
        public CameraTarget GetCurrentTarget()
        {
            // If it has targets, grabe
            if (CurrentConfiguration.HasTargets)
            {
                if (CurrentConfiguration.SelectedTargetIndex >= 0 && CurrentConfiguration.SelectedTargetIndex < CurrentConfiguration.Targets.Count)
                {
                    return CurrentConfiguration.Targets[CurrentConfiguration.SelectedTargetIndex];
                }
            }

            return null;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set the current mode of the camera settings component.
        /// </summary>
        /// <param name="_mode"></param>
        public void SetMode(CameraMode _mode)
        {
            if (this.m_mode != _mode)
            {
                this.m_mode = _mode;
                ResetCamera();
            }
        }

        /// <summary>
        /// Removes target if it already exists.
        /// </summary>
        /// <param name="_target">Target to remove.</param>
        /// <returns>Returns CameraTarget.</returns>
        public CameraTarget RemoveTarget(CameraTarget _target)
        {
            if (CurrentConfiguration.HasTargets && CurrentConfiguration.Targets.Contains(_target))
            {
                Debugger.Print("Removing target from collection.");
                CurrentConfiguration.Targets.Remove(_target);
            }
            else
            {
                Debugger.Print("Target does not contain a reference.");
            }

            return _target;
        }

        /// <summary>
        /// Adds target if it doesn't already exist.
        /// </summary>
        /// <param name="_target">Target to add.</param>
        /// <returns>Returns CameraTarget.</returns>
        public CameraTarget AddTarget(CameraTarget _target)
        {
            if (CurrentConfiguration.HasTargets && CurrentConfiguration.Targets.Contains(_target))
            {
                Debugger.Print("Target already contains reference.");
            }
            else
            {
                Debugger.Print("Adding target to collection.");
                CurrentConfiguration.Targets.Add(_target);
            }

            return _target;
        }

        /// <summary>
        /// Removes target from all the modes specified.
        /// </summary>
        /// <param name="_target">Target to remove.</param>
        /// <param name="_modes">Modes to remove targets from.</param>
        /// <returns>Returns removed camera target.</returns>
        public CameraTarget RemoveTarget(CameraTarget _target, params CameraMode[] _modes)
        {
            if (_modes.Length > 0)
            {
                foreach (CameraMode mode in _modes)
                {
                    if (Settings.ContainsKey(mode) && Settings[mode] != null && Settings[mode].Targets.Contains(_target))
                    {
                        Debugger.Print("Removed target " + _target.Name + " from Camera " + this.Name + "'s Camera Mode: " + Parse(mode) + ".");
                        Settings[mode].Targets.Remove(_target);
                    }
                }
            }

            return _target;
        }

        /// <summary>
        /// Add targets to all the modes specified.
        /// </summary>
        /// <param name="_target">Target to add.</param>
        /// <param name="_modes">Modes to add targets for.</param>
        /// <returns>Returns added camera target.</returns>
        public CameraTarget AddTarget(CameraTarget _target, params CameraMode[] _modes)
        {
            if (_modes.Length > 0)
            {
                foreach (CameraMode mode in _modes)
                {
                    if (Settings.ContainsKey(mode) && Settings[mode] != null && !Settings[mode].Targets.Contains(_target))
                    {
                        Debugger.Print("Add target " + _target.Name + " to Camera " + this.Name + "'s Camera Mode: " + Parse(mode) + ".");
                        Settings[mode].Targets.Add(_target);
                    }
                }
            }

            return _target;
        }

        #endregion

    }

    #endregion

}
