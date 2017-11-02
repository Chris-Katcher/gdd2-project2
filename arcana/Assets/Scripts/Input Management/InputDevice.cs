/************************************************
 * InputDevice.cs
 * 
 * This file contains:
 * - The InputDevice class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.InputManagement
{

    /// <summary>
    /// Represents the various controllers that can be used.
    /// </summary>
    public enum InputDevice
    {

        /// <summary>
        /// Refers to the player 1 keyboard controls.
        /// </summary>
        KeyboardP1,

        /// <summary>
        /// Refers to the Player 2 keyboard controls.
        /// </summary>
        KeyboardP2,

        /// <summary>
        /// Refers to an X-Input Controller.
        /// </summary>
        XboxController,

        /// <summary>
        /// Refers to a Playstation 4 controller.
        /// </summary>
        PS4Controller,

        /// <summary>
        /// Refers to a logitech-series controller.
        /// </summary>
        LogitechController

    }

    public class Controllers
    {
        public const string INPUT_P1_KB = "PLAYER 1";
        public const string INPUT_P2_KB = "PLAYER 2";
        public const string INPUT_XBOX_ONE = "CONTROLLER (XBOX ONE FOR WINDOWS)";
        public const string INPUT_XBOX_360 = "CONTROLLER (XBOX 360 FOR WINDOWS)";
        public const string INPUT_PS4 = "WIRELESS CONTROLLER";
        public const string INPUT_LOGITECH_F710 = "LOGITECH DUAL ACTION";
        public const string INPUT_LOGITECH_RUMBLEPAD2 = "LOGITECH CORDLESS RUMBLEPAD 2";
        public const string INPUT_LOGITECH_F710_D = "CONTROLLER (WIRELESS GAMEPAD F710)";


    }

    /* public InputDevice ParseJoystick(string _joystickName)
    {
        // Checks the joystick name against values.


    }

    */


    /*

    /// <summary>
    /// Input device represents a physical controller.
    /// </summary>
    public class InputDevice
    {

        /// <summary>
        /// Uniquie ID given to the input device.
        /// </summary>
        private int m_deviceID;

        /// <summary>
        /// The methods that this input device can tolerate.
        /// </summary>
        private List<InputMethod> m_methods = null;

        /// <summary>
        /// Type of input device.
        /// </summary>
        private DeviceType m_deviceType;

        /// <summary>
        /// JoystickNames index value, if this is a joystick.
        /// </summary>
        private int m_joyID;

        /// <summary>
        /// Name of the device.
        /// </summary>
        private string m_deviceName;

        /// <summary>
        /// Buttons on the device.
        /// </summary>
        List<KeyCode> m_buttons;

        /// <summary>
        /// Number of buttons on the device, if it is a controller.
        /// </summary>
        private int m_numberButtons;

        /// <summary>
        /// Axes on the device.
        /// </summary>
        List<string> m_axes;

        /// <summary>
        /// Number of axes on the device, if it is a controller.
        /// </summary>
        private int m_numberAxes;

        /// <summary>
        /// An input device is anything a user can interact with to put input into the system.
        /// </summary>
        /// <param name="_type">The type of device.</param>
        /// <param name="_name">Name of the device.</param>
        public InputDevice(int _id, DeviceType _type, string _name)
        {
            // Set the unique identifying information about the controller.

            // ID lets us know which controller we're dealing with globally.
            this.m_deviceID = _id;

            // Type is the "type" of controller we're dealing with.
            this.m_deviceType = _type;

            // Name is the model we're interacting with.
            this.m_deviceName = _name;

            // Create the list.
            this.m_methods = new List<InputMethod>();

        }

        


        



    }

    */
}
