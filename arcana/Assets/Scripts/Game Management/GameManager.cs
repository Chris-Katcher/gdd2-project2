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
        public bool m_init = false;
        private float max_speed = 10.0f;

        private bool grounded = true;
        private Rigidbody2D wizzard1_rb;

        private float jumpForce = 6000f;
        private float moveForce = 500f;

        public bool fly_mode = false;

        private CharacterMovement charMovement;

        public Player m_player1;

        // TODO: Stub.

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

        public void fireProjPlayer1(bool fire1_pressed)
        {
            m_player1.fireProjPlayer(fire1_pressed, wizzard1.transform.position);
        }

        public void Initialize()
        {
            wizzard1 = UnityEngine.Resources.Load("Wizzard") as GameObject;
            wizzard1 = Instantiate(wizzard1, new Vector3( 1, 0, 0), Quaternion.identity );

            charMovement = wizzard1.GetComponent<CharacterMovement>();

            Instantiate(wizzard1, new Vector3(1, 0, 0), Quaternion.identity);

            wizzard1_rb = wizzard1.GetComponent<Rigidbody2D>();
            m_init = true;

            this.m_player1 = gameObject.GetComponent<Player>();
        }

        private bool IsInitialized()
        {
            return m_init;
        }
    }
}
