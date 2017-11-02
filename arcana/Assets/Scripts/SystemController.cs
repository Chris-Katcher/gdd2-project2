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
            // HandleInput();


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
            Debugger.Print("Initialize system controller object.", this.Self.name, this.Debug);

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
            // InitializeControls();
        }
    }

    /// <summary>
    /// Initialize the system controller's controls.
    /// </summary>
    protected override ControlScheme InitializeControls()
    {
        this.m_scheme = base.InitializeControls();

        // Set the director for the system controller.
        Debugger.Print("Adding controls to the control scheme for Controller: " + this.Director, this.Self.name, this.Debug);

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

        return this.m_scheme;
    }

    #region Manager Building Methods.

    /// <summary>
    /// Constructs the manager objects and its components.
    /// </summary>
    private void BuildManagers()
    {
        // Build the managers.
        Debugger.Print("Build the managers.", this.Self.name, this.Debug);

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
        Debugger.Print("Build the input manager component.", this.Self.name, this.Debug);
        
        // Build the manager.
        Debugger.Print("Create the InputManager component, add as a child to the Managers GameObject, and retain the reference.", this.Self.name, this.Debug);

        // Create an input manager.
        this.m_inputManager = InputManager.Create(Services.AddChild(this.Managers.Self, Services.CreateEmptyObject("Input Manager")).AddComponent<ArcanaObject>());
        this.Managers.AddChild(this.m_inputManager);
        this.m_inputManager.Initialize();
        this.m_inputManager.Activate();

        // Add the system control scheme.
        BuildControlScheme();
    }

    /// <summary>
    /// Create a ControlScheme for system level actions.
    /// </summary>
    protected override void BuildControlScheme()
    {
        this.Director = Director.System;
        base.BuildControlScheme();    
    }

    /// <summary>
    /// Build and initialize the state manager.
    /// </summary>
    private void BuildStateManager()
    {
        // Build the manager.
        Debugger.Print("Build the state manager component.", this.Self.name, this.Debug);
        
        // Build the manager.
        Debugger.Print("Create the StateManager component, add as a child to the Managers GameObject, and retain the reference.", this.Self.name, this.Debug);
        
        // Create a state manager.
        this.m_stateManager = StateManager.Create(Services.AddChild(this.Managers.Self, Services.CreateEmptyObject("State Manager")).AddComponent<ArcanaObject>());
        this.Managers.AddChild(this.m_stateManager);
        this.m_stateManager.Initialize();
        this.m_stateManager.Activate();
    }
    
    /// <summary>
    /// Build and initialize the camera manager.
    /// </summary>
    private void BuildCameraManager()
    {
        // Build the camera manager.
        Debugger.Print("Build the camera manager component.", this.Self.name, this.Debug);
        
        // Build the manager.
        Debugger.Print("Create the CameraManager component, add as a child to the Managers GameObject, and retain the reference.", this.Self.name, this.Debug);

        // Create a camera manager.
        this.m_cameraManager = CameraManager.Create(Services.AddChild(this.Managers.Self, Services.CreateEmptyObject("Camera Manager")).AddComponent<ArcanaObject>());
        this.Managers.AddChild(this.m_cameraManager);
        this.m_cameraManager.Initialize();
        this.m_cameraManager.Activate();
    }

    /// <summary>
    /// Build and initialize the entity manager.
    /// </summary>
    private void BuildEntityManager()
    {
        // Build the manager.
        Debugger.Print("Build the entity manager component.", this.Self.name, this.Debug);
        
        // Build the manager.
        Debugger.Print("Create the EntityManager component, add as a child to the Managers GameObject, and retain the reference.", this.Self.name, this.Debug);

        // Create an entity manager.
        this.m_entityManager = EntityManager.Create(Services.AddChild(this.Managers.Self, Services.CreateEmptyObject("Entity Manager")).AddComponent<ArcanaObject>());
        this.Managers.AddChild(this.m_entityManager);
        this.m_entityManager.Initialize();
        this.m_entityManager.Activate();
    }

    #endregion

    #endregion

    #region Input Methods.

    /// <summary>
    /// Handles all the registered actions.
    /// </summary>
    protected override void HandleInput()
    {
        if (this.Controls.IsActivated(GetAction("Change Camera Background")))
        {
            this.CameraController.ChangeBackground(Services.GetRandomColor());
        }

        if (Controls.IsActivated(GetAction("D")))
        {
            Debugger.Print("Pressed the D key.", this.Self.name, this.Debug);
        }

        if (Controls.IsActivated(GetAction("S")))
        {
            Debugger.Print("Pressed the S key.", this.Self.name, this.Debug);
        }

        if (Controls.IsActivated(GetAction("A")))
        {
            Debugger.Print("Pressed the A key.", this.Self.name, this.Debug);
        }

        if (Controls.IsActivated(GetAction("Mouse Click")))
        {
            Vector2 mouse = Services.ToVector2(Input.mousePosition);
            Debugger.Print("Clicked the left mouse button: " + mouse, this.Self.name, this.Debug);
        }

        if (Controls.IsActivated(GetAction("Joystick1A_PRESSED")))
        {
            Debugger.Print("Joystick 1 A pressed.", this.Self.name, this.Debug);
        }

        if (Controls.IsActivated(GetAction("Joystick1A_RELEASED")))
        {
            Debugger.Print("Joystick 1 A released.", this.Self.name, this.Debug);
        }
        
    }


    #endregion
    
}
