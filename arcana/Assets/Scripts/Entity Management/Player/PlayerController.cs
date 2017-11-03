using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana;
using Arcana.Entities;
using Arcana.Utilities;
using Arcana.Resources;
using Arcana.Physics;
using UnityEngine;

namespace Arcana.Entities.Attributes
{
    /// <summary>
    /// Contains reference to physics and collision components for a player.
    /// </summary>
    public class PlayerController : ArcanaObject
    {

        #region Data Members

        #region Fields.
                
        /// <summary>
        /// The player's sprite renderer.
        /// </summary>
        private EntityRenderer m_renderer;

        /// <summary>
        /// The player's box collider.
        /// </summary>
        private EntityCollider m_collider;

        /// <summary>
        /// The player's rigidbody.
        /// </summary>
        private EntityRigidbody m_rigidbody;

        private ProjectileManager m_projectile;

        #endregion

        #region Properties.

        /// <summary>
        /// Reference to the player's sprite renderer.
        /// </summary>
        public EntityRenderer Renderer
        {
            get
            {
                if (this.m_renderer == null)
                {
                    this.m_renderer = InitializeRenderer();
                }
                return this.m_renderer;
            }
        }

        /// <summary>
        /// Reference to the player's box collider.
        /// </summary>
        public EntityCollider Collider
        {
            get
            {
                if (this.m_collider == null)
                {
                    this.m_collider = InitializeCollider();
                }
                return this.m_collider;
            }
        }

