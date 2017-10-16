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

        void OnCollisionStay2D(Collision2D coll)
        {
            if(coll.transform.name == "Cube")
            {
                jump_enabled = true;
            } else
            {
                jump_enabled = false;
            }
            
        }
    }
}
