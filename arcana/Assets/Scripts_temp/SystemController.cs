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
using Game;
using Game.InputManagement;
using Game.UI;

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

	}
	
	/// <summary>
    /// Update is called once per frame.
    /// </summary>
	void Update () 
    {
        float translation = m_inputManager.getPlayer1Translation();
        print(translation);
        // TODO: Stub code.

	}

    #endregion

    #region Service Methods

    /// <summary>
    /// Initialize the managers.
    /// </summary>
    private void Initialize()
    {

        // Create and initialize the UI, Game, and Input managers.
        m_uiManager = new UIManager();
        m_gameManager = new GameManager();
        m_inputManager = new InputManager();

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
