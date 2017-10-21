/************************************************
 * Debugger.cs
 * 
 * Debugger prints to the console for Unity.
 ************************************************/
 
/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana
{

    #region // Static Class: Debugger class.

    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// Debugger prints values to the Unity3D console.
    /// </summary>
    public static class Debugger
    {

        #region Data Members

        /////////////////////
        // Class members.
        /////////////////////

        // Constant debug reference. //

        /// <summary>
        /// Declares whether debug messages should print.
        /// </summary>
        public static bool DEBUG_MODE = true;

        #endregion

        #region Static Methods

        /// <summary>
        /// Prints input message to the Unity3D debug console, if in debug mode.
        /// </summary>
        /// <param name="_message">Message to print to the log.</param>
        /// <param name="_title">Title of message. There is no title, by default.</param>
        public static void Print(string _message, string _title = "")
        {
            if (DEBUG_MODE)
            {
                string message = "";

                if (_title.Length > 0)
                {
                    message = _title + ": ";
                }

                message += _message;

                Debug.Log(message);
            }
        }

        /// <summary>
        /// Pauses the editor, with the option to print an identifying message to the console.
        /// </summary>
        /// <param name="_message">Message to print to the log.</param>
        /// <param name="_title">Title of message. There is no title, by default.</param>
        public static void Break(string _message = "", string _title = "")
        {
            if (DEBUG_MODE)
            {
                if (_message.Length > 0)
                {
                    Print(_message, _title);
                }

                Debug.Break();
            }
        }

        /// <summary>
        /// Compare two values in an assertion.
        /// </summary>
        /// <typeparam name="T">The type of the objects to compare.</typeparam>
        /// <param name="a">Left side value.</param>
        /// <param name="b">Right side value.</param>
        /// <param name="_message">Message to print to the log.</param>
        /// <param name="_title">Title of message. There is no title, by default.</param>
        public static void Assert<T>(IComparable<T> a, IComparable<T> b, string _message = "", string _title = "")
        {
            if (DEBUG_MODE)
            {

                // Get the assertion.
                bool assertion = (a == b);

                // Prepare the result.
                string result = "Assertion Result: ";
                result += assertion.ToString();

                // Prepare the message.
                if (_message.Length > 0)
                {
                    result += " || " + _message;
                    Print(result, _title);
                }

                Debug.Assert(a == b);
            }

        }

        /// <summary>
        /// Clear errors from the developer console.
        /// </summary>
        public static void Clear()
        {
            if (DEBUG_MODE)
            {
                Debug.ClearDeveloperConsole();
            }
        }

        /// <summary>
        /// Toggle the debug mode flag.
        /// </summary>
        public static void ToggleDebugMode()
        {
            DEBUG_MODE = !DEBUG_MODE;
        }

        /// <summary>
        /// Set debug mode flag.
        /// </summary>
        /// <param name="_flag">Debug mode flag.</param>
        public static void SetDebugMode(bool _flag)
        {
            DEBUG_MODE = _flag;
        }

        #endregion
    }

    #endregion

}
