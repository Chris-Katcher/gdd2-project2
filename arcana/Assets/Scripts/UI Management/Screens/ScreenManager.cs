/************************************************
 * ScreenManager.cs
 * 
 * This file contains:
 * - The ScreenManager class. (Child of ArcanaObject).
 * - The ScreenID enum.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana.Utilities;
using UnityEngine;

namespace Arcana.UI.Screens
{

    #region Class: ScreenManager class.

    /// <summary>
    /// Creates and manages the IScreen objects.
    /// </summary>
    public class ScreenManager : ArcanaObject
    {

        #region Static Methods.

        #region Enum Parsing Method.

        /////////////////////
        // Enum parsing.
        /////////////////////

        /// <summary>
        /// Get the name of the enum.
        /// </summary>
        /// <param name="_id">ID of the enum.</param>
        /// <returns>Returns string containing the name of the screen.</returns>
        public static string Parse(ScreenID _id)
        {
            string result = "";

            switch (_id)
            {
                case ScreenID.SplashScreen:
                    result = "(Splash Screen)";
                    break;
                case ScreenID.PauseScreen:
                    result = "(Pause Screen)";
                    break;
                case ScreenID.MainMenuScreen:
                    result = "(Main Menu Screen)";
                    break;
                case ScreenID.OptionsScreen:
                    result = "(Options Screen)";
                    break;
                case ScreenID.GameplayScreen:
                    result = "(Gameplay Screen)";
                    break;
                case ScreenID.GameoverScreen:
                    result = "(GameOver Screen)";
                    break;
                case ScreenID.LoadingScreen:
                    result = "(Loading Screen)";
                    break;
                case ScreenID.NULL_SCREEN:
                    result = "(Null State)";
                    break;
                default:
                    result = "(Unknown Screen)";
                    break;
            }

            return result;
        }

        #endregion

        #region Instancing Methods.

        /////////////////////
        // Static methods for instancing.
        /////////////////////

        /// <summary>
        /// Static instance of the class. (We only want one).
        /// </summary>
        public static ScreenManager instance = null;

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        /// <returns>Returns a component.</returns>
        public static ScreenManager GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of ScreenManager.");
                instance = Services.CreateEmptyObject("Screen Manager").AddComponent<ScreenManager>();
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

        #region Component Factory Methods.

        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static ScreenManager Create(ArcanaObject _parent)
        {
            if (!HasInstance())
            {
                instance = _parent.GetComponent<ScreenManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<ScreenManager>(_parent);
            }

            return instance;
        }

        #endregion

        #region Screen Members.

        /// <summary>
        /// Screen safety scale amount.
        /// </summary>
        private static Vector2 s_safety = Vector3.one;

        // Safety scale value for calculating 'safe' boundaries.
        public static Vector2 Safety
        {
            get { return s_safety; }
            set
            {
                s_safety = Services.Max(value, new Vector2(0.5f, 0.5f));
            }
        }

        // Return the screen dimensions.

        /// <summary>
        /// Width of the game window.
        /// </summary>
        public static float WindowWidth
        {
            get { return UnityEngine.Screen.width; }
        }

        /// <summary>
        /// Height of the screen.
        /// </summary>
        public static float WindowHeight
        {
            get { return UnityEngine.Screen.height; }
        }

        /// <summary>
        /// Returns the boundaries of the window.
        /// </summary>
        public static Rect WindowBounds
        {
            get { return new Rect(0.0f, 0.0f, WindowWidth, WindowHeight); }
        }
        
        // Return the current resolution.

        /// <summary>
        /// Returns the current resolution of the window. (If in windowed mode, this will return the current resolution of the desktop).
        /// </summary>
        public static Resolution WindowResolution
        {
            get { return UnityEngine.Screen.currentResolution; }
        }

        // Return the aspect ratio.

        /// <summary>
        /// Returns the aspect ratio of the window.
        /// </summary>
        public static float AspectRatio
        {
            get { return WindowWidth / WindowHeight; }
        }
        
        // Return the center point on the screen.

        /// <summary>
        /// Returns the calculated center of the window, in window-space.
        /// </summary>
        public static Vector2 Center
        {
            get { return new Vector2(WindowWidth / 2.0f, WindowHeight / 2.0f); }
        }

        // Return a random point within the screen bounds.

        /// <summary>
        /// Returns a random point on the screen.
        /// </summary>
        /// <returns>Returns a Vector3 within the screen bounds.</returns>
        public static Vector3 GetPoint()
        {
            return Services.NextVector3(WindowBounds);
        }

        /// <summary>
        /// Returns a random point located within an on-screen boundary.
        /// </summary>
        /// <param name="_bounds">Boundary to get point from.</param>
        /// <returns>Returns a Vector2 wtihin the input bounds.</returns>
        public static Vector2 GetPointInBounds(Rect _bounds)
        {
            return Services.NextVector2(_bounds);
        }
        
        #endregion

        #endregion

        #region Data Members

        #region Fields.

        /// <summary>
        /// Map of all IScreen instances, with their associated ID's.
        /// </summary> 
        private Dictionary<ScreenID, IScreen> m_screens { get; set; }

        #endregion

        #region Properties.

        /// <summary>
        /// Reference to IScreen map.
        /// </summary>
        public Dictionary<ScreenID, IScreen> Screens
        {
            get
            {
                if (this.m_screens == null)
                {
                    this.m_screens = new Dictionary<ScreenID, IScreen>();
                }
                return this.m_screens;
            }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Update is called every frame to update the position of game objects in the frame.
        /// </summary>
        /// <param name="delta">Elapsed time since last frame (in seconds).</param>
        public override void Update()
        {
            if (!this.Initialized)
            {
                // Call the base method.
                base.Update();
            }
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initialize the StateManager.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the base method.
                base.Initialize();

                // Set the safety.
                ScreenManager.Safety = new Vector2(0.80f, 0.95f);
            }
        }

        #endregion
                
        #region Mutator Methods
        


        #endregion

        #region Accessor Methods
        
        /// <summary>
        /// Check if an entry in the map has been matched with this id.
        /// </summary>
        /// <param name="_id">ID to check for.</param>
        /// <returns>Returns true if entry exists in map.</returns>
        public bool HasScreenID(ScreenID _id)
        {
            return this.Screens.ContainsKey(_id);
        }

        /// <summary>
        /// Check if a non-null entry in the map has been matched with this id.
        /// </summary>
        /// <param name="_id">ID to check for.</param>
        /// <returns>Returns true if entry exists in map and is not null.</returns>
        public bool HasScreen(ScreenID _id)
        {
            return ((HasScreenID(_id) && Screens[_id] != null));
        }

        /// <summary>
        /// Get IScreen returns the screen at the specified ID.
        /// </summary>
        /// <param name="_id">Desired IScreen's ID.</param>
        /// <returns>Returns IScreen associated with the input ID, if it exists.</returns>
        public IScreen GetScreen(ScreenID _id)
        {
            if (HasScreen(_id))
            {
                return Screens[_id];
            }
            else
            {
                Debugger.Print("There is no screen associated with the input ScreenID " + Parse(_id) + ".");
                return null;
            }
        }

        /// <summary>
        /// Returns the Dictionary mapping ScreenIDs to Screens.
        /// </summary>
        /// <returns>Returns <see cref="m_screens"/>.</returns>
        public Dictionary<ScreenID, IScreen> GetScreens()
        {
            return this.Screens;
        }

        #endregion

    }

    #endregion

    #region Enum: ScreenID.

    /// <summary>
    /// ID values associated with given screens.
    /// </summary>
    public enum ScreenID
    {
        SplashScreen = 0,
        LoadingScreen = 1,
        MainMenuScreen = 2,
        GameplayScreen = 3,
        GameoverScreen = 4,
        OptionsScreen = 5,
        PauseScreen = 6,
        NULL_SCREEN = 7
    }

    #endregion

}
