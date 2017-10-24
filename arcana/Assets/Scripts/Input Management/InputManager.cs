/************************************************
 * InputManager.cs
 * 
 * This file contains implementation for the InputManager class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Entities.Attributes;

namespace Arcana.InputManagement
{

    #region Enum: Controller.

    /// <summary>
    /// Controller maps to a control scheme.
    /// </summary>
    public enum Controller
    {
        System, 
        Debug,
        Player1,
        Player2
    }

    #endregion

    #region Class: InputManagerFactory class.

    /////////////////////
    // Factory class.
    /////////////////////

    /// <summary>
    /// Factory that returns InputManager components.
    /// </summary>
    public class InputManagerFactory : IFactory<InputManager>
    {

        #region // Static Members.

        /////////////////////
        // Static members.
        /////////////////////

        /// <summary>
        /// Instance of the factory.
        /// </summary>
        private static InputManagerFactory instance = null;

        /// <summary>
        /// Instance of the manager.
        /// </summary>
        private static InputManager manager = null;

        /// <summary>
        /// Returns factory instance.
        /// </summary>
        /// <returns>Returns reference to manager factory instance.</returns>
        public static InputManagerFactory Instance()
        {
            if (instance == null)
            {
                instance = new InputManagerFactory();
            }

            return instance;
        }

        /// <summary>
        /// Get reference to the manager.
        /// </summary>
        /// <returns>Returns a single manager.</returns>
        public static InputManager GetManagerInstance()
        {
            return manager;
        }

        /// <summary>
        /// On creation, set this to be the instance.
        /// </summary>
        private InputManagerFactory()
        {
            instance = this;
        }

        #endregion

        #region // Factory Methods.

        /////////////////////
        // Factory methods.
        /////////////////////

        /// <summary>
        /// Get (or create) the single instance of the factory.
        /// </summary>
        /// <returns>Returns a single factory instance.</returns>
        public IFactory<InputManager> GetInstance()
        {
            return Instance();
        }

        /// <summary>
        /// Create component on new empty object with default settings.
        /// </summary>
        /// <returns>Returns newly created component.</returns>
        public InputManager CreateComponent()
        {
            if (!HasManagerInstance())
            {
                Debugger.Print("Create InputManager on an empty game object, with default parameters.");
                manager = CreateComponent(Services.CreateEmptyObject("Input Manager"), CreateSettings());
            }

            return manager;
        }

        /// <summary>
        /// Adds a new component to the parent game object, with parameters.
        /// </summary>
        /// <param name="parent">GameObject to add component to.</param>
        /// <param name="parameters">Settings to apply to the new manager.</param>
        /// <returns>Return newly created component.</returns>
        public InputManager CreateComponent(GameObject parent, Constraints parameters)
        {
            // Check if there is already an instance of the manager component.
            if (!HasManagerInstance())
            {
                // Check game object.
                if (parent == null)
                {
                    // If the parent itself is null, do not return a component.
                    Debugger.Print("Tried to add a component but parent GameObject is null.", "NULL_REFERENCE");
                    return null;
                }

                // Get reference to existing script if it already exists on this parent.
                manager = parent.GetComponent<InputManager>();

                // If the manager is null.
                if (manager == null)
                {
                    // If the manager instance is null, then create the component.
                    Debugger.Print("Create and add the InputManager component.");
                    manager = parent.AddComponent<InputManager>();
                }

                // Assign non-optional information.
                manager.Initialize();

                // Initialize the entity.
                foreach (string key in parameters.ValidEntries)
                {
                    manager.Initialize(key, parameters.GetEntry(key).Value);
                }
            }

            return manager;
        }

        /// <summary>
        /// Create component on the parent object with default settings.
        /// </summary>
        /// <param name="parent">Parent receiving the component.</param>
        /// <returns>Returns newly created component.</returns>
        public InputManager CreateComponent(GameObject parent)
        {
            if (!HasManagerInstance())
            {
                manager = CreateComponent(parent, CreateSettings());
            }

            return manager;
        }

        /// <summary>
        /// Create the Constraints for initialization of the fabricated class.
        /// </summary>
        /// <returns>Returns one Constraints object.</returns>
        public Constraints CreateSettings()
        {
            // Create the collection.
            Debugger.Print("Creating settings for InputManager initialization.");
            Constraints parameters = new Constraints();

            // TODO: Add non-nulllable types.
            // parameters.AddValue<T>(Constants., ); // Parameter.

            return parameters;
        }

        #endregion

        #region // Service Methods.

        /////////////////////
        // Service methods.
        /////////////////////

        /// <summary>
        /// Returns true if there is a manager instance.
        /// </summary>
        /// <returns>Returns flag defining instance state.</returns>
        public static bool HasManagerInstance()
        {
            return (GetManagerInstance() != null);
        }

        /// <summary>
        /// Delete the instance of the StateManager.
        /// </summary>
        public static void Release()
        {
            if (HasManagerInstance())
            {
                UnityEngine.Object.Destroy(manager);
            }
        }

        #endregion

    }
    #endregion
    
    #region Class: InputManager class.

    /// <summary>
    /// Handles input functionality.
    /// </summary>
    public class InputManager: MonoBehaviour, IFactoryElement
    {

        #region Static Members.

        /////////////////////
        // Static members.
        /////////////////////

        /// <summary>
        /// Returns the instance of the manager.
        /// </summary>
        public static InputManager Instance
        {
            get { return InputManagerFactory.GetManagerInstance(); }
        }

        /// <summary>
        /// Return true if instance exists.
        /// </summary>
        /// <returns>Returns a boolean value.</returns>
        public static bool HasInstance()
        {
            return (InputManager.Instance != null);
        }

        #endregion

        #region Data Members.

        /////////////////////
        // Fields.
        /////////////////////
        
        /// <summary>
        /// Creates control schemse for each player.
        /// </summary>
        private Dictionary<Controller, ControlScheme> m_schemes;

        /// <summary>
        /// Initialization flag.
        /// </summary>
        private bool m_initialized;

        /////////////////////
        // Properties.
        /////////////////////

        public Dictionary<Controller, ControlScheme> ControlSchemes
        {
            get { return this.m_schemes; }
        }
        
        #endregion

        #region Service Methods.

        /////////////////////
        // Service methods.
        /////////////////////

        #region UnityEngine Methods.

        /// <summary>
        /// Run when the component is created for the very first time.
        /// </summary>
        public void Start()
        {
            // Start method.
        }

        /// <summary>
        /// Update meta information about input.
        /// </summary>
        public void Update()
        {
            // Update.
        }

        #endregion

        #region Initialization Methods.
        
        /// <summary>
        /// Initialize the StateManager.
        /// </summary>
        internal void Initialize()
        {
            if (!this.m_initialized)
            {
                // Initialize the entity manager.
                Debugger.Print("Initializing input manager.", gameObject.name);

                // Initialize control scheme.
                this.m_schemes = new Dictionary<Controller, ControlScheme>();

                // Initialization flag.
                this.m_initialized = true;
            }
        }

        /// <summary>
        /// Initialize individual properties, assigned by select cases.
        /// </summary>
        /// <param name="parameter">Parameter to assign value to.</param>
        /// <param name="value">Value to assign.</param>
        public void Initialize(string parameter, object value)
        {
            switch (parameter)
            {
                // TODO.
            }
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Return flag checking if an action has been performed, based on the input player index.
        /// </summary>
        /// <param name="_action">Action being checked for.</param>
        /// <param name="_controller">Controller requesting action check.</param>
        /// <returns>Returns a boolean true if action has been completed.</returns>
        public bool GetAction(Controller _controller = Controller.System, Actions _action = Actions.Idle)
        {
            // Check player index compared to control scheme.
            if (ControlSchemes.ContainsKey(_controller))
            {
                return ControlSchemes[_controller].TriggerAction(_action);
            }

            // If no scheme, no action.
            return false;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Add a control scheme to the manager.
        /// </summary>
        /// <param name="_controller">Controller.</param>
        /// <param name="_scheme">Scheme to add.</param>
        public void AddControlScheme(Controller _controller, ControlScheme _scheme)
        {
            if (!ControlSchemes.ContainsKey(_controller))
            {
                ControlSchemes.Add(_controller, _scheme);
            }
            else
            {
                // Overwrite existing if it is there.
                ControlSchemes[_controller] = _scheme;
            }
        }

        #endregion

        #endregion

        #region Deprecated Functions

        // TODO: Stub.
        public float getPlayer1Translation()
        {
            float translation = Input.GetAxis("Horizontal");

            return translation;
        }

        /// <summary>
        /// Return a vector2 of the current facing direction of the right joystick
        /// </summary>
        /// <returns></returns>
        public Vector2 getVectorInput()
        {

            //create and set input||Vector2
            Vector2 input = new Vector2();
            input.x = Input.GetAxis("RightJoystickHorizontal");
            input.y = Input.GetAxis("RightJoystickVerticle");

            //return
            return input;

        }

        //returns whether or not the player has pressed the jump button
        public bool getPlayer1Jump()
        {
            return (bool)Input.GetButtonDown("Jump");
        }

        //returns whether or not the player has pressed the fire button
        public bool getProjectileFire()
        {
            if(Input.GetAxis("Fire1Controller") != 0)
            {
                return true;
            }
            return (bool)Input.GetButtonDown("Fire1");
        }

        #endregion

    }

    #endregion

}
