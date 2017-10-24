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
public class SystemController : MonoBehaviour {

    #region Data Members 

    /////////////////////
    // Data members.
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
    private Controller m_controller = Controller.System;

    /// <summary>
    /// Empty game object that has all the manager components on it.
    /// </summary>
    private GameObject m_managers = null;

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


    /////////////////////
    // Properties.
    /////////////////////

    /// <summary>
    /// Name of the game object.
    /// </summary>
    private string Name
    {
        get { return "Arcana (System Controller)"; }
    }

    /// <summary>
    /// Returns initialization flag.
    /// </summary>
    public bool Initialized
    {
        get { return this.m_initialized; }
    }

    /// <summary>
    /// Returns reference to the managers object.
    /// </summary>
    public GameObject Managers
    {
        get { return this.m_managers; }
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
    
    #region Service Methods

    #region MonoBehavior Methods

    /// <summary>
    /// Entry point for the program.
    /// </summary>
    void Start()
    {
        // Initialize the program.
        this.Initialize();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // This is the format for getting key input.
        if (this.GetAction(Actions.ChangeCameraBackground))
        {
            this.m_cameraManager.ChangeBackground(Services.GetRandomColor());
        }
        
        if (!Initialized)
        {
            // Initialize the controller if it hasn't been initialized.
            this.Initialize();
        }
        else
        {

        }

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

    #endregion

    #region Initialization Methods.

    /// <summary>
    /// Initialize the managers.
    /// </summary>
    private void Initialize()
    {
        // If this hasn't been initialized.

        if (!this.Initialized) {
            // Set the debug mode.
            Debugger.SetDebugMode(DEBUG_MODE);
            Debugger.Print("Initialize system controller.");

            // Set up the name for this manager.
            gameObject.name = this.Name;

            // Create the managers.
            BuildManagers();

            // Set the flags.
            this.m_initialized = true;
        }
    }

    /// <summary>
    /// Constructs the manager objects and its components.
    /// </summary>
    private void BuildManagers()
    {
        // Build the manager object. //
        Debugger.Print("Build the manager.");

        // Create the game manager component.
        this.m_managers = Services.CreateEmptyObject("Game Managers");

        // Make the system controller object the parent.
        Services.AddChild(gameObject, this.m_managers);

        // Build the components. //
        Debugger.Print("Build manager components.");

        // Build the input manager.
        BuildInputManager();

        // Build the state manager.
        BuildStateManager();

        // Build the camera manager.
        BuildCameraManager();

        // Build the entity manager.
        BuildEntityManager();
        
        // Add the controls.
        InitializeControls();

    }

    #region Manager Building Methods.

    /// <summary>
    /// Build and initialize the input manager.
    /// </summary>
    private void BuildInputManager()
    {
        // Build the manager.
        Debugger.Print("Build the input manager component.");

        // Get a reference to the factory.
        Debugger.Print("Get instance to the factory.");
        InputManagerFactory factory = InputManagerFactory.Instance();

        // Set up initialization settings for the manager.
        Debugger.Print("Create the factory constraints for the InputManager.");
        Constraints managerSettings = factory.CreateSettings(); // Filled with default values. Edit this to change component settings on creation.

        // Build the manager.
        Debugger.Print("Create the InputManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_inputManager = factory.CreateComponent(this.Managers, managerSettings);

        // Add the system control scheme.
        this.m_inputManager.AddControlScheme(m_controller, BuildControlScheme());
    }

    /// <summary>
    /// Create a ControlScheme for system level actions.
    /// </summary>
    /// <returns></returns>
    private ControlScheme BuildControlScheme()
    {
        return new ControlScheme();
    }

    /// <summary>
    /// Initialize the system controller's controls.
    /// </summary>
    private void InitializeControls()
    {
        Debugger.Print("Adding controls to the control scheme for Controller: " + this.m_controller);

        AddHandle("Mouse X", CommandTypes.MouseMovement);
        AddHandle("Mouse Y", CommandTypes.Axis);

        // Creates an "A" button command that triggers on release.
        AddHandle(Actions.ChangeCameraBackground, new Command(KeyCode.A), CommandResponseMode.Release);


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

    /// <summary>
    /// Build and initialize the state manager.
    /// </summary>
    private void BuildStateManager()
    {
        // Build the manager.
        Debugger.Print("Build the state manager component.");

        // Get a reference to the factory.
        Debugger.Print("Get instance to the factory.");
        StateManagerFactory factory = StateManagerFactory.Instance();

        // Set up initialization settings for the manager.
        Debugger.Print("Create the factory constraints for the StateManager.");
        Constraints managerSettings = factory.CreateSettings(); // Filled with default values. Edit this to change component settings on creation.

        // Build the manager.
        Debugger.Print("Create the StateManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_stateManager = factory.CreateComponent(this.Managers, managerSettings);
    }


    /// <summary>
    /// Build and initialize the camera manager.
    /// </summary>
    private void BuildCameraManager()
    {
        // Build the camera manager.
        Debugger.Print("Build the camera manager component.");

        // Get a reference to the factory.
        Debugger.Print("Get instance to the factory.");
        CameraManagerFactory factory = CameraManagerFactory.Instance();

        // Set up initialization settings for the camera manager.
        Debugger.Print("Create the factory constraints for the CameraManager.");
        Constraints managerSettings = factory.CreateSettings(_color: Constants.CORNFLOWER_BLUE); // Filled with default values. Edit this to change component settings on creation.

        // Build the manager.
        Debugger.Print("Create the CameraManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_cameraManager = factory.CreateComponent(this.Managers, managerSettings);
    }

    /// <summary>
    /// Build and initialize the entity manager.
    /// </summary>
    private void BuildEntityManager()
    {
        // Build the manager.
        Debugger.Print("Build the entity manager component.");

        // Get a reference to the factory.
        Debugger.Print("Get instance to the factory.");
        EntityManagerFactory factory = EntityManagerFactory.Instance();

        // Set up initialization settings for the manager.
        Debugger.Print("Create the factory constraints for the EntityManager.");
        Constraints managerSettings = factory.CreateSettings(); // Filled with default values. Edit this to change component settings on creation.

        // Build the manager.
        Debugger.Print("Create the EntityManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_entityManager = factory.CreateComponent(this.Managers, managerSettings);
    }

    #endregion

    #endregion

    #region Input Methods.

    /// <summary>
    /// Wrapper function for handling input.
    /// </summary>
    /// <param name="_action">Action to check for.</param>
    /// <returns>Returns true if action should be performed.</returns>
    public bool GetAction(Actions _action)
    {
        return (this.m_inputManager.GetAction(this.m_controller, _action));
    }

    /// <summary>
    /// Returns the value for a tracked axis.
    /// </summary>
    /// <param name="_name">Axis being tracked.</param>
    /// <returns>Returns a value.</returns>
    public float GetAxis(string _name)
    {
        return (this.m_inputManager.GetAxis(this.m_controller, _name));
    }

    /// <summary>
    /// Returns the raw value for a tracked axis.
    /// </summary>
    /// <param name="_name">Axis being tracked.</param>
    /// <returns>Returns a value.</returns>
    public float GetAxisRaw(string _name)
    {
        return (this.m_inputManager.GetAxisRaw(this.m_controller, _name));
    }

    /// <summary>
    /// Return the ControlScheme from the input manager.
    /// </summary>
    /// <returns>ControlScheme object.</returns>
    public ControlScheme GetScheme()
    {
        return this.m_inputManager.GetScheme(this.m_controller);
    }

    /// <summary>
    /// Link an action to perform with a command.
    /// </summary>
    /// <param name="_action">Action to perform.</param>
    /// <param name="_command">Binding that will cause action.</param>
    /// <param name="_response">Response type that will trigger action.</param>
    public void AddHandle(Actions _action, Command _command, CommandResponseMode _response)
    {
        CommandResponse response = new CommandResponse(_command, _response);
        CommandSequence sequence = new CommandSequence();
        sequence.Push(response);

        AddActionHandle(_action, sequence);
    }

    /// <summary>
    /// Link an axis with a name and response trigger.
    /// </summary>
    /// <param name="_axis">Name of the axis-trigger pair.</param>
    /// <param name="_command">Axis read by trigger.</param>
    /// <param name="_response">Trigger.</param>
    public void AddHandle(string _axis, CommandTypes _type = CommandTypes.Axis)
    {
        CommandResponse response = new CommandResponse(new Command(_axis, _type), CommandResponseMode.NonZero);
        AddAxisHandle(_axis, response);
    }
    
    /// <summary>
    /// Link an action to a series of commands.
    /// </summary>
    /// <param name="_action">Action to perform.</param>
    /// <param name="_sequence">Series of responses needed to activate action.</param>
    public void AddActionHandle(Actions _action, CommandSequence _sequence)
    {
        // Add the control to the existing scheme.
        GetScheme().AddControl(_action, _sequence);
    }

    /// <summary>
    /// Link the axis to the command response.
    /// </summary>
    /// <param name="_axis">Axis name.</param>
    /// <param name="_response">Response triggering axis value.</param>
    public void AddAxisHandle(string _axis, CommandResponse _response)
    {
        // Add tracking information for the axis in question.
        GetScheme().AddAxis(_axis, _response);
    }

    #endregion

    #endregion

    #region Accessor Methods

    // TODO: Stub.

    #endregion

    #region Mutator Methods


    // TODO: Stub.


    #endregion

}
