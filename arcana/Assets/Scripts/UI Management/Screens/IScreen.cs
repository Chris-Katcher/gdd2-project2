/************************************************
 * IScreen.cs
 * 
 * This file contains:
 * - The ScreenBackground abstract class. (Child of ScreenBase).
 * - The ScreenBase abstract class. (Child of ArcanaObject, Implements IScreen).
 * - The IScreen interface.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Entities.Attributes;

namespace Arcana.UI.Screens
{

    #region Abstract Class: ScreenBackground class.

    /// <summary>
    /// Screen that holds a sprite renderer for a background.
    /// </summary>
    public abstract class ScreenBackground : ScreenBase
    {

        #region Data Members

        #region Fields.

        /// <summary>
        /// Background ID of the screen.
        /// </summary>
        private string m_backgroundID;

        /// <summary>
        /// Location of the background sprite.
        /// </summary>
        private string m_backgroundPath;

        /// <summary>
        /// Material ID.
        /// </summary>
        private string m_materialID;

        /// <summary>
        /// Location of the background material.
        /// </summary>
        private string m_materialPath;

        /// <summary>
        /// SpriteRenderer for the background.
        /// </summary>
        private EntityRenderer m_renderer = null;

        #endregion

        #region Properties.

        /// <summary>
        /// Background ID.
        /// </summary>
        public string BackgroundID
        {
            get { return this.m_backgroundID; }
            protected set { this.m_backgroundID = value; }
        }

        /// <summary>
        /// Location of the background sprite.
        /// </summary>
        public string BackgroundPath
        {
            get { return this.m_backgroundPath; }
            protected set { this.m_backgroundPath = value; }
        }

        /// <summary>
        /// Material ID.
        /// </summary>
        public string MaterialID
        {
            get { return this.m_materialID; }
            protected set { this.m_materialID = value; }
        }

        /// <summary>
        /// Location of the background material.
        /// </summary>
        public string MaterialPath
        {
            get { return this.m_materialPath; }
            protected set { this.m_materialPath = value; }
        }

        /// <summary>
        /// Reference to the screen's sprite renderer.
        /// </summary>
        public EntityRenderer Renderer
        {
            get
            {
                if (this.m_renderer == null)
                {
                    this.m_renderer = this.InitializeRenderer();
                }

                return this.m_renderer;
            }
        }

        #endregion

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Load the background resource for the main menu.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the ScreenBase.Initialize().
                base.Initialize();

                // Rendering resources.
                this.InitializeRendererResources();
            }
        }

        /// <summary>
        /// Set up the ID's and paths for the sprite and material.
        /// </summary>
        public abstract void InitializeRendererResources();

        /// <summary>
        /// Iniitalize the renderer.
        /// </summary>
        /// <returns>Returns a SpriteRenderer component.</returns>
        public EntityRenderer InitializeRenderer()
        {
            EntityRenderer renderer = this.Self.GetComponent<EntityRenderer>();

            if (renderer == null)
            {
                renderer = this.Self.AddComponent<EntityRenderer>();
                renderer.Initialize();
                renderer.InitializeResources(
                    BackgroundID,
                    MaterialID,
                    BackgroundPath,
                    MaterialPath);
                renderer.InitializeRenderer();
            }

            return renderer;
        }

        #endregion
        
    }
    
    #endregion

    #region Abstract Class: ScreenBase class.

    /// <summary>
    /// Abstract Screen class that Screen objects will inherit.
    /// </summary>
    public abstract class ScreenBase : ArcanaObject, IScreen
    {

        #region Data Members

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// ID given to a screen.
        /// </summary>
        private ScreenID m_screenID = ScreenID.NULL_SCREEN;

        /// <summary>
        /// IScreen width (x-axis).
        /// </summary>
        private float m_width = 0.0f;

        /// <summary>
        /// IScreen height (y-axis).
        /// </summary>
        private float m_height = 0.0f;

        /// <summary>
        /// Lifetime of the screen.
        /// </summary>
        private float m_timeToLive = -1.0f;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Reference to the screen ID.
        /// </summary>
        public ScreenID ScreenID
        {
            get { return this.m_screenID; }
            protected set { this.m_screenID = value; }
        }

        /// <summary>
        /// IScreen width (x-axis).
        /// </summary>
        public float ScreenWidth
        {
            get
            {
                if (this.m_width == 0.0f)
                {
                    this.m_width = 0.1f;
                }
                return this.m_width;
            }

            set { this.m_width = Services.Max(value, 0.1f); }
        }

        /// <summary>
        /// IScreen height (y-axis).
        /// </summary>
        public float ScreenHeight
        {
            get
            {
                if (this.m_height == 0.0f)
                {
                    this.m_height = 0.1f;
                }
                return this.m_height;
            }

            set { this.m_height = Services.Max(value, 0.1f); }
        }

        /// <summary>
        /// Depth of the screen object.
        /// </summary>
        public float ScreenDepth
        {
            get
            {
                return this.Position.z;
            }

            set
            {
                Vector3 position = this.Position;
                position.z = value;
                this.Position = position;
            }
        }

        /// <summary>
        /// Boundaries of the screen in world space.
        /// </summary>
        public Rect ScreenBounds
        {
            get { return new Rect(this.Position.x, this.Position.y, this.Position.x + this.ScreenWidth, this.Position.y + this.ScreenHeight); }
        }

        /// <summary>
        /// Dimensions of the screen.
        /// </summary>
        public Rect ScreenDimensions
        {
            get { return new Rect(0.0f, 0.0f, this.ScreenWidth, this.ScreenHeight); }
        }

        /// <summary>
        /// Duration of screen's lifetime.
        /// </summary>
        public float TimeToLive
        {
            get
            {
                return this.m_timeToLive;
            }

            set
            {
                if (value == -1.0f)
                {
                    this.m_timeToLive = -1.0f;
                }

                // If set to less than zero, turn off time to live parameter.
                if (value != -1.0f)
                {
                    this.m_timeToLive = Services.Max(value, 0.0f);
                }
            }
        }

        /// <summary>
        /// Center point of the screen itself, in the world space.
        /// </summary>
        public Vector3 Center
        {
            get { return new Vector3(this.ScreenBounds.width / 2.0f, this.ScreenBounds.height / 2.0f, this.ScreenDepth); }
        }

        /// <summary>
        /// Center of the screen, in its local space.
        /// </summary>
        public Vector2 Midpoint
        {
            get { return new Vector3(this.ScreenDimensions.width / 2.0f, this.ScreenDimensions.height / 2.0f); }
        }

        /// <summary>
        /// Center of the screen object in world space.
        /// </summary>
        public Vector3 Position
        {
            get { return this.Self.transform.position; }
            set { this.Self.transform.position = value; }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Update the position of the screen.
        /// </summary>
        public override void Update()
        {
            if (!this.Initialized)
            {
                this.Initialize();
            }
            else
            {
                // Check if destroyed.
                base.Update();                
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// The initialization method.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Build ArcanaObject components.
                base.Initialize();

                // Turn off debug.
                this.Debug = false;

                // Set the name.
                this.Name = "Null Screen";
                InitializeScreenName();

                // Set default boundaries.
                this.ScreenWidth = 1.0f;
                this.ScreenHeight = 1.0f;
                InitializeDimensions();

                // Set screen ID.
                this.m_screenID = ScreenID.NULL_SCREEN;
                InitializeScreenID();
            }
        }

        /// <summary>
        /// Initialize the screen name.
        /// </summary>
        public abstract void InitializeScreenName();

        /// <summary>
        /// Initialize the dimensions of the screen.
        /// </summary>
        public abstract void InitializeDimensions();

        /// <summary>
        /// Initialize the screen ID value.
        /// </summary>
        public abstract void InitializeScreenID();

        #endregion

        #region Interface Methods.

        /// <summary>
        /// Update the current position of the screen.
        /// </summary>
        public abstract void UpdatePosition();

        /// <summary>
        /// Checks if within x and y axis bounds. (z-axis is ignored).
        /// </summary>
        /// <param name="_position">Vector3 position to check.</param>
        /// <returns>Returns true if position is within bounds.</returns>
        public virtual bool InBounds(Vector3 _position)
        {
            return InBounds(Services.ToVector2(_position));
        }

        /// <summary>
        /// Checks if within x and y axis bounds.
        /// </summary>
        /// <param name="_point">Vector2 position to check.</param>
        /// <returns>Returns true if position is within bounds.</returns>
        public virtual bool InBounds(Vector2 _point)
        {
            return this.ScreenBounds.Contains(_point);
        }

        /// <summary>
        /// Checks if a bounded area intersects with the screen's bounds on the x-y plane.
        /// </summary>
        /// <param name="_other">Rectangle to compare.</param>
        /// <returns>Returns true if intersection is present.</returns>
        public virtual bool Intersects(Rect _other)
        {
            return this.ScreenBounds.Overlaps(_other);   
        }

        /// <summary>
        /// Checks if a point is outside the boundaries on the x-y plane.
        /// </summary>
        /// <param name="_point">Point to check.</param>
        /// <returns>Returns true if point is not within the bounds.</returns>
        public bool IsOutside(Vector3 _point)
        {
            return !InBounds(_point);
        }

        /// <summary>
        /// Checks if a point is outside the boundaries on the x-y plane.
        /// </summary>
        /// <param name="_point">Point to check.</param>
        /// <returns>Returns true if point is not within the bounds.</returns>
        public bool IsOutside(Vector2 _point)
        {
            return !InBounds(_point);
        }

        /// <summary>
        /// Checks if all of the rectangle is outside of the screen on the x-y plane.
        /// </summary>
        /// <param name="_other">Rect to check.</param>
        /// <returns>Returns true if rect is even partially out of bounds.</returns>
        public bool IsOutside(Rect _other)
        {
            return !Intersects(_other);
        }

        /// <summary>
        /// Checks if any part of the rectangle is outside of the screen on the x-y plane.
        /// </summary>
        /// <param name="_other">Rect to check.</param>
        /// <returns>Returns true if rect is even partially out of bounds.</returns>
        public bool IsPartiallyOutside(Rect _other)
        {
            // If not fully outside then, check all extents.
            return (IsOutside(_other) || IsOutside(_other.max) || IsOutside(_other.min) || IsOutside(_other.center));
        }

        /// <summary>
        /// Returns a random point within the screen bounds.
        /// </summary>
        /// <returns>Returns a random Vector3 within bounds.</returns>
        public virtual Vector3 GetVector3()
        {
            return Services.NextVector3(this.ScreenBounds);
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Checks if the screen is the same.
        /// </summary>
        /// <param name="_id">ID to check.</param>
        /// <returns>Returns true if IDs match.</returns>
        public bool IsScreen(ScreenID _id)
        {
            return this.ScreenID == _id;
        }

        /// <summary>
        /// If time is equal to -1.0f, the screen can't decay.
        /// </summary>
        /// <returns>Returns true if time can decay.</returns>
        public bool IsDecay()
        {
            return this.m_timeToLive >= 0.0f;
        }

        /// <summary>
        /// Determines if the screen has time left.
        /// </summary>
        /// <returns>Returns true if time is left.</returns>
        public bool HasTimeLeft()
        {
            // Decay time is indeterminate, so return true.
            if (!IsDecay()) { return true; }
            else
            {
                // Decay time can be determined.
                if(this.m_timeToLive > 0.0f)
                {
                    return true;
                }
            }

            // Return false when time is == 0.0f.
            return false;
        }
        
        /// <summary>
        /// Update time decay value.
        /// </summary>
        public void UpdateTime()
        {
            if (IsDecay() && HasTimeLeft())
            {
                this.TimeToLive -= Time.deltaTime;

                if(this.TimeToLive == 0.0f)
                {
                    this.Status.Kill();
                }
            }
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set time left to live.
        /// </summary>
        public void DeactivateTimer()
        {
            this.TimeToLive = -1.0f;
        }

        /// <summary>
        /// Set the time left to live to the specified amount.
        /// </summary>
        /// <param name="_time">Value to set amount to.</param>
        public void SetTimer(float _time)
        {
            this.TimeToLive = _time;
        }

        #endregion

    }


    #endregion

    #region Interface: IScreen.

    /// <summary>
    /// Defines the public data members and class references for each IScreen type classes.
    /// </summary>
    public interface IScreen
    {

        #region Data Members

        /////////////////////
        // Public data fields.
        /////////////////////

        /// <summary>
        /// IScreen width (x-axis).
        /// </summary>
        float ScreenWidth { get; set; }

        /// <summary>
        /// IScreen height (y-axis).
        /// </summary>
        float ScreenHeight { get; set; }
        
        /// <summary>
        /// Depth of the screen.
        /// </summary>
        float ScreenDepth { get; set; }

        /// <summary>
        /// Boundaries of the screen.
        /// </summary>
        Rect ScreenBounds { get; }

        /// <summary>
        /// Duration of screen's lifetime.
        /// </summary>
        float TimeToLive { get; set; }

        /// <summary>
        /// Center of the screen itself.
        /// </summary>
        Vector3 Center { get; }

        /// <summary>
        /// Center of the screen object in world space.
        /// </summary>
        Vector3 Position { get; }

        #endregion

        #region Mutator Methods

        /// <summary>
        /// Update the current position of the screen object.
        /// </summary>
        void UpdatePosition();

        #endregion

        #region Accessors Methods

        /// <summary>
        /// Checks if the screen is the same.
        /// </summary>
        /// <param name="_id">ID to check.</param>
        /// <returns>Returns true if IDs match.</returns>
        bool IsScreen(ScreenID _id);

        /// <summary>
        /// Checks if within x and y axis bounds. (z-axis is ignored).
        /// </summary>
        /// <param name="_point">Vector3 position to check.</param>
        /// <returns>Returns true if position is within bounds.</returns>
        bool InBounds(Vector3 _point);

        /// <summary>
        /// Checks if within x and y axis bounds.
        /// </summary>
        /// <param name="_point">Vector2 position to check.</param>
        /// <returns>Returns true if position is within bounds.</returns>
        bool InBounds(Vector2 _point);

        /// <summary>
        /// Checks if a bounded area intersects with the screen's bounds on the x-y plane.
        /// </summary>
        /// <param name="_other">Rectangle to compare.</param>
        /// <returns>Returns true if intersection is present.</returns>
        bool Intersects(Rect _other);

        /// <summary>
        /// Checks if a point is outside the boundaries on the x-y plane.
        /// </summary>
        /// <param name="_point">Point to check.</param>
        /// <returns>Returns true if point is not within the bounds.</returns>
        bool IsOutside(Vector3 _point);

        /// <summary>
        /// Checks if a point is outside the boundaries on the x-y plane.
        /// </summary>
        /// <param name="_point">Point to check.</param>
        /// <returns>Returns true if point is not within the bounds.</returns>
        bool IsOutside(Vector2 _point);
        
        /// <summary>
        /// Checks if all of the rectangle is outside of the screen on the x-y plane.
        /// </summary>
        /// <param name="_other">Rect to check.</param>
        /// <returns>Returns true if rect is even partially out of bounds.</returns>
        bool IsOutside(Rect _other);

        /// <summary>
        /// Checks if any part of the rectangle is outside of the screen on the x-y plane.
        /// </summary>
        /// <param name="_other">Rect to check.</param>
        /// <returns>Returns true if rect is even partially out of bounds.</returns>
        bool IsPartiallyOutside(Rect _other);

        /// <summary>
        /// Returns a random Vector3 located within the screen's bounds.
        /// </summary>
        /// <returns>Random Vector3.</returns>
        Vector3 GetVector3();

        #endregion

    }

    #endregion

}
