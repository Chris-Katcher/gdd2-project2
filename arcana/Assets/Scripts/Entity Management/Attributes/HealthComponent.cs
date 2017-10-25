/************************************************
 * HealthComponent.cs
 * 
 * This file contains:
 * - The HealthComponent class. (Child of ArcanaObject).
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Utilities;

namespace Arcana.Entities.Attributes
{

    #region // Class: HealthTracker class.

    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// The HealthComponent keeps track of an ArcanaObject's life (or lifetime).
    /// </summary>
    [AddComponentMenu("Arcana/Attributes/HealthComponent")]
    public class HealthComponent : ArcanaObject
    {

        #region Data Members.

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Enum for tracking health component modes.
        /// </summary>
        private enum HealthMode
        {
            /// <summary>
            /// Cannot lose 'health' when invincible.
            /// </summary>
            Invincible,

            /// <summary>
            /// Can be 'damaged' by other elements.
            /// </summary>
            Vulnerable,   

            /// <summary>
            /// 'Damage' health by a rate of 1 unit per second.
            /// </summary>
            Decay
                
        }

        /// <summary>
        /// Tracker that ensures we only decrement the invincibility once per cycle.
        /// </summary>
        private bool m_calledThisFrame = false;

        /// <summary>
        /// Value that stores all the accumulated damage before applying it on the next frame.
        /// </summary>
        private float m_damageSinceLastFrame;

        /// <summary>
        /// Value that stores all the accumulated health recovered before applying it on the next frame.
        /// </summary>
        private float m_healthSinceLastFrame;

        /// <summary>
        /// Rate of decay for appropriate time-based damage mode.
        /// </summary>
        private float m_decayRate;

        /// <summary>
        /// Keeps track of invincibility frames, if any, on every update.
        /// </summary>
        private int m_invincibilityFrames;

        /// <summary>
        /// Data structure for tracking for our health values.
        /// </summary>
        private StatTracker m_health;

        /// <summary>
        /// Current modes attributed to this component.
        /// </summary>
        private List<HealthMode> m_modes;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Return modes currently attached to this object.
        /// </summary>
        private List<HealthMode> Modes
        {
            get {
                if (this.m_modes == null)
                {
                    this.m_modes = new List<HealthMode>();
                }

                return this.m_modes;
            }
        }

        /// <summary>
        /// Reference to object invincibility.
        /// </summary>
        public bool Invincible
        {
            get { return IsInvincible(); }
            set
            {
                if (value)
                {
                    AddMode(HealthMode.Invincible);
                }
                else
                {
                    RemoveMode(HealthMode.Invincible);
                }
            }
        }

        /// <summary>
        /// Reference to object vulnerability.
        /// </summary>
        public bool Vulnerable
        {
            get { return IsVulnerable(); }
            set
            {
                if (value)
                {
                    AddMode(HealthMode.Vulnerable);
                }
                else
                {
                    RemoveMode(HealthMode.Vulnerable);
                }
            }
        }
        
        /// <summary>
        /// Reference to object decay values.
        /// </summary>
        public bool Decaying
        {
            get { return IsVulnerable(); }
            set
            {
                if (value)
                {
                    AddMode(HealthMode.Decay);
                }
                else
                {
                    RemoveMode(HealthMode.Decay);
                }
            }
        }

        /// <summary>
        /// Set the decay rate for the component. (Value must be non-zero and positive).
        /// </summary>
        public float DecaySpeed
        {
            get { return this.m_decayRate; }
            set { this.m_decayRate = Services.Max(value, 0.001f); }
        }
        
        /// <summary>
        /// Return the current health value.
        /// </summary>
        public float CurrentHealth
        {
            get { return this.m_health.Value; }
            set { this.m_health.SetValue(value); }
        }

        /// <summary>
        /// Return the current health value as an inclusive range value between [0, 1].
        /// </summary>
        public float CurrentHealthPercent
        {
            get { return this.m_health.Percent; }
        }

        /// <summary>
        /// Maximum health.
        /// </summary>
        public float MaxHealth
        {
            get { return this.m_health.Max; }
            set { this.m_health.SetMaximum(value); }
        }

        /// <summary>
        /// Minimum health.
        /// </summary>
        public float MinHealth
        {
            get { return this.m_health.Min; }
            set { this.m_health.SetMinimum(value); }
        }

        /// <summary>
        /// Return true if the component reflects a damaged state.
        /// </summary>
        public bool IsHurt
        {
            get { return this.m_health.IsBetween() || this.m_health.IsMinimum(); }
        }

        /// <summary>
        /// Return true if the component reflects full health.
        /// </summary>
        public bool IsFullHealth
        {
            get { return this.m_health.IsMaximum(); }
        }

        /// <summary>
        /// Return true if the component reflects no health.
        /// </summary>
        public bool IsDead
        {
            get { return this.m_health.IsMinimum(); }
        }

        /// <summary>
        /// Return true if damage was received on last frame.
        /// </summary>
        public bool WasDamaged
        {
            get { return (this.m_damageSinceLastFrame > 0.0f); }
        }

        /// <summary>
        /// Return true if health was recovered on last frame.
        /// </summary>
        public bool WasHealed
        {
            get { return (this.m_healthSinceLastFrame > 0.0f); }
        }

        /// <summary>
        /// Return true if damage was received on last frame.
        /// </summary>
        public bool HasInvincibilityFrames
        {
            get { return (this.m_invincibilityFrames > 0); }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Called to handle physics updates.
        /// </summary>
        public override void Update()
        {
            // Call ArcanaObject update.
            base.Update();

            // Only update the rest while active.
            if (this.Status.IsActive())
            {
                // While not paused.
                if (!this.Status.IsPaused())
                {
                    // Reset this every frame for invincibility.
                    this.m_calledThisFrame = false;

                    // Update invincibility status.
                    UpdateInvincibility();

                    // Update health.
                    UpdateHealth();

                    // Update the component's status.
                    UpdateStatus();
                }
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize HealthComponent data members.
        /// </summary>
        public override void Initialize()
        {
            // Initialize the base.
            base.Initialize();

            // A health component isn't poolable...is it?
            this.IsPoolable = false;

            // Set up default values. (No limit on maximum damage). (100.0f as max health). (1.0f minimum for damage).
            m_health = new StatTracker("Health", 100.0f, 0.0f, 100.0f, 1.0f);

            // Create mode list and add default mode.
            m_modes = new List<HealthMode>();
            m_modes.Add(HealthMode.Vulnerable); // Doesn't decay over time.

            // Set default decay rate to 1 unit per second.
            m_decayRate = 1.0f;

            // Set frame info defaults.
            m_invincibilityFrames = 0;
            m_damageSinceLastFrame = 0.0f;
            m_healthSinceLastFrame = 0.0f;
        }

        /// <summary>
        /// Clone the values from one health component into this one.
        /// </summary>
        /// <param name="_template">HealthComponent to clone.</param>
        /// <returns>Returns this object after taking in values.</returns>
        public override ArcanaObject Clone(ArcanaObject _template)
        {
            if (_template.IsCopyable)
            {
                if (this != _template)
                {
                    if (_template.GetType() == typeof(HealthComponent))
                    {

                        // Base functionality.
                        this.Name = _template.Name + "(Clone)";
                        this.Status.Clone(_template.Status);
                        this.Status.Initialize(_template.Initialized);

                        // Get the cast of the health component.
                        HealthComponent cast = (HealthComponent)_template;

                        // Clone other members.
                        this.m_health.Copy(cast.m_health);
                        this.m_decayRate = cast.m_decayRate;
                        this.m_modes = cast.m_modes;

                    }
                }
            }

            return this;
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Update invincibility frames.
        /// </summary>
        private void UpdateInvincibility()
        {
            // If this has a mode to be damaged, we'll want to remove any invincibility mode if it doesn't have the frames.
            if (this.Vulnerable || this.Decaying)
            {
                // If it doesn't have frames, remove the Invincibility mode.
                if (!HasInvincibilityFrames)
                {
                    this.Invincible = false;
                }
            }

            // Reduce the number of frames by 1, each call.
            // Be careful to only call this once per frame.
            DecrementInvincibilityFrames();
        }

        /// <summary>
        /// Update damage.
        /// </summary>
        private void UpdateHealth()
        {
            // Check if component was healed since last frame.
            if (WasHealed)
            {
                // Heal before applying any damage.
                this.Heal(this.m_healthSinceLastFrame);
            }

            // If invincible, we don't damage the component.
            if (!this.Invincible)
            {
                // We apply our time and event based damages separately, to circumvent any 'damage' limiters that may be on the stat tracker and avoid 'double counting', leading to vanishing damaage impacts.
                if (this.Decaying)
                {
                    // Apply damage by time-based amount.
                    // This means that, for every second that has passed, we'll decrease our health by 1 unit. Useful for particles!
                    this.Damage(this.m_decayRate * Time.deltaTime); // Speed * change in time. Speed defaults to 1 unit(s)/second.
                }

                // Check if component was damaged last frame and if it is vulnerable to event driven damage calls.
                if (this.Vulnerable && WasDamaged)
                {
                    // Apply damage from passed frames.
                    this.Damage(this.m_damageSinceLastFrame);
                }
            }

            // Reset health recovery and damage values.
            ResetHealthSinceLastFrame();
            ResetDamageSinceLastFrame();
        }

        /// <summary>
        /// Add state values depending on health.
        /// </summary>
        private void UpdateStatus()
        {
            // Check if dead.
            if (this.IsDead)
            {
                // Kill the status.
                this.Status.Kill();
            }
            else
            {
                // Revive.
                this.Status.Revive();
            }
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns true if the mode collection contains the input query.
        /// </summary>
        /// <param name="_mode">Queried mode.</param>
        /// <returns>Returns true if match is found.</returns>
        private bool HasMode(HealthMode _mode)
        {
            return Modes.Contains(_mode);
        }

        /// <summary>
        /// Check if component is flagged as being invincible. Supercedes all other modes.
        /// </summary>
        /// <returns>Returns true if it is found.</returns>
        public bool IsInvincible()
        {
            return HasMode(HealthMode.Invincible);
        }

        /// <summary>
        /// Check if a component can be damaged (excluding time-based).
        /// </summary>
        /// <returns>Returns true if not invincible and the vulnerable state is found.</returns>
        public bool IsVulnerable()
        {
            return !IsInvincible() && HasMode(HealthMode.Vulnerable);
        }

        /// <summary>
        /// Check if a component receives time-based damage.
        /// </summary>
        /// <returns>Returns true if not invincible and the decay state is found.</returns>
        public bool IsDecaying()
        {
            return !IsInvincible() && HasMode(HealthMode.Decay);
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Add mode to the current modes collection.
        /// </summary>
        /// <param name="_mode">Mode to add.</param>
        private void AddMode(HealthMode _mode)
        {
            if (!HasMode(_mode))
            {
                Modes.Add(_mode);
            }
        }

        /// <summary>
        /// Remove mode from the current states collection, if found.
        /// </summary>
        /// <param name="_mode">Mode to remove.</param>
        private void RemoveMode(HealthMode _mode)
        {
            if (HasMode(_mode))
            {
                Modes.Remove(_mode);
            }
        }
                
        /// <summary>
        /// Set this to be vulnerable.
        /// </summary>
        public void SetVulnerable()
        {
            AddMode(HealthMode.Vulnerable);
        }

        /// <summary>
        /// Set this to be affected by a time decay.
        /// </summary>
        public void SetDecaying()
        {
            AddMode(HealthMode.Decay);
        }

        /// <summary>
        /// Triggers the invincibilty frame counter.
        /// </summary>
        public void TriggerInvincibility(int _frames = 1)
        {
            this.m_invincibilityFrames = Services.Max(_frames, 0);
        }
        
        /// <summary>
        /// Decrement the invincibility frames.
        /// </summary>
        public void DecrementInvincibilityFrames()
        {
            if (HasInvincibilityFrames)
            {
                this.m_invincibilityFrames--;
                this.m_calledThisFrame = true;
            }
        }

        /// <summary>
        /// Apply damage to health tracker.
        /// </summary>
        /// <param name="_damage">Damage since last frame.</param>
        private void Damage(float _damage)
        {
            // Damage the health stat tracker.
            this.m_health.Decrement(_damage);
        }

        /// <summary>
        /// Amount to damage the component by.
        /// </summary>
        /// <param name="_damage">Amount to damage the health by.</param>
        /// <param name="_debug">Debugger print statement flag.</param>
        public void Damage(float _damage = 0.0f, bool _debug = true)
        {
            Debugger.Print("" + _damage + " unit(s) of damage caused to " + Self.name + ".", gameObject.name, _debug);
            this.m_damageSinceLastFrame += Services.Max(_damage, 0.0f);
        }

        /// <summary>
        /// Reset cache storing health lost on last frame.
        /// </summary>
        public void ResetDamageSinceLastFrame()
        {
            this.m_damageSinceLastFrame = 0.0f;
        }

        /// <summary>
        /// Apply recovery to health tracker.
        /// </summary>
        /// <param name="_recovery">Health to recover since last frame.</param>
        private void Heal(float _recovery)
        {
            // Heal the health stat tracker.
            this.m_health.Increment(_recovery);
        }

        /// <summary>
        /// Amount to heal by.
        /// </summary>
        /// <param name="_recovery">Amount to heal health by.</param>
        /// <param name="_debug">Debugger print statement flag.</param>
        public void Heal(float _recovery = 0.0f, bool _debug = true)
        {
            Debugger.Print("" + _recovery + " unit(s) of health recovered for " + Self.name + ".", gameObject.name, _debug);
            this.m_healthSinceLastFrame += Services.Max(_recovery, 0.0f);
        }

        /// <summary>
        /// Reset cache storing health recovered on last frame.
        /// </summary>
        public void ResetHealthSinceLastFrame()
        {
            this.m_healthSinceLastFrame = 0.0f;
        }

        /// <summary>
        /// Set health to minimum value.
        /// </summary>
        public void Kill()
        {
            this.m_health.End();
        }

        /// <summary>
        /// Restore the health to full.
        /// </summary>
        public void Restore()
        {
            this.m_health.Reset();
        }

        #endregion

    }

    #endregion
     
}
