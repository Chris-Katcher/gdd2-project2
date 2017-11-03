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
            float translation = Input.GetAxis("Horizontal1");

            return translation;
        }

        public float getPlayer2Translation()
        {
            float translation = Input.GetAxis("Horizontal2");

            return translation;
        }

        /// <summary>
        /// Return a vector2 of the current facing direction of the right joystick
        /// </summary>
        /// <returns></returns>
        public Vector2 getVectorInputP1()
        {

            //create and set input||Vector2
            Vector2 input = new Vector2();
            input.x = Input.GetAxis("RightJoystickHorizontal1");
            input.y = Input.GetAxis("RightJoystickVerticle1");

            //return
            return input;

        }

        public Vector2 getVectorInputP2()
        {

            //create and set input||Vector2
            Vector2 input = new Vector2();
            input.x = Input.GetAxis("RightJoystickHorizontal2");
            input.y = Input.GetAxis("RightJoystickVerticle2");

            //return
            return input;

        }

        //returns whether or not the player has pressed the jump button
        public bool getPlayer1Jump()
        {
            return (bool)Input.GetButtonDown("Jump") || Input.GetButtonDown("AButton1");
        }

        //returns whether or not the player has pressed the jump button
        public bool getPlayer2Jump()
        {
            return (bool)Input.GetButtonDown("Jump") || Input.GetButtonDown("AButton2");
        }

        //returns whether or not the player has pressed the fire button
        //returns whether or not the player has pressed the fire button

        // The Red element 
        public bool getProjectileFireP1()
		{
			return (bool)Input.GetButtonDown("BButton1");
		}

        // The Red element 
        public bool getProjectileFireP2()
        {
            return (bool)Input.GetButtonDown("BButton2");
        }

        // The Blue element 
        public bool getProjectileFire2P1()
		{
			return (bool)Input.GetButtonDown("XButton1");

		}

        // The Blue element 
        public bool getProjectileFire2P2()
        {
            return (bool)Input.GetButtonDown("XButton2");

        }

        // The Green element 
        public bool getProjectileFire3P1()
		{
			return (bool)Input.GetButtonDown("YButton1");
		}

        // The Green element 
        public bool getProjectileFire3P2()
        {
            return (bool)Input.GetButtonDown("YButton2");
        }

        //returns a boolean of whether the right trigger has been pressed down
        public bool getRightTriggerP1()
		{

			return (bool)Input.GetButtonDown("RightBumper1");

		}

        //returns a boolean of whether the right trigger has been pressed down
        public bool getRightTriggerP2()
        {

            return (bool)Input.GetButtonDown("RightBumper2");

        }
        //returns a boolean of whether the right trigger has been presed down
        public bool getLeftTriggerP1()
		{

			return (bool)Input.GetButtonDown("LeftBumper1");

		}
        //returns a boolean of whether the right trigger has been presed down
        public bool getLeftTriggerP2()
        {

            return (bool)Input.GetButtonDown("LeftBumper2");

        }

        public bool getPlayerDrop()
        {
            return (bool)Input.GetButtonDown("Drop");
        }

        public bool getEscPressed()
        {
            return (bool)Input.GetKeyDown(KeyCode.Escape);
        }
    }

    
}
