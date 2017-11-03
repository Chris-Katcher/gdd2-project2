﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcana;
using Arcana.Entities.Attributes;
using Arcana.InputManagement;
using Arcana.UI;
using Arcana.Entities;
using Arcana.Physics;
using Arcana.Cameras;

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


        private ProjectileManager m_projectile;
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
                
                

                // Add a projectile manager to this player.

                BuildControlScheme();
                InitializeControls();
            }
        }


        protected override void BuildControlScheme()
        {
            // Set director.
            this.Director = InputManagement.Director.Camera;
            base.BuildControlScheme();
        }

        protected override ControlScheme InitializeControls()
        {
            this.m_scheme = base.InitializeControls();

            // Register camera background control.
            this.RegisterControl(ControlScheme.CreateAction("Move Horizontally"),
                ControlScheme.CreateTrigger(Control.LeftStickHorizontal(this.m_data.PlayerNumber)));

            this.RegisterControl(ControlScheme.CreateAction("Move Verticle"),
                ControlScheme.CreateTrigger(Control.LeftStickHorizontal(this.m_data.PlayerNumber)));

            this.RegisterControl(ControlScheme.CreateAction("Jump"),
                ControlScheme.CreateTrigger(Control.AButton(this.m_data.PlayerNumber), ResponseMode.Pressed));

            this.RegisterControl(ControlScheme.CreateAction("X Button"),
                ControlScheme.CreateTrigger(Control.XButton(this.m_data.PlayerNumber), ResponseMode.Pressed));

            this.RegisterControl(ControlScheme.CreateAction("Y Button"),
                ControlScheme.CreateTrigger(Control.YButton(this.m_data.PlayerNumber), ResponseMode.Pressed));

            this.RegisterControl(ControlScheme.CreateAction("B Button"),
                ControlScheme.CreateTrigger(Control.BButton(this.m_data.PlayerNumber), ResponseMode.Pressed));

            this.RegisterControl(ControlScheme.CreateAction("RB Button"),
                ControlScheme.CreateTrigger(Control.RightBumper(this.m_data.PlayerNumber), ResponseMode.Pressed));

            //this.RegisterControl(ControlScheme.CreateAction("Change Background"),
            //    ControlScheme.CreateTrigger(Control.CreateKey(KeyCode.A), ResponseMode.Pressed));

            //// Register camera mode controls.
            //this.RegisterControl(ControlScheme.CreateAction("Change Mode"),
            //    ControlScheme.CreateTrigger(Control.StartButton(-1), ResponseMode.Pressed));

            //this.RegisterControl(ControlScheme.CreateAction("Change Target B"),
            //        ControlScheme.CreateTrigger(Control.BButton(-1), ResponseMode.Pressed));
            //this.RegisterControl(ControlScheme.CreateAction("Change Target X"),
            //        ControlScheme.CreateTrigger(Control.XButton(-1), ResponseMode.Pressed));

            return this.m_scheme;
        }

        protected override void HandleInput()
        {
            
            if (this.Controls.IsActivated(GetAction("Move Horizontally")))
            {
                //this.ChangeBackground(Services.GetRandomColor());

                this.m_controller.UpdatePosWizzard(this.Controls.GetValue(GetAction("Move Horizontally")));
            } else
            {
                this.m_controller.UpdatePosWizzard(0.0f);
            }

            if (this.Controls.IsActivated(GetAction("Move Verticle")))
            {
                //this.ChangeBackground(Services.GetRandomColor());

                this.m_controller.UpdateDropStatus(this.Controls.GetValue(GetAction("Move Verticle")) < 0.0f);
            }

            if (this.Controls.IsActivated(GetAction("Jump")))
            {
                //this.ChangeBackground(Services.GetRandomColor());

                this.m_controller.UpdateJumpStatus(true);
            } else
            {
                this.m_controller.UpdateJumpStatus(false);
            }

            bool fire1_pressed= false, fire2_pressed= false, fire3_pressed = false, rightTrigger = false;

            if (this.Controls.IsActivated(GetAction("X Button")))
            {
                //this.ChangeBackground(Services.GetRandomColor());

                fire1_pressed = true;
            }

            if (this.Controls.IsActivated(GetAction("Y Button")))
            {
                //this.ChangeBackground(Services.GetRandomColor());

                fire2_pressed = true;
            }

            if (this.Controls.IsActivated(GetAction("B Button")))
            {
                //this.ChangeBackground(Services.GetRandomColor());

                fire3_pressed = true;
            }

            if (this.Controls.IsActivated(GetAction("RB Button")))
            {
                //this.ChangeBackground(Services.GetRandomColor());

                rightTrigger = true;
            }

            this.m_controller.fireProjPlayer(fire1_pressed, fire2_pressed, fire3_pressed, rightTrigger, this.transform.position);
        }




        public override void Update()
        {
            if (!this.Initialized) { this.Initialize(); }
            else
            {
                // The base update is called here.
                base.Update();
                HandleInput();
            }
        }

            #endregion



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



        }
}
