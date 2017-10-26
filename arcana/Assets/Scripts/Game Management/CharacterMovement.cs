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
        //void OnCollisionStay2D(Collision2D coll)
        //{
        //    //if(coll.transform.name != "Wizzard1(Clone)" && coll.transform.name != "Ceiling")
        //    //{
        //    //    jump_enabled = true;
        //    //} else
        //    //{
        //    //    jump_enabled = false;
        //    //}
            
        //}

        void OnCollisionStay2D(Collision2D collision)
        {
            if(collision.contacts[0].normal.y > 0)
            {
                jump_enabled = true;
            } else
            {
                jump_enabled = false;
            }

            //if(dropping)
            //{
            //    if(collisionY == 0)
            //    {
            //        gameObject.layer = 8;
            //    }
            //    else if(collisionY < gameObject.transform.position.y + 1.6f)
            //    {
            //        gameObject.layer = 8;
            //    } else
            //    {
            //        gameObject.layer = 0;
            //        dropping = false;
            //        collisionY = 0.0f;
            //    }
            //}

            //if(collision.transform.position.y < gameObject.transform.position.y + 1.6f && dropping)
            //{
            //    gameObject.layer = 8;
            //    if(collisionY == 0)
            //    {
            //        collisionY = collision.transform.position.y;
            //    }
            //} else if(dropping)
            //{
            //    gameObject.layer = 0;
            //    dropping = false;
            //}
        }
    }
}
