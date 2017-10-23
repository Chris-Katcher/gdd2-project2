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
    // Class members.
    /////////////////////
    
    // Create references to the UIManager, GameManager, and InputManager. //
    
    /// <summary>
    /// SystemController access to the global UIManager.
    /// </summary>
    public UIManager m_uiManager;
    
    /// <summary>
    /// SystemController access to the global GameManager.
    /// </summary>
    public GameManager m_gameManager;
    
    /// <summary>
    /// SystemController access to the global InputManager.
    /// </summary>
    public InputManager m_inputManager;

    private ProjectileManager m_projectile;

    public Player m_player;
    
    // Flags. //

    /// <summary>
    /// Initialization flag. False means this class has not yet been initialized.
    /// </summary>
    public bool m_init = false;

    #endregion

    #region MonoBehavior Methods

    /// <summary>
    /// Entry point for the program.
    /// </summary>
    void Start ()
    {

        this.Initialize();
        print("System Conrtoller Init");

	}
	
	/// <summary>
    /// Update is called once per frame.
    /// </summary>
	void Update () 
    {

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

        // TODO: Stub code.

	}

    #endregion

    #region Service Methods

    /// <summary>
    /// Initialize the managers.
    /// </summary>
    private void Initialize()
    {
        // Create and initialize managers.
        gameObject.AddComponent<InputManager>();
        gameObject.AddComponent<Player>();
        gameObject.AddComponent<GameManager>();
        gameObject.AddComponent<InputManager>();

        // Set references to managers.
        m_uiManager = new UIManager();
        m_gameManager = gameObject.GetComponent<GameManager>();
        m_inputManager = gameObject.GetComponent<InputManager>();
        //m_player = gameObject.GetComponent<Player>();
        m_gameManager.Initialize();

        // Set initialized.
        m_init = true;

    }

    #endregion

    #region Accessor Methods

    private bool IsInitialized()
    {
        return m_init;
    }

    #endregion

    #region Mutator Methods


    // TODO: Stub.


    #endregion
    
}
