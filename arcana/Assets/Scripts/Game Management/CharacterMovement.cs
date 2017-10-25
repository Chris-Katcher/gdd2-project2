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
        }
    }
}
