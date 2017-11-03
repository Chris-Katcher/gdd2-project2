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
using UnityEngine.SceneManagement;

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

    public ProjectileManager m_projectile;

    public Player m_player1;
    public Player m_player2;
    
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
        float translationP1 = m_inputManager.getPlayer1Translation();
        //gets bool of whether plyaer1 has jumped
        bool jump_pressedP1 = m_inputManager.getPlayer1Jump();

        //gets translation of player one
        float translationP2 = m_inputManager.getPlayer2Translation();
        //gets bool of whether plyaer1 has jumped
        bool jump_pressedP2 = m_inputManager.getPlayer2Jump();

        //gets bool of whether fire button has been pressed
        bool fire1_pressedP1 = m_inputManager.getProjectileFireP1();
		bool fire2_pressedP1 = m_inputManager.getProjectileFire2P1();
		bool fire3_pressedP1 = m_inputManager.getProjectileFire3P1();

        bool fire1_pressedP2 = m_inputManager.getProjectileFireP2();
        bool fire2_pressedP2 = m_inputManager.getProjectileFire2P2();
        bool fire3_pressedP2 = m_inputManager.getProjectileFire3P2();

        bool player_drop = m_inputManager.getPlayerDrop();
        
        //updates the wizard position and jump
        m_gameManager.UpdatePosWizzard(translationP1, 1);
        m_gameManager.UpdatePosWizzard(translationP2, 2);

        m_gameManager.UpdateJumpStatus(jump_pressedP1, 1);
        m_gameManager.UpdateJumpStatus(jump_pressedP2, 2);
        //fires a projectile
        m_gameManager.fireProj(1, fire1_pressedP1, fire2_pressedP1, fire3_pressedP1);
        m_gameManager.fireProj(2, fire1_pressedP1, fire2_pressedP1, fire3_pressedP1);
        //*****//m_gameManager.UpdateDropStatus(player_drop);
        // TODO: Stub code.

        if (m_inputManager.getEscPressed())
        {
            SceneManager.LoadScene(0);
        }
		//m_projectile.updateProjectiles();
		

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
        gameObject.AddComponent<Player>();
		gameObject.AddComponent<GameManager>();
		gameObject.AddComponent<InputManager>();
		gameObject.AddComponent<ProjectileManager>();

		// Set references to managers.
		m_uiManager = new UIManager();
		m_gameManager = gameObject.GetComponent<GameManager>();
		m_inputManager = gameObject.GetComponent<InputManager>();
		m_projectile = gameObject.GetComponent<ProjectileManager>();
		//m_player = gameObject.GetComponent<Player>();
		m_gameManager.Initialize();

        // Set initialized.
        m_init = true;

    }

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
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
