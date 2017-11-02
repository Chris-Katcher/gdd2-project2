using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Arcana.Entities.Attributes
{

    #region Class: EntityCollider class.

    /// <summary>
    /// Generates a BoxCollider2D for an Entity.
    /// </summary>
    public class EntityCollider : ArcanaObject
    {

        #region Data Members

        #region Fields.
        
        /// <summary>
        /// Renderer that will display the sprite.
        /// </summary>
        private BoxCollider2D m_boxCollider;

        #endregion

        #region Properties.

        /// <summary>
        /// Collider reference.
        /// </summary>
        public BoxCollider2D Collider
        {
            get
            {
                if (this.m_boxCollider == null)
                {
                    this.m_boxCollider = InitializeCollider();
                }
                return this.m_boxCollider;
            }
        }
        
        /// <summary>
        /// Reference to collider offset property.
        /// </summary>
        public Vector2 Offset
        {
            get { return this.Collider.offset; }
            set { this.Collider.offset = value; }
        }

        /// <summary>
        /// Reference to collider size property.
        /// </summary>
        public Vector2 Size
        {
            get { return this.Collider.size; }
            set { this.Collider.size = value; }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Update the collider properties.
        /// </summary>
        public override void Update()
        {
            if (!this.Initialized)
            {
                this.Initialize();
            }
            else
            {
                // Call the base method.
                base.Update();

                // Allow collisions with collider when:
                // Active.
                // Not Inactive.
                // Not Paused.
                // Visible.

                // Must follow above:
                this.Collider.enabled = this.Status.IsActive()
                    && !this.Status.IsInactive()
                    && !this.Status.IsPaused()
                    && this.Status.IsVisible();
            }
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Create the collider for the player controller.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the base class' method.
                base.Initialize();

                // Initialize the data members.
                this.m_boxCollider = this.Collider;
            }
        }
        
        /// <summary>
        /// Initializes the player's box collider.
        /// </summary>
        /// <returns>Returns the initialized box collider.</returns>
        public BoxCollider2D InitializeCollider()
        {
            this.m_boxCollider = this.Self.GetComponent<BoxCollider2D>();

            if (this.m_boxCollider == null)
            {
                this.m_boxCollider = this.Self.AddComponent<BoxCollider2D>();
            }

            // Set property defaults.
            this.m_boxCollider.isTrigger = false;
            this.m_boxCollider.usedByEffector = false;
            this.m_boxCollider.usedByComposite = false;
            this.m_boxCollider.autoTiling = false;

            return m_boxCollider;
        }

        #endregion

    }

    #endregion

}
