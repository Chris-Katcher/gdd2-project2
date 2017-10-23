/************************************************
 * CameraSettings.cs
 * 
 * This file contains implementation for the CameraSettings,
 * CameraFactory classes and definition for CameraMode enum.
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
using Arcana.Entities;

namespace Arcana.Cameras
{

    #region // Enum: CameraMode

    /////////////////////
    // CameraMode enum declaration.
    /////////////////////

    /// <summary>
    /// Determines what mode the camera will follow when tracking.
    /// </summary>
    public enum CameraMode
    {
        Disabled, // = 0
        Tracking, // = 1
        TrackOne, // = 2
        Fixed // = 3
    }

    #endregion

    #region // Class: CameraFactory class.

    /////////////////////
    // CameraFactory class.
    /////////////////////

    /// <summary>
    /// This will construct a Camera (and UnityCamera) for our game object to hold.
    /// </summary>
    public class CameraFactory : IFactory<CameraSettings> {

        #region // // Static Members.

        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static CameraFactory instance = null;
        
        /// <summary>
        /// Get reference to CameraFactory.
        /// </summary>
        /// <returns>Returns a single factory.</returns>
        public static CameraFactory Instance()
        {
            if (instance == null)
            {
                instance = new CameraFactory();
            }

            return instance;
        }
        
        #endregion

        #region // // Factory methods.

        /// <summary>
        /// Returns a reference to the factory.
        /// </summary>
        /// <returns>Returns a single factory.</returns>
        public IFactory<CameraSettings> GetInstance()
        {
            return Instance();
        }

        /// <summary>
        /// Creates a new, empty game object, and returns the CameraSettings component back.
        /// </summary>
        /// <returns>Returns newly created component.</returns>
        public CameraSettings CreateComponent()
        {
            // Creates a component using the default settings.
            return CreateComponent(Services.CreateEmptyObject("Camera Settings"), CreateSettings());
        }

        /// <summary>
        /// Adds a new component to the parent game object, with parameters.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <param name="parameters">Settings to apply to the new Entity.</param>
        /// <returns>Return newly created component.</returns>
        public CameraSettings CreateComponent(GameObject parent, Constraints parameters)
        {
            // Check game object.
            if (parent == null)
            {
                // If the parent itself is null, do not return a component.
                Debugger.Print("Tried to add a component but parent GameObject is null.", "NULL_REFERENCE");
                return null;
            }

            // Get reference to existing script if it already exists on this parent.
            CameraSettings component = parent.GetComponent<Arcana.Cameras.CameraSettings>();

            // If reference is null, create one.
            if (component == null)
            {
                // Add a new camera component to the component reference.
                component = parent.AddComponent<Arcana.Cameras.CameraSettings>();
            }

            // Get reference to the sibling camera.
            UnityCamera camera; // Declare camera variable to handle reference.

            // Check if sibling camear already exists.
            if (!HasCamera(parent, out camera))
            {
                // Will return false if it doesn't have a camera.
                camera = CreateCamera(parent); // Build and add camera component to the parent GameObject.
            }

            // Assign non-optional information.
            component.Initialize(parent, camera);

            // Initialize the entity.
            foreach (string key in parameters.ValidEntries)
            {
                component.Initialize(key, parameters.GetEntry(key).Value);
            }

            return component;
        }
        
        /// <summary>
        /// Adds a new, default component, to the parent game object.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <returns>Return newly created component.</returns>
        public CameraSettings CreateComponent(GameObject parent)
        {
            // Creates a component using the default settings.
            return CreateComponent(parent, CreateSettings());
        }

        /// <summary>
        /// Set up the parameters associated with this factory's IFactoryElement.
        /// </summary>
        /// <returns>Returns the <see cref="Constraints"/> collection object.</returns>
        public Constraints CreateSettings(
            Vector2? _position = null,
            CameraMode _mode = CameraMode.Fixed,
            float _aspect = Constants.DEFAULT_ASPECT_RATIO,
            float _size = Constants.DEFAULT_CAMERA_SIZE,
            float _distance = Constants.DEFAULT_CAMERA_DISTANCE,
            Color? _bg = null)
        {
            // Create the collection.
            Constraints parameters = new Constraints();
            
            // Verify nullable types.
            if (_position.HasValue) { parameters.AddValue<Vector2>(Constants.PARAM_POSITION, _position.Value); } // Set the position.
            if (_bg.HasValue) { parameters.AddValue<Color>(Constants.PARAM_BACKGROUND, _bg.Value); }

            // Add non-nulllable types.
            parameters.AddValue<CameraMode>(Constants.PARAM_CAMERA_MODE, _mode); // Camera mode.
            parameters.AddValue<float>(Constants.PARAM_ASPECT_RATIO, _aspect); // Aspect ratio.
            parameters.AddValue<float>(Constants.PARAM_DIMENSIONS, _size); // Orthographic size.
            parameters.AddValue<float>(Constants.PARAM_DISTANCE, _distance); // Distance.

            return parameters;
        }

        #endregion

        #region // // Static Constructors.
        
        /// <summary>
        /// Adds a new <see cref="UnityEngine.Camera"/> to the parent <see cref="GameObject"/>.
        /// </summary>
        /// <param name="parent"><see cref="GameObject"/> receiving child component.</param>
        /// <returns>Returns reference to added component.</returns>
        private static UnityCamera CreateCamera(GameObject parent)
        {
            // Check game object.
            if (parent == null)
            {
                // If the parent itself is null, do not return a component.
                Debugger.Print("Tried to add a Arcanas.Cameras.Camera component but parent GameObject is null.", "NULL_REFERENCE");
                return null;
            }

            // Add component to the parent, and, get its reference.
            UnityCamera reference = parent.AddComponent<UnityCamera>();

            // Return its reference.
            return reference;
        }

        #endregion
        
        #region // // Service Methods.

        /// <summary>
        /// Checks if the <see cref="GameObject"/> has a <see cref="UnityEngine.Camera"/> component.
        /// </summary>
        /// <param name="parent"><see cref="GameObject"/> to check for existence of a <see cref="UnityEngine.Camera"/> component.</param>
        /// <returns>Returns true if the script exists; false if otherwise.</returns>
        private static bool HasCamera(GameObject parent)
        {
            return (parent.GetComponent<UnityCamera>() != null); // Return true, if reference isn't equal to null.
        }

        /// <summary>
        /// Checks if the <see cref="GameObject"/> has a <see cref="UnityEngine.Camera"/> component.
        /// </summary>
        /// <param name="parent"><see cref="GameObject"/> to check for existence of a <see cref="UnityEngine.Camera"/> component.</param>
        /// <param name="reference">Returns reference to the <see cref="UnityEngine.Camera"/> component, if it exists.</param>
        /// <returns>Returns true if the script exists; false if otherwise.</returns>
        private static bool HasCamera(GameObject parent, out UnityCamera reference)
        {
            reference = parent.GetComponent<UnityCamera>(); // Get the component if it exists.
            return (reference != null); // Return true, if reference isn't equal to null.
        }

        #endregion

    }

    #endregion

    #region // Class: CameraSettings class.

    /////////////////////
    // CameraSettings class declaration.
    /////////////////////

    /// <summary>
    /// Holds settings that will be applied to the UnityCamera.
    /// </summary>
    public class CameraSettings : MonoBehaviour, IFactoryElement
    {

        #region // // Data Members.

        /////////////////////
        // Attributes.
        /////////////////////

        /// <summary>
        /// Reference to <see cref="UnityEngine.Camera"/> component.
        /// </summary>
        private UnityCamera m_camera;

        /// <summary>
        /// The <see cref="Arcana.Cameras.CameraMode"/> this component is in.
        /// </summary>
        private CameraMode m_cameraMode;

        /// <summary>
        /// Checks to see if the <see cref="Arcana.Cameras.CameraMode"/> has changed.
        /// </summary>
        private CameraMode m_previousMode;

        /// <summary>
        /// Current position of the parent.
        /// </summary>
        private Vector3 m_position;

        /// <summary>
        /// Original position of the parent.
        /// </summary>
        private Vector3 m_positionOriginal;

        /// <summary>
        /// The value assigned to <see cref="UnityEngine.Camera.aspect"/> is the ratio of width / height. Reset to screen with <see cref="UnityEngine.Camera.ResetAspect"/>.
        /// </summary>
        private float m_aspectRatio;

        /// <summary>
        /// The offset z-axis distance away from the point of focus.
        /// </summary>
        private float m_distance;
        
        /// <summary>
        /// The value assigned to <see cref="UnityEngine.Camera.orthographicSize"/>.
        /// </summary>
        private float m_size;

        /// <summary>
        /// The color given to <see cref="UnityEngine.Camera.backgroundColor"/>.
        /// </summary>
        private Color m_background;
        
        /// <summary>
        /// Collection of the <see cref="GameObject"/>s treated as camera subjects.
        /// </summary>
        private List<GameObject> m_targets;

        /// <summary>
        /// Index representing current selection of camera subjects in <see cref="m_targets"/>.
        /// </summary>
        private int m_targetIndex;
        
        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Reference to the camera.
        /// </summary>
        public UnityCamera Aperture
        {
            get { return this.m_camera; }
            set { this.SetCamera(value); }
        }

        /// <summary>
        /// Current Camera mode.
        /// </summary>
        public CameraMode Mode
        {
            get { return this.m_cameraMode; }
            set { this.SetMode(value); }
        }
        
        /// <summary>
        /// Determines if camera is set to be disabled.
        /// </summary>
        public bool Disabled
        {
            get { return (this.m_cameraMode == CameraMode.Disabled); }
        }

        /// <summary>
        /// Determines if this object has a camera.
        /// </summary>
        public bool HasCamera
        {
            get { return (this.m_cameraMode != CameraMode.Disabled && this.Aperture != null); }
        }

        /// <summary>
        /// Determines if the mode changed since last frame.
        /// </summary>
        private bool HasModeChanged
        {
            get { return (this.m_previousMode != this.m_cameraMode); }
        }

        /// <summary>
        /// Determine if camera has targets to follow.
        /// </summary>
        private bool HasTargets
        {
            get { return (this.m_targets != null && this.m_targets.Count > 0); }
        }

        #endregion

        #region // // Service Methods.

        /// <summary>
        /// Called when a new Camera is added to an object as a component.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="_camera"></param>
        /// <param name="_mode"></param>
        /// <param name="_aspect"></param>
        /// <param name="_size"></param>
        /// <param name="_distance"></param>
        internal void Initialize(GameObject parent, UnityCamera _camera,
            Vector2? _position = null,
            CameraMode _mode = CameraMode.Fixed,
            float _aspect = Constants.DEFAULT_ASPECT_RATIO,
            float _size = Constants.DEFAULT_CAMERA_SIZE,
            float _distance = Constants.DEFAULT_CAMERA_DISTANCE,
            Color? _bg = null)
        {
            // Set values for attributes.
            this.SetCamera(_camera);
            this.SetAspectRatio(_aspect);
            this.SetOrthographicSize(_size);
            this.SetBackground(_bg);
            this.SetDistance(_distance);
            this.SetMode(_mode);

            if (_position.HasValue)
            {
                this.SetPosition(_position.Value);
            }
            else
            {
                this.SetPosition(parent.transform.position);
            }
            
            // This should always be the initial value for the 'previous mode' at the start.
            this.m_previousMode = CameraMode.Disabled;            
        }

        /// <summary>
        /// Initailize a parameter based on input key.
        /// </summary>
        /// <param name="parameter">Parameter reference.</param>
        /// <param name="value">Value to set.</param>
        public void Initialize(string parameter, object value)
        {
            switch(parameter)
            {
                case Constants.PARAM_UNITY_CAMERA:
                    this.SetCamera((UnityCamera)value);
                    break;
                case Constants.PARAM_ASPECT_RATIO:
                    this.SetAspectRatio((float)value);
                    break;
                case Constants.PARAM_POSITION:
                    this.SetPosition((Vector2)value);
                    break;
                case Constants.PARAM_DIMENSIONS:
                    this.SetOrthographicSize((float)value);
                    break;
                case Constants.PARAM_BACKGROUND:
                    this.SetBackground((Color)value);
                    break;
                case Constants.PARAM_DISTANCE:
                    this.SetAspectRatio((float)value);
                    break;
                case Constants.PARAM_CAMERA_MODE:
                    this.SetMode((CameraMode)value);
                    break;
            }
        }

        /// <summary>
        /// Update using <see cref="MonoBehaviour"/> functionality.
        /// </summary>
        private void Update()
        {
            // If not disabled:
            if (!this.Disabled)
            {
                // Update based on current mode.
                UpdateMode();
            }
        }
        /// <summary>
        /// Update references to all properties in the <see cref="UnityEngine.Camera"/> reference.
        /// </summary>
        private void UpdateProperties()
        {
            if (HasCamera)
            {
                // Update info.
                this.SetAspectRatio(this.m_aspectRatio);
                this.SetOrthographicSize(this.m_size);
                this.SetBackground(this.m_background);
                this.SetDistance(this.m_distance);
                this.SetMode(this.m_cameraMode);
            }
        }

        /// <summary>
        /// Call the appropriate update method.
        /// </summary>
        private void UpdateMode()
        {
            if (HasCamera)
            {
                // Set the position.
                this.m_position = this.m_positionOriginal + Services.ToVector3(0.0f, 0.0f, this.m_distance);

                if (this.HasModeChanged)
                {
                    // Update properties on mode switch.
                    UpdateProperties();
                }

                if (this.m_cameraMode == CameraMode.Fixed)
                {
                    this.UpdateMode_Fixed();
                }
                else if (this.m_cameraMode == CameraMode.Tracking ||
                         this.m_cameraMode == CameraMode.TrackOne)
                {
                    this.UpdateMode_Tracking();
                }
            }

            // Update the modes.
            this.m_previousMode = this.m_cameraMode;
        }

        /// <summary>
        /// Places the <see cref="UnityEngine.Camera"/> without worry of any targets.
        /// </summary>
        private void UpdateMode_Fixed()
        {
            // Set current position to the transform.
            if (!float.IsNaN(this.m_position.magnitude))
            {
                gameObject.transform.position = this.m_position;
            }
        }

        /// <summary>
        /// Positions and clamps camera based on targets, if any.
        /// </summary>
        private void UpdateMode_Tracking()
        {
            Vector2 center = this.m_positionOriginal; // Focus on original position if no targets.

            if (HasTargets)
            {
                if (this.m_cameraMode == CameraMode.TrackOne)
                {
                    // Get the selected target's current position, and, turn it into a Vector2.
                    center = Services.ToVector2(this.m_targets[this.m_targetIndex].transform.position);
                }
                else
                {
                    // Array to store all the points.
                    Vector2[] points = new Vector2[this.m_targets.Count];

                    for(int i = 0; i < this.m_targets.Count; i++)
                    {
                        points[i] = Services.ToVector2(this.m_targets[i].transform.position);
                    }

                    // Get center of all the input vectors.
                    center = Services.Average(points);
                }
            }

            SetPosition(center);
        }

        /// <summary>
        /// If in track one mode, will go to the next index in the collection of targets.
        /// </summary>
        public void NextIndex()
        {
            if (this.m_cameraMode == CameraMode.TrackOne)
            {
                int temp = this.m_targetIndex + 1;

                if (temp < 0)
                {
                    temp = (this.m_targets.Count + 1);
                }

                this.SetIndex(temp);
            }
        }

        /// <summary>
        /// If in track one mode, will go to the previous index in the collection of targets.
        /// </summary>
        public void PreviousIndex()
        {
            if (this.m_cameraMode == CameraMode.TrackOne)
            {
                int temp = this.m_targetIndex - 1;

                if (temp < 0)
                {
                    temp = (this.m_targets.Count - 1);
                }

                this.SetIndex(temp);
            }
        }
        
        /// <summary>
        /// Remove this target if a match is found.
        /// </summary>
        /// <param name="index">Remove target by index.</param>
        public void RemoveTarget(GameObject target)
        {
            if (HasTargets && this.m_targets.Contains(target))
            {
                this.m_targets.Remove(target);
            }
        }

        /// <summary>
        /// Remove a target from the list by index.
        /// </summary>
        /// <param name="index">Remove target by index.</param>
        public void RemoveTarget(int index)
        {
            if (HasTargets && index >= 0 && index < this.m_targets.Count)
            {
                this.m_targets.RemoveAt(index);
            }
        }
        
        /// <summary>
        /// Add a target to the list if it doesn't already exist, at the specified index.
        /// </summary>
        public void AddTarget(GameObject target, int index)
        {
            if (!this.m_targets.Contains(target))
            {
                if (index >= 0 && index < this.m_targets.Count)
                {
                    this.m_targets.Insert(index, target);
                }
                else
                {
                    this.m_targets.Add(target);
                }
            }
        }

        /// <summary>
        /// Add a target to the list if it doesn't already exists.
        /// </summary>
        public void AddTarget(GameObject target)
        {
            if (!this.m_targets.Contains(target))
            {
                this.m_targets.Add(target);
            }
        }

        #endregion

        #region // // Mutator Methods.

        /// <summary>
        /// Set the <see cref="UnityEngine.Camera"/> object.
        /// </summary>
        /// <param name="_camera">Camera to set.</param>
        public void SetCamera(UnityCamera _camera)
        {
            if (_camera != null)
            {
                if (this.m_camera == null)
                {
                    this.m_camera = _camera;
                }
                else
                {
                    Debugger.Print("This camera already has a camera assigned.");
                }
            }
        }

        /// <summary>
        /// Set the index to this input value, ensuring it's clamped between the limits.
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(int index)
        {
            if (this.HasTargets)
            {
                this.m_targetIndex = Services.Clamp(index, 0, (this.m_targets.Count - 1));
            }
        }

        /// <summary>
        /// Set the aspect ratio of the camera.
        /// </summary>
        /// <param name="value">Value to set aspect ratio to.</param>
        public void SetAspectRatio(float value)
        {
            this.m_aspectRatio = Services.Clamp(value, 0.5f, 5.0f);
            this.Aperture.aspect = this.m_aspectRatio;
        }

        /// <summary>
        /// Set the size of the orthographic viewport.
        /// </summary>
        /// <param name="value">Value to set viewport size to.</param>
        public void SetOrthographicSize(float value)
        {
            this.m_size = Services.Clamp(value, 1f, 25.0f);
            this.Aperture.orthographicSize = this.m_size;
        }

        /// <summary>
        /// Set clear color of the camera.
        /// </summary>
        /// <param name="c">Color to clear screen with.</param>
        public void SetBackground(Color c)
        {
            this.m_background = c;
            this.Aperture.backgroundColor = this.m_background;
        }
        
        /// <summary>
        /// Set clear color of the camera.
        /// </summary>
        /// <param name="c">Color (nullable) to clear screen with.</param>
        public void SetBackground(Color? c)
        {
            if (c.HasValue)
            {
                this.m_background = c.Value;
            }
            else
            {
                this.m_background = Constants.DEFAULT_CAMERA_BACKGROUND;
            }

            this.Aperture.backgroundColor = this.m_background;
        }

        /// <summary>
        /// Set the <see cref="CameraMode"/> mode. 
        /// </summary>
        /// <param name="_mode">Mode to set to.</param>
        public void SetMode(CameraMode _mode)
        {
            // Validates the input.
            if (_mode == CameraMode.Tracking ||
                _mode == CameraMode.TrackOne ||
                _mode == CameraMode.Fixed ||
                _mode == CameraMode.Disabled)
            {
                // Set the mode.
                this.m_previousMode = this.m_cameraMode;
                this.m_cameraMode = _mode;

                // Update mode if need be.
                this.UpdateMode();
            }
        }

        /// <summary>
        /// Sets the distance of the camera, away from target (or target plane).
        /// </summary>
        /// <param name="offset"></param>
        public void SetDistance(float offset)
        {
            this.m_distance = Services.Clamp(offset, 0.0f, 100.0f);
        }

        /// <summary>
        /// Set the pixel displayed by the <see cref="UnityEngine.Camera"/>.
        /// </summary>
        /// <param name="_pixelRect"></param>
        public void SetPixels(Rect? _pixelRect)
        {
            if (_pixelRect.HasValue)
            {
                this.Aperture.pixelRect = _pixelRect.Value;
            }
        }

        /// <summary>
        /// Change the "original" position.
        /// </summary>
        /// <param name="position">Position to move camera to.</param>
        public void SetPosition(Vector2 position)
        {
            this.m_positionOriginal = position;
        }

        /// <summary>
        /// Set the position (and offset) of the camera.
        /// </summary>
        /// <param name="position">Position to move camera to.</param>
        public void SetPosition(Vector3 position)
        {
            // Set the position using the x and y.
            SetPosition(Services.ToVector2(position));

            // Set the distance using the z dimension.
            SetDistance(position.z);
        }

        #endregion

    }

    #endregion 

}
