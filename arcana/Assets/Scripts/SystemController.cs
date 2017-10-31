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
    /// The controller calling input actions.
    /// </summary>
    private Director m_controller = Director.SystemController;

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
    /// The ui manager handles all of the ui in the game.
    /// </summary>
    private UIManager m_uiManager = null;

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
    /// The ui manager handles all of the ui in the game.
    /// </summary>
    public UIManager UiController
    {
        get { return this.m_uiManager; }
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
           


            // ***Used to move to the game state once a player presses a key or mouse1.
            // This needs to be updated once the input management is completed.***
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_uiManager.ChangeState(States.Gameover);
                Debugger.Print("Changed state to gameover");
            }
            else if ((Input.anyKeyDown || Input.GetMouseButtonDown(0)) && !Input.GetKeyDown(KeyCode.Escape))
            {
                m_uiManager.ChangeState(States.Gameplay);
                Debugger.Print("Changed state to GAMEPLAY");
            }
            m_uiManager.UpdateState();
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
        Debugger.Print("Adding controls to the control scheme for Controller: " + this.m_controller);

        // AddHandle("Mouse X", CommandTypes.MouseMovement);
        // AddHandle("Mouse Y", CommandTypes.Axis);

        // Creates an "A" button command that triggers on release.
        RegisterAction(Actions.ChangeCameraBackground, new Command(KeyCode.A), CommandResponseMode.Release);

        // Creates an "S" button command that triggers on press.
        RegisterAction(Actions.ChangeCameraBackground, new Command(KeyCode.S), CommandResponseMode.Press);

        // Creates an "D" button command that triggers on held.
        RegisterAction(Actions.ChangeCameraBackground, new Command(KeyCode.D), CommandResponseMode.Held);

        // Add a left moust button click.
        RegisterAction(Actions.Click, new Command(_mouse: 0), CommandResponseMode.Press);


        /* *
        
        Tested out functionality: the new input system works!

        // Creates an "A" button command that triggers on release.
        AddHandle(Actions.ChangeCameraBackground, new Command(KeyCode.A), CommandResponseMode.Release);

        // Creates an "S" button command that triggers on press.
        AddHandle(Actions.ChangeCameraBackground, new Command(KeyCode.S), CommandResponseMode.Press);

        // Creates an "D" button command that triggers on held.
        AddHandle(Actions.ChangeCameraBackground, new Command(KeyCode.D), CommandResponseMode.Held);
        
        // Creates an axis detection for whenever Mouse moves.
        AddHandle(Actions.ChangeCameraBackground, new Command("Mouse X", CommandTypes.MouseMovement), CommandResponseMode.NonZero);

         * */

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

        // Build the ui manager.
        BuildUiManager();

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
        // BuildControlScheme();
    }

    /// <summary>
    /// Create a ControlScheme for system level actions.
    /// </summary>
    private void BuildControlScheme()
    {
        /*
        // If a scheme doesn't exist, build a new one.
        if (GetScheme() == null)
        {
            Debugger.Print("Creating the control scheme for " + this.Name);
            // TODO: this.m_inputManager.AddControlScheme(this.m_controller, new ControlScheme());
        }
        */
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

    private void BuildUiManager()
    {
        // Build the manager.
        Debugger.Print("Build the ui manager component.");

        // Build the manager.
        Debugger.Print("Create the UiManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_uiManager = UIManager.Create(this.Managers);
    }

    #endregion

    #endregion

    #region Input Methods.

    /// <summary>
    /// Handles all the registered actions.
    /// </summary>
    private void HandleInput()
    {
        /*
        // This is the format for getting key input.

        // If GetAction(Action) --> Perform functionality.
        if (this.GetAction(Actions.ChangeCameraBackground))
        {
            this.m_cameraManager.ChangeBackground(Services.GetRandomColor());
        }

        // Handle the actions.
        if (this.GetAction(Actions.Click))
        {
            Vector2 mouse = Services.ToVector2(Input.mousePosition);
            Debugger.Print("Left mouse button was clicked: " + mouse);
        }
        */
    }

    /*
    /// <summary>
    /// Wrapper function for handling input.
    /// </summary>
    /// <param name="_action">Action to check for.</param>
    /// <returns>Returns true if action should be performed.</returns>
    public bool GetAction(Actions _action)
    {
        // return (this.m_inputManager.GetAction(this.m_controller, _action));
    }

    /// <summary>
    /// Returns the value for a tracked axis.
    /// </summary>
    /// <param name="_name">Axis being tracked.</param>
    /// <returns>Returns a value.</returns>
    public float GetAxis(string _name)
    {
       // return (this.m_inputManager.GetAxis(this.m_controller, _name));
    }

    /// <summary>
    /// Returns the raw value for a tracked axis.
    /// </summary>
    /// <param name="_name">Axis being tracked.</param>
    /// <returns>Returns a value.</returns>
    public float GetAxisRaw(string _name)
    {
        // return (this.m_inputManager.GetAxisRaw(this.m_controller, _name));
    }

    /// <summary>
    /// Return the ControlScheme from the input manager.
    /// </summary>
    /// <returns>ControlScheme object.</returns>
    public ControlScheme GetScheme()
    {
        // return this.m_inputManager.GetScheme(this.m_controller);
    }
    */

    /// <summary>
    /// Link an action to perform with a command.
    /// </summary>
    /// <param name="_action">Action to perform.</param>
    /// <param name="_command">Binding that will cause action.</param>
    /// <param name="_response">Response type that will trigger action.</param>
    public void RegisterAction(Actions _action, Command _command, CommandResponseMode _response)
    {
        CommandResponse response = new CommandResponse(_command, _response);
        CommandSequence sequence = new CommandSequence();
        sequence.Push(response);

        AddControl(_action, sequence);
    }

    /// <summary>
    /// Link an axis with a name and response trigger.
    /// </summary>
    /// <param name="_axis">Name of the axis-trigger pair.</param>
    /// <param name="_command">Axis read by trigger.</param>
    /// <param name="_response">Trigger.</param>
    public void RegisterAxis(string _axis, CommandTypes _type = CommandTypes.Axis)
    {
        CommandResponse response = new CommandResponse(new Command(_axis, _type), CommandResponseMode.NonZero);
        AddControl(_axis, response);
    }

    /// <summary>
    /// Link an action to a series of commands.
    /// </summary>
    /// <param name="_action">Action to perform.</param>
    /// <param name="_sequence">Series of responses needed to activate action.</param>
    public void AddControl(Actions _action, CommandSequence _sequence)
    {
        // Add the control to the existing scheme.
        // GetScheme().AddControl(_action, _sequence);
    }

    /// <summary>
    /// Link the axis to the command response.
    /// </summary>
    /// <param name="_axis">Axis name.</param>
    /// <param name="_response">Response triggering axis value.</param>
    public void AddControl(string _axis, CommandResponse _response)
    {
        // Add tracking information for the axis in question.
        // GetScheme().AddAxis(_axis, _response);
    }

    #endregion
    
}
