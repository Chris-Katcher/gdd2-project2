using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Physics
{
    public class CharacterMovement : MonoBehaviour
    {

        public bool jump_enabled = true;
        public bool dropping = false;

        float collisionY = 0.0f;

        void OnCollisionStay2D(Collision2D coll)
        {
            if (coll.transform.name != "Wizzard1(Clone)" && coll.transform.name != "Ceiling")
            {
                jump_enabled = true;
            }
            else
            {
                jump_enabled = false;
            }

        }

        public GameObject wizzard1;
        public bool m_init = false;
        private float max_speed = 10.0f;

        public bool grounded = true;
        public Rigidbody2D wizzard1_rb;

        private float jumpForce = 7000f;
        private float moveForce = 2000f;

        public bool fly_mode = false;

        

        //public Player m_player1;

        public bool isFacingRight = true;

        public SpriteRenderer wizzard_sr;

        
        private float droppingPos = 0.0f;

        public void UpdatePosWizzard1(float translation)
        {
            float translate = translation * max_speed;
            translate *= Time.deltaTime;

            //wizzard1.transform.Translate(translate, 0, 0);

            if (wizzard1_rb.velocity.y <= 0.0f && !dropping)
            {
                wizzard1.layer = 0;
            }
            else if (wizzard1_rb.velocity.y <= 0.0f && dropping && droppingPos == 0.0f)
            {
                wizzard1.layer = 8;
                droppingPos = wizzard1_rb.position.y;
                grounded = true;
                jump_enabled = false;
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

            if (!grounded)
            {
                wizzard1_rb.AddForce(new Vector2(0f, jumpForce));
                grounded = true;
                jump_enabled = false;

                wizzard1.layer = 8;
            }

            if (translation > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (translation < 0 && isFacingRight)
            {
                Flip();
            }

            wizzard1.transform.rotation = Quaternion.identity;
        }

        public void Flip()
        {
            wizzard_sr.flipX = isFacingRight;
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
