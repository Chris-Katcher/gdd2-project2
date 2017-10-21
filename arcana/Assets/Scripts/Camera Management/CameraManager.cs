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

    #region // CameraManagerFactory class.

    /////////////////////
    // Factory class.
    /////////////////////

    /// <summary>
    /// Handles creation of the CameraManager class and singleton.
    /// </summary>
    public static class CameraManagerFactory
    {

        #region // Static members.

        /// <summary>
        /// Single instance of the CameraManager class.
        /// </summary>
        internal static CameraManager instance = null;

        #endregion

        #region // Static Constructor

        /// <summary>
        /// Creates a CameraManager component if it doesn't already exist.
        /// </summary>
        /// <returns>Returns instance of the camera manager.</returns>
        public static CameraManager CreateComponent(GameObject parent, GameObject _camera = null,
            float _shakeDecayFactor = Constants.DEFAULT_DECAY_FACTOR,
            float _shakeCycle = 0.0f,
            float _shakePeriod = Constants.DEFAULT_SHAKE_PERIOD,
            float _shakeStrength = Constants.DEFAULT_SHAKE_STRENGTH)
        {
            // Check if there is already an instance of the CameraManager component.
            if (!HasInstance())
            {
                // If there is no instance, create a new component and return it.
                instance = parent.AddComponent<CameraManager>();
                parent.name = "Camera Manager";

                // Initialize the CameraManager.
                instance.Initialize(_camera, _shakeDecayFactor, _shakeCycle, _shakePeriod, _shakeStrength);
            }
            
            // Return the created instance.
            return instance;
        }

        #endregion

        #region // Service Methods.

        /// <summary>
        /// Checks if there is an instance of the CameraManager already.
        /// </summary>
        /// <returns>Returns true if instance is not null.</returns>
        public static bool HasInstance()
        {
            return (instance != null);
        }

        /// <summary>
        /// Delete the instance of the CameraManager.
        /// </summary>
        public static void Release()
        {
            if (HasInstance())
            {
                UnityEngine.Object.Destroy(instance);
            }
        }

        #endregion

    }

    #endregion

    #region // CameraManager class.

    /////////////////////
    // Blueprint class.
    /////////////////////

    /// <summary>
    /// Handles all functionality related to the camera users see from.
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        #region Data Members

        /////////////////////
        // Static members.
        /////////////////////

        /// <summary>
        /// Returns the instance of the manager.
        /// </summary>
        public static CameraManager Instance {
            get { return CameraManagerFactory.instance; }
        }

        /////////////////////
        // Data members.
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
        /// Check if CameraManager has an instance.
        /// </summary>
        public bool HasInstance
        {
            get { return (CameraManagerFactory.HasInstance()); }
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
            get { return (this.HasCameraObject && (this.m_camera.GetComponentInChildren<CameraSettings>() != null)); }
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
            float _shakeDecayFactor = Constants.DEFAULT_DECAY_FACTOR,
            float _shakeCycle = 0.0f,
            float _shakePeriod = Constants.DEFAULT_SHAKE_PERIOD,
            float _shakeStrength = Constants.DEFAULT_SHAKE_STRENGTH)
        {
            if (!HasInstance)
            {
                InitializeCameraObject(_camera);
                InitializeCamera(_camera.GetComponentInChildren<CameraSettings>().gameObject); // Even if there is no child, the function will properly handle this.
                InitializeProperties(_shakeDecayFactor, _shakeCycle, _shakePeriod, _shakeStrength);
            }
        }

        /// <summary>
        /// Initialize the properties and data members of the manager.
        /// </summary>
        private void InitializeProperties(float _shakeDecayFactor = Constants.DEFAULT_DECAY_FACTOR,
            float _shakeCycle = 0.0f,
            float _shakePeriod = Constants.DEFAULT_SHAKE_PERIOD,
            float _shakeStrength = Constants.DEFAULT_SHAKE_STRENGTH)
        {
            // Set the stopped position.
            this.m_stopPosition = Services.ToVector2(this.m_camera_object.transform.position);

            // Set the defaults.
            this.m_cameraShake = new DecayTracker<float>(0.0f, _shakePeriod, _shakeDecayFactor, _shakeCycle);
            this.m_cameraShakeStrength = new DecayTracker<float>(0.005f, _shakeStrength, _shakeDecayFactor, _shakeCycle);

        }

        /// <summary>
        /// Initializes the Camera's parent.
        /// </summary>
        private void InitializeCameraObject(GameObject _cameraObject = null)
        {
            // If there is no camera and parameter isn't null.
            if (!HasCameraObject && _cameraObject != null)
            {
                this.m_camera_object = Services.AddParent(_cameraObject, gameObject);
            }
            else
            {
                // Create a new, empty game object, and parent it to this instance of the CameraManager's gameObject.
                this.m_camera_object = Services.AddParent(Services.CreateEmptyObject("Camera Object"), gameObject);
            }

            this.m_camera_object.name = "Camera Object";
        }

        /// <summary>
        /// Initializes the Camera object's child Camera.
        /// </summary>
        private void InitializeCamera(GameObject _camera = null)
        {
            if (!HasCamera && _camera != null)
            {
                this.m_camera = _camera.GetComponent<CameraSettings>();
                Services.AddParent(_camera, this.m_camera_object);
            }
            else
            {
                // This next line is a bit bulky but:
                 // 1. Create an empty game object to store the new camera component.
                 // 2. Add this new game object as a child to our wrapping camera object item.
                 // 3. Pass the return value from AddChild, as the parent parameter for CameraFactory.CreateComponent, netting us our Camera component. 
                this.m_camera = CameraFactory.GetInstance().CreateComponent(Services.AddChild(this.m_camera_object, Services.CreateEmptyObject("Camera")));
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

        #endregion        
    }

    #endregion

}
