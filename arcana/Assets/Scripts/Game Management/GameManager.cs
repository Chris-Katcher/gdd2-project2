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
using Arcana.Physics;

namespace Arcana
{
    /// <summary>
    /// Handles the main game loop.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public GameObject wizzard1;
        public GameObject wizzard2;
        public bool m_init = false;
        private float max_speed = 10.0f;

        private bool grounded = true;
        private Rigidbody2D wizzard1_rb;
        private Rigidbody2D wizzard2_rb;

        private float jumpForce = 110000f;
        private float moveForce = 7000f;

        public bool fly_mode = false;

        private CharacterMovement charMovement;
        private CharacterMovement charMovement2;

        public Player m_player1;
        public Player m_player2;

        private bool isFacingRight = true;

        private SpriteRenderer wizzard_sr;

        private bool dropping = false;
        private float droppingPos = 0.0f;
        // TODO: Stub.

        public void UpdatePosWizzard1(float translation)
        {
            float translate = translation * max_speed;
            translate *= Time.deltaTime;

            //wizzard1.transform.Translate(translate, 0, 0);

            if(wizzard1_rb.velocity.y <= 0.0f && !dropping)
            {
                wizzard1.layer = 0;
            }
            else if( wizzard1_rb.velocity.y <= 0.0f && dropping && droppingPos == 0.0f)
            {
                wizzard1.layer = 8;
                droppingPos = wizzard1_rb.position.y;
                grounded = true;
                charMovement.jump_enabled = false;
                //dropping = false;
            }
            else if(droppingPos > wizzard1_rb.position.y + 1.6f && dropping)
            {
                dropping = false;
                droppingPos = 0.0f;
                wizzard1.layer = 0;
            }

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

                wizzard1.layer = 8;
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

        public void fireProjPlayer1(bool fire1_pressed, bool fire2_pressed, bool fire3_pressed)
        {
			m_player1.fireProjPlayer(fire1_pressed, fire2_pressed, fire3_pressed, wizzard1.transform.position, this.isFacingRight);
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

        public void Initialize()
        {
            wizzard1 = UnityEngine.Resources.Load("Wizzard") as GameObject;
            wizzard1 = Instantiate(wizzard1, new Vector3( 1, 0, 0), Quaternion.identity );

            wizzard2 = UnityEngine.Resources.Load("Wizzard") as GameObject;
            wizzard2 = Instantiate(wizzard2, new Vector3(25, 0, 0), Quaternion.identity);

            charMovement = wizzard1.GetComponent<CharacterMovement>();
            charMovement2 = wizzard2.GetComponent<CharacterMovement>();

            //Instantiate(wizzard1, new Vector3(1, 0, 0), Quaternion.identity);

            wizzard1_rb = wizzard1.GetComponent<Rigidbody2D>();
            wizzard2_rb = wizzard2.GetComponent<Rigidbody2D>();
            wizzard_sr = wizzard1.GetComponent<SpriteRenderer>();
            m_init = true;

            this.m_player1 = gameObject.GetComponent<Player>();

        }

        private bool IsInitialized()
        {
            return m_init;
        }
    }
}
