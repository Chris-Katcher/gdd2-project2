/************************************************
 * InputManager.cs
 * 
 * This file contains implementation for the InputManager class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.InputManagement
{
    /// <summary>
    /// Handles input functionality.
    /// </summary>
    public class InputManager: MonoBehaviour
    {

        // TODO: Stub.
        public float getPlayer1Translation()
        {
            float translation = Input.GetAxis("Horizontal");

            return translation;
        }

        /// <summary>
        /// Return a vector2 of the current facing direction of the right joystick
        /// </summary>
        /// <returns></returns>
        public Vector2 getVectorInput()
        {

            //create and set input||Vector2
            Vector2 input = new Vector2();
            input.x = Input.GetAxis("RightJoystickHorizontal");
            input.y = Input.GetAxis("RightJoystickVerticle");

            //return
            return input;

        }

        public bool getPlayer1Jump()
        {
            return (bool)Input.GetButtonDown("AButton");
        }

        public bool getProjectileFire()
        {
            return (bool)Input.GetButtonDown("Fire1");
        }
    }

    
}
