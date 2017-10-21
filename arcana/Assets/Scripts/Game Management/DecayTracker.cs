using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcana
{

    /// <summary>
    /// General data structure that will track a decaying value over time. Generally classed to use different iterable types.
    /// </summary>
    public class DecayTracker<T> where T : IComparable<T>
    {
        /// <summary>
        /// The minimum value is considered the lower bound of the system.
        /// </summary>
        private T minimumValue;

        /// <summary>
        /// The current value.
        /// </summary>
        private T currentValue;

        /// <summary>
        /// Value removed from the current value every loop.
        /// </summary>
        private T decayFactor;

        /// <summary>
        /// The value from which decay starts.
        /// </summary>
        private T maximumValue;

        /// <summary>
        /// Current amount of time stored since last cycle was passed.
        /// </summary>
        private float timeStep;

        /// <summary>
        /// Amount of time that must be passed before decay can be triggered. Set to 0.0f, to always trigger decay on call.
        /// </summary>
        private float timeCyclePeriod;

        /// <summary>
        /// Pauses the tracker.
        /// </summary>
        private bool pause;
        
        /// <summary>
        /// Returns the current tracked value.
        /// </summary>
        public T CurrentValue
        {
            get { return this.currentValue; }
        }

        /// <summary>
        /// Return the maximum (or upper bound) set for this system.
        /// </summary>
        public T Minimum
        {
            get { return this.minimumValue; }
        }

        /// <summary>
        /// Return the maximum (or upper bound) set for this system.
        /// </summary>
        public T Maximum
        {
            get { return this.maximumValue; }
        }

        /// <summary>
        /// Determine if the decay tracker has reached the decayed state.
        /// </summary>
        public bool Decayed
        {
            get
            {
                return Services.InRange(this.CurrentValue, this.minimumValue, this.maximumValue);
            }
        }

        /// <summary>
        /// Returns flag value for <see cref="pause"/>.
        /// </summary>
        public bool IsPaused
        {
            get { return this.pause; }
        }

        /// <summary>
        /// Create a decay tracker utilizing the proper type.
        /// </summary>
        /// <param name="_min">Minimum value of the system.</param>
        /// <param name="_max">Maximum value of the system.</param>
        /// <param name="_decay">Decay factor affecting the system.</param>
        public DecayTracker(T _min, T _max, T _decay, float _cycle = 0.0f)
        {
            this.minimumValue = _min;
            this.maximumValue = _max;
            this.decayFactor = _decay;
            this.pause = true;

            this.timeStep = 0.0f;
            this.timeCyclePeriod = _cycle; // By default, this means, always trigger the decay.

            SetCurrentValue(this.minimumValue);
        }

        /// <summary>
        /// An update call will decay value if not paused.
        /// </summary>
        public void Update()
        {
            if (!IsPaused)
            {
                this.Decay();
            }
        }

        /// <summary>
        /// An update value, factoring time, will run if not paused.
        /// </summary>
        /// <param name="deltaTime">Time (in seconds) since last frame.</param>
        public void Update(float deltaTime)
        {
            if (!IsPaused)
            {
                this.TimeDecay(deltaTime);
            }
        }

        /// <summary>
        /// Pause the tracker.
        /// </summary>
        public void Pause()
        {
            this.pause = true;
        }

        /// <summary>
        /// Resume the tracker.
        /// </summary>
        public void Resume()
        {
            this.pause = false;
        }

        /// <summary>
        /// Stops the tracker and resets it to the minimum value.
        /// </summary>
        public void Reset()
        {
            SetCurrentValue(this.minimumValue);
            this.Pause();
        }

        /// <summary>
        /// Starts the tracker after setting current value to maximum.
        /// </summary>
        public void Start()
        {
            SetCurrentValue(this.maximumValue);
            Resume();
        }
        
        /// <summary>
        /// Readies the timer, but, immediately pauses it.
        /// </summary>
        public void Prime()
        {
            Start();
            Pause();
        }

        /// <summary>
        /// Reduce value by the flat decay factor.
        /// </summary>
        public void Decay()
        {
            try
            {
                // Since Unity doesn't like the .NET 4 implementation of dynamic, use this object->float->T casting workaround.
                SetCurrentValue((T)(object)((float)(object)this.currentValue * (float)(object)this.decayFactor));
            }
            catch (Exception e)
            {
                Debugger.Print("Can't decay this generic type. " + e.StackTrace, "Error.");
            }
        }

        /// <summary>
        /// Reduce value, based on the amount of seconds that have passed.
        /// </summary>
        /// <param name="deltaTime"></param>
        private void TimeDecay(float deltaTime)
        {
            if (timeStep >= timeCyclePeriod)
            {
                timeStep = 0.0f;
                try
                {
                    SetCurrentValue((T)(object)((float)(object)this.currentValue * (deltaTime * (float)(object)this.decayFactor)));
                }
                catch (Exception e)
                {
                    Debugger.Print("Can't decay this generic type. " + e.StackTrace, "Error.");
                }
            }

            timeStep += deltaTime;
        }

        /// <summary>
        /// Decrease current value by a custom amount.
        /// </summary>
        /// <param name="value">Amount to decrease current value by.</param>
        public void Deplete(T value)
        {
            try
            {
                SetCurrentValue((T)(object)((float)(object)this.currentValue - (float)(object)value));
            }
                catch (Exception e)
            {
                Debugger.Print("Can't decay this generic type. " + e.StackTrace, "Error.");
            }
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        /// <param name="value">Value being set.</param>
        public void SetCurrentValue(T value)
        {
            this.currentValue = Services.Clamp<T>(value, this.minimumValue, this.maximumValue);
        }

    }
}
