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

        //returns whether or not the player has pressed the jump button
        public bool getPlayer1Jump()
        {
            return (bool)Input.GetButtonDown("Jump");
        }

		//returns whether or not the player has pressed the fire button
		//returns whether or not the player has pressed the fire button

		// The Red element 
		public bool getProjectileFire()
		{
			return (bool)Input.GetButtonDown("BButton");
		}

		// The Blue element 
		public bool getProjectileFire2()
		{
			return (bool)Input.GetButtonDown("XButton");

		}

		// The Green element 
		public bool getProjectileFire3()
		{
			return (bool)Input.GetButtonDown("YButton");
		}

		//returns a boolean of whether the right trigger has been pressed down
		public bool getRightTrigger()
		{

			return (bool)Input.GetButtonDown("RightBumper");

		}
		//returns a boolean of whether the right trigger has been presed down
		public bool getLeftTrigger()
		{

			return (bool)Input.GetButtonDown("LeftBumper");

		}

		public bool getPlayerDrop()
        {
            return (bool)Input.GetButtonDown("Drop");
        }
    }

    
}
