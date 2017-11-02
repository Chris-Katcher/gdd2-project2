using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcana;
using Arcana.Entities.Attributes;
using Arcana.InputManagement;
using Arcana.UI;
using Arcana.Entities;

namespace Arcana.Entities
{

    /// <summary>
    /// The Player entity represents a Wizzard object that can be controlled by the player controller. As this is player number agnostic (it doesn't care who is first and who is second), it doesn't have any input.
    /// </summary>
    public class Player : Entity
    {

        #region Data Members

        #region Fields.

        /// <summary>
        /// The data field holds references to important information involving the player.
        /// </summary>
        private PlayerData m_data = null;

        /// <summary>
        /// The controller field holds reference to the script that deals with moving the player.
        /// </summary>
        private PlayerController m_controller = null;

        #endregion

        #region Properties.

        /// <summary>
        /// Reference to the player data component.
        /// </summary>
        public PlayerData Data
        {
            get
            {
                if (this.m_data == null)
                {
                    this.m_data = this.Self.GetComponent<PlayerData>();
                }

                if (this.m_data == null)
                {
                    this.m_data = this.Self.AddComponent<PlayerData>();
                    this.m_data.Initialize();
                }

                return this.m_data;
            }
        }

        /// <summary>
        /// Reference to the player controller.
        /// </summary>
        public PlayerController Controller
        {
            get
            {
                if (this.m_controller == null)
                {
                    this.m_controller = this.Self.GetComponent<PlayerController>();
                }

                if (this.m_controller == null)
                {
                    this.m_controller = this.Self.AddComponent<PlayerController>();
                    this.m_controller.Initialize();
                }

                return this.m_controller;
            }
        }

        #endregion

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Set up the player's player data and controller objects.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the base initialization method.
                base.Initialize();

                // Create the components.
                this.m_data = this.Data;
                this.m_controller = this.Controller;

                // Set the starting position.
                this.Data.SetStartPosition(this.transform.position);
            }
        }

        #endregion


        #region Deprecated Functionality

        /* 
        private ProjectileManager m_projectile;

        // Use this for initialization
        void Start () {

            gameObject.AddComponent<ProjectileManager>();
            m_projectile = gameObject.GetComponent<ProjectileManager>();

        }

        // Update is called once per frame
        void Update () {


        }

        private void FixedUpdate()
        {
            m_projectile.updateProjectiles();
        }


        /// <summary>
        /// fires a projectile based upon a projectile
        /// </summary>
        /// <param name="fire">whether or not the fire button ahs been pressed</param>
        public void fireProjPlayer(bool fire, Vector3 pos, bool facingRight)
        {
            if (facingRight)
            {
                pos.x += .5f;
                pos.y += .1f;
            } else
            {
                pos.x -= .5f;
                pos.y += .1f;
            }

            //passes in bool and player position
            m_projectile.fireProjectile(fire, pos, facingRight);

        } */

        #endregion

    }
}
