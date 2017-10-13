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

        private float jumpForce = 500f;
        private float moveForce = 50f;

        public bool fly_mode = false;

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
            
            if(!grounded && wizzard1_rb.velocity.y == 0.0f)
            {
                wizzard1_rb.AddForce(new Vector2(0f, jumpForce));
                grounded = true;
            }
        }

        public void UpdateJumpStatus(bool jump)
        {
            if(this.grounded && jump)
            {
                this.grounded = !jump;
            }
        }

        public void Initialize()
        {
            wizzard1 = Resources.Load("Wizzard") as GameObject;
            wizzard1 = Instantiate(wizzard1);

            wizzard1_rb = wizzard1.GetComponent<Rigidbody2D>();
            m_init = true;
        }

        private bool IsInitialized()
        {
            return m_init;
        }
    }
}
