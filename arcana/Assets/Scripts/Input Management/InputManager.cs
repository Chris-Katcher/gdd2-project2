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
    [AddComponentMenu("Arcana/Managers/Input Manager")]
    public class InputManager : ArcanaObject
    {

        #region Static Methods.

        #region Enum Parsing Methods.
        
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
                case Director.System:
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
        /// Static instance of the input manager class.
        /// </summary>
        public static InputManager instance = null;

        /// <summary>
        /// Returns true if instance exists.
        /// </summary>
        /// <returns>Returns boolean indicating instance existence.</returns>
        public static bool HasInstance()
        {
            return (instance != null);
        }

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        /// <returns>Returns a component.</returns>
        public static InputManager GetInstance()
        {
            if (!HasInstance())
            {
                instance = Create(null);
            }

            return instance;
        }


        #endregion

        #region Component Factory Methods.

        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <param name="_parent">Object that the component will be added to.</param>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static InputManager Create(ArcanaObject _parent = null)
        {
            ArcanaObject parent = _parent;

            if (parent == null)
            {
                parent = Services.CreateEmptyObject().AddComponent<ArcanaObject>();
                parent.Initialize();
            }

            if (!HasInstance())
            {
                instance = parent.GetComponent<InputManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<InputManager>(parent);
                instance.Initialize();
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

        /// <summary>
        /// Number of controllers plugged into the system.
        /// </summary>
        private int m_controllerCount;

        /// <summary>
        /// Timer in place for controller check updates.
        /// </summary>
        private StatTracker m_tracker;

        /// <summary>
        /// Creates control schemse for each player.
        /// </summary>
        private Dictionary<Director, ControlScheme> m_schemes;

        #endregion

        #region Properties.

        /////////////////////
        // Properities.
        /////////////////////

        /// <summary>
        /// Returns collection of all control schemes.
        /// </summary>
        public Dictionary<Director, ControlScheme> ControlSchemes
        {
            get {
                if (this.m_schemes == null)
                {
                    this.m_schemes = new Dictionary<Director, ControlScheme>();
                }
                return this.m_schemes;
            }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Have input management in the late update.
        /// </summary>
        public override void Update()
        {
            // Call the base update.
            base.Update();

            // Update the tracker.
            UpdateCounter();

            // Check if the tracker is expired.
            if (m_tracker.IsMinimum())
            {
                // Perform the update that needs to be done.
                UpdateControllerStates();

                // Reset the counter.
                this.m_tracker.Reset();
            }
        }

        #endregion

        #region Update Methods.

        /// <summary>
        /// Update the tracker.
        /// </summary>
        public void UpdateCounter()
        {
            // Decrement by the change in time.
            this.m_tracker.Decrement(Time.deltaTime);
        }

        /// <summary>
        /// Update the controller states.
        /// </summary>
        public void UpdateControllerStates()
        {
            // Get the array of joystick names.
            string[] joys = Input.GetJoystickNames();

            // Check controller count.
            if (this.m_controllerCount != joys.Length)
            {
                // Change in the number of controllers has occured.
                Debugger.Print("There are " + joys.Length + " controllers.", this.Name, this.Debug);
                this.m_controllerCount = Input.GetJoystickNames().Length;

                // Print out each of the joysticks in the array.
                for (int i = 0; i < joys.Length; i++)
                {
                    // Print statement made for each controller.
                    Debugger.Print("[" + i + "] Controller " + (i + 1).ToString() + ": \'" + joys[i] + "\'.", this.Name, this.Debug);
                }
            }

            // If there are no controllers, then there are none.
            if (this.m_controllerCount == 0)
            {
                Debugger.Print("There are no controllers.", this.Name, this.Debug);
            }            
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Create the data members for the InputManager.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Initialize the base values.
                base.Initialize();

                // Initialize the members.
                InputManager.instance = this;
                this.Name = "Arcana (Input Manager)";
                this.m_controllerCount = 0; // Set the initial controller count.
                this.IsPoolable = false; // This isn't a poolable element.
                this.transform.position = Vector3.zero; // Ensure no movement.
                this.transform.rotation = Quaternion.Euler(Vector3.zero); // Ensure no rotation.

                // Initialize the input manager.
                Debugger.Print("Initializing input manager.", this.Self.name, this.Debug);

                // Make the timer.
                this.m_tracker = new StatTracker("Controller Connection Check", 3.0f, 0.0f, 4.0f, -1.0f, -1.0f);
                this.m_tracker.Reset(); // Set up the tracker for the start.

                // Activate and run the manager.
                this.Activate();
                this.Run();
            }
        }

        #endregion

        #region Processing Methods.

        /// <summary>
        /// Process the input joysticks.
        /// </summary>
        /// <param name="_joysticks">Joysticks.</param>
        public void ProcessJoysticks(string[] _joysticks)
        {
            // If we have at least one controller, we want to "register" it.
            if (this.m_controllerCount > 0)
            {
                // 


            }
        }

        #endregion

        #region Misc.

        /// <summary>
        /// Registers a new control scheme, or overwrites an existing one.
        /// </summary>
        /// <param name="_director">Director to register control scheme to.</param>
        /// <param name="_scheme">Register the scheme.</param>
        public ControlScheme RegisterControlScheme(Director _director, ControlScheme _scheme)
        {
            if (!this.ControlSchemes.ContainsKey(_director))
            {
                this.ControlSchemes.Add(_director, _scheme);
            }
            else
            {
                this.ControlSchemes[_director] = _scheme;
            }
            
            return _scheme;
        }

        /// <summary>
        /// Creates a new control scheme and adds it to the parent.
        /// </summary>
        /// <param name="_parent">Parent the scheme is being added to.</param>
        /// <returns></returns>
        public ControlScheme AddControlScheme(ArcanaObject _parent)
        {
            Director director = Director.System;
            ControlScheme scheme = null;

            if (_parent != null)
            {
                director = _parent.Director;
            }

            if (director != Director.None)
            {
                scheme = ControlScheme.Create(_parent);
                RegisterControlScheme(director, scheme);
            }

            return scheme;
        }

        /// <summary>
        /// Get the control scheme associated with the director value.
        /// </summary>
        /// <param name="_director">Director to return.</param>
        /// <returns></returns>
        public ControlScheme GetControlScheme(Director _director)
        {
            if (this.ControlSchemes.ContainsKey(_director))
            {
                return this.ControlSchemes[_director];
            }

            return null;
        }

        #endregion

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
        DInput,

        /// <summary>
        /// Represents the logitech dual action controller.
        /// </summary>
        LogitechDualAction
    }

    #endregion

    #region Enum: Director.

    /// <summary>
    /// Represents the requesting 'director' that may be requesting a certain command.
    /// </summary>
    public enum Director
    {
        /// <summary>
        /// The system controller can handle high-level input.
        /// </summary>
        System,

        /// <summary>
        /// Input specific to a particular state. (eg. It's HUD).
        /// </summary>
        State,

        /// <summary>
        /// Camera controls.
        /// </summary>
        Camera,

        /// <summary>
        /// Any player inputs.
        /// </summary>
        Player,

        /// <summary>
        /// Inputs explicitly for player 1.
        /// </summary>
        Player1,

        /// <summary>
        /// Inputs explicitly for player 2.
        /// </summary>
        Player2,

        /// <summary>
        /// Debugger director.
        /// </summary>
        Debug,

        /// <summary>
        /// Null director value.
        /// </summary>
        None

    }

    #endregion
    
    /*
    
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
        /// Initialization flag.
        /// </summary>
        private bool m_initialized;

        /////////////////////
        // Properties.
        /////////////////////

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
        public float GetAxis(Director _controller = Director.System, string _axis = "")
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
            if (this.ControlSchemes.ContainsKey(_controller))
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
    
    */

}
