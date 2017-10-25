/************************************************
 * CameraTarget.cs
 * 
 * This file contains:
 * - The CameraTarget class. (Child of ArcanaObject).
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

    #region Class: CameraTarget class.

    /// <summary>
    /// The camera target is an ArcanaObject that a camera can track.
    /// </summary>
    [AddComponentMenu("Arcana/Cameras/CameraTarget")]
    public class CameraTarget : ArcanaObject
    {

        #region Static Methods.

        #region Component Factory Methods.

        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static CameraTarget Create(ArcanaObject _parent)
        {
            return ComponentFactory.Create<CameraTarget>(_parent);
        }

        /// <summary>
        /// Clone a component and set it equal to another.
        /// </summary>
        /// <param name="_parent">Parent to add clone to.</param>
        /// <param name="_template">Component to clone.</param>
        /// <returns>Returns a new component that has been cloned.</returns>
        public static CameraTarget Clone(ArcanaObject _parent, CameraTarget _template)
        {
            return (CameraTarget)(CameraTarget.Create(_parent)).Clone(_template);
        }

        #endregion

        #endregion

        #region Data Members

        #region Fields.

        /// <summary>
        /// Camera tracking flag. If true, a camera can choose to track this object.
        /// </summary>
        private bool m_trackable = true;

        /// <summary>
        /// Represents the location the Camera should be tracking.
        /// </summary>
        private Vector3 m_trackPositionOffset = Vector3.zero;

        /// <summary>
        /// Size of the tracking target.
        /// </summary>
        private float m_radius = 1.0f;

        #endregion

        #region Properties.

        /// <summary>
        /// Reference to the position offset to track.
        /// </summary>
        private Vector3 LocationOffset
        {
            get { return this.m_trackPositionOffset; }
            set { this.m_trackPositionOffset = value; }
        }

        /// <summary>
        /// Reference to the position to track.
        /// </summary>
        public Vector3 Location
        {
            get { return this.gameObject.transform.position + this.LocationOffset; }
        }

        /// <summary>
        /// Reference to the target's radius.
        /// </summary>
        public float Radius
        {
            get { return this.m_radius; }
        }

        #endregion

        #endregion
        
        #region Initialization Method.

        /// <summary>
        /// Initialize the component.
        /// </summary>
        public override void Initialize()
        {
            if (!Initialized)
            {

                // Set the default radius.
                this.m_radius = 0.0f;

                // Set the tracking flag.
                this.m_trackable = true;

                // Create the status, set the name, and children collection.
                base.Initialize();

                // Append the camera target to the name.
                this.Name = this.Name + " (Camera Target)";
            }
        }

        /// <summary>
        /// Copy data from the input component into this one.
        /// </summary>
        /// <param name="_template">Template to pull data from.</param>
        public override ArcanaObject Clone(ArcanaObject _template)
        {
            // Check type.
            if (Services.IsSameOrSubclassOf(_template.GetType(), this.GetType()))
            {
                // Ensure they are different.
                if (this != _template)
                {
                    base.Clone(_template);
                    this.m_trackable = ((CameraTarget)_template).m_trackable;
                    this.m_radius = ((CameraTarget)_template).m_radius;
                    this.m_trackPositionOffset = ((CameraTarget)_template).m_trackPositionOffset;
                }
            }

            return this;
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Get's a message declaring the state of the Status component in human readable form.
        /// </summary>
        /// <returns>Returns a report of all active states.</returns>
        public override string ToString()
        {
            // Default message.
            string report = "CameraTarget \"" + gameObject.name + "\" is not initialized.";

            if (this.Status.IsInitialized())
            {
                // Return all currently active status names as comma separated list.
                report = "CameraTarget \"" + gameObject.name + "\"[Radius: " + this.m_radius + ", Position:(" + this.Location.ToString() + ")]";
            }

            // Return the report.
            return report;
        }

        /// <summary>
        /// Calculate the distance between the opposite edges of two camera targets.
        /// </summary>
        /// <param name="_other">Target to compare.</param>
        /// <returns>Return distance.</returns>
        public float CalculateDistance(CameraTarget _other)
        {
            Vector3 otherPosition = _other.Location;

            Vector3 a = this.Location;
            Vector3 b = otherPosition;

            // If this target is to the left of the other target.
            if (this.Location.x < otherPosition.x)
            {
                a += new Vector3(-this.Radius, 0, 0);
                b += new Vector3(this.Radius, 0, 0);
            }
            else
            {
                a += new Vector3(this.Radius, 0, 0);
                b += new Vector3(-this.Radius, 0, 0);
            }

            // If this target is under the other target.
            if (this.Location.y < otherPosition.y)
            {
                a += new Vector3(0, -this.Radius, 0);
                b += new Vector3(0, this.Radius, 0);
            }
            else
            {
                a += new Vector3(0, this.Radius, 0);
                b += new Vector3(0, -this.Radius, 0);
            }

            // Find the distance between these two vectors.
            return Vector3.Distance(a, b);
        }

        /// <summary>
        /// Update the target flag based on the status.
        /// </summary>
        public bool IsTargetable()
        {
            bool response = this.m_trackable;

            // If tracking is allowed, check status.
            if (response)
            {

                /*
                    Tracking should be allowed if, and only if:
                    - IsInitialized.
                    - Not Inactive.
                    - Not IsPaused.
                    - Not IsDestroy.
                */

                response = (this.Status.IsInitialized())
                    && (!this.Status.IsPaused())
                    && (!this.Status.IsInactive())
                    && (!this.Status.IsDestroy());

            }

            return response;
        }

        #endregion

    }

    #endregion

}
