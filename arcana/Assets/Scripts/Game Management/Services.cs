/************************************************
 * Services.cs
 * 
 * Services contain references to program-wide constants and has helper functions that can be called.
 * Services offers program-wide reference to constants and helper functions.
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

namespace Arcana
{
    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// Constants stored for use across the programs.
    /// </summary>
    public static class Constants
    {

        #region Constants.

        /////////////////////
        // Constants.
        /////////////////////

        #region // Vector References

        // Vector2 references.

        /// <summary>
        /// Returns a <see cref="Vector2"/> with +1 on the y-axis.
        /// </summary>
        public static readonly Vector2 Up = new Vector2(0.0f, 1.0f);

        /// <summary>
        /// Returns a <see cref="Vector2"/> with -1 on the x-axis.
        /// </summary>
        public static readonly Vector2 Left = new Vector2(-1.0f, 0.0f);

        /// <summary>
        /// Returns a <see cref="Vector2"/> with -1 on the y-axis.
        /// </summary>
        public static readonly Vector2 Down = new Vector2(0.0f, -1.0f);

        /// <summary>
        /// Returns a <see cref="Vector2"/> with 1 on the x-axis.
        /// </summary>
        public static readonly Vector2 Right = new Vector2(1.0f, 0.0f);

        // Vector3 references.

        /// <summary>
        /// Returns a <see cref="Vector3"/> with +1 on the z-axis.
        /// </summary>
        public static readonly Vector3 Forward = new Vector3(0.0f, 0.0f, 1.0f);

        /// <summary>
        /// Returns a <see cref="Vector3"/> with -1 on the z-axis.
        /// </summary>
        public static readonly Vector3 Backward = new Vector3(0.0f, 0.0f, -1.0f);

        /// <summary>
        /// Returns a <see cref="Vector3"/> with +1 on the y-axis.
        /// </summary>
        public static readonly Vector3 Up3 = Services.ToVector3(Up);

        /// <summary>
        /// Returns a <see cref="Vector3"/> with -1 on the x-axis.
        /// </summary>
        public static readonly Vector3 Left3 = Services.ToVector3(Left);

        /// <summary>
        /// Returns a <see cref="Vector3"/> with -1 on the y-axis.
        /// </summary>
        public static readonly Vector3 Down3 = Services.ToVector3(Down);

        /// <summary>
        /// Returns a <see cref="Vector3"/> with +1 on the x-axis.
        /// </summary>
        public static readonly Vector3 Right3 = Services.ToVector3(Right);

        #endregion
        
        #region // Constraint Constants

        /// <summary>
        /// Parameter flag for <see cref="Arcana.Entities.EntityType"/>.
        /// </summary>
        public const string PARAM_ENTITY_TYPE = "entitytype";

        /// <summary>
        /// Parameter flag for <see cref="Arcana.Entities.Entity"/> position.
        /// </summary>
        public const string PARAM_POSITION = "position";

        /// <summary>
        /// Parameter flag for <see cref="Arcana.Entities.Entity"/> dimensions.
        /// </summary>
        public const string PARAM_DIMENSIONS = "dimensions";

        /// <summary>
        /// Parameter flag for an <see cref="Arcana.Entities.Entity"/>'s health tracker.
        /// </summary>
        public const string PARAM_HEALTH_TRACKER = "healthtracker";

        /// <summary>
        /// Parameter flag.
        /// </summary>
        public const string PARAM_UNITY_CAMERA = "unitycamera";

        /// <summary>
        /// Parameter flag.
        /// </summary>
        public const string PARAM_ASPECT_RATIO = "aspectratio";

        /// <summary>
        /// Parameter flag.
        /// </summary>
        public const string PARAM_BACKGROUND = "background";

        /// <summary>
        /// Parameter flag.
        /// </summary>
        public const string PARAM_DISTANCE = "distance";

        /// <summary>
        /// Parameter flag.
        /// </summary>
        public const string PARAM_CAMERA_MODE = "cameramode";

        #endregion

        #region // Entity Constants

        /// <summary>
        /// Default width and height to use when it isn't specified.
        /// </summary>
        public const int DEFAULT_DIMENSION = 100; // in pixels.

        #endregion
        
        #region // Camera Constants.

        /// <summary>
        /// Default camera shake period of time in seconds.
        /// </summary>
        public const float DEFAULT_SHAKE_PERIOD = 1.0f; // in seconds.

        /// <summary>
        /// Default camera shake strength in float coordinate points.
        /// </summary>
        public const float DEFAULT_SHAKE_STRENGTH = 25.0f; // Change in value clamp.
        
        /// <summary>
        /// Default camera shake strength decay factor.
        /// </summary>
        public const float DEFAULT_DECAY_FACTOR = 1.0f; // 100% = no decay value.

        /// <summary>
        /// Default aspect ratio is set to 16:9.
        /// </summary>
        public const float DEFAULT_ASPECT_RATIO = 1.7778f;

        /// <summary>
        /// 16:9 aspect ratio.
        /// </summary>
        public const float ASPECT_RATIO_16_9 = 1.7778f;

        /// <summary>
        /// 4:3 aspect ratio.
        /// </summary>
        public const float ASPECT_RATIO_4_3 = 1.333f;

        /// <summary>
        /// 1:1 aspect ratio (Square).
        /// </summary>
        public const float ASPECT_RATIO_SQAURE = 1;

        /// <summary>
        /// Default orthographic size for the <see cref="UnityEngine.Camera"/>.
        /// </summary>
        public const float DEFAULT_CAMERA_SIZE = 5.0f;

        /// <summary>
        /// Default distance away from the original position.
        /// </summary>
        public const float DEFAULT_CAMERA_DISTANCE = 1.0f;

        /// <summary>
        /// Color of the 
        /// </summary>
        public static readonly Color DEFAULT_CAMERA_BACKGROUND = Color.cyan;

        #endregion

        #region // HealthTracker Constants

        /// <summary>
        /// Default time-based health.
        /// </summary>
        public const bool DEFAULT_TIMEBASED_HEALTH = false;

        /// <summary>
        /// Default time-based decay steps.
        /// </summary>
        public const int DEFAULT_DECAY_STEP = 1; // In units of health.

        /// <summary>
        /// Default time-based decay rate.
        /// </summary>
        public const int DEFAULT_DECAY_RATE = 1; // In seconds.

        /// <summary>
        /// If set to this value, entity is invulnerable.
        /// </summary>
        public const int INVULNERABLE_MODE = -1;

        /// <summary>
        /// Default invulnerability period.
        /// </summary>
        public const int DEFAULT_INVULNERABILITY_PERIOD = 10; // In seconds.

        /// <summary>
        /// Ignore the damage limit.
        /// </summary>
        public const int IGNORE_DAMAGE_LIMIT = -1;

        /// <summary>
        /// Default damage limit per frame.
        /// </summary>
        public const int DEFAULT_DAMAGE_LIMIT = -1; // Ignored when equal to -1.

        /// <summary>
        /// Default health value to assign when it isn't specified.
        /// </summary>
        public const int DEFAULT_HEALTH = -1; // When less than zero, we don't consider it to have a health state.

        /// <summary>
        /// Default maximum health value.
        /// </summary>
        public const int DEFAULT_MAX_HEALTH = 100; // Default maximum health value.

        /// <summary>
        /// Default minimum health value.
        /// </summary>
        public const int DEFAULT_MIN_HEALTH = 100; // Default minimum health value.

        #endregion

        #endregion

    }

    /// <summary>
    /// Services called to aid in comparisons and completions.
    /// </summary>
    public static class Services
    {

        #region UnityEngine Helpers

        /// <summary>
        /// Creates an empty <see cref="GameObject"/>, with input title, and returns it.
        /// </summary>
        /// <param name="title">Title of the <see cref="GameObject"/>.</param>
        /// <returns>Returns created <see cref="GameObject"/>.</returns>
        public static GameObject CreateEmptyObject(string title = "GameObject (Empty)")
        {
            return new GameObject(title);
        }

        /// <summary>
        /// Add a child to the parent <see cref="GameObject"/>. Returns the child <see cref="GameObject"/>.
        /// </summary>
        /// <param name="parent">The object receiving the child.</param>
        /// <param name="child">The object placed as the child.</param>
        /// <returns>Returns the child object.</returns>
        public static GameObject AddChild(GameObject parent, GameObject child)
        {
            child.transform.parent = parent.transform;
            return child;
        }

        /// <summary>
        /// Add a parent to the child <see cref="GameObject"/>. Returns the parent <see cref="GameObject"/>.
        /// </summary>
        /// <param name="child">The object receiving the parent.</param>
        /// <param name="parent">The object placed as the parent.</param>
        /// <returns>Returns the child object.</returns>
        public static GameObject AddParent(GameObject child, GameObject parent)
        {
            child.transform.parent = parent.transform;
            return parent;
        }

        #endregion

        #region Math Functions.

        #region // Dimension math functions.

        /// <summary>
        /// Multiplies two to three products together.
        /// </summary>
        /// <param name="terms">Multiplies these terms.</param>
        /// <returns>Returns product of terms.</returns>
        public static float Product(params float[] terms)
        {
            if (terms.Length > 0)
            {
                float product = 1.0f;

                for (int i = 0; i < terms.Length; i++)
                {
                    product *= terms[i];
                }
            }

            return 0.0f;
        }

        /// <summary>
        /// Get the area of the dimension.
        /// </summary>
        /// <param name="d">Dimension to get area of.</param>
        /// <returns>Get the dimension's 2D area.</returns>
        public static float Area(Dimension d)
        {
            float a = Abs(d.Width);
            float b = Abs(d.Height);

            if (a == 0.0f) { a = 1.0f; }
            if(b == 0.0f) { b = 1.0f; }

            return Product(a, b);
        }

        /// <summary>
        /// Get the volume of the dimension. If depth is zero, it's treated as a '1'.
        /// </summary>
        /// <param name="d">Dimension to get volume of.</param>
        /// <returns>Get the dimension's 3D volume.</returns>
        public static float Volume(Dimension d)
        {
            float a = Abs(d.Width);
            float b = Abs(d.Height);
            float c = Abs(d.Depth);

            if (a == 0.0f) { a = 1.0f; }
            if (b == 0.0f) { b = 1.0f; }
            if (c == 0.0f) { c = 1.0f; }

            return Product(a, b, c);
        }

        #endregion

        #region // Vector Math Functions.

        /////////////////////
        // Vector Math Functions.
        /////////////////////

        /// <summary>
        /// Check if a vector is null.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static bool IsEmpty(Vector2? vector)
        {
            if (vector.HasValue)
            {
                return (float.IsNaN(vector.Value.sqrMagnitude)
                    || (vector.Value == Vector2.zero)
                    || (vector.Value.magnitude == 0.0f));
            }

            return false;
        }


        /// <summary>
        /// Check if a vector is null.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static bool IsEmpty(Vector3? vector)
        {
            if (vector.HasValue)
            {
                return (float.IsNaN(vector.Value.sqrMagnitude)
                    || (vector.Value == Vector3 .zero)
                    || (vector.Value.magnitude == 0.0f));
            }

            return false;
        }

        /// <summary>
        /// Get the magnitude of a given <see cref="Vector2"/>.
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/> to find the magnitude of.</param>
        /// <returns>Returns the length of the vector.</returns>
        public static float Magnitude(Vector2 vector)
        {
            return vector.magnitude;
        }

        /// <summary>
        /// Get the magnitude of a given <see cref="Vector3"/>.
        /// </summary>
        /// <param name="vector"><see cref="Vector3"/> to find the magnitude of.</param>
        /// <returns>Returns the length of the vector.</returns>
        public static float Magnitude(Vector3 vector)
        {
            return vector.magnitude;
        }

        /// <summary>
        /// Get a normalized vector, based off of input <see cref="Vector2"/>.
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/> to normalize.</param>
        /// <returns>Returns the vector with a length of 1.</returns>
        public static Vector2 Normalize(Vector2 vector)
        {
            return vector.normalized;
        }
        
        /// <summary>
        /// Get a normalized vector, based off of input <see cref="Vector3"/>.
        /// </summary>
        /// <param name="vector"><see cref="Vector3"/> to normalize.</param>
        /// <returns>Returns the vector with a length of 1.</returns>
        public static Vector3 Normalize(Vector3 vector)
        {
            return vector.normalized;
        }

        /// <summary>
        /// <para>
        /// Wrapper for the Unity Dot class.
        /// </para>
        /// <para>
        /// Returns 1 if vectors have the same direction,
        /// -1 if the vectors point in opposite directions,
        /// and 0 if the vectors are perpendicular.
        /// </para>
        /// </summary>
        /// <param name="a">Left-hand vector.</param>
        /// <param name="b">Right-hand vector.</param>
        /// <returns>Returns 1 if they have the same direction, -1 if they have opposite directions, and zero if they are perpendicular.</returns>
        public static float Dot(Vector2 a, Vector2 b)
        {
            return Vector2.Dot(a, b);
        }

        /// <summary>
        /// <para>
        /// Wrapper for the Unity Dot class.
        /// </para>
        /// <para>
        /// Returns 1 if vectors have the same direction,
        /// -1 if the vectors point in opposite directions,
        /// and 0 if the vectors are perpendicular.
        /// </para>
        /// </summary>
        /// <param name="a">Left-hand vector.</param>
        /// <param name="b">Right-hand vector.</param>
        /// <returns>Returns 1 if they have the same direction, -1 if they have opposite directions, and zero if they are perpendicular.</returns>
        public static float Dot(Vector3 a, Vector3 b)
        {
            return Vector3.Dot(a, b);
        }
        
        /// <summary>
        /// Calculates the sum of the input terms.
        /// </summary>
        /// <param name="vectors">Vectors to find sum of.</param>
        /// <returns>Returns sum of all the terms as a <see cref="Vector2"/>.</returns>
        public static Vector2 Sum(params Vector2[] vectors)
        {
            if (vectors == null || vectors.Length == 0) { return Vector2.zero; }
            if (vectors.Length == 1) { return vectors[0]; }

            Vector2 sum = Vector2.zero;

            foreach (Vector2 vector in vectors)
            {
                sum += vector;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum of the input terms.
        /// </summary>
        /// <param name="vectors">Vectors to find sum of.</param>
        /// <returns>Returns sum of all the terms as a <see cref="Vector2"/>.</returns>
        public static Vector2 Sum(List<Vector2> vectors)
        {
            if (vectors == null || vectors.Count == 0) { return Vector2.zero; }
            if (vectors.Count == 1) { return vectors[0]; }

            Vector2 sum = Vector2.zero;

            foreach (Vector2 vector in vectors)
            {
                sum += vector;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum of the input terms.
        /// </summary>
        /// <param name="vectors">Vectors to find sum of.</param>
        /// <returns>Returns sum of all the terms as a <see cref="Vector3"/>.</returns>
        public static Vector3 Sum(params Vector3[] vectors)
        {
            if (vectors == null || vectors.Length == 0) { return Vector3.zero; }
            if (vectors.Length == 1) { return vectors[0]; }

            Vector3 sum = Vector3.zero;

            foreach (Vector3 vector in vectors)
            {
                sum += vector;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum of the input terms.
        /// </summary>
        /// <param name="vectors">Vectors to find sum of.</param>
        /// <returns>Returns sum of all the terms as a <see cref="Vector3"/>.</returns>
        public static Vector3 Sum(List<Vector3> vectors)
        {
            if (vectors == null || vectors.Count == 0) { return Vector3.zero; }
            if (vectors.Count == 1) { return vectors[0]; }

            Vector3 sum = Vector3.zero;

            foreach (Vector3 vector in vectors)
            {
                sum += vector;
            }

            return sum;
        }

        /// <summary>
        /// Calculate the center (or average) of all the input vectors.
        /// </summary>
        /// <param name="vectors">Vectors to find average of.</param>
        /// <returns>Returns average as <see cref="Vector2"/>.</returns>
        public static Vector2 Average(params Vector2[] vectors)
        {
            if (vectors == null || vectors.Length == 0) { return Vector2.zero; }
            if (vectors.Length == 1) { return vectors[0]; }
            return Sum(vectors) / vectors.Length;
        }

        /// <summary>
        /// Calculate the center (or average) of all the input vectors.
        /// </summary>
        /// <param name="vectors">Vectors to find average of.</param>
        /// <returns>Returns average as <see cref="Vector2"/>.</returns>
        public static Vector2 Average(List<Vector2> vectors)
        {
            if (vectors == null || vectors.Count == 0) { return Vector2.zero; }
            if (vectors.Count == 1) { return vectors[0]; }
            return Sum(vectors) / vectors.Count;
        }

        /// <summary>
        /// Calculate the center (or average) of all the input vectors.
        /// </summary>
        /// <param name="vectors">Vectors to find average of.</param>
        /// <returns>Returns average as <see cref="Vector3"/>.</returns>
        public static Vector3 Average(params Vector3[] vectors)
        {
            if (vectors == null || vectors.Length == 0) { return Vector3.zero; }
            if (vectors.Length == 1) { return vectors[0]; }
            return Sum(vectors) / vectors.Length;
        }

        /// <summary>
        /// Calculate the center (or average) of all the input vectors.
        /// </summary>
        /// <param name="vectors">Vectors to find average of.</param>
        /// <returns>Returns average as <see cref="Vector3"/>.</returns>
        public static Vector3 Average(List<Vector3> vectors)
        {
            if (vectors == null || vectors.Count == 0) { return Vector3.zero; }
            if (vectors.Count == 1) { return vectors[0]; }
            return Sum(vectors) / vectors.Count;
        }

        /////////////////////
        // Vector Wrappers.
        /////////////////////

        /// <summary>
        /// Returns <see cref="Vector2"/> as a <see cref="Vector2"/> object.
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/> object to return.</param>
        /// <returns>Returns <see cref="Vector2"/> object.</returns>
        public static Vector2 ToVector2(Vector2 vector)
        {
            return vector;
        }
        
        /// <summary>
        /// Truncates a <see cref="Vector3"/> to return its x and y values as a vector2.
        /// </summary>
        /// <param name="vector"><see cref="Vector3"/> to truncate.</param>
        /// <returns>Returns <see cref="Vector2"/> object.</returns>
        public static Vector2 ToVector2(Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Returns a pair of x-y values as a <see cref="Vector2"/> object.
        /// </summary>
        /// <param name="x">X-value.</param>
        /// <param name="y">Y-value.</param>
        /// <returns>Returns <see cref="Vector2"/> object.</returns>
        public static Vector2 ToVector2(float x, float y)
        {
            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="vector"><see cref="Vector3"/> to return.</param>
        /// <returns>Returns <see cref="Vector3"/> object.</returns>
        public static Vector3 ToVector3(Vector3 vector)
        {
            return vector;
        }

        /// <summary>
        /// Expand a <see cref="Vector2"/> to return its x and y values as a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/> to expand.</param>
        /// <param name="z">Optional parameter to set the Z-value to something other than 0.0f.</param>
        /// <returns>Returns <see cref="Vector3"/> object.</returns>
        public static Vector3 ToVector3(Vector2 vector, float z = 0.0f)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        /// <summary>
        /// Returns a set of x-y-z values as a <see cref="Vector3"/> object.
        /// </summary>
        /// <param name="x">X-value.</param>
        /// <param name="y">Y-value.</param>
        /// <param name="z">Z-value.</param>
        /// <returns>Returns <see cref="Vector2"/> object.</returns>
        public static Vector3 ToVector3(float x, float y, float z)
        {
            return new Vector3(x, y, z);
        }
        
        #endregion

        #region // Random Functions.

        /////////////////////
        // Random Number Functions.
        /////////////////////

        /// <summary>
        /// Generate random int using <see cref="UnityEngine.Random.Range(int, int)"/>.
        /// </summary>
        /// <param name="min">Inclusive minimum of range.</param>
        /// <param name="max">Inclusive maximum of range.</param>
        /// <returns>Returns a random integer within inclusive range.</returns>
        public static int NextInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /// <summary>
        /// Generate random float using <see cref="UnityEngine.Random.Range(float, float)"/>.
        /// </summary>
        /// <param name="min">Inclusive minimum of range.</param>
        /// <param name="max">Inclusive maximum of range.</param>
        /// <returns>Returns a random float within inclusive range.</returns>
        public static float NextFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /// <summary>
        /// Generate <see cref="Vector2"/> object with a random direction, applying the specified magnitude.
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns>Returns a <see cref="Vector2"/> object with a random direction, applying the specified magnitude.</returns>
        public static Vector2 NextVector2(float magnitude = 1.0f)
        {
            Max(magnitude, 0.0f);
            return UnityEngine.Random.insideUnitCircle * magnitude;
        }
        
        /// <summary>
        /// Generate <see cref="Vector2"/> object within a set boundary.
        /// </summary>
        /// <param name="x">Minimum x-value of the boundary.</param>
        /// <param name="y">Minimum y-value of the boundary.</param>
        /// <param name="width">Maximum x-value of the boundary.</param>
        /// <param name="height">Maximum y-value of the boundary.</param>
        /// <returns>Returns a <see cref="Vector2"/> object.</returns>
        public static Vector2 NextVector2(float x, float y, float width, float height)
        {
            return new Vector2(NextFloat(x, width), NextFloat(y, height));
        }

        /// <summary>
        /// Generate <see cref="Vector2"/> object within a set boundary.
        /// </summary>
        /// <param name="a">Vector containing minimum values.</param>
        /// <param name="b">Vector containing maximum values.</param>
        /// <returns>Returns a <see cref="Vector2"/> object.</returns>
        public static Vector2 NextVector2(Vector2 a, Vector2 b)
        {
            return new Vector2(NextFloat(a.x, b.x), NextFloat(a.y, b.y));
        }

        /// <summary>
        /// Generate <see cref="Vector3"/> object within a set boundary.
        /// </summary>
        /// <param name="x">Minimum x-value of the boundary.</param>
        /// <param name="y">Minimum y-value of the boundary.</param>
        /// <param name="width">Maximum x-value of the boundary.</param>
        /// <param name="height">Maximum y-value of the boundary.</param>
        /// <returns>Returns a <see cref="Vector3"/> object.</returns>
        public static Vector3 NextVector3(float x_min, float y_min, float z_min, float x_max, float y_max, float z_max)
        {
            return new Vector3(NextFloat(x_min, x_max), NextFloat(y_min, y_max), NextFloat(z_min, z_max));
        }

        /// <summary>
        /// Generate <see cref="Vector3"/> object within a set boundary.
        /// </summary>
        /// <param name="a">Vector containing minimum values.</param>
        /// <param name="b">Vector containing maximum values.</param>
        /// <returns>Returns a <see cref="Vector3"/> object.</returns>
        public static Vector3 NextVector3(Vector3 a, Vector3 b)
        {
            return new Vector3(NextFloat(a.x, b.x), NextFloat(a.y, b.y), NextFloat(a.z, b.z));
        }

        #endregion

        #region // Comparator Functions.

        /////////////////////
        // Comparator Functions.
        /////////////////////
        
        /// <summary>
        /// Checks two vectors for being anti-parallel.
        /// </summary>
        /// <param name="a">Leftside vector.</param>
        /// <param name="b">Rightside vector.</param>
        /// <param name="leeway">Amount of which the dot product can deviate from -1.0f, and still return true. Can be between -1.0f and 1.0f.</param>
        /// <returns>Returns true if the dot product is equal to zero.</returns>
        public static bool AntiParallel(Vector2 a, Vector2 b, float leeway = 0.0f)
        {
            float dotProduct = Dot(a, b);
            float expected = -1.0f;

            if (leeway != 0.0f)
            {
                float lower = Clamp(expected - (leeway / 2.0f), -1.0f, 1.0f);
                float upper = Clamp(expected + (leeway / 2.0f), -1.0f, 1.0f);
                return InRange(dotProduct, lower, upper);
            }
            else
            {
                return (dotProduct == expected);
            }
        }

        /// <summary>
        /// Checks two vectors for being anti-parallel.
        /// </summary>
        /// <param name="a">Leftside vector.</param>
        /// <param name="b">Rightside vector.</param>
        /// <param name="leeway">Amount of which the dot product can deviate from -1.0f, and still return true. Can be between -1.0f and 1.0f.</param>
        /// <returns>Returns true if the dot product is equal to zero.</returns>
        public static bool AntiParallel(Vector3 a, Vector3 b, float leeway = 0.0f)
        {
            float dotProduct = Dot(a, b);
            float expected = -1.0f;

            if (leeway != 0.0f)
            {
                float lower = Clamp(expected - (leeway / 2.0f), -1.0f, 1.0f);
                float upper = Clamp(expected + (leeway / 2.0f), -1.0f, 1.0f);
                return InRange(dotProduct, lower, upper);
            }
            else
            {
                return (dotProduct == expected);
            }
        }

        /// <summary>
        /// Checks two vectors for being parallel.
        /// </summary>
        /// <param name="a">Leftside vector.</param>
        /// <param name="b">Rightside vector.</param>
        /// <param name="leeway">Amount of which the dot product can deviate from 1.0f, and still return true. Can be between -1.0f and 1.0f.</param>
        /// <returns>Returns true if the dot product is equal to zero.</returns>
        public static bool Parallel(Vector2 a, Vector2 b, float leeway = 0.0f)
        {
            float dotProduct = Dot(a, b);
            float expected = 1.0f;
            
            if (leeway != 0.0f)
            {
                float lower = Clamp(expected - (leeway / 2.0f), -1.0f, 1.0f);
                float upper = Clamp(expected + (leeway / 2.0f), -1.0f, 1.0f);
                return InRange(dotProduct, lower, upper);
            }
            else
            {
                return (dotProduct == expected);
            }
        }

        /// <summary>
        /// Checks two vectors for being parallel.
        /// </summary>
        /// <param name="a">Leftside vector.</param>
        /// <param name="b">Rightside vector.</param>
        /// <param name="leeway">Amount of which the dot product can deviate from 1.0f, and still return true. Can be between -1.0f and 1.0f.</param>
        /// <returns>Returns true if the dot product is equal to zero.</returns>
        public static bool Parallel(Vector3 a, Vector3 b, float leeway = 0.0f)
        {
            float dotProduct = Dot(a, b);
            float expected = 1.0f;

            if (leeway != 0.0f)
            {
                float lower = Clamp(expected - (leeway / 2.0f), -1.0f, 1.0f);
                float upper = Clamp(expected + (leeway / 2.0f), -1.0f, 1.0f);
                return InRange(dotProduct, lower, upper);
            }
            else
            {
                return (dotProduct == expected);
            }
        }

        /// <summary>
        /// Checks two vectors for perpendicularity.
        /// </summary>
        /// <param name="a">Leftside vector.</param>
        /// <param name="b">Rightside vector.</param>
        /// <param name="leeway">Amount of which the dot product can deviate from 0.0f, and still return true. Can be between -1.0f and 1.0f.</param>
        /// <returns>Returns true if the dot product is equal to zero.</returns>
        public static bool Perpendicular(Vector2 a, Vector2 b, float leeway = 0.0f)
        {
            float dotProduct = Dot(a, b);
            float expected = 0.0f;

            if (leeway != 0.0f)
            {
                float lower = Clamp(expected - (leeway / 2.0f), -1.0f, 1.0f);
                float upper = Clamp(expected + (leeway / 2.0f), -1.0f, 1.0f);
                return InRange(dotProduct, lower, upper);
            }
            else
            {
                return (dotProduct == expected);
            }
        }
        
        /// <summary>
        /// Checks two vectors for perpendicularity.
        /// </summary>
        /// <param name="a">First vector; relative to the dot operation it is the informal "forward" vector.</param>
        /// <param name="b">Dot vector.</param>
        /// <param name="leeway">Amount of which the dot product can deviate from 0.0f, and still return true. Can be between -1.0f and 1.0f.</param>
        /// <returns>Returns true if the dot product is equal to zero.</returns>
        public static bool Perpendicular(Vector3 a, Vector3 b, float leeway = 0.0f)
        {
            float dotProduct = Dot(a, b);
            float expected = 0.0f;

            if (leeway != 0.0f)
            {
                float lower = Clamp(expected - (leeway / 2.0f), -1.0f, 1.0f);
                float upper = Clamp(expected + (leeway / 2.0f), -1.0f, 1.0f);
                return InRange(dotProduct, lower, upper);
            }
            else
            {
                return (dotProduct == expected);
            }
        }

        /// <summary>
        /// Determine if a value is within an inclusive range.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="min">Lower bound of range.</param>
        /// <param name="max">Upper bound of range.</param>
        /// <returns>Returns true if values are within range.</returns>
        public static bool InRange(int value, int min, int max)
        {
            // Swap range values if need be.
            if (min > max)
            {
                int temp = max;
                max = min;
                min = temp;
            }

            // Determines if a value is in range.
            // float testA = Max(value, max);
            // float testB = Min(value, min);

            return (Max(value, max) == max && Min(value, min) == min);
        }

        /// <summary>
        /// Determine if a value is within an inclusive range.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="min">Lower bound of range.</param>
        /// <param name="max">Upper bound of range.</param>
        /// <returns>Returns true if values are within the range.</returns>
        public static bool InRange(float value, float min, float max)
        {
            // Swap range values if need be.
            if (min > max)
            {
                float temp = max;
                max = min;
                min = temp;
            }

            // Determines if a value is in range.
            // float testA = Max(value, max);
            // float testB = Min(value, min);

            return (Max(value, max) == max && Min(value, min) == min);
        }

        /// <summary>
        /// Determine if a value of a <see cref="IComparable{T}"/> generic type is within an inclusive range.
        /// </summary>
        /// <typeparam name="T">Comparable type.</typeparam>
        /// <param name="value">Value to check.</param>
        /// <param name="min">Lower bound of range.</param>
        /// <param name="max">Upper bound of range.</param>
        /// <returns>Returns true if values are within the range.</returns>
        public static bool InRange<T>(T value, T min, T max) where T : IComparable<T>
        {
            // Swap range values if need be.
            if (min.CompareTo(max) > 0)
            {
                T temp = max;
                max = min;
                min = temp;
            }

            return (Max<T>(value, max).CompareTo(max) == 0 && Min<T>(value, min).CompareTo(min) == 0);
        }

        /// <summary>
        /// Get the absolute value of a term.
        /// </summary>
        /// <param name="value">Value to get the absolute value of.</param>
        /// <returns>Returns absolute value of input.</returns>
        public static float Abs(float value)
        {
            if (value < 0.0f)
            {
                value *= -1.0f;
            }

            return value;
        }

        /// <summary>
        /// Returns the smaller of two values.
        /// </summary>
        /// <param name="a">Value to compare.</param>
        /// <param name="b">Second value to compare.</param>
        /// <returns>Smallest value.</returns>
        public static float Min(float a, float b)
        {
            if (a <= b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        /// <summary>
        /// Compares the magnitudes of two <see cref="Vector2"/> objects, returning the one with the smaller length.
        /// </summary>
        /// <param name="a">First vector to compare.</param>
        /// <param name="b">Second vector to compare.</param>
        /// <returns>Smallest vector.</returns>
        public static Vector2 Min(Vector2 a, Vector2 b)
        {
            if (Magnitude(a) <= Magnitude(b))
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        /// <summary>
        /// Compares the magnitudes of two <see cref="Vector3"/> objects, returning the one with the smaller length.
        /// </summary>
        /// <param name="a">First vector to compare.</param>
        /// <param name="b">Second vector to compare.</param>
        /// <returns>Smallest vector.</returns>
        public static Vector3 Min(Vector3 a, Vector3 b)
        {
            if (Magnitude(a) <= Magnitude(b))
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        /// <summary>
        /// Returns the smallest value in a set of values.
        /// </summary>
        /// <param name="terms">Array of terms to compare.</param>
        /// <returns>Smallest float in set.</returns>
        public static float MinOf(params float[] terms)
        {
            // Check size of array.
            if (terms.Length == 1)
            {
                return terms[0];
            }
            else
            {
                float min = terms[0];

                for (int i = 0; i < terms.Length; i++)
                {
                    if (terms[i] < min)
                    {
                        min = terms[i];
                    }
                }

                return min;
            }
        }

        /// <summary>
        /// Returns the smallest <see cref="Vector2"/> in a set of <see cref="Vector2"/> objects.
        /// </summary>
        /// <param name="terms">Array of vectors to compare.</param>
        /// <returns>Smallest vector in set.</returns>
        public static Vector2 MinOf(params Vector2[] terms)
        {
            // Check size of array.
            if (terms.Length == 1)
            {
                return terms[0];
            }
            else
            {
                int index = 0;
                float min = Magnitude(terms[0]);

                for (int i = 0; i < terms.Length; i++)
                {
                    if (Magnitude(terms[i]) < min)
                    {
                        index = i;
                        min = Magnitude(terms[i]);
                    }
                }

                return terms[index];
            }
        }

        /// <summary>
        /// Returns the smallest <see cref="Vector3"/> in a set of <see cref="Vector3"/> objects.
        /// </summary>
        /// <param name="terms">Array of vectors to compare.</param>
        /// <returns>Smallest vector in set.</returns>
        public static Vector3 MinOf(params Vector3[] terms)
        {
            // Check size of array.
            if (terms.Length == 1)
            {
                return terms[0];
            }
            else
            {
                int index = 0;
                float min = Magnitude(terms[0]);

                for (int i = 0; i < terms.Length; i++)
                {
                    if (Magnitude(terms[i]) < min)
                    {
                        index = i;
                        min = Magnitude(terms[i]);
                    }
                }

                return terms[index];
            }
        }

        /// <summary>
        /// Returns the larger of two values.
        /// </summary>
        /// <param name="a">Value to compare.</param>
        /// <param name="b">Second value to compare.</param>
        /// <returns>Largest value.</returns>
        public static float Max(float a, float b)
        {
            if (a >= b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        /// <summary>
        /// Compares the magnitudes of two <see cref="Vector2"/> objects, returning the one with the larger length.
        /// </summary>
        /// <param name="a">First vector to compare.</param>
        /// <param name="b">Second vector to compare.</param>
        /// <returns>Largest vector.</returns>
        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            if (Magnitude(a) >= Magnitude(b))
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        /// <summary>
        /// Compares the magnitudes of two <see cref="Vector3"/> objects, returning the one with the larger length.
        /// </summary>
        /// <param name="a">First vector to compare.</param>
        /// <param name="b">Second vector to compare.</param>
        /// <returns>Largest vector.</returns>
        public static Vector3 Max(Vector3 a, Vector3 b)
        {
            if (Magnitude(a) >= Magnitude(b))
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        /// <summary>
        /// Returns the largest value in a set of values.
        /// </summary>
        /// <param name="terms">Array of terms to compare.</param>
        /// <returns>Largest term in set.</returns>
        public static float MaxOf(params float[] terms)
        {
            // Check size of array.
            if (terms.Length == 1)
            {
                return terms[0];
            }
            else
            {
                float max = terms[0];

                for (int i = 0; i < terms.Length; i++)
                {
                    if (terms[i] > max)
                    {
                        max = terms[i];
                    }
                }

                return max;
            }
        }

        /// <summary>
        /// Returns the largest <see cref="Vector2"/> in a set of <see cref="Vector2"/> objects.
        /// </summary>
        /// <param name="terms">Array of vectors to compare.</param>
        /// <returns>Largest vector in set.</returns>
        public static Vector2 MaxOf(params Vector2[] terms)
        {
            // Check size of array.
            if (terms.Length == 1)
            {
                return terms[0];
            }
            else
            {
                int index = 0;
                float max = Magnitude(terms[0]);

                for (int i = 0; i < terms.Length; i++)
                {
                    if (Magnitude(terms[i]) > max)
                    {
                        index = i;
                        max = Magnitude(terms[i]);
                    }
                }

                return terms[index];
            }
        }

        /// <summary>
        /// Returns the largest <see cref="Vector3"/> in a set of <see cref="Vector3"/> objects.
        /// </summary>
        /// <param name="terms">Array of vectors to compare.</param>
        /// <returns>Largest vector in set.</returns>
        public static Vector3 MaxOf(params Vector3[] terms)
        {
            // Check size of array.
            if (terms.Length == 1)
            {
                return terms[0];
            }
            else
            {
                int index = 0;
                float max = Magnitude(terms[0]);

                for (int i = 0; i < terms.Length; i++)
                {
                    if (Magnitude(terms[i]) > max)
                    {
                        index = i;
                        max = Magnitude(terms[i]);
                    }
                }

                return terms[index];
            }
        }

        /// <summary>
        /// Returns a value, clamped between two others.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Inclusive minimum value to return.</param>
        /// <param name="max">Inclusive maximum value to return.</param>
        /// <returns>Clamped float value.</returns>
        public static float Clamp(float value, float min, float max)
        {
            // Ensure the max and min values are correct.
            if (max <= min)
            {
                if (max == min)
                {
                    return min;
                }
                else
                {
                    float temp = max;
                    max = min;
                    min = temp;
                }
            }

            // Clamp the actual value.

            // If value is less than minimum, return minimum.
            if (value < min)
            {
                value = min;
            }

            // If value is greater than maximum, return maximum.
            if (value > max)
            {
                value = max;
            }

            return value;
        }

        /// <summary>
        /// Returns a value, clamped between two others.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Inclusive minimum value to return.</param>
        /// <param name="max">Inclusive maximum value to return.</param>
        /// <returns>Clamped int value.</returns>
        public static int Clamp(int value, int min, int max)
        {
            // Ensure the max and min values are correct.
            if (max <= min)
            {
                if (max == min)
                {
                    return min;
                }
                else
                {
                    int temp = max;
                    max = min;
                    min = temp;
                }
            }

            // Clamp the actual value.

            // If value is less than minimum, return minimum.
            if (value < min)
            {
                value = min;
            }

            // If value is greater than maximum, return maximum.
            if (value > max)
            {
                value = max;
            }

            return value;
        }

        /// <summary>
        /// Clamp the magnitude of the <see cref="Vector2"/>, if necessary, and return it.
        /// </summary>
        /// <param name="vector">Vector to clamp.</param>
        /// <param name="magnitude">Value to clamp to.</param>
        /// <returns>Vector to clamp.</returns>
        public static Vector2 ClampMagnitude(Vector2 vector, float magnitude)
        {
            if (magnitude < 0.001f)
            {
                magnitude = 0.001f;
            }

            if (Magnitude(vector) <= magnitude)
            {
                return vector;
            }
            else
            {
                return Normalize(vector) * magnitude;
            }

        }
        
        /// <summary>
        /// Clamp the magnitude of the <see cref="Vector3"/>, if necessary, and return it.
        /// </summary>
        /// <param name="vector">Vector to clamp.</param>
        /// <param name="magnitude">Value to clamp to.</param>
        /// <returns>Vector to clamp.</returns>
        public static Vector3 ClampMagnitude(Vector3 vector, float magnitude)
        {
            if (magnitude < 0.001f)
            {
                magnitude = 0.001f;
            }

            if (Magnitude(vector) <= magnitude)
            {
                return vector;
            }
            else
            {
                return Normalize(vector) * magnitude;
            }

        }

        /// <summary>
        /// Generic function that compares values and chooses the smaller.
        /// </summary>
        /// <typeparam name="T">Any generic type that can be compared.</typeparam>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns smaller of the two terms.</returns>
        public static T Min<T>(T a, T b) where T : IComparable<T>
        {
            if (a.CompareTo(b) <= 0)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        /// <summary>
        /// Determine the smallest element in a collection of generics.
        /// </summary>
        /// <typeparam name="T">Any generic type that can be compared.</typeparam>
        /// <param name="terms">Terms to compare.</param>
        /// <returns>Returns the smallest term.</returns>
        public static T MinOf<T>(params T[] terms) where T : IComparable<T>
        {
            // Check size of array.
            if (terms.Length == 1)
            {
                return terms[0];
            }
            else
            {
                int index = 0;
                T min = terms[0];

                for (int i = 0; i < terms.Length; i++)
                {
                    if (terms[i].CompareTo(min) < 0)
                    {
                        index = i;
                        min = terms[i];
                    }
                }

                return terms[index];
            }
        }

        /// <summary>
        /// Generic function that compares values and chooses the larger.
        /// </summary>
        /// <typeparam name="T">Any generic type that can be compared.</typeparam>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns larger of the two terms.</returns>
        public static T Max<T>(T a, T b) where T : IComparable<T>
        {
            if (a.CompareTo(b) >= 0)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        /// <summary>
        /// Determine the largest element in a collection of generics.
        /// </summary>
        /// <typeparam name="T">Any generic type that can be compared.</typeparam>
        /// <param name="terms">Terms to compare.</param>
        /// <returns>Returns the largest term.</returns>
        public static T MaxOf<T>(params T[] terms) where T : IComparable<T>
        {
            // Check size of array.
            if (terms.Length == 1)
            {
                return terms[0];
            }
            else
            {
                int index = 0;
                T max = terms[0];

                for (int i = 0; i < terms.Length; i++)
                {
                    if (terms[i].CompareTo(max) > 0)
                    {
                        index = i;
                        max = terms[i];
                    }
                }

                return terms[index];
            }
        }

        /// <summary>
        /// Generic function that compares values and clamps them.
        /// </summary>
        /// <typeparam name="T">Any generic type representing a value that can be clamped.</typeparam>
        /// <param name="value">Value to clamp.</param>
        /// <param name="minimum">Minimum value to clamp to.</param>
        /// <param name="maximum">Maximum value to clamp to.</param>
        /// <returns>Returns clamped value.</returns>
        public static T Clamp<T>(T value, T minimum, T maximum) where T : IComparable<T>
        {
            if (minimum.CompareTo(maximum) >= 1)
            {
                T swap = maximum;
                maximum = minimum;
                minimum = swap;
            }

            if (minimum.CompareTo(maximum) == 0)
            {
                return minimum;
            }

            T temp = value;
            temp = Max<T>(temp, minimum);
            temp = Min<T>(temp, maximum);

            return temp;
        }

        #endregion

        #endregion

    }

}
