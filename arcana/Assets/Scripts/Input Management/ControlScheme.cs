using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.InputManagement
{

    #region Enum: Actions.

    /// <summary>
    /// Enum keeps track of possible in-game commands.
    /// </summary>
    public enum Actions
    {
        ChangeCameraBackground,
        Idle,
        Fire,
        Jump,
        Crouch,
        MoveLeft,
        MoveRight,
        Left,
        Right,
        Up,
        Down,
        Select,
        Back,
        Escape,
        Pause,
        Resume,
        CastFire,
        CastWater,
        CastEarth
    }

    #endregion
        
    #region Class: ControlScheme class.

    /// <summary>
    /// The control scheme translates 'commands' to 'inputs' and sends back event responses.
    /// </summary>
    public class ControlScheme
    {

        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Control scheme created for a single player.
        /// </summary>
        private Dictionary<Actions, CommandSequence> m_controls = null;
        
        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Reference to the control scheme.
        /// </summary>
        public Dictionary<Actions, CommandSequence> Controls
        {
            get { return this.m_controls; }
        }

        #endregion

        #region Constructor.

        /// <summary>
        /// Control Schemes are just repositories for control mappings.
        /// </summary>
        public ControlScheme()
        {
            this.m_controls = new Dictionary<Actions, CommandSequence>();
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns true if action has been triggered.
        /// </summary>
        /// <param name="_action">Action to check for.</param>
        /// <returns>Returns true if action has been triggered; false if action hasn't or hasn't been added to the collection.</returns>
        public bool TriggerAction(Actions _action)
        {
            CommandSequence input = GetActionTrigger(_action);
            if (input != null)
            {
                return TriggerAction(_action, input);
            }
            else
            {
                Debugger.Print("Action doesn't have a command sequence set up.");
                return false;
            }
        }

        /// <summary>
        /// If true, the action should be triggered.
        /// </summary>
        /// <param name="_action">Action to be performed.</param>
        /// <param name="_input">Input sequence.</param>
        /// <returns></returns>
        private bool TriggerAction(Actions _action, CommandSequence _input)
        {
            return (this.m_controls.ContainsKey(_action) && this.m_controls[_action].Matches(_input));
        }

        /// <summary>
        /// Returns the action's trigger command.
        /// </summary>
        /// <param name="_action">Action to be performed.</param>
        /// <returns>Returns the input sequence required.</returns>
        private CommandSequence GetActionTrigger(Actions _action)
        {
            if (this.m_controls.ContainsKey(_action))
            {
                return this.m_controls[_action];
            }

            return null;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Add an action with its mapped command.
        /// </summary>
        /// <param name="_action">Action to perform.</param>
        /// <param name="_input">Mapped input.</param>
        public void AddControl(Actions _action, CommandSequence _input)
        {
            if (this.m_controls.ContainsKey(_action))
            {
                // Overwrite the value.
                this.m_controls[_action] = _input;
            }
            else
            {
                // Add the scheme.
                this.m_controls.Add(_action, _input);
            }
        }
        
        #endregion

    }

    #endregion

    #region Class: Command class.

    /// <summary>
    /// Represents the type of commands that might exists.
    /// </summary>
    public enum CommandTypes
    {
        // Unassigned.
        Unassigned,

        // Mouse controls.
        MouseMovement,
        MouseButton,
        MouseWheel,

        // Keyboard controls.
        Key,
        
        // Gamepad.
        Button,
        Axis
    }

    /// <summary>
    /// A command class represents input that Unity can handle.
    /// </summary>
    public class Command {
        
        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Type of command. (Determines how it will be processed).
        /// </summary>
        private CommandTypes m_type = CommandTypes.Unassigned;

        /// <summary>
        /// The keycode associated with keyboard and gamepad button presses.
        /// </summary>
        private KeyCode m_keyCode = KeyCode.Numlock; // Whose really going to use numlock as a command?

        /// <summary>
        /// Mouse button number associated with mouse button presses.
        /// </summary>
        private int m_mouseButton = -1;

        /// <summary>
        /// Name of the command for joystick and mouse axes.
        /// </summary>
        private string m_name = "";

        /// <summary>
        /// The timeout associated with a command. Timeouts prevent players from spamming commands.
        /// </summary>
        private float m_timeout = 0.5f;

        /// <summary>
        /// Deadzone to compare axis input to.
        /// </summary>
        private float m_deadzone = 0.0f;
        
        /////////////////////
        // Properties.
        /////////////////////
        
        /// <summary>
        /// Reference to the button's handling type.
        /// </summary>
        public CommandTypes CommandType
        {
            get { return this.m_type; }
        }

        /// <summary>
        /// The command doesn't have a ruleset attached to it.
        /// </summary>
        public bool Unassigned
        {
            get { return CommandType == CommandTypes.Unassigned; }
        }

        /// <summary>
        /// Returns true if not an axis.
        /// </summary>
        public bool IsButton
        {
            get { return !IsAxis && !Unassigned; }
        }

        /// <summary>
        /// Returns true if maps to an axis control.
        /// </summary>
        public bool IsAxis
        {
            get
            {
                return CommandType == CommandTypes.Axis || CommandType == CommandTypes.MouseMovement || CommandType == CommandTypes.MouseWheel;
            }
        }

        #endregion

        #region Constructors.

        /// <summary>
        /// Construct an unassigned command.
        /// </summary>
        public Command()
        {
            Reset();
        }

        /// <summary>
        /// Assign a type of button or key to the command. Assumes keyboard by default.
        /// </summary>
        /// <param name="_input">KeyCode to match.</param>
        /// <param name="_type">Type of press.</param>
        /// <param name="_timeout">Timeout of command.</param>
        public Command(KeyCode _input, CommandTypes _type = CommandTypes.Key, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            if (_type == CommandTypes.Button)
            {
                Reset(_type: _type, _code: _input, _timeout: _timeout);
            }
            else
            {
                Reset(_type: CommandTypes.Key, _code: _input, _timeout: _timeout);
            }
        }

        /// <summary>
        /// Assign a mouse button press.
        /// </summary>
        /// <param name="_mouse">Mouse button index.</param>
        /// <param name="_timeout">Timeout of press.</param>
        public Command(int _mouse, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            Reset(_type: CommandTypes.MouseButton, _mouse: _mouse, _timeout: _timeout);
        }
                
        /// <summary>
        /// Reset commands and make unassigned.
        /// </summary>
        private void Reset(
            CommandTypes _type = CommandTypes.Unassigned,
            KeyCode _code = KeyCode.Numlock,
            int _mouse = -1,
            string _name = "",
            float _timeout = Constants.DEFAULT_INPUT_TIMEOUT,
            float _deadzone = 0.0f)
        {
            this.m_type = _type;
            this.m_keyCode = _code;
            this.m_mouseButton = Services.Clamp(_mouse, -1, 2);
            this.m_name = _name;
            this.m_timeout = Services.Abs(_timeout);
        }

        /// <summary>
        /// Assign a joystick axis with a deadzone. (Or, optionally, assign a mouse axis).
        /// </summary>
        /// <param name="_axisName">Name of the axis to return.</param>
        /// <param name="_timeout">Timeout of command.</param>
        public Command(string _axisName, CommandTypes _type = CommandTypes.Axis, float _deadzone = 0.2f, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            if (_type == CommandTypes.MouseMovement)
            {
                Reset(_type: _type, _name: _axisName, _timeout: _timeout);
            }
            else
            {
                Reset(_type: CommandTypes.Axis, _name: _axisName, _timeout: _timeout,  _deadzone: _deadzone);
            }
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Determine if two commands are equal.
        /// </summary>
        /// <param name="other">Command to compare.</param>
        /// <returns>Returns true if they have the same type, code, and timeout.</returns>
        public bool Equals(Command other)
        {
            if (!this.Unassigned && !other.Unassigned) {
                if (this.m_timeout == other.m_timeout)
                {
                    if(this.CommandType == other.CommandType)
                    {
                        if (this.IsAxis)
                        {
                            return this.m_name == other.m_name;
                        }

                        if (this.CommandType == CommandTypes.Key || this.CommandType == CommandTypes.Button)
                        {
                            return this.m_keyCode == other.m_keyCode;
                        }

                        if (this.CommandType == CommandTypes.MouseButton)
                        {
                            return this.m_mouseButton == other.m_mouseButton;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Assign a keycode to the command.
        /// </summary>
        /// <param name="_input">Keyboard/Joystick Button associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignKey(KeyCode _input, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            Reset(_type: CommandTypes.Key, _code: _input, _timeout: _timeout);
        }

        /// <summary>
        /// Assign a control name to the command.
        /// </summary>
        /// <param name="_input">Axis name associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignAxis(string _input, float _timeout = 0.5f)
        {
            Reset(_type: CommandTypes.Axis, _name: _input, _timeout: _timeout);
        }

        /// <summary>
        /// Assign a keycode to the command.
        /// </summary>
        /// <param name="_input">Keyboard/Joystick Button associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignJoystickButton(KeyCode _input, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            Reset(_type: CommandTypes.Button, _code: _input, _timeout: _timeout);
        }

        /// <summary>
        /// Assign a control name to the command.
        /// </summary>
        /// <param name="_input">Mouse button index associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignMouseButton(int _input, float _timeout = 0.5f)
        {
            Reset(_type: CommandTypes.MouseButton, _mouse: _input, _timeout: _timeout);
        }

        /// <summary>
        /// Assign a control name to the command.
        /// </summary>
        /// <param name="_input">Mouse button index associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignMouseMovement(float _timeout = 0.5f)
        {
            Reset(_type: CommandTypes.MouseMovement, _timeout: _timeout);
        }

        #endregion

        #region Accessor Methods.

        #region Keyboard Key / Mouse Button / Gamepad Button Input.

        /// <summary>
        /// Checks if a KeyCode input is being held down.
        /// </summary>
        /// <returns>Returns a response.</returns>
        public bool IsKeyHeld()
        {
            if (CommandType == CommandTypes.Key || CommandType == CommandTypes.Button)
            {
                return Input.GetKey(this.m_keyCode);
            }

            if (CommandType == CommandTypes.MouseButton)
            {
                return Input.GetMouseButton(this.m_mouseButton);
            }

            return false;
        }

        /// <summary>
        /// Checks if a KeyCode input was just released.
        /// </summary>
        /// <returns>Returns a response.</returns>
        public bool IsKeyReleased()
        {
            if (CommandType == CommandTypes.Key || CommandType == CommandTypes.Button)
            {
                return Input.GetKeyUp(this.m_keyCode);
            }

            if (CommandType == CommandTypes.MouseButton)
            {
                return Input.GetMouseButtonUp(this.m_mouseButton);
            }

            return false;
        }


        /// <summary>
        /// Checks if a KeyCode input was just pressed.
        /// </summary>
        /// <returns>Returns a response.</returns>
        public bool IsKeyPressed()
        {
            if (CommandType == CommandTypes.Key || CommandType == CommandTypes.Button)
            {
                return Input.GetKeyDown(this.m_keyCode);
            }

            if (CommandType == CommandTypes.MouseButton)
            {
                return Input.GetMouseButtonDown(this.m_mouseButton);
            }

            return false;
        }

        /// <summary>
        /// Checks if a KeyCode input is not being pressed down.
        /// </summary>
        /// <returns>Returns a response.</returns>
        public bool IsKeyUp()
        {
            if (CommandType == CommandTypes.Key || CommandType == CommandTypes.Button || CommandType == CommandTypes.MouseButton)
            {
                return !IsKeyHeld();
            }

            return true;
        }

        #endregion

        #region Mouse Axis / Joystick Axis Input.

        /// <summary>
        /// Returns the raw value from the appropriate axis input function.
        /// </summary>
        /// <returns>Returns the raw float value from the axis.</returns>
        public float GetAxisRaw()
        {
            if (this.IsAxis)
            {
                return Input.GetAxisRaw(this.m_name);
            }
            else
            {
                return 0.0f;
            }
        }
        
        /// <summary>
        /// Returns the axis value from the appropriate axis input function.
        /// </summary>
        /// <returns>Returns the raw float value from the axis.</returns>
        public float GetAxis()
        {
            if (this.IsAxis)
            {
                return Input.GetAxis(this.m_name);
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Check if the axis is stationary.
        /// </summary>
        /// <returns>Returns a true or false based on the input axis value and deadzone.</returns>
        public bool IsAxisStationary()
        {
            if (this.IsAxis)
            {
                return !IsAxisMoved();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check if the axis is not stationary.
        /// </summary>
        /// <returns>Returns a true or false based on the input axis value and deadzone.</returns>
        public bool IsAxisMoved()
        {
            if (this.IsAxis)
            {
                return (Services.Abs(GetAxis()) > this.m_deadzone);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

    }

    #endregion

    #region Class: CommandResponse class.
    
    /// <summary>
    /// Determines what fires a command.
    /// </summary>
    public enum CommandResponseMode
    {
        Held,
        Press,
        Release,
        NonZero, // For axes
        Unassigned
    }

    /// <summary>
    /// Stores a response for a given command.
    /// </summary>
    public class CommandResponse
    {

        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Command to check.
        /// </summary>
        private Command m_command = null;

        /// <summary>
        /// Response mode to check.
        /// </summary>
        private CommandResponseMode m_response = CommandResponseMode.Unassigned;

        /////////////////////
        // Properties
        /////////////////////
        
        /// <summary>
        /// Return command response.
        /// </summary>
        public CommandResponseMode Response
        {
            get { return this.m_response; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool OnHold
        {
            get { return Response == CommandResponseMode.Held; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool OnAxisChange
        {
            get { return Response == CommandResponseMode.NonZero; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool OnPress
        {
            get { return Response == CommandResponseMode.Press; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool OnRelease
        {
            get { return Response == CommandResponseMode.Release; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool Unassigned
        {
            get { return Response == CommandResponseMode.Unassigned; }
        }

        /// <summary>
        /// The command is a button.
        /// </summary>
        public bool IsButton
        {
            get { return this.m_command.IsButton; }
        }

        /// <summary>
        /// The command is an axis.
        /// </summary>
        public bool IsAxis {
            get { return this.m_command.IsAxis; }
        }

        #endregion

        #region Constructor.

        /// <summary>
        /// Response.
        /// </summary>
        /// <param name="_command">Command to get response.</param>
        /// <param name="_crm"></param>
        public CommandResponse(Command _command, CommandResponseMode _crm)
        {
            this.m_command = _command;
            this.m_response = _crm;
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Returns true if the response mode and command has been activated.
        /// </summary>
        /// <returns>Returns response status.</returns>
        public bool IsCommandActive()
        {
            if (IsButton)
            {
                if (OnHold)
                {
                    return m_command.IsKeyHeld();
                }

                if (OnPress)
                {
                    return m_command.IsKeyPressed();
                }

                if (OnRelease)
                {
                    return m_command.IsKeyReleased();
                }
            }

            if (IsAxis)
            {
                if (OnAxisChange)
                {
                    return m_command.IsAxisMoved();
                }
            }

            return false;
        }

        /// <summary>
        /// Checks response for equality.
        /// </summary>
        /// <param name="other">Response to compare.</param>
        /// <returns>Returns a boolean flag representing equality of statements.</returns>
        public bool Equals(CommandResponse other)
        {
            return ((this.m_response == other.m_response) && (this.m_command.Equals(other.m_command)));
        }

        #endregion
        
    }

    #endregion

    #region Class: CommandSequence class.

    /// <summary>
    /// A command is a sequence of one (or multiple) button presses that will be stored in order to check.
    /// </summary>
    public class CommandSequence
    {

        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// A sequence of button presses.
        /// </summary>
        private List<CommandResponse> m_sequence;

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Reference to sequence.
        /// </summary>
        public List<CommandResponse> Sequence
        {
            get { return this.m_sequence; }
        }

        #endregion

        #region Constructors.

        /// <summary>
        /// Create storage container for a sequence.
        /// </summary>
        public CommandSequence()
        {
            m_sequence = new List<CommandResponse>();
        }

        /// <summary>
        /// Create a sequence, with default timeouts of 0.5 seconds.
        /// </summary>
        /// <param name="_commands">Sequence of commands.</param>
        public CommandSequence(List<CommandResponse> _commands)
        {
            if (_commands.Count > 0)
            {
                // Send keys to array.
                m_sequence = _commands;
            }
            else
            {
                m_sequence = new List<CommandResponse>();
            }
        }

        #endregion

        #region Service Methods.
        
        /// <summary>
        /// Add a command to the end of the sequence.
        /// </summary>
        /// <param name="_command">Command to add.</param>
        public void Push(CommandResponse _command)
        {
            this.Sequence.Add(_command);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_input"></param>
        /// <returns></returns>
        public bool Matches(CommandSequence _input)
        {
            return Matches(_input.m_sequence);
        }

        /// <summary>
        /// Checks if an input list of hardware commands matches the sequence.
        /// </summary>
        /// <param name="_input">Sequence that should match.</param>
        /// <returns>Returns true if the input sequence AND timeouts match.</returns>
        public bool Matches(List<CommandResponse> _input)
        {
            // Check lengths. If they don't match, return false.
            if(m_sequence.Count > _input.Count) { return false; }
            
            // Loop through for match checking.
            for (int i = 0; i < m_sequence.Count; i++)
            {
                // Check command keys.
                if (!this.m_sequence[i].Equals(_input[i]))
                {
                    return false;
                }
            }

            // Return true if input matched.
            return true;
        }

        #endregion

    }

    #endregion

}
