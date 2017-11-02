using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana;
using Arcana.Entities;
using Arcana.Utilities;
using UnityEngine;

namespace Arcana.Entities.Attributes
{

    /// <summary>
    /// The player data struct holds technical information about the player entity.
    /// </summary>
    public class PlayerData : ArcanaObject
    {

        #region Data Members

        #region Fields.

        /// <summary>
        /// Represents the player number.
        /// </summary>
        private int m_playerNumber = -1;

        /// <summary>
        /// The starting position of the player.
        /// </summary>
        private Vector3 m_startPosition = Vector3.zero;
        
        /// <summary>
        /// Represents the health of the player.
        /// </summary>
        private HealthComponent m_health = null;

        /// <summary>
        /// Data structure holding information pertaining to player movement speed.
        /// </summary>
        private StatTracker m_speed;

        /// <summary>
        /// Represents the current score value a player has.
        /// </summary>
        private int m_score;

        #endregion

        #region Properties.

        /// <summary>
        /// Returns 1, 2, or -1, representing the player number.
        /// </summary>
        public int PlayerNumber
        {
            get
            {
                switch (m_playerNumber)
                {
                    case 1:
                    case 2:
                        return this.m_playerNumber;
                    default:
                        return -1;
                }
            }
        }

        /// <summary>
        /// Reference to the starting position of the player.
        /// </summary>
        public Vector3 StartPosition
        {
            get { return this.m_startPosition; }
            set { this.m_startPosition = value; }
        }

        /// <summary>
        /// Reference to the health stat.
        /// </summary>
        public HealthComponent Hitpoints
        {
            get { return this.m_health; }
        }

        /// <summary>
        /// Reference to the speed stat.
        /// </summary>
        public StatTracker SpeedStat
        {
            get { return this.m_speed; }
        }

        /// <summary>
        /// Reference to the current speed of the player.
        /// </summary>
        public float CurrentSpeed
        {
            get { return this.m_speed.Value; }
            set { this.m_speed.SetValue(value); }
        }

        /// <summary>
        /// Reference to the max speed of the player.
        /// </summary>
        public float MaxSpeed
        {
            get { return this.m_speed.Max; }
            set { this.m_speed.SetMaximum(value); }
        }
        
        /// <summary>
        /// Reference to the min speed of the player.
        /// </summary>
        public float MinSpeed
        {
            get { return this.m_speed.Min; }
            set { this.m_speed.SetMinimum(value); }
        }
        
        #endregion

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initialization of the player data object.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Base initialization.
                base.Initialize();

                // Set up data members.
                this.m_playerNumber = -1; // -1 means "all players". 1 means player 1. 2 means player 2.
                this.m_health = HealthComponent.Create(this); // Add health component to current object.
                this.m_speed = new StatTracker("Player Speed", 0.0f, 0.0f, 100.0f); // Set up the speed value for the player.
                this.m_score = 0; // Start the score at zero.
                this.m_startPosition = this.transform.position; // The initial starting position may be started based off of where it is located in world-space.
            }
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns the current score.
        /// </summary>
        /// <returns>Score value as float.</returns>
        public int GetScore()
        {
            return this.m_score;
        }

        /// <summary>
        /// Returns the starting position vector.
        /// </summary>
        /// <returns>Vector3 representing the starting position of the player.</returns>
        public Vector3 GetStartPosition()
        {
            return this.m_startPosition;
        }

        /// <summary>
        /// Returns the current health of the player.
        /// </summary>
        /// <returns>Float value representing current player health.</returns>
        public float GetCurrentHealth()
        {
            return this.Hitpoints.CurrentHealth;
        } 
        
        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set up the player number value.
        /// </summary>
        /// <param name="_player">Number player to assign this data to.</param>
        public void SetPlayerNumber(int _player)
        {
            // Set up the player data name.
            this.Name = "Player " + this.m_playerNumber + "Data";

            // Assign the number.
            this.m_playerNumber = _player;
        }

        /// <summary>
        /// Assigns the player's starting position.
        /// </summary>
        /// <param name="_start">Starting position to be assigned.</param>
        public void SetStartPosition(Vector3 _start)
        {
            this.m_startPosition = _start;
        }

        /// <summary>
        /// Add value to the score.
        /// </summary>
        /// <param name="_score">Score value to add.</param>
        public void AddScore(int _score)
        {
            this.m_score += _score;
        }
                
        #endregion
        
    }
}
