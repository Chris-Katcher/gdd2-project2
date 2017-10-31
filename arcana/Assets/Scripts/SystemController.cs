/************************************************
 * SystemController.cs
 * 
 * This file is the entry point for the game.
 * It creates instances of the managers.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using UnityEngine;
using Arcana;
using Arcana.InputManagement;
using Arcana.UI;
using Arcana.Entities;
using Arcana.Cameras;
using Arcana.States;
using System.Collections.Generic;

/////////////////////
// Class declaration.
/////////////////////

/// <summary>
/// SystemController acts as the entry point for the program.
/// C# scripts for Unity must inherit the <see cref="MonoBehavior"/> class.
/// </summary>
[AddComponentMenu("Arcana/Managers/SystemController")]
public class SystemController : ArcanaObject {

    #region Data Members 

    #region Fields.

    /////////////////////
    // Fields.
    /////////////////////

    /// <summary>
    /// If true, debug messages will print for this program.
    /// </summary>
    public bool DEBUG_MODE = true;    // Will allow setting of debug mode.

    /// <summary>
    /// Initialization flag.
    /// </summary>
    private bool m_initialized = false;

    // Set up the managers.

    /// <summary>
    /// Empty game object that has all the manager components on it.
    /// </summary>
    private ArcanaObject m_managers = null;

    /// <summary>
    /// The input manager handles commands and sequences.
    /// </summary>
    private InputManager m_inputManager = null;

    /// <summary>
    /// The state manager handles all states in the game.
    /// </summary>
    private StateManager m_stateManager = null;

    /// <summary>
    /// The camera manager handles the cameras in the game.
    /// </summary>
    private CameraManager m_cameraManager = null;

    /// <summary>
    /// The entity manager handles all entities in the game.
    /// </summary>
    private EntityManager m_entityManager = null;

    /// <summary>
    /// Reference to the system's control scheme.
    /// </summary>
    private ControlScheme m_scheme = null;

    #endregion

    #region Properties.
    
    /////////////////////
    // Properties.
    /////////////////////
    
    /// <summary>
    /// Returns reference to the managers object.
    /// </summary>
    public ArcanaObject Managers
    {
        get { return this.m_managers; }
    }

    /// <summary>
    /// Returns reference to the InputManager.
    /// </summary>
    public InputManager InputController
    {
        get { return this.m_inputManager; }
    }

    /// <summary>
    /// Returns reference to the StateManager.
    /// </summary>
    public StateManager StateController
    {
        get { return this.m_stateManager; }
    }

    /// <summary>
    /// Returns reference to the CameraManager.
    /// </summary>
    public CameraManager CameraController
    {
        get { return this.m_cameraManager; }
    }

    /// <summary>
    /// Returns reference to the EntityManager.
    /// </summary>
    public EntityManager EntityController
    {
        get { return this.m_entityManager; }
    }

    /// <summary>
    /// Returns reference to the control scheme.
    /// </summary>
    public ControlScheme Controls
    {
        get { return this.m_scheme; }
    }

    #endregion

    #endregion

    #region UnityEngine Methods
    
    /// <summary>
    /// Update high-level system event responses for the main game loop.
    /// </summary>
    public override void Update()
    {
        // For the system controller, don't call the base update.
        // We don't want it to be destroyed when flagged for destruction, since, it's the main game loop.

        // If the system controller hasn't been initialized:
        if (!Initialized)
        {
            // Initialize the controller if it hasn't been initialized.
            this.Initialize();
        }
        else
        {
            // If the system controller has been initialized, perform these actions.
            
            // Handle user input.
            HandleInput();


            /*
            //gets translation of player one
            float translation = m_inputManager.getPlayer1Translation();
            //gets bool of whether plyaer1 has jumped
            bool jump_pressed = m_inputManager.getPlayer1Jump();
            //gets bool of whether fire button has been pressed
            bool fire1_pressed = m_inputManager.getProjectileFire();


            //updates the wizard position and jump
            m_gameManager.UpdatePosWizzard1(translation);
            m_gameManager.UpdateJumpStatus(jump_pressed);

            //fires a projectile
            m_gameManager.fireProjPlayer1(fire1_pressed);

            // TODO: Stub code.*/

        }
    }

    #endregion

    #region Initialization Methods.

    /// <summary>
    /// Initialize the main game loop.
    /// </summary>
    public override void Initialize()
    {
        if (!this.Initialized)
        {
            // Call the base. (This will also set our initialization flag to true).
            base.Initialize();

            // Set the debug mode.
            Debugger.SetDebugMode(DEBUG_MODE);

            // Build the system controller object.
            Debugger.Print("Initialize system controller object.");

            // Create an empty game object and add the manager object.
            this.m_managers = Services.CreateEmptyObject("System Managers").AddComponent<ArcanaObject>();

            // Make the system controller object the parent.
            Services.AddChild(this.Self, this.m_managers.Self);

            // Set up the name for the manager.
            this.Name = "Arcana (System Controller)";

            // Ensure no movement.
            this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

            // Build (or request) the managers.
            BuildManagers();

            // Add the controls.
            InitializeControls();
        }
    }

    /// <summary>
    /// Initialize the system controller's controls.
    /// </summary>
    private void InitializeControls()
    {
        // Set the director for the system controller.
        Debugger.Print("Adding controls to the control scheme for Controller: " + this.Director);

        // AddHandle("Mouse X", CommandTypes.MouseMovement);
        // AddHandle("Mouse Y", CommandTypes.Axis);

        // Creates an "A" button command that triggers on release.
        Action action = Action.CreateAction("Change Camera Background", this.Director);
        Control control = Control.CreateKey(KeyCode.A);
        ResponseMode response = ResponseMode.Released;
        RegisterControl(action, control, response);
        RegisterControl("A", Control.CreateKey(KeyCode.A), ResponseMode.Pressed);
        // RegisterAction(Actions.ChangeCameraBackground, new Command(KeyCode.A), CommandResponseMode.Release);

        // Creates an "S" button command that triggers on press.
        action = Action.CreateAction("Change Camera Background", this.Director);
        control = Control.CreateKey(KeyCode.S);
        response = ResponseMode.Pressed;
        Trigger trigger = ControlScheme.CreateTrigger(control, response);
        RegisterControl(action, trigger);
        RegisterControl("S", Control.CreateKey(KeyCode.S), ResponseMode.Pressed);
        // RegisterAction(Actions.ChangeCameraBackground, new Command(KeyCode.S), CommandResponseMode.Press);


        // Creates an "D" button command that triggers on held.
        action = Action.CreateAction("Change Camera Background", this.Director);
        trigger = ControlScheme.CreateTrigger(Control.CreateKey(KeyCode.D), ResponseMode.Held);
        RegisterControl(action, trigger);
        RegisterControl("D", Control.CreateKey(KeyCode.D), ResponseMode.Pressed);
        // RegisterAction(Actions.ChangeCameraBackground, new Command(KeyCode.D), CommandResponseMode.Held);


        RegisterControl("Joystick1A_RELEASED", Control.CreateKey(KeyCode.Joystick1Button0), ResponseMode.Released);
        RegisterControl("Joystick1A_PRESSED", Control.CreateKey(KeyCode.Joystick1Button0), ResponseMode.Pressed);

        // Add a left moust button click.
        RegisterControl("Change Camera Background", Control.CreateMouseButton(MouseButton.LMB), ResponseMode.Pressed);
        RegisterControl("Mouse Click", Control.CreateMouseButton(MouseButton.LMB), ResponseMode.Pressed);
        // RegisterAction(Actions.Click, new Command(_mouse: 0), CommandResponseMode.Press);

    }

    #region Manager Building Methods.

    /// <summary>
    /// Constructs the manager objects and its components.
    /// </summary>
    private void BuildManagers()
    {
        // Build the managers.
        Debugger.Print("Build the managers.");

        // Build the input manager.
        BuildInputManager();

        // Build the state manager.
        BuildStateManager();

        // Build the camera manager.
        BuildCameraManager();

        // Build the entity manager.
        BuildEntityManager();

    }
    
    /// <summary>
    /// Build and initialize the input manager.
    /// </summary>
    private void BuildInputManager()
    {
        // Build the manager.
        Debugger.Print("Build the input manager component.");
        
        // Build the manager.
        Debugger.Print("Create the InputManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_inputManager = InputManager.Create(this.Managers);

        // Add the system control scheme.
        BuildControlScheme();
    }

    /// <summary>
    /// Create a ControlScheme for system level actions.
    /// </summary>
    private void BuildControlScheme()
    {
        if (this.m_scheme == null)
        {
            this.Director = Director.System;
            this.m_scheme = this.InputController.AddControlScheme(this);
            this.m_scheme.Initialize();
        }
    }

    /// <summary>
    /// Build and initialize the state manager.
    /// </summary>
    private void BuildStateManager()
    {
        // Build the manager.
        Debugger.Print("Build the state manager component.");
        
        // Build the manager.
        Debugger.Print("Create the StateManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_stateManager = StateManager.Create(this.Managers);
    }
    
    /// <summary>
    /// Build and initialize the camera manager.
    /// </summary>
    private void BuildCameraManager()
    {
        // Build the camera manager.
        Debugger.Print("Build the camera manager component.");
        
        // Build the manager.
        Debugger.Print("Create the CameraManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_cameraManager = CameraManager.Create(this.Managers);
    }

    /// <summary>
    /// Build and initialize the entity manager.
    /// </summary>
    private void BuildEntityManager()
    {
        // Build the manager.
        Debugger.Print("Build the entity manager component.");
        
        // Build the manager.
        Debugger.Print("Create the EntityManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_entityManager = EntityManager.Create(this.Managers);
    }

    #endregion

    #endregion

    #region Input Methods.

    /// <summary>
    /// Handles all the registered actions.
    /// </summary>
    private void HandleInput()
    {
        if (this.Controls.IsActivated(GetAction("Change Camera Background")))
        {
            this.CameraController.ChangeBackground(Services.GetRandomColor());
        }

        if (Controls.IsActivated(GetAction("D")))
        {
            Debugger.Print("Pressed the D key.");
        }

        if (Controls.IsActivated(GetAction("S")))
        {
            Debugger.Print("Pressed the S key.");
        }

        if (Controls.IsActivated(GetAction("A")))
        {
            Debugger.Print("Pressed the A key.");
        }

        if (Controls.IsActivated(GetAction("Mouse Click")))
        {
            Vector2 mouse = Services.ToVector2(Input.mousePosition);
            Debugger.Print("Clicked the left mouse button: " + mouse);
        }

        if (Controls.IsActivated(GetAction("Joystick1A_PRESSED")))
        {
            Debugger.Print("Joystick 1 A pressed.");
        }

        if (Controls.IsActivated(GetAction("Joystick1A_RELEASED")))
        {
            Debugger.Print("Joystick 1 A released.");
        }
        
    }

    /// <summary>
    /// Return the action from the control scheme.
    /// </summary>
    /// <param name="_id">ID of the action to request.</param>
    /// <returns>Returns an action.</returns>
    private Action GetAction(string _id)
    {
        return Action.GetAction(_id, this.Director);
    }
    
    /// <summary>
    /// Link an action to perform with a command.
    /// </summary>
    /// <param name="_action">Action to perform.</param>
    /// <param name="_trigger">Trigger that will cause action.</param>
    /// <param name="_control">Binding that will cause action.</param>
    /// <param name="_response">Response type that will trigger action.</param>
    public void RegisterControl(Action _action, Trigger _trigger)
    {
        if (this.Controls != null)
        {
            this.Controls.AddMap(_action, _trigger);
        }
    }
    
    /// <summary>
    /// Link an action to perform with a command.
    /// </summary>
    /// <param name="_action">Action to perform.</param>
    /// <param name="_control">Binding that will cause action.</param>
    /// <param name="_response">Response type that will trigger action.</param>
    public void RegisterControl(Action _action, Control _control, ResponseMode _mode = ResponseMode.None)
    {

        // Set up reference.
        Trigger trigger;

        if (_mode == ResponseMode.None)
        {
            trigger = ControlScheme.CreateTrigger(_control);
        }
        else
        {
            trigger = ControlScheme.CreateTrigger(_control, _mode);
        }

        RegisterControl(_action, trigger);
    }
    
    /// <summary>
    /// Link an action to perform with a command.
    /// </summary>
    /// <param name="_action">Action to perform.</param>
    /// <param name="_control">Binding that will cause action.</param>
    /// <param name="_response">Response type that will trigger action.</param>
    public void RegisterControl(string _actionID, Control _control, ResponseMode _mode = ResponseMode.None)
    {
        Action action = ControlScheme.CreateAction(_actionID, this.Director);
        RegisterControl(action, _control, _mode);
    }   

    #endregion
    
}
