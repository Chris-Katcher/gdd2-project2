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

namespace Game
{
    /// <summary>
    /// Handles the main game loop.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public GameObject wizzard1Clone;
        public bool m_init = false;
        // TODO: Stub.

        public void Initialize()
        {
            GameObject wizzard1 = Instantiate(wizzard1Clone, transform.position, Quaternion.identity) as GameObject;
            m_init = true;
        }

        private bool IsInitialized()
        {
            return m_init;
        }
    }
}
