/************************************************
 * HealthTracker.cs
 * 
 * This file contains implementation for the HealthTracker class
 * and it's accompanying factory.
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
    // Factory declaration.
    /////////////////////

    /// <summary>
    /// The HealthTrackerFactory builds components for those that request them.
    /// </summary>
    public static class HealthTrackerFactory {

        #region Static Constructor 

        /// <summary>
        /// Creates a <see cref="HealthTracker"/> component.
        /// </summary>
        /// <param name="parent">GameObject to which the component is being added.</param>
        /// <param name="minHealth">Minimum health.</param>
        /// <param name="maxHealth">Max health.</param>
        /// <returns>Return a HealthTracker component reference.</returns>
        public static HealthTracker CreateComponent(GameObject parent, float minHealth = 0.0f, float maxHealth = Constants.DEFAULT_MAX_HEALTH)
        {
            // Create and add the instance to the calling game object.
            HealthTracker ht = parent.AddComponent<HealthTracker>();

            // Initlaize the added component with default values.
            ht.Initialize();

            // Set values.
            ht.SetMinimum(minHealth);
            ht.SetMaximum(maxHealth);
            ht.SetHealth(maxHealth);

            // Return our constructed object back to the calling game object for keeping reference.
            return ht;
        }

        #endregion

    }
    
    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// The HealthTracker component keeps track of Entity life and health.
    /// </summary>
    public class HealthTracker : MonoBehaviour
    {
        #region Data Members

        /////////////////////
        // Attributes.
        /////////////////////

        /// <summary>
        /// When set to false, health decreases when damage occurs.
        /// When set to true, health also decreases when time passes.
        /// </summary>
        private bool m_timeBased;

        /// <summary>
        /// When <see cref="m_timeBased"/> is true, health decreases by this step * rate of decay.
        /// </summary>
        private float m_decayStep;

        /// <summary>
        /// When <see cref="m_timeBased"/> is true, tracks how much time has passed since last frame.
        /// </summary>
        private float m_decayCurrent;

        /// <summary>
        /// When <see cref="m_timeBased"/> is true, health decreases by <see cref="m_decayStep"/> every <see cref="m_rateOfDecay"/> seconds.
        /// </summary>
        private float m_rateOfDecay; // in seconds.

        /// <summary>
        /// When damaged, this counter is set to the period value. One frame depletes the invulnerability frame counter. Cannot take damage when not equal to zero.
        /// </summary>
        private float m_invulnerabilityFrames; // in frames. Should start at zero.

        /// <summary>
        /// Amount of frames after taking damage object will be invulnerable.
        /// </summary>
        private float m_invulnerabilityPeriod; // in frames.

        /// <summary>
        /// Total amount of damage that has occured since last frame.
        /// </summary>
        private float m_damageSinceLastFrame;

        /// <summary>
        /// Maximum amount of damage that can be received in a frame. Ignore when equal to -1.0f.
        /// </summary>
        private float m_maximumDamagePerFrame;

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// The current health value associated with this object.
        /// </summary>
        public float Health
        {
            get;
            private set;
        }

        /// <summary>
        /// The minimum health this value can hold.
        /// </summary>
        public float MinimumHealth
        {
            get;
            private set;
        }

        /// <summary>
        /// The maximum health this value can hold.
        /// </summary>
        public float MaximumHealth
        {
            get;
            private set;
        }

        #endregion

        #region Service Methods

        /// <summary>
        /// Initialize the class with defaults.
        /// </summary>
        internal void Initialize()
        {
            // Attributes.
            this.m_timeBased = Constants.DEFAULT_TIMEBASED_HEALTH; // false
            this.m_decayStep = Constants.DEFAULT_DECAY_STEP; // 1.0f
            this.m_rateOfDecay = Constants.DEFAULT_DECAY_RATE; // in seconds. 1.0f
            this.m_invulnerabilityPeriod = Constants.DEFAULT_INVULNERABILITY_PERIOD; // in frames. 10.0f
            this.m_maximumDamagePerFrame = Constants.DEFAULT_DAMAGE_LIMIT; // -1.0f

            this.m_damageSinceLastFrame = 0.0f;
            this.m_decayCurrent = 0.0f;
            this.m_invulnerabilityFrames = 0.0f;

            // Properties.
            this.SetMaximum(Constants.DEFAULT_MAX_HEALTH);
            this.SetMinimum(Constants.DEFAULT_MIN_HEALTH);
            this.SetHealth(Constants.DEFAULT_MAX_HEALTH);
        }

        /// <summary>
        /// Start method gifted by the UnityEngine.
        /// </summary>
        void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Update the damage, decay, and invulnerability frames.
        /// </summary>
        void Update()
        {
            // Update damage collected over a frame.
            this.UpdateDamage();

            // Update decay value if health is based on time.
            this.UpdateHealthByDecay();

            // Update invulnerability frames.
            this.UpdateInvulnerabilityFrames();
        }

        /// <summary>
        /// Update damage based on damage since the last frame.
        /// </summary>
        private void UpdateDamage()
        {
            // Check if there is a limiter on damage per frame.
            if (HasDamageLimit())
            {
                // Clamp damage if there is a damage limit.
                this.m_damageSinceLastFrame = Services.Clamp(this.m_damageSinceLastFrame, 0.0f, this.m_maximumDamagePerFrame);
            }            
            
            // Add the damage.
            this.Damage(this.m_damageSinceLastFrame);

            // Reset damage since last frame.
            this.m_damageSinceLastFrame = 0.0f;
        }

        /// <summary>
        /// Update health in case time based decay is active.
        /// </summary>
        private void UpdateHealthByDecay()
        {
            // If decay occurs based upon time decay.
            if (this.m_timeBased && !IsInvulnerable())
            {
                // Get the amount of seconds that have passed since last frame.
                this.m_decayCurrent += Time.deltaTime;

                // Check if decay time meets the threshold for damage.
                if (this.m_decayCurrent >= this.m_rateOfDecay)
                {

                    // Cause damage based on the amount set to occur every time it should happen.
                    Damage(this.m_decayStep);

                    // Reset decay count.
                    this.m_decayCurrent = 0.0f;

                }
            }
        }

        /// <summary>
        /// Update the invulnerability frames.
        /// </summary>
        private void UpdateInvulnerabilityFrames()
        {
            // If invulernable and not permanently invulnerable.
            if (IsInvulnerable() && (this.m_invulnerabilityFrames != Constants.INVULNERABLE_MODE))
            {
                // Decrement by one frame.
                this.DecrementInvulnerabilityCounter(1);
            }
        }

        /// <summary>
        /// Add absolute value of input value to current health.
        /// </summary>
        /// <param name="value">Value to add to health.</param>
        public void Heal(float value)
        {
            this.SetHealth(Services.Abs(value));
        }

        /// <summary>
        /// Subtract absolute value of input value from current health.
        /// </summary>
        /// <param name="value">Value to subtract from health.</param>
        private void Damage(float value)
        {
            if (this.IsVulnerable())
            {
                this.SetHealth(-Services.Abs(value));
                this.TriggerInvulnerability();
            }
        }
        
        /// <summary>
        /// Add damage to take place during the next update.
        /// </summary>
        /// <param name="value"></param>        
        public void Hit(float value)
        {
            this.m_damageSinceLastFrame += value;
        }

        /// <summary>
        /// Make Entity constantly invulnerable.
        /// </summary>
        public void MakeInvulernable()
        {
            this.m_invulnerabilityFrames = Constants.INVULNERABLE_MODE;
        }

        /// <summary>
        /// Make Entity immediately vulnerable.
        /// </summary>
        public void MakeAlwaysVulnerable()
        {
            this.m_invulnerabilityPeriod = 0.0f;
        }

        /// <summary>
        /// Trigger invulnerability upon damage.
        /// </summary>
        public void TriggerInvulnerability()
        {
            this.m_invulnerabilityFrames = this.m_invulnerabilityPeriod;
        }

        /// <summary>
        /// Set invulnerability frame counter back to zero.
        /// </summary>
        public void CancelInvulnerability()
        {
            this.m_invulnerabilityFrames = 0.0f;
        }

        #endregion

        #region Accessor Methods

        /// <summary>
        /// Returns true if the damage limit is -1.
        /// </summary>
        /// <returns>Does the Entity have a damage per frame limit?</returns>
        public bool HasDamageLimit()
        {
            return (this.m_maximumDamagePerFrame != Constants.IGNORE_DAMAGE_LIMIT);
        }

        /// <summary>
        /// Determines if the entity is still alive. 
        /// </summary>
        /// <returns>Returns boolean indicating status.</returns>
        public bool IsAlive()
        {
            return (this.Health > this.MinimumHealth);
        }

        /// <summary>
        /// Determines if the entity has been damaged.
        /// </summary>
        /// <returns>Returns boolean indicating status.</returns>
        public bool IsDamaged()
        {
            return (this.Health < this.MaximumHealth);
        }

        /// <summary>
        /// Is the Entity on an invulnerability frame / invulnerable?
        /// </summary>
        /// <returns>Returns boolean indicating status.</returns>
        public bool IsInvulnerable()
        {
            return (this.m_invulnerabilityFrames > 0.0f || this.m_invulnerabilityFrames == Constants.INVULNERABLE_MODE);
        }

        /// <summary>
        /// Is the Entity vulnerable?
        /// </summary>
        /// <returns></returns>
        public bool IsVulnerable()
        {
            return (this.m_invulnerabilityFrames == 0.0f);
        }

        #endregion

        #region Mutator Methods

        /// <summary>
        /// Decrease the invulnerability counter by input amount.
        /// </summary>
        /// <param name="value">Value to decrement counter by.</param>
        private void DecrementInvulnerabilityCounter(float value)
        {
            if (this.m_invulnerabilityFrames != Constants.INVULNERABLE_MODE && this.m_invulnerabilityFrames > 0.0f) {
                float result = this.m_invulnerabilityFrames - Services.Abs(value);
                this.m_invulnerabilityFrames = Services.Max(result, 0.0f);
            }
        }

        /// <summary>
        /// Set the damage limit.
        /// </summary>
        /// <param name="limit">Limit damage by ths amount per frame.</param>
        public void SetDamageLimit(float limit)
        {
            if (limit == Constants.IGNORE_DAMAGE_LIMIT)
            {
                this.m_maximumDamagePerFrame = limit;
            }
            else
            {
                this.m_maximumDamagePerFrame = Services.Max(limit, 0.1f);
            }
        }

        /// <summary>
        /// Set the invulnerability period.
        /// </summary>
        public void SetInvulnerabilityPeriod(float value)
        {
            this.m_invulnerabilityPeriod = Services.Max(value, 0.0f);
        }

        /// <summary>
        /// Set Health to clamped value.
        /// </summary>
        /// <param name="value">New health value.</param>
        internal void SetHealth(float value)
        {
            this.Health = Services.Clamp(value, this.MinimumHealth, this.MaximumHealth);
        }

        /// <summary>
        /// Set maximum health.
        /// </summary>
        /// <param name="value">New health value.</param>
        internal void SetMaximum(float value)
        {
            float upper = Services.Abs(value);

            if (upper <= this.MinimumHealth)
            {
                this.MaximumHealth = this.MinimumHealth + 0.1f; // Can't have them be the same. 
            }
            else
            {
                this.MaximumHealth = upper;
            }

            SetHealth(this.Health); // Re-clamp values.
        }

        /// <summary>
        /// Set maximum health.
        /// </summary>
        /// <param name="value">New health value.</param>
        internal void SetMinimum(float value)
        {
            float lower = Services.Abs(value);

            if (lower >= this.MaximumHealth)
            {
                this.MinimumHealth = this.MaximumHealth - 0.1f; // Can't have them be the same. 
            }
            else
            {
                this.MinimumHealth = lower;
            }

            SetHealth(this.Health); // Re-clamp values.
        }
        
        #endregion



    }
}
