/************************************************
 * GameManager.cs
 * 
 * This file contains implementation for the GameManager class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Arcana.Utilities;
using Arcana.Physics;

namespace Arcana
{

    #region Class: GameManager class.
    /// <summary>
    /// Handles the main game loop.
    /// </summary>
    public class GameManager : ArcanaObject
    {
        public GameObject wizzard1;
        public bool m_init = false;
        private float max_speed = 10.0f;

        private bool grounded = true;
        private Rigidbody2D wizzard1_rb;

        private float jumpForce = 6000f;
        private float moveForce = 500f;

        public bool fly_mode = false;

        private CharacterMovement charMovement;

        public Player m_player1;

        private bool isFacingRight = true;

        private SpriteRenderer wizzard_sr;

        public const int maxHealth = 100;

        public int currentHealth_p1 = maxHealth;

        public int currentHealth_p2 = maxHealth;

        public void UpdatePosWizzard1(float translation)
        {
            float translate = translation * max_speed;
            translate *= Time.deltaTime;

            //wizzard1.transform.Translate(translate, 0, 0);

            if(translation * wizzard1_rb.velocity.x < max_speed)
            {
            
                wizzard1_rb.AddForce(Vector2.right * translation * moveForce);
                
            }

            if(Mathf.Abs(wizzard1_rb.velocity.x) > max_speed)
            {
                wizzard1_rb.velocity = new Vector2(Mathf.Sign(wizzard1_rb.velocity.x) * max_speed, wizzard1_rb.velocity.y);
            }
            
            if(!grounded)
            {
                wizzard1_rb.AddForce(new Vector2(0f, jumpForce));
                grounded = true;
                charMovement.jump_enabled = false;
            }

            if(translation > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (translation < 0 && isFacingRight)
            {
                Flip();
            }

            wizzard1.transform.rotation = Quaternion.identity;
        }

        public void UpdateJumpStatus(bool jump)
        {
            //this.grounded && jump && 
            if (charMovement.jump_enabled && jump)
            {
                this.grounded = !jump;
                charMovement.jump_enabled = false;
            }
        }

        public void Flip()
        {
            wizzard_sr.flipX = isFacingRight;
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        public void fireProjPlayer1(bool fire1_pressed)
        {
            m_player1.fireProjPlayer(fire1_pressed, wizzard1.transform.position, this.isFacingRight);
        }

        public void TakeDamage(int player_id, int amount)
        {
            if(player_id == 1)
            {
                currentHealth_p1 -= amount;
                if (currentHealth_p1 <= 0)
                {
                    currentHealth_p1 = 0;
                    // Handle Player Death
                }
            }
            else if(player_id == 2)
            {
                currentHealth_p2 -= amount;
                if (currentHealth_p2 <= 0)
                {
                    currentHealth_p2 = 0;
                    // Handle Player Death
                }
            }
        }

        public int GetCurrentHealth(int player_id)
        {
            if (player_id == 1)
            {
                return currentHealth_p1;
            }
            else if (player_id == 2)
            {
                return currentHealth_p2;
            }
            else return 0;
        }

        public RectTransform GetWizzard_1HealthReact()
        {
            // Lolwut
            foreach(RectTransform rect in wizzard1.GetComponentsInChildren<RectTransform>())
            {
                if (rect.gameObject.name == "Foreground")
                    return rect;
            }
            return null;

        }

        #region Initialization Methods.

        /// <summary>
        /// Create the data members for the UIManager.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Initialize the base values.
                base.Initialize();

                // Set this name.
                this.Name = "Game Manager";

                wizzard1 = UnityEngine.Resources.Load("Wizzard") as GameObject;
                wizzard1 = Instantiate(wizzard1, new Vector3(1, 0, 0), Quaternion.identity);

                charMovement = wizzard1.GetComponent<CharacterMovement>();

                //Instantiate(wizzard1, new Vector3(1, 0, 0), Quaternion.identity);

                wizzard1_rb = wizzard1.GetComponent<Rigidbody2D>();
                wizzard_sr = wizzard1.GetComponent<SpriteRenderer>();
                m_init = true;

                this.m_player1 = gameObject.GetComponent<Player>();
            }
        }

        #endregion

        private bool IsInitialized()
        {
            return m_init;
        }

        #region Instancing Methods.
        /// <summary>
        /// Static instance of the class. (We only want one).
        /// </summary>
        public static GameManager instance = null;

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        /// <returns>Returns a component.</returns>
        public static GameManager GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of EntityManager.");
                instance = Services.CreateEmptyObject("Game Manager").AddComponent<GameManager>();
            }

            return instance;
        }

        /// <summary>
        /// Returns true if instance exists.
        /// </summary>
        /// <returns>Returns boolean indicating instance existence.</returns>
        public static bool HasInstance()
        {
            return (instance != null);
        }
        #endregion

        #region Component Factory Methods.
        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static GameManager Create(ArcanaObject _parent)
        {
            if (!HasInstance())
            {
                instance = _parent.GetComponent<GameManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<GameManager>(_parent);
            }

            return instance;
        }

        #endregion
    }
    #endregion
}
