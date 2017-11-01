/************************************************
 * UIManager.cs
 * 
 * This file holds the implementation for the UIManager class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Arcana.Utilities;
using Arcana.UI.Screens;

// The UI namespace contains the visual layer for all of the user-facing information.
namespace Arcana.UI
{

    #region Enum: States.

    /// <summary>
    /// Enum keeps track of possible in-game states.
    /// </summary>
    public enum States
    {
        Gameover,
        Gameplay,
        Loading,
        MainMenu,
        Options,
        Pause,
        Splash
    }

    #endregion

    #region Class: UIManager class.

    /// <summary>
    /// UIManager controls visual elements that show up on the player's screen.
    /// </summary>
    [AddComponentMenu("Arcana/Managers/UIManager")]
    public class UIManager : ArcanaObject
    {

        States state = States.Splash;

        private bool stateLoaded = false;

        private ScreenManager m_screenManager = null;

        public RectTransform healthBar1;
        public RectTransform healthBar2;

        #region UnityEngine Methods.

        /// <summary>
        /// Run when the component is created for the very first time.
        /// </summary>
        public override void Start()
        {
            // Start method.
        }

       

        #endregion

        #region Component Factory Methods.

        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static UIManager Create(ArcanaObject _parent)
        {
            if (!HasInstance())
            {
                instance = _parent.GetComponent<UIManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<UIManager>(_parent);
            }

            return instance;
        }

        #endregion


        #region Mutator Methods

        /// <summary>
        /// Update UI on change of state.
        /// </summary>
        public void UpdateState()
        {
            if (!Initialized)
            {
                this.Initialize();
            }
            else
            {
                // Update.
                if (m_screenManager != null && !stateLoaded)
                {
                    
                    PerformChangeState();
                    this.stateLoaded = true;
                }
            }
        }

        public void ChangeState(States _state)
        {
            if(this.stateLoaded && _state != this.state)
            {
                
                this.state = _state;
                this.stateLoaded = false;
            }
        }

        private void PerformChangeState()
        {
            switch (this.state)
            {
                // Change state to Gameover
                case States.Gameover:

                    m_screenManager.DisplayGameOverScreen();
                    break;

                // Change state to GamePlay
                case States.Gameplay:

                    m_screenManager.DisplayGameplayScreen();
                    break;

                // Change state to Loading
                case States.Loading:

                    //TODO

                    break;

                // Change state to Main Menu
                case States.MainMenu:

                    //TODO

                    break;

                // Change state to Options
                case States.Options:

                    //TODO

                    break;

                // Change state to Pause
                case States.Pause:

                    //TODO

                    break;

                // Change state to Splash
                case States.Splash:

                    m_screenManager.DisplaySplashScreen();
                    break;
            }
        }

        public void UpdateHealthBars(int p1, int p2)
        {
            healthBar1.sizeDelta = new Vector2(p1, healthBar1.sizeDelta.y);
            //healthBar2.sizeDelta = new Vector2(p2, healthBar2.sizeDelta.y);
        }

        public void SetHealthBarRect(RectTransform rect)
        {
            this.healthBar1 = rect;
        }

        #endregion

        #region Instancing Methods.
        /// <summary>
        /// Static instance of the class. (We only want one).
        /// </summary>
        public static UIManager instance = null;

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        /// <returns>Returns a component.</returns>
        public static UIManager GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of EntityManager.");
                instance = Services.CreateEmptyObject("UI Manager").AddComponent<UIManager>();
            }

            return instance;
        }

        /// <summary>
        /// Returns true if instance exists.
        /// </summary>
        /// <returns>Returns boolean indicating instance existence.</returns>
        public static bool HasInstance()
        {
            return (instance != null);
        }
        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Create the data members for the UIManager.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Initialize the base values.
                base.Initialize();

                // Set this name.
                this.Name = "UI Manager";

                // Initialize the ui manager.
                Debugger.Print("Initializing ui manager.", this.Self.name);

                m_screenManager = new ScreenManager();
                m_screenManager.Initialize();

                
            }
        }

        #endregion
    }

    #endregion  
}