        /// <summary>
        /// Reference to the player's rigidbody.
        /// </summary>
        public EntityRigidbody Rigidbody
        {
            get
            {
                if (this.m_rigidbody == null)
                {
                    this.m_rigidbody = InitializeRigidbody();
                }
                return this.m_rigidbody;
            }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Update physical calculations.
        /// </summary>
        public void FixedUpdate()
        {
            HandleFacing();
            HandleMovement();
            m_projectile.updateProjectiles();
        }

        /// <summary>
        /// Update the player's component visibility.
        /// </summary>
        public override void Update()
        {
            if (!this.Initialized)
            {
                this.Initialize();
            }
            else
            {
                // Call the base update function.
                base.Update();

                //UpdatePosWizzard(Input.GetAxis("P1_LSX"));

                //UpdateJumpStatus(Input.GetKeyDown(KeyCode.Space));

                if (debug_delete)
                {
                    this.Status.Destroy();
                }

                if (debug_active)
                {
                    this.Activate();
                }
                else
                {
                    this.Deactivate();
                }

                if (debug_visible)
                {
                    this.Show();
                }
                else
                {
                    this.Hide();
                }

                if (debug_pause)
                {
                    this.Pause();
                }
                else
                {
                    this.Resume();
                }

            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Create the renderer, collider, and rigidbody for the player controller.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the base class' method.
                base.Initialize();

                // Initialize the data members.
                this.m_renderer = this.Renderer;
                this.m_collider = this.Collider;
                this.m_rigidbody = this.Rigidbody;

                gameObject.AddComponent<ProjectileManager>();
                this.charMovement = gameObject.AddComponent<CharacterMovement>();
                charMovement.wizzard1_rb = gameObject.GetComponent<Rigidbody2D>();
                charMovement.wizzard1 = gameObject;
                if (gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    charMovement.wizzard_sr = gameObject.GetComponent<SpriteRenderer>();
                }
                m_projectile = gameObject.AddComponent<ProjectileManager>();

                

                gameObject.transform.localScale = new Vector3(10, 10, 10);
            }
        }

        /// <summary>
        /// Initializes the player's sprite renderer.
        /// </summary>
        /// <returns>Returns the initialized sprite renderer.</returns>
        private EntityRenderer InitializeRenderer()
        {
            EntityRenderer renderer = this.Self.GetComponent<EntityRenderer>();

            if (renderer == null)
            {
                renderer = this.Self.AddComponent<EntityRenderer>();

                renderer.Initialize();
                renderer.InitializeResources(
                    "PLAYER_WIZZARD_SPRITE", 
                    "PLAYER_WIZZARD_MAT",
                    "Images/wizardBodyA",
                    "Materials/WizzardBodyAMat");
                renderer.InitializeRenderer();

            }
            
            return renderer;
        }

        /// <summary>
        /// Initializes the player's box collider.
        /// </summary>
        /// <returns>Returns the initialized box collider.</returns>
        private EntityCollider InitializeCollider()
        {
            EntityCollider collider = this.Self.GetComponent<EntityCollider>();

            if (collider == null)
            {
                collider = this.Self.AddComponent<EntityCollider>();

                // Initialize the property.
                collider.Initialize();
                collider.InitializeCollider();

                // Set up property defaults.
                collider.Offset = new Vector2(0.008312151f, -0.01187449f);
                collider.Size = new Vector2(0.1418829f, 0.296251f);
                collider.Collider.edgeRadius = 0;
                
            }

            return collider;
        }

        /// <summary>
        /// Initializes the player's rigidbody2D.
        /// </summary>
        /// <returns>Returns the initialized rigidbody2D.</returns>
        private EntityRigidbody InitializeRigidbody()
        {
            EntityRigidbody rigidbody = this.Self.GetComponent<EntityRigidbody>();

            if (rigidbody == null)
            {
                rigidbody = this.Self.AddComponent<EntityRigidbody>();

                // Init.
                rigidbody.Initialize();
                rigidbody.InitializeRigidbody();

                // Set gravity to zero.
                rigidbody.Gravity = 0.0f;

            }

            return rigidbody;
        }

        private Player InitializePlayer()
        {
            Player player = this.Self.GetComponent<Player>();

            if (player == null)
            {
                player = this.Self.AddComponent<Player>();

                // Init.
                player.Initialize();


                
            }

            return player;
        }


        #region Mutator Methods
        private SpriteRenderer wizzard_sr;
        private CharacterMovement charMovement;
        private bool grounded = true;
        public bool dropping = false;
        public void UpdateJumpStatus(bool jump)
        {
            //this.grounded && jump && 
            if (charMovement.jump_enabled && jump)
            {
                this.grounded = !jump;
                charMovement.grounded = !jump;
                charMovement.jump_enabled = false;
            }
        }

        public void UpdatePosWizzard(float translation)
        {
            charMovement.UpdatePosWizzard1(translation);
        }

        public void UpdateDropStatus(bool player_drop)
        {
            if (player_drop)
            {
                charMovement.dropping = true;
                dropping = true;
            }
            //else if(!wizzard1.GetComponent<CharacterMovement>().dropping)
            //{
            //    dropping = false;
            //}

            //else
            //wizzard1.layer = 0;
        }

        /// <summary>
        /// fires a projectile based upon a projectile
        /// </summary>
        /// <param name="fire">whether or not the fire button has been pressed</param>
        public void fireProjPlayer(bool fire1, bool fire2, bool fire3, bool rightTrigger, Vector3 pos, bool facingRight)
        {
            if (facingRight)
            {
                pos.x += .5f;
                pos.y += .1f;
            }
            else
            {
                pos.x -= .5f;
                pos.y += .1f;
            }

            //passes in bool and player position
            // if else prevents players from mashing all buttons at once  

            //really janky mehtod to determine whihc button has been presed. NEEDS IMPROVEMENT
            if (rightTrigger == true)
            {

                m_projectile.fireProjectile(fire1, fire2, fire3, rightTrigger, pos, facingRight);

            }
            else if (fire1)
            {
                m_projectile.fireProjectile(fire1, fire2, fire3, rightTrigger, pos, facingRight);
            }
            else if (fire2)
            {
                m_projectile.fireProjectile(fire1, fire2, fire3, rightTrigger, pos, facingRight);
            }
            else if (fire3)
            {
                m_projectile.fireProjectile(fire1, fire2, fire3, rightTrigger, pos, facingRight);
            }

        }
        #endregion

        #endregion

        #region Status Methods.

        /// <summary>
        /// Activate the Entity's components.
        /// </summary>
        public override void Activate()
        {
            base.Activate();
            this.Renderer.Activate();
            this.Collider.Activate();
            this.Rigidbody.Activate();
        }

        /// <summary>
        /// Deactivate the Entity's components.
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();
            this.Renderer.Deactivate();
            this.Collider.Deactivate();
            this.Rigidbody.Deactivate();
        }

        /// <summary>
        /// Pause the Entity's components.
        /// </summary>
        public override void Pause()
        {
            base.Pause();
            this.Renderer.Pause();
            this.Collider.Pause();
            this.Rigidbody.Pause();
        }

        /// <summary>
        /// Resume the Entity's components.
        /// </summary>
        public override void Resume()
        {
            base.Resume();
            this.Renderer.Resume();
            this.Collider.Resume();
            this.Rigidbody.Resume();
        }

        /// <summary>
        /// Hide the Entity's sprite.
        /// </summary>
        public override void Hide()
        {
            base.Hide();
            this.Renderer.Hide();
        }

        /// <summary>
        /// Show the Entity's sprite.
        /// </summary>
        public override void Show()
        {
            base.Show();
            this.Renderer.Show();
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Store heading.
        /// </summary>
        public void HandleFacing()
        {
            // Facing
            this.Rigidbody.Heading = this.Rigidbody.Velocity;

            // If facing right.
            if (this.Rigidbody.Heading.x >= 0)
            {
                // Facing right.
                this.Renderer.FlipX = false;
            }
            else
            {
                // Facing left.
                this.Renderer.FlipX = false;
            }
        }

        /// <summary>
        /// Keeps the player rotation proper and within bounds.
        /// </summary>
        public void HandleMovement()
        {
            // Fix rotation.
            this.transform.rotation = Quaternion.identity;
        }

        #endregion

    }
}
