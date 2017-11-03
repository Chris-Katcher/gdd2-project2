using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana.Utilities;
using Arcana.Entities.Attributes;
using UnityEngine;

namespace Arcana.Environment
{
    public class Platform : Environment
    {        

        #region Data Members

        #region Fields.

        /// <summary>
        /// Box collider given to the platform.
        /// </summary>
        private BoxCollider m_collider = null;
        
        #endregion

        #region Properties.
        
        /// <summary>
        /// Collider reference.
        /// </summary>
        public BoxCollider Collider
        {
            get
            {
                if(this.m_collider == null)
                {
                    this.m_collider = this.InitializeCollider();
                }
                return this.m_collider;
            }
        }

        #endregion

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialization method.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                base.Initialize();

                // Initialize with platform type.
                this.AddEnvironmentType(EnvironmentType.Platform);

                // Set the default dimensions.
                this.SetScale(new Vector3(3.0f, 0.25f, 1.0f));

                // Set the name.
                this.Name = "Platform";

                // Set up the collider.
                m_collider = this.Collider;
            }
        }

        /// <summary>
        /// Initialize collider.
        /// </summary>
        /// <returns>Reference to initialized collider.</returns>
        public BoxCollider InitializeCollider()
        {
            BoxCollider collider = this.Self.GetComponent<BoxCollider>();

            if (collider == null)
            {
                collider = this.Self.AddComponent<BoxCollider>();
            }

            return collider;
        }
        
        #endregion
        
    }
}
