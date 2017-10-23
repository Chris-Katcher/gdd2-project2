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
    /// Empty game object that has all the manager components on it.
    /// </summary>
    private GameObject m_managers = null;

    /// <summary>
    /// The camera manager handles the cameras in the game.
    /// </summary>
    private CameraManager m_cameraManager = null;

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

    #endregion

    #region MonoBehavior Methods

    /// <summary>
    /// Entry point for the program.
    /// </summary>
    void Start ()
    {
        // Initialize the program.
        this.Initialize();
	}
	
	/// <summary>
    /// Update is called once per frame.
    /// </summary>
	void Update () 
    {
        if (!Initialized)
        {
            // Initialize the controller if it hasn't been initialized.
            this.Initialize();
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

    #region Service Methods

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
        // Build the manager object.
        Debugger.Print("Build the manager.");
        if (this.Managers == null)
        {
            this.m_managers = Services.CreateEmptyObject("Game Managers");
        }

        // Build the components.
        Debugger.Print("Build manager components.");

    }

    /// <summary>
    /// Builds the camera manager.
    /// </summary>
    private void BuildCameraManager()
    {
        // Get a reference to the factory.
        Debugger.Print("Get instance to the factory.");
        CameraManagerFactory factory = CameraManagerFactory.Instance();

        // Set up initialization settings for the camera manager.
        Debugger.Print("Create the factory constraints for the CameraManager.");
        Constraints managerSettings = factory.CreateSettings(); // Filled with default values. Edit this to change component settings on creation.

        // Build the manager.
        Debugger.Print("Create the CameraManager component, add it to the Managers GameObject, and retain the reference.");
        this.m_cameraManager = factory.CreateComponent(this.Managers, managerSettings);
    }

    #endregion

    #region Accessor Methods

    // TODO: Stub.

    #endregion

    #region Mutator Methods


    // TODO: Stub.


    #endregion
    
}
