/************************************************
 * DecayTracker.cs
 * 
 * This file contains:
 * - The DecayTracker generic class. (Child of ArcanaObject).
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Utilities
{

    /// <summary>
    /// DecayTracker is a generic component that inherits ArcanaObject and can only operate on IComparable objects.
    /// </summary>
    [AddComponentMenu("Arcana/Utilities/Decay Tracker")] // Limits DecayTracker to only numeric value types.
    public class DecayTracker : ArcanaObject
    {

        #region Data Members.

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Represents the total time (in seconds) passed since the start of the decay tracker.
        /// </summary>
        private float m_time;
        
        /// <summary>
        /// Length of interpolation.
        /// </summary>
        private float m_length;

        /// <summary>
        /// Calculated percentage, from 0 to 1.
        /// </summary>
        private float m_percentage;

        /// <summary>
        /// Represents the speed of decay (in units per second).
        /// </summary>
        private float m_lerpSpeed;

        /// <summary>
        /// Start value of the decay tracker.
        /// </summary>
        private float m_startValue;

        /// <summary>
        /// End value of the decay tracker.
        /// </summary>
        private float m_endValue;

        /// <summary>
        /// Value calculated as a result of lerping.
        /// </summary>
        private float m_middleValue;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns the currently stored calculated value.
        /// </summary>
        public float Value
        {
            get { return this.m_middleValue; }
        }

        /// <summary>
        /// Left hand value used in lerp.
        /// </summary>
        public float StartValue
        {
            get { return this.m_startValue; }
            set { SetStart(value); }
        }

        /// <summary>
        /// Right hand value used in lerp.
        /// </summary>
        public float EndValue
        {
            get { return this.m_endValue; }
            set { SetEnd(value); }
        }

        /// <summary>
        /// Decay speed represented in units per second.
        /// </summary>
        public float Speed
        {
            get { return this.m_lerpSpeed; }
        }

        /// <summary>
        /// Return the change in delta.
        /// </summary>
        public float Delta
        {
            get { return Time.deltaTime; }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Initialize and restart the DecayTracker.
        /// </summary>
        public override void Start()
        {

            // Initialize the component.
            base.Start();

            // Restart the timer.
            this.Restart();

            // Start the tracker.
            this.StartTimer();

        }

        /// <summary>
        /// Update the decay tracker.
        /// </summary>
        public override void Update()
        {
            // Destroy the tracker if set to be destroyed.
            base.Update();

            // If the timer is running.
            if (this.Status.IsRunning())
            {
                // If the tracker is not paused.
                if (!this.Status.IsPaused())
                {
                    // If the tracker hasn't completed its run.
                    if (!Completed())
                    {
                        // Update the lerp percentage and calculated value.
                        this.UpdateValue();
                    }
                }
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Set the default values for the decay tracker.
        /// </summary>
        public override void Initialize()
        {
            // Initialize the base.
            base.Initialize();

            // Set running.
            this.Status.Run();

            // Set default values.
            this.SetSpeed(1.0f); // 1 unit/second.

            // Set the default length of decay.
            this.SetLength(10.0f);

            // Decay tracker is not a cloneable object.
            this.SetCopyable(false);

            // Initialize values.
            ResetTimer();            
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Update the value by calculating result of the lerp.
        /// </summary>
        private void UpdateValue()
        {
            UpdatePercentage();
            this.m_middleValue = CalculateValue();
        }

        /// <summary>
        /// Update the percentage.
        /// </summary>
        private void UpdatePercentage()
        {

            // Add the current passage of delta time.
            this.m_time += this.Delta;

            // Speed of decay.
            float progress = this.m_time * this.m_lerpSpeed;

            // Map the value to percentages.
            this.m_percentage = progress.MapValue(0.0f, this.m_length, 0.0f, 1.0f);

        }

        /// <summary>
        /// Reset the tracker.
        /// </summary>
        public void Restart()
        {
            // Reset the timer.
            this.ResetTimer();

            // Reset the start value.
            this.m_middleValue = this.StartValue;
        }

        /// <summary>
        /// Reverse the start and end points, while continuing calculations.
        /// </summary>
        public void Reverse()
        {
            float temp = this.StartValue;
            this.m_startValue = this.m_endValue;
            this.m_endValue = temp;
        }

        /// <summary>
        /// Reverse the start and end points and restart the tracker.
        /// </summary>
        public void Swap()
        {
            this.Reverse();
            this.Restart();
        }

        /// <summary>
        /// Reset the timer to start from zero.
        /// </summary>
        public void ResetTimer()
        {
            this.m_time = 0.0f;
            this.m_percentage = 0.0f;
        }

        /// <summary>
        /// Stop the timer and skip to the end value.
        /// </summary>
        public void Stop()
        {
            this.m_middleValue = this.m_endValue;
            this.Status.Stop();
        }

        /// <summary>
        /// Start the tracker.
        /// </summary>
        public void StartTimer()
        {
            this.Status.Run();
        }

        #endregion

        #region Accessor Methods

        /// <summary>
        /// Returns true if the interpolation has been completed.
        /// </summary>
        /// <returns>Returns true on completion.</returns>
        public bool Completed()
        {
            return (this.m_middleValue == this.m_endValue);
        }


        /// <summary>
        /// Return the calculated value.
        /// </summary>
        /// <returns>Returns calculated value from lerp.</returns>
        public float CalculateValue()
        {
            return Mathf.Lerp(this.m_startValue, this.m_endValue, this.m_percentage);
        }

        #endregion

        #region Mutator Methods

        /// <summary>
        /// Set the lefthand side value for lerping.
        /// </summary>
        /// <param name="_value">Start value.</param>
        public void SetStart(float _value)
        {
            this.m_startValue = _value;
        }

        /// <summary>
        /// Set the righthand side value for lerping.
        /// </summary>
        /// <param name="_value">End value.</param>
        public void SetEnd(float _value)
        {
            this.m_endValue = _value;
        }

        /// <summary>
        /// Set the number of seconds it will take 1 unit to pass.
        /// </summary>
        /// <param name="_seconds">Speed is set to unit per this number of seconds.</param>
        public void SetSpeedAsSeconds(float _seconds)
        {
            // speed = 1 unit / _seconds seconds.
            this.SetSpeed(1 / _seconds);
        }

        /// <summary>
        /// Set the unit/second speed of decay.
        /// </summary>
        /// <param name="_unitsPerSecond">Speed of decay.</param>
        public void SetSpeed(float _unitsPerSecond)
        {
            // Decay speed should always be positive and greater than zero.
            this.m_lerpSpeed = Services.Max(Services.Abs(_unitsPerSecond), 0.05f);
        }

        /// <summary>
        /// Set the length of the decay.
        /// </summary>
        /// <param name="_length">Time period (in seconds) decay should occur over.</param>
        public void SetLength(float _length)
        {
            this.m_length = _length;
        }

        #endregion
        
    }
}
