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

        private bool groundedP1 = true;
        private bool groundedP2 = true;
        private Rigidbody2D wizzard1_rb;
        private Rigidbody2D wizzard2_rb;

        private float jumpForce = 7000f;
        private float moveForce = 3000f;

        public bool fly_mode = false;

        private CharacterMovement charMovement;
        private CharacterMovement charMovement2;

        public Player m_player1;
        public Player m_player2;

        private bool isFacingRightP1 = true;
        private bool isFacingRightP2 = true;

        private SpriteRenderer wizzard1_sr;
        private SpriteRenderer wizzard2_sr;

        private bool dropping = false;
        private float droppingPos = 0.0f;

        private bool droppingP2 = false;
        private float droppingPosP2 = 0.0f;

        // TODO: Stub.

        public void UpdatePosWizzard(float translation, int wizzard)
        {
            float translate = translation * max_speed;
            translate *= Time.deltaTime;

            //wizzard1.transform.Translate(translate, 0, 0);
            if(wizzard == 1)
            {
                if (wizzard1_rb.velocity.y <= 0.0f && !dropping)
                {
                    wizzard1.layer = 0;
                }
                else if (wizzard1_rb.velocity.y <= 0.0f && dropping && droppingPos == 0.0f)
                {
                    wizzard1.layer = 8;
                    droppingPos = wizzard1_rb.position.y;
                    groundedP1 = true;
                    charMovement.jump_enabled = false;
                    //dropping = false;
                }
                else if (droppingPos > wizzard1_rb.position.y + 1.6f && dropping)
                {
                    dropping = false;
                    droppingPos = 0.0f;
                    wizzard1.layer = 0;
                }

                if (translation * wizzard1_rb.velocity.x < max_speed)
                {

                    wizzard1_rb.AddForce(Vector2.right * translation * moveForce);

                }

                if (Mathf.Abs(wizzard1_rb.velocity.x) > max_speed)
                {
                    wizzard1_rb.velocity = new Vector2(Mathf.Sign(wizzard1_rb.velocity.x) * max_speed, wizzard1_rb.velocity.y);
                }

                if (!groundedP1)
                {
                    wizzard1_rb.AddForce(new Vector2(0f, jumpForce));
                    groundedP1 = true;
                    charMovement.jump_enabled = false;

                    wizzard1.layer = 8;
                }

                if (translation > 0 && !isFacingRightP1)
                {
                    Flip(wizzard);
                }
                else if (translation < 0 && isFacingRightP1)
                {
                    Flip(wizzard);
                }

                wizzard1.transform.rotation = Quaternion.identity;
            }
            else
            {
                if (wizzard2_rb.velocity.y <= 0.0f && !droppingP2)
                {
                    wizzard2.layer = 0;
                }
                else if (wizzard2_rb.velocity.y <= 0.0f && droppingP2 && droppingPosP2 == 0.0f)
                {
                    wizzard2.layer = 8;
                    droppingPosP2 = wizzard2_rb.position.y;
                    groundedP2 = true;
                    charMovement2.jump_enabled = false;
                    //dropping = false;
                }
                else if (droppingPosP2 > wizzard2_rb.position.y + 1.6f && droppingP2)
                {
                    droppingP2 = false;
                    droppingPosP2 = 0.0f;
                    wizzard2.layer = 0;
                }

                if (translation * wizzard2_rb.velocity.x < max_speed)
                {

                    wizzard2_rb.AddForce(Vector2.right * translation * moveForce);

                }

                if (Mathf.Abs(wizzard2_rb.velocity.x) > max_speed)
                {
                    wizzard2_rb.velocity = new Vector2(Mathf.Sign(wizzard2_rb.velocity.x) * max_speed, wizzard2_rb.velocity.y);
                }

                if (!groundedP2)
                {
                    wizzard2_rb.AddForce(new Vector2(0f, jumpForce));
                    groundedP2 = true;
                    charMovement2.jump_enabled = false;

                    wizzard2.layer = 8;
                }

                if (translation > 0 && !isFacingRightP2)
                {
                    Flip(wizzard);
                }
                else if (translation < 0 && isFacingRightP2)
                {
                    Flip(wizzard);
                }

                wizzard2.transform.rotation = Quaternion.identity;
            }
            
        }

        public void UpdateJumpStatus(bool jump, int wizzard)
        {
            //this.grounded && jump && 
            if(wizzard == 1)
            {
                if (charMovement.jump_enabled && jump)
                {
                    this.groundedP1 = !jump;
                    charMovement.jump_enabled = false;
                }
            } else
            {
                if (charMovement2.jump_enabled && jump)
                {
                    this.groundedP2 = !jump;
                    charMovement2.jump_enabled = false;
                }
            }
            
        }

        public void Flip(int wizzard)
        {
            if(wizzard == 1)
            {
                wizzard1_sr.flipX = isFacingRightP1;
                isFacingRightP1 = !isFacingRightP1;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            } else
            {
                wizzard2_sr.flipX = isFacingRightP2;
                isFacingRightP2 = !isFacingRightP2;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            
        }

        public void fireProj(int wizzard, bool fire1_pressed, bool fire2_pressed, bool fire3_pressed)
        {
            if(wizzard == 1)
            {
                m_player1.fireProjPlayer(fire1_pressed, fire2_pressed, fire3_pressed, wizzard1.transform.position, this.isFacingRightP1);
            } else
            {
                m_player2.fireProjPlayer(fire1_pressed, fire2_pressed, fire3_pressed, wizzard1.transform.position, this.isFacingRightP2);
            }
			
        }

        public void UpdateDropStatus(bool player_drop, int wizzard)
        {
            if(wizzard == 1)
            {
                if (player_drop)
                {
                    charMovement.dropping = true;
                    dropping = true;
                }
            } else
            {
                if (player_drop)
                {
                    charMovement2.dropping = true;
                    droppingP2 = true;
                }
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
            wizzard1_sr = wizzard1.GetComponent<SpriteRenderer>();
            wizzard2_sr = wizzard1.GetComponent<SpriteRenderer>();
            m_init = true;

            this.m_player1 = gameObject.GetComponent<Player>();
            this.m_player2 = gameObject.GetComponent<Player>();

        }

        private bool IsInitialized()
        {
            return m_init;
        }
    }
}
