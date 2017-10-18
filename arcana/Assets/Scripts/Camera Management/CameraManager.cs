/************************************************
 * CameraManager.cs
 * 
 * This file contains implementation for the CameraManager class.
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

namespace Arcana.Camera
{

    /// <summary>
    /// Handles all functionality related to the camera users see from.
    /// </summary>
    public class CameraManager
    {
        #region Data Members

        /////////////////////
        // Data members.
        /////////////////////

        /// <summary>
        /// Collection of camera objects maintained by the Camera Manager.
        /// </summary>
        private List<UnityCamera> m_cameras;

        /// <summary>
        /// Current index of <see cref="m_cameras"/> referencing the current Camera object.
        /// </summary>
        private int m_currentCameraIndex;
        
        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Number of cameras in <see cref="m_cameras"/>
        /// </summary>
        public int Count
        {
            get {
                if (HasCameras)
                {
                    return m_cameras.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Determines if the manager has any cameras.
        /// </summary>
        public bool HasCameras
        {
            get {
                if (m_cameras != null && m_cameras.Count != 0)
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Constructor

        /////////////////////
        // Constructor methods.
        /////////////////////

        /// <summary>
        /// Empty constructor for the Camera Manager.
        /// </summary>
        public CameraManager() {

            // Initialize the camera.
            Initialize();
            
        }
        public CameraManager(GameObject go) { }





        #endregion

        #region Service Methods

        /////////////////////
        // Service methods.
        /////////////////////

        /// <summary>
        /// Initializes the data members for the camera manager.
        /// </summary>
        public void Initialize()
        {
            m_currentCameraIndex = -1;
            m_cameras = new List<UnityCamera>();
        }

        /// <summary>
        /// Add camera to the list of cameras.
        /// </summary>
        /// <param name="camera"></param>
        public void AddCamera(UnityCamera camera, bool select = false)
        {


        }

       

        #endregion

        #region Accessor Methods

        /////////////////////
        // Accessor methods.
        /////////////////////

        #endregion

        #region Mutator Methods

        /////////////////////
        // Mutator methods.
        /////////////////////

        #endregion

    }
}
