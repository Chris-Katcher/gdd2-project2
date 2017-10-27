/************************************************
 * CameraConfiguration.cs
 * 
 * This file contains:
 * - The CameraConfiguration class.
 * - The Velocity structure.
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

namespace Arcana.Cameras
{

    #region Class: CameraConfiguration class.

    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// Data structure for storing information about a camera's important visual settings.
    /// </summary>
    public class CameraConfiguration
    {
        
        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Initial background color for the camera.
        /// </summary>
        private Color32 m_initBackground;

        /// <summary>
        /// Current background color for the camera.
        /// </summary>
        private Color32 m_currentBackground;

        /// <summary>
        /// Target background to lerp towards.
        /// </summary>
        private Color32 m_targetBackground;

        /// <summary>
        /// Orthographic mode flag.
        /// </summary>
        private bool m_orthographic;

        /// <summary>
        /// Initial FOV for the configuration.
        /// </summary>
        private float m_initFOV;

        /// <summary>
        /// Current FOV for the configuration.
        /// </summary>
        private float m_currentFOV;

        /// <summary>
        /// Target FOV for the configuration.
        /// </summary>
        private float m_targetFOV;

        /// <summary>
        /// Initial aspect ratio.
        /// </summary>
        private float m_initRatio;

        /// <summary>
        /// Current aspect ratio.
        /// </summary>
        private float m_currentRatio;

        /// <summary>
        /// Target aspect ratio.
        /// </summary>
        private float m_targetRatio;

        /// <summary>
        /// Initial orthographic size.
        /// </summary>
        private float m_initSize;

        /// <summary>
        /// Current orthographic size.
        /// </summary>
        private float m_currentSize;

        /// <summary>
        /// Target orthographic size.
        /// </summary>
        private float m_targetSize;

        /// <summary>
        /// Speed at which properties will lerp.
        /// </summary>
        private float m_propertySpeed;

        /// <summary>
        /// Lerp finality threshold.
        /// </summary>
        private float m_threshold;

        /// <summary>
        /// Starting position of the camera.
        /// </summary>
        private Vector3 m_initPosition;

        /// <summary>
        /// Current position of the camera.
        /// </summary>
        private Vector3 m_currentPosition;

        /// <summary>
        /// Target position of the camera.
        /// </summary>
        private Vector3 m_targetPosition;

        /// <summary>
        /// Display position of the camera.
        /// </summary>
        private Vector3 m_displayPosition;

        /// <summary>
        /// Iniital offset position.
        /// </summary>
        private Vector3 m_initOffset;

        /// <summary>
        /// Offset position for the x, y, and z axes.
        /// </summary>
        private Vector3 m_offsetPosition;

        /// <summary>
        /// Target offset position.
        /// </summary>
        private Vector3 m_targetOffset;

        /// <summary>
        /// Speed of the camera in the x, y, and z directions.
        /// </summary>
        private Velocity m_cameraSpeed;

        /// <summary>
        /// List of targets to follow.
        /// </summary>
        private List<CameraTarget> m_targets = null;

        /// <summary>
        /// Index representing currently selected camera target.
        /// </summary>
        private int m_targetIndex = -1;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Background color for the camera.
        /// </summary>
        public Color32 InitialBackground
        {
            get { return this.m_initBackground; }
            set { this.m_initBackground = value; }
        }

        /// <summary>
        /// Current background color for the camera.
        /// </summary>
        public Color32 Background
        {
            get { return this.m_currentBackground; }
            set { this.m_currentBackground = value; }
        }
        
        /// <summary>
        /// Target background to lerp towards.
        /// </summary>
        public Color32 TargetBackground
        {
            get { return this.m_targetBackground; }
            set { this.m_targetBackground = value; }
        }

        /// <summary>
        /// Orthographic mode flag.
        /// </summary>
        public bool Orthographic
        {
            get { return this.m_orthographic; }
            set { this.m_orthographic = value; }
        }

        /// <summary>
        /// Perspective mode flag.
        /// </summary>
        public bool Perspective
        {
            get { return !Orthographic; }
            set { this.m_orthographic = !value; }
        }

        /// <summary>
        /// Initial FOV for the configuration.
        /// </summary>
        public float InitialFOV
        {
            get { return this.m_initFOV; }
            set { this.m_initFOV = Services.Clamp(value, 0, 360); }
        }

        /// <summary>
        /// Current FOV for the configuration.
        /// </summary>
        public float FOV
        {
            get { return this.m_currentFOV; }
            set { this.m_currentFOV = Services.Clamp(value, 0, 360); }
        }

        /// <summary>
        /// Target FOV for the configuration.
        /// </summary>
        public float TargetFOV
        {
            private get { return this.m_targetFOV; }
            set { this.m_targetFOV = Services.Clamp(value, 0, 360); }
        }

        /// <summary>
        /// Initial aspect ratio.
        /// </summary>
        public float InitialAspectRatio
        {
            get { return this.m_initRatio; }
            set { this.m_initRatio = Services.Clamp(value, 0, 3); }
        }

        /// <summary>
        /// Current aspect ratio.
        /// </summary>
        public float AspectRatio
        {
            get { return this.m_currentRatio; }
            set { this.m_currentRatio = Services.Clamp(value, 0, 3); }
        }
        
        /// <summary>
        /// Target aspect ratio.
        /// </summary>
        public float TargetAspectRatio
        {
            private get { return this.m_targetRatio; }
            set { this.m_targetRatio = Services.Clamp(value, 0, 3); }
        }

        /// <summary>
        /// Initial orthographic size.
        /// </summary>
        public float InitialOrthographicSize
        {
            get { return this.m_initSize; }
            set { this.m_initSize = Services.Clamp(value, 1, 25); }
        }

        /// <summary>
        /// Current orthographic size.
        /// </summary>
        public float OrthographicSize
        {
            get { return this.m_currentSize; }
            set { this.m_currentSize = Services.Clamp(value, 1, 25); }
        }
        
        /// <summary>
        /// Target orthographic size.
        /// </summary>
        public float TargetOrthographicSize
        {
            get { return this.m_targetSize; }
            set { this.m_targetSize = Services.Clamp(value, 1, 25); }
        }
        
        /// <summary>
        /// Speed at which properties will lerp.
        /// </summary>
        public float PropertySpeed
        {
            get { return this.m_propertySpeed; }
            set { this.m_propertySpeed = value; }
        }

        /// <summary>
        /// Reference to fixed delta time elapsed in seconds.
        /// </summary>
        public float Delta
        {
            get { return Time.fixedDeltaTime; }
        }

        /// <summary>
        /// Returns the (delta x property) speed product.
        /// </summary>
        public float PropertyDelta
        {
            get { return (Delta * PropertySpeed); }
        }

        /// <summary>
        /// Lerp finality threshold.
        /// </summary>
        public float Threshold
        {
            get { return this.m_threshold; }
            set { this.m_threshold = value; }
        }

        /// <summary>
        /// Starting position of the camera. (Doesn't consider offset).
        /// </summary>
        public Vector3 InitialPosition
        {
            get { return (Vector3)this.m_initPosition; }
            set { this.m_initPosition = value; }
        }

        /// <summary>
        /// Current position of the camera. (Doesn't consider offset).
        /// </summary>
        private Vector3 Position
        {
            get { return (Vector3)this.m_currentPosition; }
            set { this.m_currentPosition = value; }
        }

        /// <summary>
        /// Target position of the camera. (Doesn't consider offset).
        /// </summary>
        public Vector3 TargetPosition
        {
            private get { return this.m_targetPosition; }
            set { this.m_targetPosition = value; }
        }

        /// <summary>
        /// Display position of the camera. (Includes offset).
        /// </summary>
        public Vector3 DisplayPosition
        {
            get { return this.m_displayPosition; }
            private set { this.m_displayPosition = value; }
        }

        /// <summary>
        /// Initial offset position for the x, y, and z axes.
        /// </summary>
        public Vector3 InitialOffset
        {
            get { return this.m_initOffset; }
            set { this.m_initOffset = value; }
        }

        /// <summary>
        /// Offset position for the x, y, and z axes.
        /// </summary>
        public Vector3 Offset
        {
            get { return this.m_offsetPosition; }
            set { this.m_offsetPosition = value; }
        }
        
        /// <summary>
        /// Target offset position for the x, y, and z axes.
        /// </summary>
        public Vector3 TargetOffset
        {
            private get { return this.m_targetOffset; }
            set { this.m_targetOffset = value; }
        }

        /// <summary>
        /// Assign the z-axis offset for the offset.
        /// </summary>
        public float TargetOffsetZ
        {
            private get { return this.m_targetOffset.z; }
            set { this.m_targetOffset = new Vector3(m_targetOffset.x, m_targetOffset.y, value); }
        }

        /// <summary>
        /// Speed of the camera in the x, y, and z directions.
        /// </summary>
        public Velocity CameraVelocity
        {
            get { return this.m_cameraSpeed; }
            private set { this.m_cameraSpeed = value; }
        }

        /// <summary>
        /// Speed of the camera in all three directions.
        /// </summary>
        public float Speed
        {
            set {
                float min = this.m_cameraSpeed.MinSpeed;
                float max = this.m_cameraSpeed.MaxSpeed;
                this.m_cameraSpeed = new Velocity(value, value, value, min, max);
            }
        }

        /// <summary>
        /// Returns true if there is a collection of targets.
        /// </summary>
        public bool HasTargets
        {
            get
            {
                if (this.Targets != null && this.Targets.Count > 0)
                {
                    foreach (CameraTarget target in this.Targets)
                    {
                        // If at least one target is targetable, return true.
                        if (target.IsTargetable())
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Returns boolean indicating whether or not there is a selectable target.
        /// </summary>
        public bool HasSelectableTarget
        {
            get { return (SelectedTarget != null); }
        }

        /// <summary>
        /// List of targets to follow.
        /// </summary>
        public List<CameraTarget> Targets
        {
            get
            {
                if (this.m_targets == null)
                {
                    this.m_targets = new List<CameraTarget>();
                }

                return this.m_targets;
            }
        }

        /// <summary>
        /// Return the currently selected target if one exists and can be selected.
        /// </summary>
        public CameraTarget SelectedTarget
        {
            get
            {
                if (HasTargets && Services.InRange<int>(SelectedTargetIndex, 0, (Targets.Count - 1)))
                {
                    // Returns selected target.
                    return Targets[SelectedTargetIndex];
                }
                else
                {
                    // Return nothing if can't select.
                    return null;
                }
            }
        }

        /// <summary>
        /// Index representing currently selected camera target.
        /// </summary>
        public int SelectedTargetIndex
        {
            get { return this.m_targetIndex; }
            set { this.m_targetIndex = Services.Clamp(value, -1, Targets.Count); }
        }

        #endregion

        #region Constructor.

        /// <summary>
        /// Returns a default configuration object that's been initialized.
        /// </summary>
        /// <returns>Returns a CameraConfiguration object.</returns>
        public static CameraConfiguration Create()
        {
            return new CameraConfiguration();
        }

        /// <summary>
        /// Returns a confirguation option with values that can be specified.
        /// </summary>
        /// <param name="_bg">Initial background.</param>
        /// <param name="_aspect">Initial aspect ratio.</param>
        /// <param name="_fov">Initial field of view.</param>
        /// <param name="_offset">Initial offset.</param>
        /// <param name="_pos">Initial position.</param>
        /// <param name="_ortho">Orthogrpahic (true) or Perspective (false)?</param>
        /// <param name="_speed">Speed of property value lerping.</param>
        /// <param name="_cameraSpeed">Speed of camera movement lerping.</param>
        /// <param name="_threshold">Threshold for lerping activation.</param>
        /// <returns>Returns a CameraConfiguration object.</returns>
        public static CameraConfiguration Create(
            Color32? _bg = null,
            float _aspect = Constants.DEFAULT_ASPECT_RATIO,
            float _fov = Constants.DEFAULT_FIELD_OF_VIEW,
            Vector3? _offset = null,
            Vector3? _pos = null,
            bool _ortho = false,
            float _speed = 1.0f,
            float _cameraSpeed = 1.0f,
            float _threshold = 0.1f)
        {
            return new CameraConfiguration(_bg, _aspect, _fov, _offset, _pos,
                _ortho, _speed, _cameraSpeed, _threshold);
        }

        /// <summary>
        /// Returns a confirguation option with values that can be specified.
        /// </summary>
        /// <param name="_bg">Initial background.</param>
        /// <param name="_aspect">Initial aspect ratio.</param>
        /// <param name="_fov">Initial field of view.</param>
        /// <param name="_offset">Initial offset.</param>
        /// <param name="_pos">Initial position.</param>
        /// <param name="_ortho">Orthogrpahic (true) or Perspective (false)?</param>
        /// <param name="_speed">Speed of property value lerping.</param>
        /// <param name="_cameraSpeed">Speed of camera movement lerping.</param>
        /// <param name="_threshold">Threshold for lerping activation.</param>
        /// <returns>Returns a CameraConfiguration object.</returns>
        public static CameraConfiguration Create(
            Color32? _bg = null,
            float _aspect = Constants.DEFAULT_ASPECT_RATIO,
            float _fov = Constants.DEFAULT_FIELD_OF_VIEW,
            Vector3? _offset = null,
            Vector3? _pos = null,
            bool _ortho = false,
            float _speed = 1.0f,
            float _cameraSpeed = 1.0f,
            float _threshold = 0.1f,
            Color32? _targetBG = null,
            float _targetAspect = Constants.DEFAULT_ASPECT_RATIO,
            float _targetFov = Constants.DEFAULT_FIELD_OF_VIEW,
            Vector3? _targetOffset = null,
            Vector3? _targetPos = null)
        {
            return new CameraConfiguration(_bg, _aspect, _fov, _offset, _pos,
                _ortho, _speed, _cameraSpeed, _threshold, _targetBG,
                _targetAspect, _targetFov, _targetOffset, _targetPos);
        }

        /// <summary>
        /// Default configuration.
        /// </summary>
        private CameraConfiguration()
        {
            // Default assignments to the configuration values.
            this.InitialBackground = Constants.CORNFLOWER_BLUE;
            this.InitialAspectRatio = Constants.DEFAULT_ASPECT_RATIO;
            this.InitialFOV = Constants.DEFAULT_FIELD_OF_VIEW;
            this.InitialOffset = new Vector3(0.0f, 0.0f, Constants.DEFAULT_OFFSET);
            this.InitialPosition = new Vector3(0.0f, 0.0f, 0.0f);
            this.InitialOrthographicSize = 5.0f;
            this.Orthographic = false;
            this.PropertySpeed = 1.0f;
            this.CameraVelocity = new Velocity(1.0f);
            this.Threshold = 0.1f;

            // Initialize the members.
            Initialize();
        }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="_bg">Initial background.</param>
        /// <param name="_aspect">Initial aspect ratio.</param>
        /// <param name="_fov">Initial field of view.</param>
        /// <param name="_offset">Initial offset.</param>
        /// <param name="_pos">Initial position.</param>
        /// <param name="_ortho">Orthogrpahic (true) or Perspective (false)?</param>
        /// <param name="_speed">Speed of property value lerping.</param>
        /// <param name="_cameraSpeed">Speed of camera movement lerping.</param>
        /// <param name="_threshold">Threshold for lerping activation.</param>
        private CameraConfiguration(Color32? _bg = null,
            float _aspect = Constants.DEFAULT_ASPECT_RATIO,
            float _fov = Constants.DEFAULT_FIELD_OF_VIEW,
            Vector3? _offset = null,
            Vector3? _pos = null,
            bool _ortho = false,
            float _speed = 1.0f,
            float _cameraSpeed = 1.0f,
            float _threshold = 0.1f)
        {
            // Assignments with defaults / to non-nullable values.
            this.InitialBackground = Constants.CORNFLOWER_BLUE;
            this.InitialAspectRatio = _aspect;
            this.InitialFOV = _fov;
            this.InitialOffset = new Vector3(0.0f, 0.0f, Constants.DEFAULT_OFFSET);
            this.InitialPosition = new Vector3(0.0f, 0.0f, 0.0f);
            this.InitialOrthographicSize = 5.0f;
            this.Orthographic = _ortho;
            this.PropertySpeed = _speed;
            this.CameraVelocity = new Velocity(_cameraSpeed);
            this.Threshold = _threshold;

            // Assignment of parameters.
            if (_bg.HasValue) { this.InitialBackground = _bg.Value; }         
            if (_offset.HasValue) { this.InitialOffset = _offset.Value; }
            if (_pos.HasValue) { this.InitialPosition = _pos.Value; }

            // Initialization.
            Initialize();
        }

        /// <summary>
        /// Create configurations with initial targets that can also be specified.
        /// </summary>
        /// <param name="_bg">Initial background.</param>
        /// <param name="_aspect">Initial aspect ratio.</param>
        /// <param name="_fov">Initial field of view.</param>
        /// <param name="_offset">Initial offset.</param>
        /// <param name="_pos">Initial position.</param>
        /// <param name="_ortho">Orthogrpahic (true) or Perspective (false)?</param>
        /// <param name="_speed">Speed of property value lerping.</param>
        /// <param name="_cameraSpeed">Speed of camera movement lerping.</param>
        /// <param name="_threshold">Threshold for lerping activation.</param>
        /// <param name="_targetBG">Target background.</param>
        /// <param name="_targetAspect">Target aspect ratio.</param>
        /// <param name="_targetFov">Target field of view.</param>
        /// <param name="_targetOffset">Target offset.</param>
        /// <param name="_targetPos">Target position.</param>
        private CameraConfiguration(Color32? _bg = null,
            float _aspect = Constants.DEFAULT_ASPECT_RATIO,
            float _fov = Constants.DEFAULT_FIELD_OF_VIEW,
            Vector3? _offset = null,
            Vector3? _pos = null,
            bool _ortho = false,
            float _speed = 1.0f,
            float _cameraSpeed = 1.0f,
            float _threshold = 0.1f,
            Color32? _targetBG = null,
            float _targetAspect = Constants.DEFAULT_ASPECT_RATIO,
            float _targetFov = Constants.DEFAULT_FIELD_OF_VIEW,
            Vector3? _targetOffset = null,
            Vector3? _targetPos = null)
        {
            // Assignments with defaults / to non-nullable values.
            this.InitialBackground = Constants.CORNFLOWER_BLUE;
            this.InitialAspectRatio = _aspect;
            this.InitialFOV = _fov;
            this.InitialOffset = new Vector3(0.0f, 0.0f, Constants.DEFAULT_OFFSET);
            this.InitialPosition = new Vector3(0.0f, 0.0f, 0.0f);
            this.InitialOrthographicSize = 5.0f;
            this.Orthographic = _ortho;
            this.PropertySpeed = _speed;
            this.CameraVelocity = new Velocity(_cameraSpeed);
            this.Threshold = _threshold;

            // Assignment of parameters.
            if (_bg.HasValue) { this.InitialBackground = _bg.Value; }
            if (_offset.HasValue) { this.InitialOffset = _offset.Value; }
            if (_pos.HasValue) { this.InitialPosition = _pos.Value; }

            // Initialization.
            Initialize();

            // Set targets.
            if (_targetBG.HasValue) { this.TargetBackground = _targetBG.Value; }
            if (_targetOffset.HasValue) { this.TargetOffset = _targetOffset.Value; }
            if (_targetPos.HasValue) { this.TargetPosition = _targetPos.Value; }

            // Non-nullable types.
            this.TargetAspectRatio = _targetAspect;
            this.TargetFOV = _targetFov;
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize with the default members for this class.
        /// </summary>
        public void Initialize()
        {
            // Background.
            Background = InitialBackground;
            TargetBackground = InitialBackground;

            // Orthographic.
            OrthographicSize = InitialOrthographicSize;

            // FOV.
            FOV = InitialFOV;
            TargetFOV = InitialFOV;

            // Aspect Ratio.
            AspectRatio = InitialAspectRatio;
            TargetAspectRatio = InitialAspectRatio;

            // Offset
            Offset = InitialOffset;
            TargetOffset = InitialOffset;

            // Position.
            Position = InitialPosition;
            TargetPosition = InitialPosition;
            DisplayPosition = Position + Offset;
        }

        #endregion

        #region Main Update Method.

        /// <summary>
        /// The update method needs to be called on camera configuration, since it isn't a MonoBehaviour.
        /// </summary>
        public void Update(CameraSettings settings)
        {
            if (settings != null && settings.Camera != null)
            {
                // List<CameraTarget> queue = new List<CameraTarget>();

                // Remove targets that no longer are active.
                for(int t = 0; t < Targets.Count; t++)
                {
                    if (t < Targets.Count)
                    {
                        if (Targets[t].Status.IsInactive())
                        {
                            Targets.RemoveAt(t);
                            // queue.Add(Targets[t]);
                        }
                    }
                }

                // Update background.
                UpdateBackground();
                settings.Camera.backgroundColor = Background;

                // Update Orthographic Size.
                UpdateOrthographicSize();
                settings.Camera.orthographic = Orthographic;
                settings.Camera.orthographicSize = OrthographicSize;

                // Update FOV.
                UpdateFOV();
                settings.Camera.fieldOfView = FOV;

                // Update Aspect Ratio.
                UpdateAspectRatio();
                settings.Camera.aspect = AspectRatio;
                
                // Update offset.
                UpdateOffset();
                
                // Update position.
                UpdatePosition();

                // Update display position.
                UpdateDisplayPosition();
                settings.Self.transform.position = DisplayPosition;                
            }
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Update the color of the background.
        /// </summary>
        public void UpdateBackground()
        {
            // If not the same and below the threshold for difference.
            if (!Background.IsSame(TargetBackground) && !InProximity(Background.Distance(TargetBackground)))
            {
                // Lerp the background.
                Background = Color32.Lerp(Background, TargetBackground, PropertyDelta);
            }
        }

        /// <summary>
        /// Update the field of view.
        /// </summary>
        public void UpdateFOV()
        {
            // Update using the update value lerp.
            FOV = UpdateValue(FOV, TargetFOV, PropertyDelta);
        }

        /// <summary>
        /// Update the aspect ratio.
        /// </summary>
        public void UpdateAspectRatio()
        {
            // Update using the update value lerp.
            AspectRatio = UpdateValue(AspectRatio, TargetAspectRatio, PropertyDelta);
        }

        /// <summary>
        /// Update the orthographic size.
        /// </summary>
        public void UpdateOrthographicSize()
        {
            // Update using the update value lerp.
            OrthographicSize = UpdateValue(OrthographicSize, TargetOrthographicSize, PropertyDelta);
        }

        /// <summary>
        /// Upset offset values.
        /// </summary>
        public void UpdateOffset()
        {
            // Update the offset. (using the camera speed * delta).
            Offset = UpdateVector(Offset, TargetOffset, CameraVelocity.Vector * Delta);
        }

        /// <summary>
        /// Update the position values.
        /// </summary>
        public void UpdatePosition()
        {
            // Update the position.
            Position = UpdateVector(Position, TargetPosition, CameraVelocity.Vector * Delta);
        }

        /// <summary>
        /// Update the display position.
        /// </summary>
        public void UpdateDisplayPosition()
        {
            // The display position is simply the position, offset.
            DisplayPosition = Position + Offset;
        }

        /// <summary>
        /// Update a vector value using this function.
        /// </summary>
        /// <param name="_value">Value to start lerp from.</param>
        /// <param name="_target">Target to lerp to.</param>
        /// <param name="_time">Time to use in lerp, for each axis.</param>
        /// <returns>Returns a Vector3.</returns>
        private Vector3 UpdateVector(Vector3 _value, Vector3 _target, Vector3 _time)
        {
            Vector3 result = _value;

            // If not the same and below the threshold for difference.
            if ((_value != _target) && (!InProximity(Vector3.Distance(_value, _target))))
            {
                float x = UpdateValue(_value.x, _target.x, _time.x);
                float y = UpdateValue(_value.y, _target.y, _time.y);
                float z = UpdateValue(_value.z, _target.z, _time.z);

                result = new Vector3(x, y, z);
            }

            return result;
        }

        /// <summary>
        /// Update a value and lerp it based on input values.
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_target"></param>
        /// <returns></returns>
        private float UpdateValue(float _value, float _target, float _time)
        {
            // If not the same and below the threshold for difference.
            if ((_value != _target) && (!InProximity(_value - _target)))
            {
                // Lerp the value.
                _value = Mathf.Lerp(_value, _target, _time);
            }

            return _value;
        }

        /// <summary>
        /// Returns true if the distance is less than the threshold.
        /// </summary>
        /// <param name="distance">Distance to check.</param>
        /// <returns>Returns boolean.</returns>
        public bool InProximity(float distance)
        {
            return (Math.Abs(distance) < Math.Abs(Threshold));
        }

        /// <summary>
        /// Return the z-axis.
        /// </summary>
        /// <returns>Returns the z-offset value.</returns>
        public float GetZOffset()
        {
            return Offset.z;
        }

        /// <summary>
        /// Set new initial, current, and target offsets.
        /// </summary>
        public void SetOffsetRange(Vector3 _init, Vector3 _target)
        {
            InitialOffset = _init;
            Offset = InitialOffset;
            TargetOffset = _target;
        }

        /// <summary>
        /// Set new initial, current, and target offsets.
        /// </summary>
        public void SetOffsetRange(float _init, float _target)
        {
            InitialOffset = new Vector3(0.0f, 0.0f, _init);
            Offset = InitialOffset;
            TargetOffset = new Vector3(0.0f, 0.0f, _target);
        }
                
        #endregion

    }

    #endregion

    #region Struct: Velocity structure.

    /// <summary>
    /// Struct representing the camera speed.
    /// </summary>
    public struct Velocity
    {

        #region Data Members.

        #region Fields.

        /// <summary>
        /// The dimensions of the struct.
        /// </summary>
        float x, y, z;

        /// <summary>
        /// Optional max and min values.
        /// </summary>
        float max, min;

        #endregion

        #region Properties.

        /// <summary>
        /// Return values as a vector.
        /// </summary>
        public Vector3 Vector
        {
            get { return new Vector3(x, y, z); }
            set
            {
                this.X = value.x;
                this.Y = value.y;
                this.Z = value.z;
            }
        }

        /// <summary>
        /// Set the x value.
        /// </summary>
        public float X
        {
            get { return this.x; }
            set { this.x = Services.Clamp(value, min, max); }
        }

        /// <summary>
        /// Set the y value.
        /// </summary>
        public float Y
        {
            get { return this.y; }
            set { this.y = Services.Clamp(value, min, max); }
        }

        /// <summary>
        /// Set the z value.
        /// </summary>
        public float Z
        {
            get { return this.z; }
            set { this.z = Services.Clamp(value, min, max); }
        }

        /// <summary>
        /// Return the max speed.
        /// </summary>
        public float MaxSpeed
        {
            get { return this.max; }
            set { this.max = Services.MaxOf(x, y, z, max, min, value); }
        }

        /// <summary>
        /// Return the min speed.
        /// </summary>
        public float MinSpeed
        {
            get { return this.min; }
            set { this.min = Services.MinOf(x, y, z, max, min, value); }
        }

        #endregion

        #endregion

        #region Constructors.

        /// <summary>
        /// Create the camera speed object.
        /// </summary>
        /// <param name="_value">Speed for all three axes./param>
        public Velocity(float _value) : this(_value, _value, _value)
        {
            this.max = _value;
            this.min = _value;
        }

        /// <summary>
        /// Create the camera speed object.
        /// </summary>
        /// <param name="_x">X axis.</param>
        /// <param name="_y">Y axis.</param>
        /// <param name="_z">Z axis.</param>
        public Velocity(float _x, float _y, float _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.max = Services.MaxOf(x, y, z);
            this.min = Services.MinOf(x, y, z);
        }

        /// <summary>
        /// Create the camera speed object.
        /// </summary>
        /// <param name="_x">X axis.</param>
        /// <param name="_y">Y axis.</param>
        /// <param name="_z">Z axis.</param>
        /// <param name="_max">Maximum speed.</param>
        /// <param name="_min">Minimum speed. Can be zero).</param>
        public Velocity(float _x, float _y, float _z, float _min, float _max)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.max = Services.MaxOf(x, y, z, _max, _min);
            this.min = Services.MinOf(x, y, z, _max, _min);
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Print this struct as a string.
        /// </summary>
        /// <returns>Value of the ToString() method.</returns>
        public override string ToString()
        {
            string result = "";

            result = "Velocity: (" + this.x + ", " + this.y + ", " + this.z + ") on inclusive range [" + this.min + ", " + this.max + "].";

            return result;
        }


        /// <summary>
        /// Scale the object.
        /// </summary>
        /// <param name="_value">Value to scale by.</param>
        public void Scale(float _value)
        {
            if (_value == 0)
            {
                this.X = 0.0f;
                this.Y = 0.0f;
                this.Z = 0.0f;
            }

            // Scale the vector three object and set it back.
            this.Vector = (this.Vector * _value);
        }

        /// <summary>
        /// Scale the x axis.
        /// </summary>
        /// <param name="_x">Value to scale x by.</param>
        public void ScaleX(float _x)
        {
            if (_x == 0) { this.X = 0.0f; }
            this.X = this.X * _x;
        }

        /// <summary>
        /// Scale the y axis.
        /// </summary>
        /// <param name="_y">Value to scale y by.</param>
        public void ScaleY(float _y)
        {
            if (_y == 0) { this.Y = 0.0f; }
            this.Y = this.Y * _y;
        }

        /// <summary>
        /// Scale the z axis.
        /// </summary>
        /// <param name="_z">Value to scale z by.</param>
        public void ScaleZ(float _z)
        {
            if (_z == 0) { this.Z = 0.0f; }
            this.Z = this.Z * _z;
        }

        /// <summary>
        /// Divide the object.
        /// </summary>
        /// <param name="_value">Value to divide by.</param>
        public void Divide(float _value)
        {
            if (_value == 0)
            {
                this.X = 0.0f;
                this.Y = 0.0f;
                this.Z = 0.0f;
            }

            // Scale the vector three object and set it back.
            Scale(1 / _value);
        }

        /// <summary>
        /// Divide the x axis.
        /// </summary>
        /// <param name="_x">Value to scale x by.</param>
        public void DivideX(float _x)
        {
            Scale(1 / _x);
        }

        /// <summary>
        /// Divide the y axis.
        /// </summary>
        /// <param name="_y">Value to scale y by.</param>
        public void DivideY(float _y)
        {
            Scale(1 / _y);
        }

        /// <summary>
        /// Divide the z axis.
        /// </summary>
        /// <param name="_z">Value to scale z by.</param>
        public void DivideZ(float _z)
        {
            Scale(1 / _z);
        }

        #endregion

    }

    #endregion


}
