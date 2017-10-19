/************************************************
 * Dimension.cs
 * 
 * Dimension is a wrapper class for data involving
 * boundaries.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Entities.Attributes
{
    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// The dimension class makes it easier to package Entity dimensions around.
    /// </summary>
    public class Dimension
    {
        #region Data Members.

        /////////////////////
        // Attributes.
        /////////////////////

        /// <summary>
        /// Width of the Entity.
        /// </summary>
        private float m_width;

        /// <summary>
        /// Height of the Entity.
        /// </summary>
        private float m_height;

        /// <summary>
        /// Level depth of the Entity.
        /// </summary>
        private float m_depth;

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Return the dimensions as a 2D vector.
        /// </summary>
        public Vector2 Dimensions
        {
            get { return Services.ToVector2(m_width, m_height);  }
        }

        /// <summary>
        /// Return the dimensions as a 3D vector, with z being depth.
        /// </summary>
        public Vector3 Dimensions3D
        {
            get { return Services.ToVector3(m_width, m_height, m_depth);  }
        }

        /// <summary>
        /// Width of the Entity.
        /// </summary>
        public float Width
        {
            get { return this.m_width; }
            set { SetWidth(value); }
        }

        /// <summary>
        /// Height of the Entity.
        /// </summary>
        public float Height
        {
            get { return this.m_height; }
            set { SetHeight(value); }
        }

        /// <summary>
        /// Depth of the Entity.
        /// </summary>
        public float Depth
        {
            get { return this.m_depth; }
            set { SetDepth(value); }
        }

        #endregion

        #region Constructor.

        /// <summary>
        /// Default, empty constructor.
        /// </summary>
        public Dimension()
        {
            Initialize();
        }

        /// <summary>
        /// Dimension class with set dimensions.
        /// </summary>
        /// <param name="sides">The width and height.</param>
        /// <param name="depth">The depth.</param>
        public Dimension(int sides, int depth = 0)
        {
            Initialize(sides, sides, depth);
        }

        /// <summary>
        /// Dimension class with set dimensions.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        public Dimension(int width, int height, int depth = 0)
        {
            Initialize(width, height, depth);
        }
        
        /// <summary>
        /// Dimension class with set dimensions.
        /// </summary>
        /// <param name="sides">The width and height.</param>
        /// <param name="depth">The depth.</param>
        public Dimension(float sides, float depth = 0)
        {
            Initialize(sides, sides, depth);
        }

        /// <summary>
        /// Dimension class with set dimensions.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        public Dimension(float width, float height, float depth = 0)
        {
            Initialize(width, height, depth);
        }

        #endregion

        #region Service Methods

        /// <summary>
        /// Initialize to default values.
        /// </summary>
        private void Initialize()
        {
            SetWidth(Constants.DEFAULT_DIMENSION);
            SetHeight(Constants.DEFAULT_DIMENSION);
            SetDepth(0.0f);
        }

        /// <summary>
        /// Initialize the dimension with set values.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        private void Initialize(float width, float height, float depth = 0.0f)
        {
            SetWidth(width);
            SetHeight(height);
            SetDepth(depth);
        }
        
        /// <summary>
        /// Scale the dimension and return a copy.
        /// </summary>
        /// <param name="scale">Value to scale by.</param>
        /// <returns>Returns a <see cref="Dimension"/> that has been scaled.</returns>
        public Dimension Scale(float scale)
        {
            scale = Services.Max(Services.Abs(scale), 0.001f);
            return new Dimension(this.Width * scale, this.Height * scale);
        }

        #endregion

        #region Mutator Methods

        /// <summary>
        /// Set width of the dimension.
        /// </summary>
        /// <param name="width">Value to set width to.</param>
        public void SetWidth(float width)
        {
            this.m_width = Services.Max(width, 0.1f);
        }

        /// <summary>
        /// Set height of the dimension.
        /// </summary>
        /// <param name="height">Value to set height to.</param>
        public void SetHeight(float height)
        {
            this.m_height = Services.Max(height, 0.1f);
        }

        /// <summary>
        /// Set depth of the dimension.
        /// </summary>
        /// <param name="depth">Value to set depth to.</param>
        public void SetDepth(float depth)
        {
            this.m_depth = Services.Max(depth, 0.1f);
        }

        #endregion

    }
}
