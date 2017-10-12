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

namespace Game
{
    /// <summary>
    /// Handles input functionality.
    /// </summary>
    public class InputManager: MonoBehaviour
    {

        // TODO: Stub.
        public float getPlayer1Translation()
        {
            // Declare UnityEngine references directly; this will help us know where code is coming from.
            float translation = Input.GetAxis("Horizontal");

            return translation;
        }
    }

    
}
