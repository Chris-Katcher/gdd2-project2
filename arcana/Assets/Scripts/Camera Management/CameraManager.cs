/************************************************
 * CameraManager.cs
 * 
 * This file contains implementation for the CameraManager class,
 * and the CameraManagerFactory class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityCamera = UnityEngine.Camera; // To prevent overwriting our own namespace.

namespace Arcana.Cameras
{

    #region // Class: CameraManagerFactory class.

    /////////////////////
    // Factory class.
    /////////////////////

    /// <summary>
    /// Handles creation of the CameraManager class and singleton.
    /// </summary>
    public class CameraManagerFactory : IFactory<CameraManager>
    {

        #region // Static members.

        /// <summary>
        /// Single instance of the factory.
        /// </summary>
        private static CameraManagerFactory instance = null;

        /// <summary>
        /// Single instance of the CameraManager class.
        /// </summary>
        private static CameraManager manager = null;

        /// <summary>
        /// Get reference to CameraManagerFactory.
        /// </summary>
        /// <returns>Returns a single factory.</returns>
        public static CameraManagerFactory Instance()
        {
            if (instance == null)
            {
                instance = new CameraManagerFactory();
            }

            return instance;
        }

        /// <summary>
        /// Get reference to CameraManager.
        /// </summary>
        /// <returns>Returns a single manager.</returns>
        public static CameraManager GetManagerInstance()
        {
            return manager;
        }

        /// <summary>
        /// On creation, set this to be the instance.
        /// </summary>
        public CameraManagerFactory()
        {
            instance = this;
        }

        #endregion
        
        #region // Factory methods.

        /// <summary>
        /// Return a single CameraManagerFactory reference.
        /// </summary>
        /// <returns>Returns a single factory.</returns>
        public IFactory<CameraManager> GetInstance()
        {
            return Instance();
        }

        /// <summary>
        /// Creates a new, empty game object, and returns the CameraManager component back.
        /// </summary>
        /// <returns></returns>
        public CameraManager CreateComponent()
        {
            if (!HasManagerInstance())
            {
                // Creates a component using the default settings.
                Debugger.Print("Create CameraManager on an empty game object, with the default settings.");
                manager = CreateComponent(Services.CreateEmptyObject("Camera Manager"), CreateSettings());
            }

            return manager;
        }

        /// <summary>
        /// Adds a new component to the parent game object, with parameters.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <param name="parameters">Settings to apply to the new Entity.</param>
        /// <returns>Return newly created component.</returns>
        public CameraManager CreateComponent(GameObject parent, Constraints parameters)
        {
            // Check if there is already an instance of the CameraManager component.
            if (!HasManagerInstance())
            {
                // Check game object.
                if (parent == null)
                {
                    // If the parent itself is null, do not return a component.
                    Debugger.Print("Tried to add a component but parent GameObject is null.", "NULL_REFERENCE");
                    return null;
                }

                // Get reference to existing script if it already exists on this parent.
                manager = parent.GetComponent<CameraManager>();

                // If the manager is null.
                if (manager == null)
                {
                    // If the manager instance is null, then create the component.
                    Debugger.Print("Create and add the CameraManager component.");
                    manager = parent.AddComponent<CameraManager>();
                }

                // Assign non-optional information.
                manager.Initialize();

                // Initialize the entity.
                foreach (string key in parameters.ValidEntries)
                {
                    Debugger.Print("Initialize the CameraManager component.");
                    manager.Initialize(key, parameters.GetEntry(key).Value);
                }
            }

            return manager;
        }

        /// <summary>
        /// Adds a new, default component, to the parent game object.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <returns>Return newly created component.</returns>
        public CameraManager CreateComponent(GameObject parent)
        {
            if (!HasManagerInstance())
            {
                // Creates a component using the default settings.
                manager = CreateComponent(parent, CreateSettings());
            }

            return manager;
        }

        /// <summary>
        /// Set up the parameters associated with this factory's IFactoryElement.
        /// </summary>
        /// <returns>Returns the <see cref="Constraints"/> collection object.</returns>
        public Constraints CreateSettings(
            Color? _color = null,
            float _shakeDecayFactor = Constants.DEFAULT_DECAY_FACTOR,
            float _shakeCycle = 0.0f,
            float _shakePeriod = Constants.DEFAULT_SHAKE_PERIOD,
            float _shakeStrength = Constants.DEFAULT_SHAKE_STRENGTH)
        {
            // Create the collection.
            Debugger.Print("Building parameters for CameraSettings Component constructor.");
            Constraints parameters = new Constraints();


            // Add nullable types.
            Debugger.Print("Adding color.");
            parameters.AddValue<Color?>(Constants.PARAM_BACKGROUND, _color);

            // Add non-nulllable types.
            Debugger.Print("Adding decay trackers.");
            parameters.AddValue<DecayTracker<float>>(Constants.PARAM_CAMERA_SHAKE, new DecayTracker<float>(0.0f, _shakePeriod, _shakeDecayFactor, _shakeCycle)); // Shake time decay.
            parameters.AddValue<DecayTracker<float>>(Constants.PARAM_CAMERA_STRENGTH, new DecayTracker<float>(0.005f, _shakeStrength, _shakeDecayFactor, _shakeCycle)); // Shake strength decay.
            
            return parameters;
        }

        #endregion

        #region // Service Methods.

        /// <summary>
        /// Checks if there is an instance of the CameraManager already.
        /// </summary>
        /// <returns>Returns true if instance is not null.</returns>
        public static bool HasManagerInstance()
        {
            return (manager != null);
        }

        /// <summary>
        /// Delete the instance of the CameraManager.
        /// </summary>
        public static void Release()
        {
            if (HasManagerInstance())
            {
                UnityEngine.Object.Destroy(manager);
            }
        }

        #endregion

    }

    #endregion

    #region // Class: CameraManager class.

    /////////////////////
    // Blueprint class.
    /////////////////////

    /// <summary>
    /// Handles all functionality related to the camera users see from.
    /// </summary>
    public class CameraManager : MonoBehaviour, IFactoryElement
    {

        #region Static Members

        /////////////////////
        // Static members.
        /////////////////////

        /// <summary>
        /// Returns the instance of the manager.
        /// </summary>
        public static CameraManager Instance
        {
            get { return CameraManagerFactory.GetManagerInstance(); }
        }

        /// <summary>
        /// Return true if instance exists.
        /// </summary>
        /// <returns>Returns a boolean value.</returns>
        public static bool HasInstance()
        {
            return (CameraManager.Instance != null);
        }

        #endregion

        #region Data Members
        
        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Reference to the GameObject containing the camera.
        /// </summary>
        private GameObject m_camera_object;

        /// <summary>
        /// CameraSettings reference.
        /// </summary>
        private CameraSettings m_camera;

        /// <summary>
        /// Position of the camera; stored for when camera shakes occur.
        /// </summary>
        private Vector2 m_stopPosition;
    
        /// <summary>
        /// Determines length of time of in which camera shaking should occur, time of active shaking.
        /// </summary>
        private DecayTracker<float> m_cameraShake;

        /// <summary>
        /// Tracks camera shaking strength.
        /// </summary>
        private DecayTracker<float> m_cameraShakeStrength;
        
        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns the time currently left on the shake timer.
        /// </summary>
        public float ShakeTimeLeft
        {
            get { return this.m_cameraShake.CurrentValue; }
        }

        /// <summary>
        /// Returns the current strength of the shake, that should get weaker as we go on.
        /// </summary>
        public float ShakeStrength
        {
            get { return this.m_cameraShakeStrength.CurrentValue; }
        }

        /// <summary>
        /// Determines if the camera is currently shaking.
        /// </summary>
        public bool IsShaking
        {
            get { return (ShakeTimeLeft > 0.0f); }
        }
        
        /// <summary>
        /// Check if CameraManager has a reference to its camera object.
        /// </summary>
        public bool HasCameraObject
        {
            get { return (this.m_camera != null); }
        }

        /// <summary>
        /// Determine if there is a camera to begin with.
        /// </summary>
        public bool HasCamera
        {
            get { return (this.HasCameraObject && (this.m_camera.gameObject.GetComponent<UnityEngine.Camera>() != null)); }
        }

        /// <summary>
        /// Return a reference to the UnityEngine.Camera component.
        /// </summary>
        public UnityCamera Viewport
        {
            get { return this.m_camera.gameObject.GetComponent<UnityEngine.Camera>(); }
        }

        #endregion

        #region Service Methods

        /////////////////////
        // Service methods.
        /////////////////////

        /// <summary>
        /// Initialize the CameraManager.
        /// </summary>
        /// <param name="_camera">Optional object containing the camera.</param>
        internal void Initialize(GameObject _camera = null,
            GameObject _unityCamera = null,
            Color? _color = null,
            float _shakeDecayFactor = Constants.DEFAULT_DECAY_FACTOR,
            float _shakeCycle = 0.0f,
            float _shakePeriod = Constants.DEFAULT_SHAKE_PERIOD,
            float _shakeStrength = Constants.DEFAULT_SHAKE_STRENGTH)
        {
            InitializeCameraObject(_camera);
            InitializeCamera(_unityCamera); // Even if there is no child, the function will properly handle this.
            InitializeProperties(_color, _shakeDecayFactor, _shakeCycle, _shakePeriod, _shakeStrength);
        }

        /// <summary>
        /// Initialize the properties and data members of the manager.
        /// </summary>
        private void InitializeProperties(
            Color? _color = null,
            float _shakeDecayFactor = Constants.DEFAULT_DECAY_FACTOR,
            float _shakeCycle = 0.0f,
            float _shakePeriod = Constants.DEFAULT_SHAKE_PERIOD,
            float _shakeStrength = Constants.DEFAULT_SHAKE_STRENGTH)
        {
            Debugger.Print("Initializing camera manager properties.");

            // Set the stopped position.
            this.m_stopPosition = Services.ToVector2(this.m_camera_object.transform.position);

            // Set the defaults.
            this.m_cameraShake = new DecayTracker<float>(0.0f, _shakePeriod, _shakeDecayFactor, _shakeCycle);
            this.m_cameraShakeStrength = new DecayTracker<float>(0.005f, _shakeStrength, _shakeDecayFactor, _shakeCycle);

            // Change the color.
            if (_color.HasValue) { ChangeBackground(_color.Value); } else { ChangeBackground(Color.black); }
        }
        
        /// <summary>
        /// Set the value of a property.
        /// </summary>
        /// <param name="parameter">Switch trigger that determines which property is set.</param>
        public virtual void Initialize(string parameter, object value)
        {
            switch (parameter)
            {
                case Constants.PARAM_BACKGROUND:
                    Debugger.Print("Setting background by parameter: " + ((Color)value));
                    ChangeBackground((Color)value);
                    break;
                case Constants.PARAM_CAMERA_SHAKE:
                    Debugger.Print("Setting camera shake delay tracker by parameter: " + ((DecayTracker<float>)value));
                    this.m_cameraShake = ((DecayTracker<float>)value);
                    break;
                case Constants.PARAM_CAMERA_STRENGTH:
                    Debugger.Print("Setting camera strength delay tracker by parameter: " + ((DecayTracker<float>)value));
                    this.m_cameraShakeStrength = ((DecayTracker<float>)value);
                    break;
            }
        }

        /// <summary>
        /// Initializes the Camera's parent.
        /// </summary>
        private void InitializeCameraObject(GameObject _cameraObject = null)
        {
            Debugger.Print("Initializing Camera Object.");

            // If there is no camera and parameter isn't null.
            if (!HasCameraObject && _cameraObject != null)
            {
                this.m_camera_object = Services.AddChild(gameObject, _cameraObject);
            }
            else
            {
                // Create a new, empty game object, and parent it to this instance of the CameraManager's gameObject.
                this.m_camera_object = Services.AddChild(gameObject, Services.CreateEmptyObject("Camera Object"));
            }

            this.m_camera_object.name = "Camera Object";
        }

        /// <summary>
        /// Initializes the Camera object's child Camera.
        /// </summary>
        private void InitializeCamera(GameObject _camera = null)
        {
            Debugger.Print("Initializing Camera object and UnityEngine.Camera component.");

            // 1. Get the Camera's GameObject.
            GameObject temp_object = _camera;

            if (temp_object == null)
            {
                // If the Camera's GameObject is null and a main camera still doesn't exist.
                // 1b. Create a new one.
                temp_object = Services.CreateEmptyObject("Camera");
            }
            
            // 2. Make it a child of the camera object data member.
            temp_object = Services.AddChild(this.m_camera_object, temp_object);

            // 3. Get the Camera component.
            UnityCamera temp_component = temp_object.GetComponent<UnityEngine.Camera>();

            if (temp_component == null)
            {
                // If the component is null:
                // 3b. create a new one.          
                temp_object.AddComponent<UnityEngine.Camera>();
            }

            // 4. Get the CameraSettings component.
            this.m_camera = temp_object.GetComponent<CameraSettings>();

            if (this.m_camera == null)
            {
                // If the component is null:
                // 3b. create a new one.          
                this.m_camera = CameraFactory.Instance().CreateComponent(temp_object);
            }
            
            // Since we know our references are hooked properly, we can change the name to reflect this.
            this.m_camera.gameObject.name = "Camera";
        }

        /// <summary>
        /// Update in the UnityEngine loop.
        /// </summary>
        public void Update()
        {
            // If the camera is sahking.
            if (IsShaking)
            {
                // Shake the camera if need be.
                ShakeCamera(Time.deltaTime);

                // Update decay trackers.
                this.m_cameraShake.Update(Time.deltaTime);
                this.m_cameraShakeStrength.Update(Time.deltaTime);
            }
            else
            {
                // Stop shaking if it's shaking.
                this.StopShaking();
            }
        }

        /// <summary>
        /// Move the camera object.
        /// </summary>
        /// <param name="position">Place to move camera object to.</param>
        public void MoveCameraObject(Vector2 position)
        {
            this.m_camera_object.transform.position = Services.ToVector3(position, this.m_camera_object.transform.position.z);
            this.m_stopPosition = position;
        }

        /// <summary>
        /// This will offset the shake camera based on the camera shaking settings.
        /// </summary>
        private void ShakeCamera(float deltaTime)
        {
            if (IsShaking)
            {
                Debugger.Print("Shaking camera...");
                if (!Services.IsEmpty((Vector2?) this.m_stopPosition))
                {
                    // Only shake if the decay isn't paused.
                    if (!this.m_cameraShake.IsPaused)
                    {
                        float minX = this.m_stopPosition.x - this.m_cameraShake.CurrentValue;
                        float maxX = this.m_stopPosition.x + this.m_cameraShake.CurrentValue;
                        float minY = this.m_stopPosition.y - this.m_cameraShake.CurrentValue;
                        float maxY = this.m_stopPosition.y + this.m_cameraShake.CurrentValue;

                        float x = Services.NextFloat(minX, maxX) * this.m_cameraShakeStrength.CurrentValue;
                        float y = Services.NextFloat(minY, maxY) * this.m_cameraShakeStrength.CurrentValue;

                        this.m_camera_object.transform.position = Services.ToVector3(Services.ToVector2(x, y), this.m_camera_object.transform.position.z);
                    }
                }
            }
        }

        /// <summary>
        /// Resets Camera position after a shake.
        /// </summary>
        public void StopShaking()
        {
            if (IsShaking)
            {
                Debugger.Print("Stopping camera...");
                if (!Services.IsEmpty((Vector2?)this.m_stopPosition))
                {
                    this.m_camera_object.transform.position = this.m_stopPosition;
                }

                StopCameraShake();
            }
        }

        /// <summary>
        /// Trigger the camera shake functionality.
        /// </summary>
        public void TriggerCameraShake()
        {
            // Reset the strength and shaker.
            this.m_cameraShake.Start(); // Set to maximum and start count down.
            this.m_cameraShakeStrength.Start(); // Set to maximum and start count down.
        }

        /// <summary>
        /// Stop the camera shake and reset it to zero.
        /// </summary>
        private void StopCameraShake()
        {
            // Stop the shaking.
            this.m_cameraShake.Reset();
            this.m_cameraShakeStrength.Reset();
        }

        /// <summary>
        /// Pause the camera shaking.
        /// </summary>
        public void PauseCameraShake()
        {
            // Pause the camera shake.
            this.m_cameraShake.Pause();
            this.m_cameraShakeStrength.Pause();
        }

        /// <summary>
        /// Resume the camera shaking.
        /// </summary>
        public void ResumeCameraShake()
        {
            this.m_cameraShake.Resume();
            this.m_cameraShakeStrength.Resume();
        }

        /// <summary>
        /// Change the color of the camera.
        /// </summary>
        /// <param name="_color">Color to set the background of the viewport to.</param>
        public void ChangeBackground(Color _color)
        {
            Viewport.backgroundColor = _color;
        }

        #endregion        
    }

    #endregion

}
