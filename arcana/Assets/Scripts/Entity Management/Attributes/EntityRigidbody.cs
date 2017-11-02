using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Arcana.Entities.Attributes
{

    #region Class: EntityRigidbody class.

    /// <summary>
    /// Generates a Rigidbody2D for an Entity.
    /// </summary>
    public class EntityRigidbody : ArcanaObject
    {

        #region Data Members

        #region Fields.

        /// <summary>
        /// Renderer that will display the sprite.
        /// </summary>
        private Rigidbody2D m_rigidbody;

        /// <summary>
        /// Entity heading in a 2D vector space.
        /// </summary>
        private Vector2 m_direction;
        
        /// <summary>
        /// Frozen position used when freezing the rigidbody.
        /// </summary>
        private Vector3 m_position = Vector3.zero;

        /// <summary>
        /// Flag to freeze movement.
        /// </summary>
        private bool m_freeze = false;

        #endregion

        #region Properties.

        /// <summary>
        /// Rigidbody reference.
        /// </summary>
        public Rigidbody2D Rigidbody
        {
            get
            {
                if (this.m_rigidbody == null)
                {
                    this.m_rigidbody = InitializeRigidbody();
                }
                return this.m_rigidbody;
            }
        }
        
        /// <summary>
        /// Reference to the direction vector.
        /// </summary>
        public Vector2 Heading
        {
            get { return this.m_direction.normalized; }
            set { this.m_direction = (value).normalized; }
        }

        /// <summary>
        /// Reference to the velocity vector.
        /// </summary>
        public Vector2 Velocity
        {
            get { return this.Rigidbody.velocity; }
        }
        
        /// <summary>
        /// Reference to the rigidbody's gravity scale property.
        /// </summary>
        public float Gravity
        {
            get { return this.Rigidbody.gravityScale; }
            set { this.Rigidbody.gravityScale = value; }
        }

        /// <summary>
        /// Reference to the rigidbody's mass.
        /// </summary>
        public float Mass
        {
            get { return this.Rigidbody.mass; }
            set { this.Rigidbody.mass = Services.Max(value, 0.001f); }
        }

        /// <summary>
        /// Reference to the rigidbody's drag.
        /// </summary>
        public float Drag
        {
            get { return this.Rigidbody.drag; }
            set { this.Rigidbody.drag = value; }
        }

        /// <summary>
        /// Reference to the rigidbody's angular drag.
        /// </summary>
        public float AngularDrag
        {
            get { return this.Rigidbody.angularDrag; }
            set { this.Rigidbody.angularDrag = value; }
        }

        /// <summary>
        /// Freeze the movement of the rigidbody.
        /// </summary>
        public bool Freeze
        {
            get { return this.m_freeze; }
            set { this.m_freeze = value; }
        }

        /// <summary>
        /// Position to use when freezing the rigidbody.
        /// </summary>
        public Vector3 FrozenPosition
        {
            get { return this.m_position; }
            set { this.m_position = value; }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.
        
        /// <summary>
        /// Update the rigidbody properties.
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

                if (!Freeze)
                {
                    // When not frozen, keep track of position.
                    this.m_position = this.transform.position;
                }

                // Make not kinematic when:
                // Active.
                // Not Inactive.
                // Not Paused.
                // Visible.

                // Must follow above:
                this.Rigidbody.isKinematic = !(this.Status.IsActive()
                    && !this.Status.IsInactive()
                    && !this.Status.IsPaused()
                    && this.Status.IsVisible());

                FreezePosition(this.Rigidbody.isKinematic);
            }
        }

        /// <summary>
        /// Turn the freeze position on and off.
        /// </summary>
        public void FreezePosition(bool _flag)
        {
            this.Freeze = _flag;

            if (_flag)
            {
                this.transform.position = this.m_position;
                this.Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                this.Rigidbody.constraints = RigidbodyConstraints2D.None;
                this.Rigidbody.freezeRotation = true;
            }
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Create the rigidbody for the player controller.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the base class' method.
                base.Initialize();

                // Initialize the data members.
                this.m_rigidbody = this.Rigidbody;
            }
        }

        /// <summary>
        /// Initialize the direction.
        /// </summary>
        /// <param name="_init">Initialization direction.</param>
        public void InitializeDirection(Vector2 _init)
        {
            this.m_direction = _init;
        }

        /// <summary>
        /// Initializes the player's rigidbody.
        /// </summary>
        /// <returns>Returns the initialized rigidbody.</returns>
        public Rigidbody2D InitializeRigidbody()
        {
            this.m_rigidbody = this.Self.GetComponent<Rigidbody2D>();

            if (this.m_rigidbody == null)
            {
                this.m_rigidbody = this.Self.AddComponent<Rigidbody2D>();
            }

            // Set property defaults.
            m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
            m_rigidbody.simulated = true;
            m_rigidbody.useAutoMass = false;
            m_rigidbody.mass = 5.0f;
            m_rigidbody.drag = 2.5f;
            m_rigidbody.angularDrag = 0.05f;
            m_rigidbody.gravityScale = 4.0f;
            m_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            m_rigidbody.sleepMode = RigidbodySleepMode2D.StartAwake;
            m_rigidbody.interpolation = RigidbodyInterpolation2D.None;
            m_rigidbody.freezeRotation = false;

            return m_rigidbody;
        }

        #endregion

    }

    #endregion

}
