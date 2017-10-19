/************************************************
 * Services.cs
 * 
 * Services contain references to program-wide constants and has helper functions that can be called.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana
{
    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// Services offers program-wide reference to constants and helper functions.
    /// </summary>
    public static class Constants
    {

        #region Constants.

        /////////////////////
        // Constants.
        /////////////////////

        #region Vector References
        
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

        #region Entity Constants

        /// <summary>
        /// Default width and height to use when it isn't specified.
        /// </summary>
        public const float DEFAULT_DIMENSION = 100.0f; // in pixels.

        /// <summary>
        /// Default health value to assign when it isn't specified.
        /// </summary>
        public const int DEFAULT_HEALTH = -1; // When less than zero, we don't consider it to have a health state.

        #endregion

        #endregion

    }

    public static class Services
    {

        #region Math Functions.
        
        #region // Vector Math Functions.

        /////////////////////
        // Vector Math Functions.
        /////////////////////

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

        #endregion

        #endregion

    }

}
