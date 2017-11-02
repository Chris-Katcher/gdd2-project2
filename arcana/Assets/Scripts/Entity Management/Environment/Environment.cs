/************************************************
 * Environment.cs
 * 
 * This file contains:
 * - The Environment class. (Child of Entity).
 * - The EnvironmentType enum.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Entities;

namespace Arcana.Environment
{
    
    #region Class: Environment class.

    /// <summary>
    /// Environment is the base class for platforms.
    /// </summary>
    public class Environment : Entity
    {

        #region Data Members.

        #region Fields.

        /////////////////////
        // Data members.
        /////////////////////

        /// <summary>
        /// Represents the type of environment being dealt with.
        /// </summary>
        private List<EnvironmentType> m_type;

        /// <summary>
        /// Current position of the environment set-piece.
        /// </summary>
        private Vector3 m_position;

        /// <summary>
        /// Current rotation of the environment set-piece.
        /// </summary>
        private Vector3 m_rotation;

        /// <summary>
        /// Current scale of the environment set-piece.
        /// </summary>
        private Vector3 m_scale;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Reference to the environment type.
        /// </summary>
        public List<EnvironmentType> EnvironmentTypes
        {
            get
            {
                if (this.m_type == null)
                {
                    this.m_type = new List<EnvironmentType>();
                }
                return this.m_type;
            }
        }

        /// <summary>
        /// Returns true if base class IsNull is true or if it has a null environment type.
        /// </summary>
        public override bool IsNull
        {
            get
            {
                return base.IsNull && this.EnvironmentTypes.Contains(EnvironmentType.NULL);
            }
        }

        /// <summary>
        /// Reference to the current position.
        /// </summary>
        public Vector3 CurrentPosition
        {
            get { return this.m_position; }
            set { SetPosition(value); }
        }

        /// <summary>
        /// Set the rotation.
        /// </summary>
        public Vector3 Rotation
        {
            get { return this.m_rotation; }
            set { SetRotation(value); }
        }

        /// <summary>
        /// Reference to the current scale.
        /// </summary>
        public Vector3 Scale
        {
            get { return this.m_scale; }
            set { SetScale(value); }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Handle environment updates.
        /// </summary>
        public override void Update()
        {
            if (!this.Initialized)
            {
                this.Initialize();
            }
            else
            {
                // Base update.
                base.Update();
            }
        }

        /// <summary>
        /// Update transform positions.
        /// </summary>
        public void FixedUpdate()
        {
            this.transform.position = this.CurrentPosition;
            this.transform.rotation = Quaternion.Euler(this.Rotation);
            this.transform.localScale = this.Scale;
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the environment.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the base class's initialization function.
                base.Initialize();

                // Initialize with null type.
                this.m_type = new List<EnvironmentType>() { EnvironmentType.NULL };
            }
        }

        #endregion

        #region Status Methods

        /// <summary>
        /// Hide the environment.
        /// </summary>
        public override void Hide()
        {
            base.Hide();

            // Hide objects.
            RendererVisibility(this.Self.GetComponent<MeshRenderer>(), false);
            RendererVisibility(this.Self.GetComponent<SpriteRenderer>(), false);
        }

        /// <summary>
        /// Show the environment.
        /// </summary>
        public override void Show()
        {
            base.Show();

            // Show objects.
            RendererVisibility(this.Self.GetComponent<MeshRenderer>(), true);
            RendererVisibility(this.Self.GetComponent<SpriteRenderer>(), true);
        }

        /// <summary>
        /// Adjust a renderer's visibility.
        /// </summary>
        /// <param name="r">Renderer to adjust.</param>
        /// <param name="_enabled">Flag to designate visibility.</param>
        private void RendererVisibility(Renderer r, bool _enabled)
        {
            if(r != null)
            {
                r.enabled = _enabled;
            }
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns true if it contains the type.
        /// </summary>
        /// <param name="_type">Type to check for.</param>
        /// <returns>Returns true if match is found.</returns>
        public bool HasEnvironmentType(EnvironmentType _type)
        {
            return this.EnvironmentTypes.Contains(_type);
        }
        
        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Adds type.
        /// </summary>
        /// <param name="_type">Adds a type to the environment.</param>
        public void AddEnvironmentType(EnvironmentType _type)
        {
            if (!HasEnvironmentType(_type))
            {
                this.EnvironmentTypes.Add(_type);
            }

            if (_type != EnvironmentType.NULL)
            {
                RemoveEnvironmentType(EnvironmentType.NULL);
            }
        }

        /// <summary>
        /// Remove a type if it exists.
        /// </summary>
        /// <param name="_type">Type to remove.</param>
        public void RemoveEnvironmentType(EnvironmentType _type)
        {
            if (HasEnvironmentType(_type))
            {
                this.EnvironmentTypes.Remove(_type);
            }
        }

        /// <summary>
        /// Set the position of the environment piece.
        /// </summary>
        /// <param name="_position"></param>
        public void SetPosition(Vector3 _position)
        {
            if (_position != Vector3.zero)
            {
                this.m_position = _position;
            }
        }

        /// <summary>
        /// Set the 
        /// </summary>
        /// <param name="_rotation"></param>
        public void SetRotation(Vector3 _rotation)
        {
            this.m_rotation = _rotation;
        }

        /// <summary>
        /// Set scale for the environment piece.
        /// </summary>
        /// <param name="_scale">Scale to set.</param>
        public void SetScale(Vector3 _scale)
        {
            if (_scale != Vector3.zero)
            {
                Vector3 scale = _scale;
                scale.x = Services.Max(scale.x, 0.1f);
                scale.y = Services.Max(scale.y, 0.1f);
                scale.z = Services.Max(scale.z, 0.1f);
                this.m_scale = scale;
            }
        }
                
        #endregion

    }

    #endregion

    #region Enum: EnvironmentType

    /// <summary>
    /// Represents the different type of environmental pieces.
    /// </summary>
    public enum EnvironmentType
    {
        /// <summary>
        /// Default type.
        /// </summary>
        NULL,

        /// <summary>
        /// A background set-piece that can't be interacted with.
        /// </summary>
        Decor,

        /// <summary>
        /// A set-piece that can be moved.
        /// </summary>
        Obstacle,

        /// <summary>
        /// Solid platform.
        /// </summary>
        Platform,
        
        /// <summary>
        /// Platform modifier that allows players to pass through the bottom.
        /// </summary>
        Passthrough,
        
        /// <summary>
        /// Platform that cannot be passed through.
        /// </summary>
        Wall,

        /// <summary>
        /// Wall modifier that indicates top of the screen.
        /// </summary>
        Ceiling,

        /// <summary>
        /// Ground modifier that indicates bottom of the screen.
        /// </summary>
        Ground
    }


    #endregion

}
