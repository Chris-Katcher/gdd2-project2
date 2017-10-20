/************************************************
 * Entity.cs
 * 
 * This file contains implementation for the Entity class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System.Collections;
using System.Collections.Generic;
using Arcana;
using UnityEngine;
using Arcana.Entities.Attributes;

namespace Arcana.Entities
{

    /////////////////////
    // EntityFactory Declaration.
    /////////////////////

    public static class EntityFactory {
            // TODO
    }


    /////////////////////
    // Entity class.
    /////////////////////

    /// <summary>
    /// Entity helps utilize the <see cref="GameObject"/> in ways that align with our program.
    /// </summary>
    public class Entity : MonoBehaviour
    {

        #region Data Members

        /////////////////////
        // Data members.
        /////////////////////
        
        /// <summary>
        /// Entity dimension manager. (Usually for calculations).
        /// </summary>
        private Dimension m_dimensions;

        /// <summary>
        /// Entity's health tracker.
        /// </summary>
        private HealthTracker m_health;
        
        /////////////////////
        // Properties.
        /////////////////////
        
        /// <summary>
        /// Reference to Entity width.
        /// </summary>
        public float Width
        {
            get { return m_dimensions.Width; }
        }
        
        /// <summary>
        /// Reference to Entity width.
        /// </summary>
        public float Height
        {
            get { return m_dimensions.Height; }
        }

        /// <summary>
        /// Reference to Entity depth level.
        /// </summary>
        public float Depth
        {
            get { return m_dimensions.Depth; }
        }

        /// <summary>
        /// Property tracking whether or not the entity is alive.
        /// </summary>
        public bool IsAlive
        {
            get;
            set;
        }

        #endregion

        #region Service Methods

        /// <summary>
        /// Initilizes the Entity.
        /// </summary>
        void Start()
        {
            this.Initialize();
        }

        /// <summary>
        /// Update UnityEngine components.
        /// </summary>
        void Update()
        {
            // Call update entity for inherited classes.
            UpdateEntity();

            if (this.IsAlive)
            {
                // Call update life for inherited classes.
                UpdateLife();
            }
            else
            {
                // Call on death for inherited classes.
                OnDeath();
            }
        }

        protected virtual void UpdateEntity()
        {
            if (m_health.IsVulnerable())
            {
                this.IsAlive = m_health.IsAlive();
            }
        }

        protected virtual void UpdateLife()
        {
            // Stub.
        }

        protected virtual void OnDeath()
        {
            // Stub.
        }

        protected virtual void Initialize()
        {
            // Create objects.
            this.m_dimensions = new Dimension(Constants.DEFAULT_DIMENSION, 0.0f);

            // Get components.
            this.m_health = HealthTrackerFactory.CreateComponent(gameObject);
            
        }

        #endregion

        #region Mutator Methods

        /// <summary>
        /// Set the vertical dimension of the Entity.
        /// </summary>
        /// <param name="height">Height of the Entity.</param>
        public void SetHeight(float height)
        {
            this.m_dimensions.SetHeight(Services.Max(height, 0.1f));
        }

        /// <summary>
        /// Set the horizontal dimension of the Entity.
        /// </summary>
        /// <param name="width">Width of the Entity.</param>
        public void SetWidth(float width)
        {
            this.m_dimensions.SetWidth(Services.Max(width, 0.1f));
        }

        /// <summary>
        /// Set the depth level of the Entity.
        /// </summary>
        /// <param name="depth">Depth level of the Entity.</param>
        public void SetDepth(int depth)
        {
            this.m_dimensions.SetDepth(Services.Max(depth, 0.1f));
        }

        #endregion

    }

}
