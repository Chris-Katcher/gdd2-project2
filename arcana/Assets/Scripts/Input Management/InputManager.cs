/************************************************
 * InputManager.cs
 * 
 * This file contains:
 * - The InputManager class.
 * - The InputMethod enum.
 * - The OperatingSystem enum.
 * - The Device enum.
 * - The Director enum.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Utilities;

namespace Arcana.InputManagement
{

    #region Class: InputManager class.

    /////////////////////
    // Manager declaration.
    /////////////////////

    /// <summary>
    /// Handles all input functionality and controller mapping functions.
    /// </summary>
    [AddComponentMenu("Arcana/Managers/InputManager")]
    public class InputManager : ArcanaObject
    {

        #region Static Methods.

        #region Enum Parsing Methods.

        #region Input Method Parsing Method.

        /// <summary>
        /// Parse the type of the enum as a string.
        /// </summary>
        /// <param name="_method">Enum value to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(InputMethod _method)
        {
            string result = "";

            switch (_method)
            {
                case InputMethod.Button:
                    result = "(Button)";
                    break;
                case InputMethod.Key:
                    result = "(Key)";
                    break;
                case InputMethod.MouseButton:
                    result = "(Mouse Button)";
                    break;
                case InputMethod.GamepadButton:
                    result = "(Gamepad Button)";
                    break;
                case InputMethod.DPadButton:
                    result = "(D-Pad Button)";
                    break;
                case InputMethod.JoystickButton:
                    result = "(Joystick Button)";
                    break;
                case InputMethod.Axis:
                    result = "(Axis)";
                    break;
                case InputMethod.MouseWheel:
                    result = "(Mouse Wheel)";
                    break;
                case InputMethod.DPadAxis:
                    result = "(D-Pad Axis)";
                    break;
                case InputMethod.JoystickAxis:
                    result = "(Joystick Axis)";
                    break;
                case InputMethod.TriggerAxis:
                    result = "(Trigger Axis)";
                    break;
                case InputMethod.LeftTriggerAxis:
                    result = "(Left Trigger Axis)";
                    break;
                case InputMethod.RightTriggerAxis:
                    result = "(Right Trigger Axis)";
                    break;
                default:
                    result = "(Unknown Input Method)";
                    break;
            }

            return result;
        }

        #endregion

        #region Operating System Parsing Method.

        /// <summary>
        /// Parse the type of the enum as a string.
        /// </summary>
        /// <param name="_os">Enum value to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(OperatingSystem _os)
        {
            string result = "";

            switch (_os)
            {
                case OperatingSystem.Windows:
                    result = "(Windows)";
                    break;
                case OperatingSystem.MacOS:
                    result = "(MacOS)";
                    break;
                case OperatingSystem.LINUX:
                    result = "(LINUX distro)";
                    break;
                default:
                    result = "(Unknown OS)";
                    break;
            }

            return result;
        }

        #endregion

        #region Device Parsing Method.

        /// <summary>
        /// Parse the type of the enum as a string.
        /// </summary>
        /// <param name="_device">Enum value to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(Device _device)
        {
            string result = "";

            switch (_device)
            {
                case Device.Keyboard:
                    result = "(Keyboard)";
                    break;
                case Device.Mouse:
                    result = "(Mouse)";
                    break;
                case Device.XInput:
                    result = "(XInput)";
                    break;
                case Device.DInput:
                    result = "(D-Input)";
                    break;
                default:
                    result = "(Unknown Input Device)";
                    break;
            }

            return result;
        }

        #endregion

        #region Director Parsing Method.

        /// <summary>
        /// Parse the type of the enum as a string.
        /// </summary>
        /// <param name="_director">Enum value to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(Director _director)
        {
            string result = "";

            switch (_director)
            {
                case Director.Debug:
                    result = "(Debugging Director)";
                    break;
                case Director.SystemController:
                    result = "(System Controller)";
                    break;
                case Director.Player1:
                    result = "(Player 1)";
                    break;
                case Director.Player2:
                    result = "(Player 2)";
                    break;
                case Director.State:
                    result = "(State)";
                    break;
                default:
                    result = "(Unknown Director)";
                    break;
            }

            return result;
        }

        #endregion

        #endregion

        #region Instancing Methods.

        /////////////////////
        // Static methods for instancing.
        /////////////////////

        /// <summary>
        /// Static instance of the class. (We only want one).
        /// </summary>
        public static InputManager instance = null;

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        /// <returns>Returns a component.</returns>
        public static InputManager GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of InputManager.");
                instance = Services.CreateEmptyObject("Input Manager").AddComponent<InputManager>();
            }

            return instance;
        }

        /// <summary>
        /// Returns true if instance exists.
        /// </summary>
        /// <returns>Returns boolean indicating instance existence.</returns>
        public static bool HasInstance()
        {
            return (instance != null);
        }

        #endregion

        #region Component Factory Methods.

        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static InputManager Create(ArcanaObject _parent)
        {
            if (!HasInstance())
            {
                instance = _parent.GetComponent<InputManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<InputManager>(_parent);
            }

            return instance;
        }

        #endregion

        #endregion

        #region Data Members.

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        // TODO: Handle a queue of commands.

        #endregion

        #region Properties.

        /////////////////////
        // Properities.
        /////////////////////

        // TODO: Handle a queue of commands.

        #endregion

        #endregion

        #region Initialization Methods.
                    
        /// <summary>
        /// Create the data members for the InputManager.
        /// </summary>
        public override void Initialize()
        {
            if (this.Initialized)
            {
                // Initialize the base values.
                base.Initialize();

                // Set this name.
                this.Name = "Input Manager";

                // Initialize the input manager.
                Debugger.Print("Initializing input manager.", this.Self.name);
                
                // Make the new list.
                // TODO: make basic collections.

                // This isn't a poolable element.
                this.IsPoolable = false;
            }
        }

        #endregion
        
    }

    #endregion
    
    #region Class: InputManager_d class.

    /// <summary>
    /// Handles input functionality.
    /// </summary>
    public class InputManager_d : MonoBehaviour
    {
        

        #region Data Members.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Creates control schemse for each player.
        /// </summary>
        private Dictionary<Director, ControlScheme> m_schemes;

        /// <summary>
        /// Initialization flag.
        /// </summary>
        private bool m_initialized;

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns collection of all control schemes.
        /// </summary>
        public Dictionary<Director, ControlScheme> ControlSchemes
        {
            get { return this.m_schemes; }
        }

        #endregion

        #region Service Methods.

        /////////////////////
        // Service methods.
        /////////////////////

        #region UnityEngine Methods.

        /// <summary>
        /// Run when the component is created for the very first time.
        /// </summary>
        public void Start()
        {
            // Start method.
        }

        /// <summary>
        /// Update meta information about input.
        /// </summary>
        public void Update()
        {
            // Update.
        }

        #endregion
        
        #region Accessor Methods.

        /// <summary>
        /// Get the axis value.
        /// </summary>
        /// <param name="_controller">Controller requesting axis data.</param>
        /// <param name="_axis">Axis to check.</param>
        /// <returns>Returns value.</returns>
        public float GetAxis(Director _controller = Director.SystemController, string _axis = "")
        {
            if (_controller == Director.Debug)
            {
                // If a debug controller is requesting input and this isn't in debug mode, return false.
                if (!Debugger.DEBUG_MODE)
                {
                    return 0.0f;
                }
            }

            // Check player index compared to control scheme.
            if (ControlSchemes.ContainsKey(_controller))
            {
                return ControlSchemes[_controller].GetAxis(_axis);
            }

            // If no scheme, no action.
            return 0.0f;
        }


        /// <summary>
        /// Get the raw axis value.
        /// </summary>
        /// <param name="_controller">Controller requesting axis data.</param>
        /// <param name="_axis">Axis to check.</param>
        /// <returns>Returns value.</returns>
        public float GetAxisRaw(Director _controller = Director.SystemController, string _axis = "")
        {
            if (_controller == Director.Debug)
            {
                // If a debug controller is requesting input and this isn't in debug mode, return false.
                if (!Debugger.DEBUG_MODE)
                {
                    return 0.0f;
                }
            }

            // Check player index compared to control scheme.
            if (ControlSchemes.ContainsKey(_controller))
            {
                return ControlSchemes[_controller].GetAxisRaw(_axis);
            }

            // If no scheme, no action.
            return 0.0f;
        }

        /// <summary>
        /// Return flag checking if an action has been performed, based on the input player index.
        /// </summary>
        /// <param name="_action">Action being checked for.</param>
        /// <param name="_controller">Controller requesting action check.</param>
        /// <returns>Returns a boolean true if action has been completed.</returns>
        public bool GetAction(Director _controller = Director.SystemController, Actions _action = Actions.Idle)
        {
            if (_controller == Director.Debug)
            {
                // If a debug controller is requesting input and this isn't in debug mode, return false.
                if (!Debugger.DEBUG_MODE)
                {
                    return false;
                }
            }

            // Check player index compared to control scheme.
            if (ControlSchemes.ContainsKey(_controller))
            {
                return ControlSchemes[_controller].TriggerAction(_action);
            }

            // If no scheme, no action.
            return false;
        }

        /// <summary>
        /// Returns control scheme for requesting controller, if it exists.
        /// </summary>
        /// <param name="_controller">Controller requesting scheme.</param>
        /// <returns>Returns a ControlScheme object.</returns>
        public ControlScheme GetScheme(Director _controller)
        {
            if (ControlSchemes.ContainsKey(_controller))
            {
                return ControlSchemes[_controller];
            }

            return null;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Add a control scheme to the manager.
        /// </summary>
        /// <param name="_controller">Controller.</param>
        /// <param name="_scheme">Scheme to add.</param>
        public void AddControlScheme(Director _controller, ControlScheme _scheme)
        {
            if (!ControlSchemes.ContainsKey(_controller))
            {
                ControlSchemes.Add(_controller, _scheme);
            }
            else
            {
                // Overwrite existing if it is there.
                ControlSchemes[_controller] = _scheme;
            }
        }

        #endregion

        #endregion

        #region Deprecated Functions

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
        public bool getProjectileFire()
        {
            if (Input.GetAxis("Fire1Controller") != 0)
            {
                return true;
            }
            return (bool)Input.GetButtonDown("Fire1");
        }

        #endregion

    }

    #endregion

    #region Enum: InputMethod

    /// <summary>
    /// Represents the input method that UnityEngine interfaces with.
    /// </summary>
    public enum InputMethod
    {
        /// <summary>
        /// General button. (Can encompass Key, MouseButton, JoystickButton, or GamepadButton).
        /// </summary>
        Button,

        /// <summary>
        /// Represents Keyboard keys, with <see cref="KeyCode"/>s from UnityEngine.
        /// </summary>
        Key,

        /// <summary>
        /// Represents mouse buttons.
        /// </summary>
        MouseButton,

        /// <summary>
        /// Represents gamepad buttons. Some may use UnityEngine <see cref="KeyCode"/>s.
        /// </summary>
        GamepadButton,
        
        /// <summary>
        /// Represents the joystick buttons.
        /// </summary>
        JoystickButton,

        /// <summary>
        /// Represents the digital pad buttons. (May be treated as axis by certain controllers).
        /// </summary>
        DPadButton,

        /// <summary>
        /// General axis. (Can encompass MouseWheel, JoystickAxis, DPadAxis, TriggerAxis).
        /// </summary>
        Axis,

        /// <summary>
        /// Represents the mouse wheel.
        /// </summary>
        MouseWheel,

        /// <summary>
        /// Represents the digital pad axis. (May be treated as buttons by certain controllers).
        /// </summary>
        DPadAxis,

        /// <summary>
        /// Represents the joystick axis.
        /// </summary>
        JoystickAxis,

        /// <summary>
        /// Represents the trigger axes.
        /// </summary>
        TriggerAxis,

        /// <summary>
        /// Represents the left trigger axis.
        /// </summary>
        LeftTriggerAxis,

        /// <summary>
        /// Represents the right trigger axis.
        /// </summary>
        RightTriggerAxis
    }

    #endregion

    #region Enum: OS.

    /// <summary>
    /// Represents the operating system.
    /// </summary>
    public enum OperatingSystem
    {
        /// <summary>
        /// Represents a Windows 10, Windows 7, or Windows 8.1 system.
        /// </summary>
        Windows,

        /// <summary>
        /// Represents a MacOS system.
        /// </summary>
        MacOS,

        /// <summary>
        /// Represents a common UNIX distro.
        /// </summary>
        LINUX
    }

    #endregion

    #region Enum: Device.

    /// <summary>
    /// Represents the interface a user may be using to 'input' values to the system.
    /// </summary>
    public enum Device
    {
        /// <summary>
        /// Represents keyboard inputs.
        /// </summary>
        Keyboard,

        /// <summary>
        /// Represents the mouse buttons.
        /// </summary>
        Mouse,

        /// <summary>
        /// Represents Xbox controllers.
        /// </summary>
        XInput,

        /// <summary>
        /// Represents Direct input devices. (Usually non-XInput controllers, eg. Logitech controllers).
        /// </summary>
        DInput
    }

    #endregion

    #region Enum: Director.

    /// <summary>
    /// Represents the requesting 'director' that may be requesting a certain command.
    /// </summary>
    public enum Director
    {
        /// <summary>
        /// The SystemController 
        /// </summary>
        SystemController,
        State,
        Debug,
        Player1,
        Player2
    }

    #endregion
    
}
