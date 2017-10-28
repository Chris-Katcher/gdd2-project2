/************************************************
 * ControlScheme.cs
 * 
 * This file contains:
 * - The ControlScheme class.
 * - The Actions enum.
 * - The Command class.
 * - The CommandResponse class.
 * - The CommandSequence class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.InputManagement
{

    #region Class: ControlScheme class.

    /// <summary>
    /// The ControlScheme links Directors to their commands.
    /// </summary>
    public class ControlScheme : ArcanaObject
    {

        #region Static Members.

        /// <summary>
        /// Creates an empty control scheme for a particular director.
        /// </summary>
        /// <param name="_parent">Parent receiving the control scheme.</param>
        /// <returns>Returns the control scheme component.</returns>
        public static ControlScheme Create(ArcanaObject _parent)
        {
            ArcanaObject parent = _parent;

            if (parent == null)
            {
                Debugger.Print("Creating empty controller.");
                parent = Services.CreateEmptyObject("Controller").AddComponent<ArcanaObject>();
            }
                        
            // Add the component to the parent.
            return parent.Self.AddComponent<ControlScheme>();
        }
        
        /// <summary>
        /// Creates an action, if it doesn't already exist, or returns the reference to the existing one.
        /// </summary>
        /// <returns>Action object.</returns>
        public static Action CreateAction(string _id, Director _director = Director.System)
        {
            Action action;

            // Check if key exists.
            if (Action.HasAction(_id))
            {
                action = Action.GetAction(_id);
                action.AddDirector(_director);
            }
            else
            {
                action = new Action(_id, _director);
            }

            if (action.IsNull) { return null; }

            return action;
        }

        /// <summary>
        /// Create a control object using a keycode.
        /// </summary>
        /// <param name="_key">Keycode to set.</param>
        /// <returns>Returns a control object.</returns>
        public static Control CreateControl(KeyCode _key)
        {
            return new Control(_key);
        }

        /// <summary>
        /// Create a control object using a mouse button.
        /// </summary>
        /// <param name="_mouseButton">Mouse button to set.</param>
        /// <returns>Returns a control object.</returns>
        public static Control CreateControl(MouseButton _mouseButton)
        {
            return new Control(_mouseButton);
        }

        /// <summary>
        /// Create a control object using an axis name.
        /// </summary>
        /// <param name="_axis">Axis to set.</param>
        /// <returns>Returns a control object.</returns>
        public static Control CreateControlAxis(string _axis)
        {
            return new Control(_axis, ControlType.Axis);
        }

        /// <summary>
        /// Create a control object using a button name.
        /// </summary>
        /// <param name="_button">Button to set.</param>
        /// <returns>Returns a control object.</returns>
        public static Control CreateControlButton(string _button)
        {
            return new Control(_button, ControlType.Button);
        }

        /// <summary>
        /// Creates a trigger object.
        /// </summary>
        /// <param name="_control">Control to be triggered.</param>
        /// <returns>Returns created trigger.</returns>
        public static Trigger CreateTrigger(Control _control)
        {
            return new Trigger(_control);
        }

        /// <summary>
        /// Creates a trigger object.
        /// </summary>
        /// <param name="_control">Control to be triggered.</param>
        /// <param name="_mode">Response mode type.</param>
        /// <returns>Returns created trigger.</returns>
        public static Trigger CreateTrigger(Control _control, ResponseMode _mode)
        {
            return new Trigger(_control, _mode);
        }
        
        #endregion

        #region Data Members

        #region Fields.

        /// <summary>
        /// Actions associated with this control scheme.
        /// </summary>
        private List<Action> m_actions;

        /// <summary>
        /// Keeps track of values from previous frame.
        /// </summary>
        private Dictionary<Action, float> m_previousValues;

        /// <summary>
        /// Keeps track of values on current frame.
        /// </summary>
        private Dictionary<Action, float> m_currentValues;

        /// <summary>
        /// Maps actions to controls.
        /// </summary>
        private Dictionary<Action, List<Trigger>> m_controlMaps;

        #endregion

        #region Properties.

        /// <summary>
        /// Reference to actions in this control scheme.
        /// </summary>
        public List<Action> Actions
        {
            get
            {
                if (ActionCount == 0)
                {
                    this.m_actions = new List<Action>();
                }
                return this.m_actions;
            }
        }
        
        /// <summary>
        /// Reference to the previous values collection.
        /// </summary>
        private Dictionary<Action, float> PreviousValues
        {
            get
            {
                if (ActionCount == 0)
                {
                    this.m_previousValues = new Dictionary<Action, float>();
                }
                return this.m_previousValues;
            }
        }
        
        /// <summary>
        /// Reference to the current values collection.
        /// </summary>
        private Dictionary<Action, float> CurrentValues
        {
            get
            {
                if (ActionCount == 0)
                {
                    this.m_currentValues = new Dictionary<Action, float>();
                }
                return this.m_currentValues;
            }
        }

        /// <summary>
        /// Reference to action-control-trigger map.
        /// </summary>
        public Dictionary<Action, List<Trigger>> Triggers
        {
            get {
                if (this.m_controlMaps == null)
                {
                    this.m_controlMaps = new Dictionary<Action, List<Trigger>>();
                }
                return this.m_controlMaps;
            }
        }

        /// <summary>
        /// Count the number of actions with triggers.
        /// </summary>
        public int ActionCount
        {
            get
            {
                return Triggers.Count;
            }
        }

        /// <summary>
        /// Counts the triggers that have been assigned.
        /// </summary>
        public int TriggerCount
        {
            get
            {
                int count = 0;

                foreach(Action action in Actions)
                {
                    // Add the size of triggers under each one.
                    count += GetTriggers(action).Count;
                }

                return count;
            }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Updates the current and previous axis values.
        /// </summary>
        public override void Update()
        {
            // Call the base update.
            base.Update();

            // If not paused.
            if (!this.Status.IsPaused() && !this.Status.IsInactive())
            {
                UpdateValues();
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the control scheme properties.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Initialize the base.
                base.Initialize();

                // Init properties.
                this.m_controlMaps = new Dictionary<Action, List<Trigger>>();
                this.m_previousValues = new Dictionary<Action, float>();
                this.m_currentValues = new Dictionary<Action, float>();
            }
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns a true of false value if the action trigger value has changed since last frame.
        /// </summary>
        /// <param name="_action">Action to check.</param>
        /// <returns>Returns true if values differ.</returns>
        public bool ValueChanged(Action _action)
        {
            if (HasAction(_action))
            {
                return (this.PreviousValues[_action] != this.CurrentValues[_action]);
            }

            // If action doesn't exist, it couldn't have changed.
            return false;
        }

        /// <summary>
        /// Returns axis value for input action, if possible.
        /// </summary>
        /// <param name="_action">Action to get value for.</param>
        /// <returns>Returns float value.</returns>
        public float GetValue(Action _action)
        {
            if (HasAction(_action))
            {
                foreach (Trigger trigger in Triggers[_action])
                {
                    // If any of the triggers are activated, return value.
                    if (trigger.IsActivated())
                    {
                        return trigger.Value;
                    }
                }
            }

            // Return 0.0f if doesn't exist or no triggers were active. 
            return 0.0f;
        }

        /// <summary>
        /// Returns true if the action has been activated.
        /// </summary>
        /// <param name="_action">Action to check triggers for.</param>
        /// <returns>Returns true if activated</returns>
        public bool IsActivated(Action _action)
        {
            if (HasAction(_action))
            {
                foreach (Trigger trigger in Triggers[_action])
                {
                    // If any of the triggers are activated, return true.
                    if (trigger.IsActivated())
                    {
                        return true;
                    }
                }
            }

            // Return false if doesn't exist or no triggers were active. 
            return false;
        }

        /// <summary>
        /// Returns true if action exists in the collection.
        /// </summary>
        /// <param name="_action">Checks to see if action exists.</param>
        /// <returns>Returns true if action exists.</returns>
        public bool HasAction(Action _action)
        {
            Action action = _action;

            if (action != null && ActionCount > 0)
            {
                return this.Triggers.ContainsKey(action);
            }

            return false;
        }

        /// <summary>
        /// Checks to see if a control has been used.
        /// </summary>
        /// <param name="_control">Control to check for.</param>
        /// <returns>Returns true if control exists.</returns>
        public bool HasControl(Control _control)
        {
            if (ActionCount > 0)
            {
                foreach (Action action in Actions)
                {
                    foreach (Trigger trigger in GetTriggers(action))
                    {
                        if (trigger.Control == _control)
                        {
                            return true;
                        }
                    }
                }
            }
            
            // Return false by default.
            return false;            
        }

        /// <summary>
        /// Returns collection of all actions in the control scheme.
        /// </summary>
        /// <returns>Returns collection of actions.</returns>
        public List<Action> GetActions()
        {
            return Actions;
        }

        /// <summary>
        /// Returns the collection of triggers associated with an action.
        /// </summary>
        /// <param name="_action">Action to get triggers for.</param>
        /// <returns>Returns collection of triggers.</returns>
        public List<Trigger> GetTriggers(Action _action)
        {
            if (HasAction(_action))
            {
                return Triggers[_action];
            }

            // Return an empty list if action doesn't exist.
            return new List<Trigger>();
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Update the value collections.
        /// </summary>
        private void UpdateValues()
        {
            // Set the previous values to the current.
            this.m_previousValues = new Dictionary<Action, float>(this.m_currentValues);

            for (int i = 0; i < ActionCount; i++)
            {
                for (int j = 0; j < Triggers[Actions[i]].Count; j++)
                {
                    bool axisFound = false;
                    foreach (Trigger trigger in Triggers[Actions[i]])
                    {
                        if (!axisFound && trigger.IsButtonTrigger())
                        {
                            this.m_currentValues[Actions[i]] = trigger.Value;
                        }

                        if (trigger.IsAxisTrigger())
                        {
                            this.m_currentValues[Actions[i]] = trigger.Value;
                            axisFound = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add action if it doesn't exist, and add a trigger to it.
        /// </summary>
        /// <param name="_action">Action to add to collection if it doesn't exist.</param>
        /// <param name="_trigger">Trigger to add to action.</param>
        public void AddMap(Action _action, Trigger _trigger)
        {
            if (!HasAction(_action) && _action != null)
            {
                this.Actions.Add(_action);
                this.PreviousValues.Add(_action, 0.0f);
                this.CurrentValues.Add(_action, 0.0f);
                this.Triggers.Add(_action, new List<Trigger>());
            }
            
            this.AddTrigger(_action, _trigger);
        }

        /// <summary>
        /// Add a trigger to specified action.
        /// </summary>
        /// <param name="_action">Action to add trigger to.</param>
        /// <param name="_trigger">Trigger to add to action.</param>
        public void AddTrigger(Action _action, Trigger _trigger)
        {
            if (HasAction(_action))
            {
                if (!Triggers[_action].Contains(_trigger))
                {
                    Triggers[_action].Add(_trigger);
                }
            }
        }

        #endregion

    }

    #endregion

    #region Class: Action class.

    /// <summary>
    /// The input action maps a string to an action.
    /// Actions do not care about the 'type' of input used.
    /// </summary>
    public class Action
    {

        #region Static Members.

        #region Static Fields.

        /// <summary>
        /// Unique action IDs associated with every input action.
        /// </summary>
        private static List<string> s_actionIDs = null;

        /// <summary>
        /// Map of unique identifiers to specified actions.
        /// </summary>
        private static Dictionary<string, Action> s_actions = null;

        #endregion

        #region Static Properties.

        /// <summary>
        /// Returns collection of all IDs.
        /// </summary>
        public static List<string> ActionIDs
        {
            get
            {
                if (s_actionIDs == null)
                {
                    s_actionIDs = new List<string>();
                }
                return s_actionIDs;
            }
        }

        /// <summary>
        /// Returns map of all the actions.
        /// </summary>
        public static Dictionary<string, Action> ActionMap
        {
            get
            {
                // Check actions map.
                if (s_actions == null)
                {
                    s_actions = new Dictionary<string, Action>();
                }

                return s_actions;
            }
        }

        /// <summary>
        /// Returns all of the actions as a list.
        /// </summary>
        public static List<Action> Actions
        {
            get
            {
                List<Action> actions = new List<Action>();

                // Loop through UIDs.
                foreach (string id in ActionIDs)
                {
                    if (ActionMap.ContainsKey(id))
                    {
                        actions.Add(ActionMap[id]);
                    }
                }

                return actions;
            }
        }

        /// <summary>
        /// Returns total amount of input actions that have been registered.
        /// </summary>
        public static int ActionCount
        {
            get
            {
                if (s_actions == null)
                {
                    return 0;
                }
                return s_actions.Count;
            }
        }

        #endregion

        #region Static Accessor Methods.

        /// <summary>
        /// Creates a new Action or returns the existing one.
        /// </summary>
        /// <param name="_id">Unique ID associated with the action.</param>
        /// <returns>Returns action object.</returns>
        public static Action CreateAction(string _id, Director _director)
        {
            // Get key.
            string key = MakeKey(_id);

            if (HasKey(key) && HasAction(key))
            {
                Action action = ActionMap[key];

                if (action.HasDirector(_director))
                {
                    action.AddDirector(_director);
                }

                return action;
            }

            return new Action(key, _director);
        }

        /// <summary>
        /// Returns the Action associated with the given ID.
        /// </summary>
        /// <param name="_id">Unique ID of an action.</param>
        /// <returns>Returns an Action struct.</returns>
        public static Action GetAction(string _id)
        {
            // Set up values to check.
            Action response = null;
            string key = MakeKey(_id); // Trimmed all uppercase.

            // Check if the key is valid.
            if (key.Length == 0 || ActionCount == 0)
            {
                return null;
            }

            // Get reference to the key, if it exists in the map.
            if (ActionMap.ContainsKey(key))
            {
                response = ActionMap[key];
            }

            // Return the sought after action.
            return response;
        }

        /// <summary>
        /// Returns the Action associated with the given ID.
        /// </summary>
        /// <param name="_id">Unique ID of an action.</param>
        /// <param name="_director">Director to filter for.</param>
        /// <returns>Returns an Action struct.</returns>
        public static Action GetAction(string _id, Director _director)
        {
            // Set up values to check.
            Action response = null;
            string key = MakeKey(_id); // Trimmed all uppercase.

            // Check if the key is valid.
            if (key.Length == 0 || ActionCount == 0)
            {
                return null;
            }

            // Get reference to the key, if it exists in the map.
            if (ActionMap.ContainsKey(key))
            {
                response = Action.GetAction(key);
                if (response.HasDirector(_director))
                {
                    response = ActionMap[key];
                }
            }
            
            // Return the sought after action.
            return response;
        }

        /// <summary>
        /// Check if the key exists in the collection.
        /// </summary>
        /// <param name="_id">Input ID.</param>
        /// <returns>Returns true if key exists.</returns>
        public static bool HasKey(string _id)
        {
            // Get key.
            string key = MakeKey(_id);

            // Check key.
            if (key.Length == 0)
            {
                return false;
            }

            return ActionIDs.Contains(key);
        }

        /// <summary>
        /// Check if there actually is an action in the collection.
        /// </summary>
        /// <param name="_id">Input ID.</param>
        /// <returns>Returns true if key-value pair exists.</returns>
        public static bool HasAction(string _id)
        {
            // Get key.
            string key = MakeKey(_id);

            // Check if valid key is in collection.
            return (HasKey(key)) && (ActionMap.ContainsKey(key));
        }

        /// <summary>
        /// Returns a list of actions that associate with a particular director.
        /// </summary>
        /// <param name="_director">Director to find actions for.</param>
        /// <returns>Returns a collection of actions.</returns>
        public static List<Action> GetActions(Director _director)
        {
            List<Action> actions = new List<Action>();

            // Check each action.
            foreach (Action action in Actions)
            {
                if (action.HasDirector(_director))
                {
                    actions.Add(action);
                }
            }

            return actions;
        }

        #endregion

        #region Static Mutator Methods

        /// <summary>
        /// Takes an input and turns it into a key.
        /// </summary>
        /// <param name="_id">Input ID.</param>
        /// <returns>Returns a trimmed, uppercase string.</returns>
        public static string MakeKey(string _id)
        {
            return _id.Trim().ToUpper();
        }

        /// <summary>
        /// Takes an input and adds it as a key.
        /// </summary>
        /// <param name="_id">Input ID.</param>
        /// <returns>Returns a boolean if key was just added.</returns>
        private static bool RegisterKey(string _id)
        {
            // Get key from input.
            string key = MakeKey(_id);

            // Check if the key is valid and doesn't already exist.
            if (key.Length > 0 && !HasKey(key))
            {
                ActionIDs.Add(key);
                return true;
            }

            // Return false by default.
            return false;
        }

        /// <summary>
        /// Unregister a key.
        /// </summary>
        /// <param name="_id">ID to remove.</param>
        private static string RemoveKey(string _id)
        {
            // Get key from input.
            string key = MakeKey(_id);

            // Check if the key is valid and exists.
            if (key.Length > 0 && HasKey(key))
            {
                ActionIDs.Remove(key);
                return key; // Return the value of the key used.
            }

            // Return an empty value.
            return "";
        }

        /// <summary>
        /// Registers an action to an ID.
        /// </summary>
        /// <param name="_id">ID to map to action.</param>
        /// <param name="_action">Action to be mapped.</param>
        /// <returns>Registers action.</returns>
        public static bool RegisterAction(string _id, Action _action)
        {
            // Get key from input.
            string key = MakeKey(_id);

            // Get the action.
            Action action = _action;

            if (key.Length > 0 && action != null)
            {
                // Register the key.
                RegisterKey(key);

                // Check if the key is valid and doesn't already exist.
                if (HasKey(key) && !HasAction(key))
                {
                    ActionMap.Add(key, action);
                    return true;
                }
            }

            // Return false by default.
            return false;
        }

        /// <summary>
        /// Updates an existing action.
        /// </summary>
        /// <param name="_id">ID of action to update.</param>
        /// <param name="_action">Action to set.</param>
        /// <returns>Returns true if successful operation.</returns>
        public static bool UpdateAction(string _id, Action _action)
        {
            // Get the key.
            string key = MakeKey(_id);

            // Get the action.
            Action action = _action;

            // Check if key exists and action exists.
            if (HasKey(key) && action != null)
            {
                // If action exists, replace it.
                if (HasAction(key))
                {
                    ActionMap[key] = action;
                }
                else // If action doesn't exist, register new action.
                {
                    RegisterAction(key, action);
                }

                return true;
            }

            // If key doesn't exist at all, get rid of it.
            return false;
        }

        /// <summary>
        /// Returns the last removed Action value.
        /// </summary>
        /// <param name="_id">Input ID.</param>
        /// <returns>Returns an Action object.</returns>
        public static Action RemoveAction(string _id)
        {
            // Get key from input.
            string key = MakeKey(_id);

            // Get the action.
            Action action = null;

            // Check if the key is valid and doesn't already exist.
            if (key.Length > 0 && HasKey(key) && HasAction(key))
            {
                action = ActionMap[key];
                ActionMap.Remove(key);
            }

            // Return a null by default.
            return action;
        }

        #endregion

        #endregion

        #region Data Members

        #region Fields.

        /// <summary>
        /// Unique string associated with an action.
        /// </summary>
        private string m_actionName;

        /// <summary>
        /// List of directors that may call this command.
        /// </summary>
        private List<Director> m_directors;

        #endregion

        #region Properties.

        /// <summary>
        /// Returns the unique name identifying this action.
        /// </summary>
        public string ID
        {
            get { return this.m_actionName; }
        }

        /// <summary>
        /// Returns list of directors that may access this command.
        /// </summary>
        public List<Director> ValidDirectors
        {
            get
            {
                if (this.m_directors == null)
                {
                    this.m_directors = new List<Director>();
                }
                return this.m_directors;
            }
        }

        /// <summary>
        /// Returns true if the action has no name.
        /// </summary>
        public bool IsNull
        {
            get
            {
                return this.m_actionName.Length == 0;
            }
        }

        #endregion

        #endregion

        #region Constructor.

        /// <summary>
        /// Empty constructor makes an empty action.
        /// </summary>
        public Action()
        {
            this.m_actionName = "";
            this.m_directors = new List<Director>();
        }

        /// <summary>
        /// Creates an Action, while initializing the action name and initial director.
        /// </summary>
        /// <param name="_id">Unique identifier for this action.</param>
        /// <param name="_director">A director that can access this action.</param>
        public Action(string _id, Director _director = Director.System)
        {
            this.m_actionName = _id;
            this.m_directors = new List<Director>();
            RegisterAction(_id, this);
            AddDirector(_director);
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns the name of this control scheme.
        /// </summary>
        /// <returns>Returns string.</returns>
        public string GetName()
        {
            return MakeKey(this.m_actionName);
        }

        /// <summary>
        /// Returns true if has directors.
        /// </summary>
        /// <returns>Returns a boolean</returns>
        public bool HasDirector(Director _director)
        {
            if (this.m_directors == null || this.m_directors.Count == 0)
            {
                this.m_directors = new List<Director>();
            }

            return this.m_directors.Contains(_director);
        }

        /// <summary>
        /// Returns Action as a string.
        /// </summary>
        /// <returns>Returns string containing information about the Action.</returns>
        public override string ToString()
        {
            string result = "";

            if (this.m_actionName.Length > 0)
            {
                result += "Action [\"" + this.m_actionName + "\"]: Owned by ";

                List<string> directors = new List<string>();
                foreach (Director _director in this.ValidDirectors)
                {
                    directors.Add(InputManager.Parse(_director));
                }

                result += Services.Concat(", ", directors.ToArray()) + ".";
            }

            return result;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set the unique name of the action.
        /// </summary>
        /// <param name="_id">Input ID.</param>
        public void SetName(string _id)
        {
            // Check key.
            string key = MakeKey(_id);

            // Check length of key.
            if (key.Length > 0 && !HasKey(key))
            {
                this.m_actionName = key;
                RegisterAction(this.m_actionName, this);
            }
        }

        /// <summary>
        /// Adds and registeres a director to own this particular control scheme.
        /// </summary>
        /// <param name="_director">Director to add.</param>
        public void AddDirector(Director _director)
        {
            // Check if director doesn't exist.
            if (!HasDirector(_director))
            {
                // Add the director.
                this.ValidDirectors.Add(_director);
                UpdateAction(this.m_actionName, this);
            }

        }

        /// <summary>
        /// Remove director from this action, if it exists.
        /// </summary>
        /// <param name="_director">Director to remove.</param>
        public void RemoveDirector(Director _director)
        {
            // Check if director doesn't exist.
            if (HasDirector(_director))
            {
                // Add the director.
                this.ValidDirectors.Remove(_director);
                UpdateAction(this.m_actionName, this);
            }

        }

        #endregion

    }

    #endregion

    #region Struct: Trigger struct.

    /// <summary>
    /// Maps controls to input response types.
    /// </summary>
    public struct Trigger
    {

        #region Data Members.

        #region Fields.

        /// <summary>
        /// The control associated with this trigger.
        /// </summary>
        private Control m_control;

        /// <summary>
        /// The trigger response mode.
        /// </summary>
        private ResponseMode m_triggerMode;

        #endregion

        #region Properties.

        /// <summary>
        /// Reference to the control.
        /// </summary>
        public Control Control
        {
            get { return this.m_control; }
        }

        /// <summary>
        /// Reference to response mode.
        /// </summary>
        public ResponseMode Mode
        {
            get { return this.m_triggerMode; }
        }

        /// <summary>
        /// Returns axis value depending on trigger mode, as well as 1.0f if button mode is true.
        /// </summary>
        /// <returns>Returns a value between -1.0f and 1.0f</returns>
        public float Value
        {
            get
            {
                // Can't be triggered if response mode is set to none.
                if (this.Mode == ResponseMode.None) { return 0.0f; }

                // If an axis.
                if (IsAxisTrigger() && IsAxisMode())
                {
                    switch (this.Mode)
                    {
                        case ResponseMode.Axis:
                            return this.Control.GetAxis();
                        case ResponseMode.AxisRaw:
                            return this.Control.GetAxisRaw();
                    }
                }

                // If a button.
                if(IsActivated()) { return 1.0f; }

                // Return 0.0f if nothing is valid.
                return 0.0f;
            }
        }

        #endregion

        #endregion

        #region Constructors.
        
        /// <summary>
        /// Create a trigger, setting the response mode based on the input control's ControlType.
        /// </summary>
        /// <param name="_control">Control to assign trigger to.</param>
        public Trigger(Control _control)
        {
            this.m_control = _control;
            this.m_triggerMode = ResponseMode.None;
            SetControl(_control);
        }

        /// <summary>
        /// Create a control with an explicitly specified response mode.
        /// </summary>
        /// <param name="_control">Control to assign trigger to.</param>
        /// <param name="_mode">Trigger mode.</param>
        public Trigger(Control _control, ResponseMode _mode)
        {
            this.m_control = _control;
            this.m_triggerMode = _mode;
            SetControl(_control);
            SetResponseMode(_mode);
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set the control and parse out the correct default response mode.
        /// </summary>
        /// <param name="_control">Control to assign trigger to.</param>
        public void SetControl(Control _control)
        {
            this.m_control = _control;

            if (_control.IsAxis())
            {
                // If the control is an axis, set the response mode to axis, by default.
                SetResponseMode(ResponseMode.Axis);
            }

            if (_control.IsButton())
            {
                // If the control is a button, set the response mode to held, by default.
                SetResponseMode(ResponseMode.Held);
            }
        }

        /// <summary>
        /// Set the control with an explicitly specified response mode.
        /// </summary>
        /// <param name="_control">Control to assign trigger to.</param>
        /// <param name="_mode">Mode to set trigger to.</param>
        public void SetControl(Control _control, ResponseMode _mode)
        {
            SetControl(_control);
            SetResponseMode(_mode);
        }

        /// <summary>
        /// Set the response mode.
        /// </summary>
        /// <param name="_mode">Mode to set trigger to.</param>
        public void SetResponseMode(ResponseMode _mode)
        {
            if (IsButtonTrigger())
            {
                if (IsButtonMode(_mode))
                {
                    this.m_triggerMode = _mode;
                    return;
                }
            }

            if (IsAxisTrigger())
            {
                if (IsAxisMode(_mode))
                {
                    this.m_triggerMode = _mode;
                    return;
                }
            }

            Debugger.Print("Cannot set response mode to incompatible control type.");
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// If this is a trigger for a button control, return true.
        /// </summary>
        /// <returns>Returns true if control is a button.</returns>
        public bool IsButtonTrigger()
        {
            return (this.m_control.IsButton());
        }

        /// <summary>
        /// If this is a trigger for an axis control, return true.
        /// </summary>
        /// <returns>Returns true if control is an axis.</returns>
        public bool IsAxisTrigger()
        {
            return (this.m_control.IsAxis());
        }

        /// <summary>
        /// Returns true if the current response mode is for a button.
        /// </summary>
        /// <returns>Returns true based on response mode.</returns>
        public bool IsButtonMode()
        {
            return IsButtonMode(this.m_triggerMode);
        }

        /// <summary>
        /// Returns true if the input response mode is for a button.
        /// </summary>
        /// <returns>Returns true based on input response mode.</returns>
        public bool IsButtonMode(ResponseMode _mode)
        {
            return (_mode == ResponseMode.Held || _mode == ResponseMode.Pressed || _mode == ResponseMode.Released || _mode == ResponseMode.Up || _mode == ResponseMode.None);
        }

        /// <summary>
        /// Returns true if the current response mode is for an axis.
        /// </summary>
        /// <returns>Returns true based on response mode.</returns>
        public bool IsAxisMode()
        {
            return IsAxisMode(this.m_triggerMode);
        }

        /// <summary>
        /// Returns true if the input response mode is for an axis.
        /// </summary>
        /// <returns>Returns true based on input response mode.</returns>
        public bool IsAxisMode(ResponseMode _mode)
        {
            return (_mode == ResponseMode.Axis || _mode == ResponseMode.AxisRaw || _mode == ResponseMode.None);
        }
        
        /// <summary>
        /// Determines if the trigger has been activated.
        /// </summary>
        /// <returns>Returns true if activated.</returns>
        public bool IsActivated()
        {
            // Can't be triggered if response mode is set to none.
            if (this.Mode == ResponseMode.None) { return false; }

            // If a button.
            if (IsButtonTrigger() && IsButtonMode())
            {
                switch (this.Mode)
                {
                    case ResponseMode.Held:
                        return this.Control.IsHeld();
                    case ResponseMode.Pressed:
                        return this.Control.IsPressed();
                    case ResponseMode.Released:
                        return this.Control.IsReleased();
                    case ResponseMode.Up:
                        return !(this.Control.IsHeld() || this.Control.IsPressed());
                }
            }

            // If an axis.
            if (IsAxisTrigger() && IsAxisMode())
            {
                switch (this.Mode)
                {
                    case ResponseMode.Axis:
                        return (this.Control.GetAxis() != 0.0f);
                    case ResponseMode.AxisRaw:
                        return (this.Control.GetAxisRaw() != 0.0f);
                }
            }

            // Return false if nothing is valid.
            return false;
        }

        #endregion

    }

    #endregion

    #region Struct: Control struct.

    /// <summary>
    /// A control directly references a button, axis, or other type of input.
    /// It doesn't care about who calls it.
    /// </summary>
    public struct Control
    {

        #region Static Members.

        /// <summary>
        /// Create a key control.
        /// </summary>
        /// <param name="_key">Key name associated with control.</param>
        /// <returns>Returns Control object.</returns>
        public static Control CreateKey(KeyCode _key)
        {
            return new Control(_key);
        }

        /// <summary>
        /// Create a button control.
        /// </summary>
        /// <param name="_button">Button name associated with control.</param>
        /// <returns>Returns Control object.</returns>
        public static Control CreateButton(string _button)
        {
            return new Control(_button, ControlType.Button);
        }

        /// <summary>
        /// Create a mouse button control.
        /// </summary>
        /// <param name="_button">Mouse button to be associated with control.</param>
        /// <returns>Returns Control object.</returns>
        public static Control CreateMouseButton(MouseButton _button)
        {
            return new Control(_button);
        }

        /// <summary>
        /// Create an axis control.
        /// </summary>
        /// <param name="_button">Axis name associated with control.</param>
        /// <returns>Returns Control object.</returns>
        public static Control CreateAxis(string _axis)
        {
            return new Control(_axis, ControlType.Axis);
        }

        #region Common Controls

        /// <summary>
        /// Returns the A button for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control AButton(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button0);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button0);
                default:
                    return CreateKey(KeyCode.JoystickButton0);
            }
        }
        
        /// <summary>
        /// Returns the B button for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control BButton(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button1);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button1);
                default:
                    return CreateKey(KeyCode.JoystickButton1);
            }
        }

        /// <summary>
        /// Returns the X button for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control XButton(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button2);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button2);
                default:
                    return CreateKey(KeyCode.JoystickButton2);
            }
        }

        /// <summary>
        /// Returns the Y button for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control YButton(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button3);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button3);
                default:
                    return CreateKey(KeyCode.JoystickButton3);
            }
        }

        /// <summary>
        /// Returns the left bumper for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control LeftBumper(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button4);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button4);
                default:
                    return CreateKey(KeyCode.JoystickButton4);
            }
        }
        
        /// <summary>
        /// Returns the right bumper for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control RightBumper(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button5);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button5);
                default:
                    return CreateKey(KeyCode.JoystickButton5);
            }
        }

        /// <summary>
        /// Returns the back button for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control BackButton(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button6);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button6);
                default:
                    return CreateKey(KeyCode.JoystickButton6);
            }
        }
        
        /// <summary>
        /// Returns the start button for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control StartButton(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button7);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button7);
                default:
                    return CreateKey(KeyCode.JoystickButton7);
            }
        }

        /// <summary>
        /// Returns the left stick button for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control LeftStickButton(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button8);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button8);
                default:
                    return CreateKey(KeyCode.JoystickButton8);
            }
        }

        /// <summary>
        /// Returns the right stick button for specified player.
        /// </summary>
        /// <param name="_playerNum">Player specified.</param>
        /// <returns>Control object.</returns>
        public static Control RightStickButton(int _playerNum)
        {
            switch (_playerNum)
            {
                case 1:
                    return CreateKey(KeyCode.Joystick1Button8);
                case 2:
                    return CreateKey(KeyCode.Joystick2Button8);
                default:
                    return CreateKey(KeyCode.JoystickButton8);
            }
        }


        #endregion

        #endregion

        #region Data Members

        #region Fields.

        /// <summary>
        /// Type of input to be associated with the control name.
        /// </summary>
        private ControlType m_controlType;

        /// <summary>
        /// KeyCode associated with the button or keyboard key to request.
        /// </summary>
        private KeyCode m_key;

        /// <summary>
        /// The axis or button-axis name to request from Unity. 
        /// </summary>
        private string m_axis;

        /// <summary>
        /// Name of the button to use.
        /// </summary>
        private string m_button;

        /// <summary>
        /// Button to use for mouse input.
        /// </summary>
        private MouseButton m_mouse;

        #endregion

        #region Properties.

        /// <summary>
        /// Return the type of control input.
        /// </summary>
        public ControlType ControlType
        {
            get { return this.m_controlType; }
        }

        /// <summary>
        /// Returns the button or key code used for the input.
        /// </summary>
        public KeyCode Key
        {
            get { return this.m_key; }
        }

        /// <summary>
        /// Returns the axis name used for the input.
        /// </summary>
        public string Axis
        {
            get { return this.m_axis; }
        }

        /// <summary>
        /// Returns the name of the button used for the input.
        /// </summary>
        public string Button
        {
            get { return this.m_button; }
        }
        
        /// <summary>
        /// Returns the enum value for the mouse button being pressed.
        /// </summary>
        public MouseButton MouseButton
        {
            get { return this.m_mouse; }
        }

        #endregion

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a control struct with given inputs. By defualt, creates an axis.
        /// </summary>
        /// <param name="_input">Name of the input to request values from.</param>
        public Control(string _input, ControlType _control = ControlType.Axis)
        {
            // Validate values.
            string input = _input.Trim();

            // Set defaults.
            this.m_controlType = _control;
            this.m_key = KeyCode.None;
            this.m_button = "";
            this.m_axis = "";
            this.m_mouse = MouseButton.None;

            switch (this.ControlType)
            {
                case ControlType.Button:
                    this.m_button = input;
                    break;
                case ControlType.Axis:
                    this.m_axis = input;
                    break;
            }
        }

        /// <summary>
        /// Creates a control struct associated with a specific keycode.
        /// </summary>
        /// <param name="_code">KeyCode to associate control with.</param>
        public Control(KeyCode _code)
        {
            this.m_controlType = ControlType.Key;
            this.m_key = _code;
            this.m_button = "";
            this.m_axis = "";
            this.m_mouse = MouseButton.None;
        }

        /// <summary>
        /// Creates a control struct associated with a specific mouse button.
        /// </summary>
        /// <param name="_mouseButton">MouseButton to associate control with.</param>
        public Control(MouseButton _mouseButton)
        {
            this.m_controlType = ControlType.MouseButton;
            this.m_key = KeyCode.None;
            this.m_button = "";
            this.m_axis = "";
            this.m_mouse = _mouseButton;

            switch (this.MouseButton)
            {
                case MouseButton.LMB:
                    this.m_key = KeyCode.Mouse0;
                    break;
                case MouseButton.RMB:
                    this.m_key = KeyCode.Mouse1;
                    break;
                case MouseButton.MMB:
                    this.m_key = KeyCode.Mouse2;
                    break;
            }
        }

        #endregion

        #region Accessor Methods

        /// <summary>
        /// Check if this is a button, keycode, or mouse button.
        /// </summary>
        /// <returns>Returns true if it has appropriate ControlType.</returns>
        public bool IsButton()
        {
            return this.ControlType == ControlType.Button || this.ControlType == ControlType.MouseButton || this.ControlType == ControlType.Key;
        }

        /// <summary>
        /// Check if this is an axis.
        /// </summary>
        /// <returns>Returns true if it has appropriate ControlType.</returns>
        public bool IsAxis()
        {
            return this.ControlType == ControlType.Axis;
        }
        
        /// <summary>
        /// Check if it has a KeyCode.
        /// </summary>
        /// <returns>Returns true if it has a KeyCode.</returns>
        public bool HasKeyCode()
        {
            return IsButton() && this.Key != KeyCode.None;
        }

        /// <summary>
        /// Check if it has a Button.
        /// </summary>
        /// <returns>Returns true if it has a button name.</returns>
        public bool HasButton()
        {
            return IsButton() && this.Button.Length > 0;
        }

        /// <summary>
        /// Check if it has a MouseButton
        /// </summary>
        /// <returns>Returns true if it has a mouse button.</returns>
        public bool HasMouseButton()
        {
            return IsButton() && this.MouseButton == MouseButton.None;
        }

        /// <summary>
        /// Check if it has an axis name.
        /// </summary>
        /// <returns>Returns true if it has an Axis name.</returns>
        public bool HasAxis()
        {
            return IsAxis() && this.Axis.Length > 0;
        }

        /// <summary>
        /// Returns a KeyCode.
        /// </summary>
        /// <returns>Returns KeyCode.None if not a button.</returns>
        public KeyCode GetKeyCode()
        {
            if (HasKeyCode()) { return this.Key; }
            return KeyCode.None;
        }

        /// <summary>
        /// Returns the name of the button.
        /// </summary>
        /// <returns>Returns empty string if no button name exists.</returns>
        public string GetButtonName()
        {
            if (HasButton()) { return this.Button; }
            return "";
        }

        /// <summary>
        /// Returns the mouse button as Unity recognized values.
        /// </summary>
        /// <returns>Returns MouseButton.None if no mouse button is there.</returns>
        public int GetMouseButton()
        {
            if (HasMouseButton()) { return (int)this.MouseButton; }
            return (int)MouseButton.None;
        }

        /// <summary>
        /// Returns the mouse button as its keycode version, if applicable.
        /// </summary>
        /// <returns>Returns a KeyCode corresponding to the button inputs.</returns>
        public KeyCode GetMouseCode()
        {
            if (HasMouseButton()) { return this.Key; }
            return KeyCode.None;
        }

        /// <summary>
        /// Return the axis name.
        /// </summary>
        /// <returns>Returns an empty string if there is no axis name.</returns>
        public string GetAxisName()
        {
            if (HasAxis()) { return this.Axis; }
            return "";
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Returns axis value, with smoothing.
        /// </summary>
        /// <returns>Value of input axis.</returns>
        public float GetAxis()
        {
            if (IsAxis())
            {
                return Input.GetAxis(this.Axis);
            }

            return 0.0f;
        }

        /// <summary>
        /// Returns raw axis value.
        /// </summary>
        /// <returns>Value of input axis.</returns>
        public float GetAxisRaw()
        {
            if (IsAxis())
            {
                return Input.GetAxisRaw(this.Axis);
            }

            return 0.0f;
        }

        /// <summary>
        /// Returns true if control is being held.
        /// </summary>
        /// <returns>Returns true based on input.</returns>
        public bool IsHeld()
        {
            if (IsButton())
            {
                if (HasKeyCode())
                {
                    return this.IsKeyHeld();
                }

                if (HasButton())
                {
                    return this.IsButtonHeld();
                }

                if (HasMouseButton())
                {
                    return this.IsMouseButtonHeld();
                }
            }

            // Return false if it is an axis.
            return false;
        }

        /// <summary>
        /// Check if key is being held.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsKeyHeld()
        {
            if (HasKeyCode())
            {
                return Input.GetKey(this.Key);
            }

            return false;
        }

        /// <summary>
        /// Check if button is being held.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsButtonHeld()
        {
            if (HasButton())
            {
                return Input.GetButton(this.Button);
            }

            return false;
        }
        
        /// <summary>
        /// Check if the mouse button is being held.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsMouseButtonHeld()
        {
            if (HasMouseButton())
            {
                return Input.GetMouseButton(this.GetMouseButton());
            }

            return false;
        }

        /// <summary>
        /// Returns true if control was just pressed.
        /// </summary>
        /// <returns>Returns true based on input.</returns>
        public bool IsPressed()
        {
            if (IsButton())
            {
                if (HasKeyCode())
                {
                    return this.IsKeyPressed();
                }

                if (HasButton())
                {
                    return this.IsButtonPressed();
                }

                if (HasMouseButton())
                {
                    return this.IsMouseButtonPressed();
                }
            }

            // Return false if it is an axis.
            return false;
        }

        /// <summary>
        /// Check if key was just pressed.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsKeyPressed()
        {
            if (HasKeyCode())
            {
                return Input.GetKeyDown(this.Key);
            }

            return false;
        }

        /// <summary>
        /// Check if button was just pressed.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsButtonPressed()
        {
            if (HasButton())
            {
                return Input.GetButtonDown(this.Button);
            }

            return false;
        }

        /// <summary>
        /// Check if the mouse button was just pressed.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsMouseButtonPressed()
        {
            if (HasMouseButton())
            {
                return Input.GetMouseButtonDown(this.GetMouseButton());
            }

            return false;
        }

        /// <summary>
        /// Returns true if control was just released.
        /// </summary>
        /// <returns>Returns true based on input.</returns>
        public bool IsReleased()
        {
            if (IsButton())
            {
                if (HasKeyCode())
                {
                    return this.IsKeyReleased();
                }

                if (HasButton())
                {
                    return this.IsButtonReleased();
                }

                if (HasMouseButton())
                {
                    return this.IsMouseButtonReleased();
                }
            }

            // Return false if it is an axis.
            return false;
        }

        /// <summary>
        /// Check if key was just released.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsKeyReleased()
        {
            if (HasKeyCode())
            {
                return Input.GetKeyUp(this.Key);
            }

            return false;
        }

        /// <summary>
        /// Check if button was just released.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsButtonReleased()
        {
            if (HasButton())
            {
                return Input.GetButtonUp(this.Button);
            }

            return false;
        }

        /// <summary>
        /// Check if the mouse button was just released.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsMouseButtonReleased()
        {
            if (HasMouseButton())
            {
                return Input.GetMouseButtonUp(this.GetMouseButton());
            }

            return false;
        }

        /// <summary>
        /// Check if controls are the same.
        /// </summary>
        /// <param name="other">Other Control to compare.</param>
        /// <returns>Returns boolean.</returns>
        public bool Equals(Control other)
        {
            if (other.ControlType == this.ControlType)
            {
                switch (this.ControlType)
                {
                    case ControlType.Axis:
                        return this.Axis == other.Axis;
                    case ControlType.Key:
                        return this.Key == other.Key;
                    case ControlType.Button:
                        return this.Button == other.Button;
                    case ControlType.MouseButton:
                        return this.MouseButton == other.MouseButton;
                }
            }

            // Return false by default.
            return false;
        }

        /// <summary>
        /// Check if object is a control and is the same.
        /// </summary>
        /// <param name="obj">Other object to compare.</param>
        /// <returns>Returns boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Control))
            {
                return Equals((Control)obj);
            }

            return false;
        }

        /// <summary>
        /// Return the hash code.
        /// </summary>
        /// <returns>Returns integer as hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Operator Overload

        /// <summary>
        /// Checks controls for equality.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if equal.</returns>
        public static bool operator== (Control a, Control b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Checks controls for inequality.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if unequal.</returns>
        public static bool operator!= (Control a, Control b)
        {
            return !(a.Equals(b));
        }


        #endregion

    }

    #endregion

    #region Enum: ResponseMode

    /// <summary>
    /// The response mode determines how a trigger will be activated.
    /// </summary>
    public enum ResponseMode
    {
        /// <summary>
        /// Trigger activated when control is held.
        /// </summary>
        Held,

        /// <summary>
        /// Trigger activated when control is released.
        /// </summary>
        Released,

        /// <summary>
        /// Trigger activated when control is pressed.
        /// </summary>
        Pressed,

        /// <summary>
        /// Trigger activated when there is no button input.
        /// </summary>
        Up,

        /// <summary>
        /// Trigger returns axis value.
        /// </summary>
        Axis,

        /// <summary>
        /// Trigger returns raw axis value.
        /// </summary>
        AxisRaw,
        
        /// <summary>
        /// No trigger response.
        /// </summary>
        None
    }

    #endregion

    #region Enum: ControlType 

    /// <summary>
    /// Enum regarding input mapping methods.
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// A Key, Joystick, or Mouse button.
        /// </summary>
        Key,

        /// <summary>
        /// A button accessed through the GetButton method.
        /// </summary>
        Button,

        /// <summary>
        /// A Mouse or Joystick axis.
        /// </summary>
        Axis,

        /// <summary>
        /// Refers to the left or right mouse buttons.
        /// </summary>
        MouseButton
    }

    #endregion

    #region Enum: MouseButton

    /// <summary>
    /// Enum representing the mouse button to request.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// Left mouse button.
        /// </summary>
        LMB = 0,

        /// <summary>
        /// Right mouse button.
        /// </summary>
        RMB = 1,

        /// <summary>
        /// Middle mouse button.
        /// </summary>
        MMB = 2,

        /// <summary>
        /// Null mouse button option.
        /// </summary>
        None = -1
    }

    #endregion
    
    /*

    #region Class: ControlScheme class.

    /// <summary>
    /// The control scheme translates 'commands' to 'inputs' and sends back event responses.
    /// </summary>
    public class ControlScheme
    {
        #region Data Members

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Control scheme created for a single player.
        /// </summary>
        private Dictionary<Actions, List<CommandSequence>> m_controls = null;

        /// <summary>
        /// List of axes that should be tracked.
        /// </summary>
        private Dictionary<string, CommandResponse> m_axes = null;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Reference to the control scheme.
        /// </summary>
        public Dictionary<Actions, List<CommandSequence>> Controls
        {
            get { return this.m_controls; }
        }

        /// <summary>
        /// Returns axes.
        /// </summary>
        public Dictionary<string, CommandResponse> Axes
        {
            get { return this.m_axes; }
        }

        #endregion

        #endregion

        #region Constructor.

        /// <summary>
        /// Control Schemes are just repositories for control mappings.
        /// </summary>
        public ControlScheme()
        {
            this.m_controls = new Dictionary<Actions, List<CommandSequence>>();
            this.m_axes = new Dictionary<string, CommandResponse>();
        }

        #endregion
        
        #region Accessor Methods.

        /// <summary>
        /// Return the axis value.
        /// </summary>
        /// <param name="_name">Axis to search for.</param>
        /// <returns>Float between -1 and 1.</returns>
        public float GetAxis(string _name)
        {
            if (m_axes.ContainsKey(_name))
            {
                if (m_axes[_name].IsCommandActive())
                {
                    return m_axes[_name].GetAxis();
                }
            }

            return 0.0f;
        }

        /// <summary>
        /// Return the axis raw value.
        /// </summary>
        /// <param name="_name">Axis to search for.</param>
        /// <returns>Float between -1 and 1.</returns>
        public float GetAxisRaw(string _name)
        {
            if (m_axes.ContainsKey(_name))
            {
                if (m_axes[_name].IsCommandActive())
                {
                    return m_axes[_name].GetAxisRaw();
                }
            }

            return 0.0f;
        }

        /// <summary>
        /// Returns true if action has been triggered.
        /// </summary>
        /// <param name="_action">Action to check for.</param>
        /// <returns>Returns true if action has been triggered; false if action hasn't or hasn't been added to the collection.</returns>
        public bool TriggerAction(Actions _action)
        {
            List<CommandSequence> validInputs = GetActionTriggers(_action);

            if (validInputs == null)
            {
                Debugger.Print("Action doesn't have any command sequences set up.");
            }
            else
            {
                foreach (CommandSequence seq in validInputs)
                {
                    if (TriggerAction(_action, seq)) { return true; }
                }
            }

            return false;
        }

        /// <summary>
        /// If true, the action should be triggered.
        /// </summary>
        /// <param name="_action">Action to be performed.</param>
        /// <param name="_input">Input sequence.</param>
        /// <returns></returns>
        private bool TriggerAction(Actions _action, CommandSequence _input)
        {
            if (this.m_controls.ContainsKey(_action))
            {
                // An action exists. Check to see if an input sequence triggers it.
                List<CommandSequence> triggers = this.m_controls[_action];

                foreach (CommandSequence seq in triggers)
                {
                    // If any one matches:
                    if (seq.Matches(_input) && _input.IsSequenceActive(_input))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the action's trigger command.
        /// </summary>
        /// <param name="_action">Action to be performed.</param>
        /// <returns>Returns the input sequence required.</returns>
        private List<CommandSequence> GetActionTriggers(Actions _action)
        {
            if (this.m_controls.ContainsKey(_action))
            {
                return this.m_controls[_action];
            }

            return null;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Add an action with its mapped command.
        /// </summary>
        /// <param name="_action">Action to perform.</param>
        /// <param name="_input">Mapped input.</param>
        public void AddControl(Actions _action, CommandSequence _input)
        {
            if (!this.m_controls.ContainsKey(_action))
            {
                // Add a new scheme, if it doesn't already exist.
                this.m_controls.Add(_action, new List<CommandSequence>());
            }

            // Add the sequence to the list.
            this.m_controls[_action].Add(_input);
        }

        /// <summary>
        /// Add an axis to track.
        /// </summary>
        /// <param name="_axis">Track this axis</param>
        /// <param name="_axisTrigger">Trigger axis values on this movement trigger.</param>
        public void AddAxis(string _axis, CommandResponse _axisTrigger)
        {
            if (!this.m_axes.ContainsKey(_axis))
            {
                // Add a new scheme, if it doesn't already exist.
                this.m_axes.Add(_axis, _axisTrigger);
            }
            else
            {
                // Overwrite value.
                this.m_axes[_axis] = _axisTrigger;
            }
        }

        #endregion

    }

    #endregion

    #region Enum: Actions.

    /// <summary>
    /// Enum keeps track of possible in-game commands.
    /// </summary>
    public enum Actions
    {
        ChangeCameraBackground,
        Click,
        Idle,
        Fire,
        Jump,
        Crouch,
        MoveLeft,
        MoveRight,
        Left,
        Right,
        Up,
        Down,
        Select,
        Back,
        Escape,
        Pause,
        Resume,
        AddFire,
        AddWater,
        AddEarth
    }

    #endregion
        
    #region Class: Command class.

    /// <summary>
    /// Represents the type of commands that might exists.
    /// </summary>
    public enum CommandTypes
    {
        // Unassigned.
        Unassigned,

        // Mouse controls.
        MouseMovement,
        MouseButton,
        MouseWheel,

        // Keyboard controls.
        Key,
        
        // Gamepad.
        Button,
        Axis
    }

    /// <summary>
    /// A command class represents input that Unity can handle.
    /// </summary>
    public class Command {
        
        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Type of command. (Determines how it will be processed).
        /// </summary>
        private CommandTypes m_type = CommandTypes.Unassigned;

        /// <summary>
        /// The keycode associated with keyboard and gamepad button presses.
        /// </summary>
        private KeyCode m_keyCode = KeyCode.Numlock; // Whose really going to use numlock as a command?

        /// <summary>
        /// Mouse button number associated with mouse button presses.
        /// </summary>
        private int m_mouseButton = -1;

        /// <summary>
        /// Name of the command for joystick and mouse axes.
        /// </summary>
        private string m_name = "";

        /// <summary>
        /// The timeout associated with a command. Timeouts prevent players from spamming commands.
        /// </summary>
        private float m_timeout = 0.5f;

        /// <summary>
        /// Deadzone to compare axis input to.
        /// </summary>
        private float m_deadzone = 0.0f;
        
        /////////////////////
        // Properties.
        /////////////////////
        
        /// <summary>
        /// Reference to the button's handling type.
        /// </summary>
        public CommandTypes CommandType
        {
            get { return this.m_type; }
        }

        /// <summary>
        /// The command doesn't have a ruleset attached to it.
        /// </summary>
        public bool Unassigned
        {
            get { return CommandType == CommandTypes.Unassigned; }
        }

        /// <summary>
        /// Returns true if not an axis.
        /// </summary>
        public bool IsButton
        {
            get { return !IsAxis && !Unassigned; }
        }

        /// <summary>
        /// Returns true if maps to an axis control.
        /// </summary>
        public bool IsAxis
        {
            get
            {
                return CommandType == CommandTypes.Axis || CommandType == CommandTypes.MouseMovement || CommandType == CommandTypes.MouseWheel;
            }
        }

        #endregion

        #region Constructors.

        /// <summary>
        /// Construct an unassigned command.
        /// </summary>
        public Command()
        {
            Reset();
        }

        /// <summary>
        /// Assign a type of button or key to the command. Assumes keyboard by default.
        /// </summary>
        /// <param name="_input">KeyCode to match.</param>
        /// <param name="_type">Type of press.</param>
        /// <param name="_timeout">Timeout of command.</param>
        public Command(KeyCode _input, CommandTypes _type = CommandTypes.Key, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            if (_type == CommandTypes.Button)
            {
                Reset(_type: _type, _code: _input, _timeout: _timeout);
            }
            else
            {
                Reset(_type: CommandTypes.Key, _code: _input, _timeout: _timeout);
            }
        }

        /// <summary>
        /// Assign a mouse button press.
        /// </summary>
        /// <param name="_mouse">Mouse button index.</param>
        /// <param name="_timeout">Timeout of press.</param>
        public Command(int _mouse, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            Reset(_type: CommandTypes.MouseButton, _mouse: _mouse, _timeout: _timeout);
        }
                
        /// <summary>
        /// Reset commands and make unassigned.
        /// </summary>
        private void Reset(
            CommandTypes _type = CommandTypes.Unassigned,
            KeyCode _code = KeyCode.Numlock,
            int _mouse = -1,
            string _name = "",
            float _timeout = Constants.DEFAULT_INPUT_TIMEOUT,
            float _deadzone = 0.0f)
        {
            this.m_type = _type;
            this.m_keyCode = _code;
            this.m_mouseButton = Services.Clamp(_mouse, -1, 2);
            this.m_name = _name;
            this.m_timeout = Services.Abs(_timeout);
        }

        /// <summary>
        /// Assign a joystick axis with a deadzone. (Or, optionally, assign a mouse axis).
        /// </summary>
        /// <param name="_axisName">Name of the axis to return.</param>
        /// <param name="_timeout">Timeout of command.</param>
        public Command(string _axisName, CommandTypes _type = CommandTypes.Axis, float _deadzone = 0.2f, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            if (_type == CommandTypes.MouseMovement)
            {
                Reset(_type: _type, _name: _axisName, _timeout: _timeout);
            }
            else
            {
                Reset(_type: CommandTypes.Axis, _name: _axisName, _timeout: _timeout,  _deadzone: _deadzone);
            }
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Determine if two commands are equal.
        /// </summary>
        /// <param name="other">Command to compare.</param>
        /// <returns>Returns true if they have the same type, code, and timeout.</returns>
        public bool Equals(Command other)
        {
            if (!this.Unassigned && !other.Unassigned) {
                if (this.m_timeout == other.m_timeout)
                {
                    if(this.CommandType == other.CommandType)
                    {
                        if (this.IsAxis)
                        {
                            return this.m_name == other.m_name;
                        }

                        if (this.CommandType == CommandTypes.Key || this.CommandType == CommandTypes.Button)
                        {
                            return this.m_keyCode == other.m_keyCode;
                        }

                        if (this.CommandType == CommandTypes.MouseButton)
                        {
                            return this.m_mouseButton == other.m_mouseButton;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Assign a keycode to the command.
        /// </summary>
        /// <param name="_input">Keyboard/Joystick Button associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignKey(KeyCode _input, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            Reset(_type: CommandTypes.Key, _code: _input, _timeout: _timeout);
        }

        /// <summary>
        /// Assign a control name to the command.
        /// </summary>
        /// <param name="_input">Axis name associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignAxis(string _input, float _timeout = 0.5f)
        {
            Reset(_type: CommandTypes.Axis, _name: _input, _timeout: _timeout);
        }

        /// <summary>
        /// Assign a keycode to the command.
        /// </summary>
        /// <param name="_input">Keyboard/Joystick Button associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignJoystickButton(KeyCode _input, float _timeout = Constants.DEFAULT_INPUT_TIMEOUT)
        {
            Reset(_type: CommandTypes.Button, _code: _input, _timeout: _timeout);
        }

        /// <summary>
        /// Assign a control name to the command.
        /// </summary>
        /// <param name="_input">Mouse button index associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignMouseButton(int _input, float _timeout = 0.5f)
        {
            Reset(_type: CommandTypes.MouseButton, _mouse: _input, _timeout: _timeout);
        }

        /// <summary>
        /// Assign a control name to the command.
        /// </summary>
        /// <param name="_input">Mouse button index associated with the press.</param>
        /// <param name="_timeout">Optional timeout that determines how long a button will stay 'valid'.</param>
        public void AssignMouseMovement(float _timeout = 0.5f)
        {
            Reset(_type: CommandTypes.MouseMovement, _timeout: _timeout);
        }

        #endregion

        #region Accessor Methods.

        #region Keyboard Key / Mouse Button / Gamepad Button Input.

        /// <summary>
        /// Checks if a KeyCode input is being held down.
        /// </summary>
        /// <returns>Returns a response.</returns>
        public bool IsKeyHeld()
        {
            if (CommandType == CommandTypes.Key || CommandType == CommandTypes.Button)
            {
                if(Input.GetKey(this.m_keyCode))
                {
                    Debugger.Print("Key/Gamepad Button " + m_keyCode + " is being held.");
                    return true;
                }               
            }

            if (CommandType == CommandTypes.MouseButton)
            {
                if (Input.GetMouseButton(this.m_mouseButton))
                {
                    Debugger.Print("Mouse Button of index " + m_mouseButton + " is being held.");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a KeyCode input was just released.
        /// </summary>
        /// <returns>Returns a response.</returns>
        public bool IsKeyReleased()
        {
            if (CommandType == CommandTypes.Key || CommandType == CommandTypes.Button)
            {
                if (Input.GetKeyUp(this.m_keyCode))
                {
                    Debugger.Print("Key/Gamepad Button " + m_keyCode + " was just released.");
                    return true;
                }
            }

            if (CommandType == CommandTypes.MouseButton)
            {
                if (Input.GetMouseButtonUp(this.m_mouseButton))
                {
                    Debugger.Print("Mouse Button of index " + m_mouseButton + " was just released.");
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Checks if a KeyCode input was just pressed.
        /// </summary>
        /// <returns>Returns a response.</returns>
        public bool IsKeyPressed()
        {
            if (CommandType == CommandTypes.Key || CommandType == CommandTypes.Button)
            {
                if (Input.GetKeyDown(this.m_keyCode))
                {
                    Debugger.Print("Key/Gamepad Button " + m_keyCode + " was just pressed.");
                    return true;
                }
            }

            if (CommandType == CommandTypes.MouseButton)
            {
                if (Input.GetMouseButtonDown(this.m_mouseButton))
                {
                    Debugger.Print("Mouse Button of index " + m_mouseButton + " was just pressed.");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a KeyCode input is not being pressed down.
        /// </summary>
        /// <returns>Returns a response.</returns>
        public bool IsKeyUp()
        {
            if (CommandType == CommandTypes.Key || CommandType == CommandTypes.Button || CommandType == CommandTypes.MouseButton)
            {
                return !IsKeyHeld();
            }

            return true;
        }

        #endregion

        #region Mouse Axis / Joystick Axis Input.

        /// <summary>
        /// Returns the raw value from the appropriate axis input function.
        /// </summary>
        /// <returns>Returns the raw float value from the axis.</returns>
        public float GetAxisRaw()
        {
            if (this.IsAxis)
            {
                float response = Input.GetAxisRaw(this.m_name);
                Debugger.Print("Requested raw axis value for " + m_name + ": " + response);
                return response;
            }
            else
            {
                return 0.0f;
            }
        }
        
        /// <summary>
        /// Returns the axis value from the appropriate axis input function.
        /// </summary>
        /// <returns>Returns the raw float value from the axis.</returns>
        public float GetAxis()
        {
            if (this.IsAxis)
            {
                float response = Input.GetAxis(this.m_name);
                Debugger.Print("Requested axis value for " + m_name + ": " + response);
                return response;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Check if the axis is stationary.
        /// </summary>
        /// <returns>Returns a true or false based on the input axis value and deadzone.</returns>
        public bool IsAxisStationary()
        {
            if (this.IsAxis)
            {
                return !IsAxisMoved();
            }

            return true;            
        }

        /// <summary>
        /// Check if the axis is not stationary.
        /// </summary>
        /// <returns>Returns a true or false based on the input axis value and deadzone.</returns>
        public bool IsAxisMoved()
        {
            if (this.IsAxis)
            {
                if (Services.Abs(GetAxis()) > this.m_deadzone)
                {
                    Debugger.Print("Axis " + m_name + " has moved beyond the " + m_deadzone + " deadzone (" + GetAxis() + ").");
                    return true;
                }
            }

            return false;            
        }

        #endregion

        #endregion

    }

    #endregion

    #region Class: CommandResponse class.
    
    /// <summary>
    /// Determines what fires a command.
    /// </summary>
    public enum CommandResponseMode
    {
        Held,
        Press,
        Release,
        NonZero, // For axes
        Unassigned
    }

    /// <summary>
    /// Stores a response for a given command.
    /// </summary>
    public class CommandResponse
    {

        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Command to check.
        /// </summary>
        private Command m_command = null;

        /// <summary>
        /// Response mode to check.
        /// </summary>
        private CommandResponseMode m_response = CommandResponseMode.Unassigned;

        /////////////////////
        // Properties
        /////////////////////
        
        /// <summary>
        /// Return command response.
        /// </summary>
        public CommandResponseMode Response
        {
            get { return this.m_response; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool OnHold
        {
            get { return Response == CommandResponseMode.Held; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool OnAxisChange
        {
            get { return Response == CommandResponseMode.NonZero; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool OnPress
        {
            get { return Response == CommandResponseMode.Press; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool OnRelease
        {
            get { return Response == CommandResponseMode.Release; }
        }

        /// <summary>
        /// Returns true if response merits it.
        /// </summary>
        public bool Unassigned
        {
            get { return Response == CommandResponseMode.Unassigned; }
        }

        /// <summary>
        /// The command is a button.
        /// </summary>
        public bool IsButton
        {
            get { return this.m_command.IsButton; }
        }

        /// <summary>
        /// The command is an axis.
        /// </summary>
        public bool IsAxis {
            get { return this.m_command.IsAxis; }
        }

        #endregion

        #region Constructor.

        /// <summary>
        /// Response.
        /// </summary>
        /// <param name="_command">Command to get response.</param>
        /// <param name="_crm"></param>
        public CommandResponse(Command _command, CommandResponseMode _crm)
        {
            this.m_command = _command;
            this.m_response = _crm;
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Returns true if the response mode and command has been activated.
        /// </summary>
        /// <returns>Returns response status.</returns>
        public bool IsCommandActive()
        {
            if (IsButton)
            {
                if (OnHold)
                {
                    return m_command.IsKeyHeld();
                }

                if (OnPress)
                {
                    return m_command.IsKeyPressed();
                }

                if (OnRelease)
                {
                    return m_command.IsKeyReleased();
                }
            }

            if (IsAxis)
            {
                if (OnAxisChange)
                {
                    return m_command.IsAxisMoved();
                }
            }

            return false;
        }

        /// <summary>
        /// Get the axis if the command is active.
        /// </summary>
        /// <returns>Returns value.</returns>
        public float GetAxis()
        {
            if (IsCommandActive() && IsAxis)
            {
                return m_command.GetAxis();
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Get the axis raw if the command is active.
        /// </summary>
        /// <returns>Returns value.</returns>
        public float GetAxisRaw()
        {
            if (IsCommandActive() && IsAxis)
            {
                return m_command.GetAxisRaw();
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Checks response for equality.
        /// </summary>
        /// <param name="other">Response to compare.</param>
        /// <returns>Returns a boolean flag representing equality of statements.</returns>
        public bool Equals(CommandResponse other)
        {
            return ((this.m_response == other.m_response) && (this.m_command.Equals(other.m_command)));
        }

        #endregion
        
    }

    #endregion

    #region Class: CommandSequence class.

    /// <summary>
    /// A command is a sequence of one (or multiple) button presses that will be stored in order to check.
    /// </summary>
    public class CommandSequence
    {

        #region Data Members

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// A sequence of button presses.
        /// </summary>
        private List<CommandResponse> m_sequence;

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Reference to sequence.
        /// </summary>
        public List<CommandResponse> Sequence
        {
            get { return this.m_sequence; }
        }

        #endregion

        #region Constructors.

        /// <summary>
        /// Create storage container for a sequence.
        /// </summary>
        public CommandSequence()
        {
            m_sequence = new List<CommandResponse>();
        }

        /// <summary>
        /// Create a sequence, with default timeouts of 0.5 seconds.
        /// </summary>
        /// <param name="_commands">Sequence of commands.</param>
        public CommandSequence(List<CommandResponse> _commands)
        {
            if (_commands.Count > 0)
            {
                // Send keys to array.
                m_sequence = _commands;
            }
            else
            {
                m_sequence = new List<CommandResponse>();
            }
        }

        #endregion

        #region Service Methods.
        
        /// <summary>
        /// Add a command to the end of the sequence.
        /// </summary>
        /// <param name="_command">Command to add.</param>
        public void Push(CommandResponse _command)
        {
            this.Sequence.Add(_command);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_input"></param>
        /// <returns></returns>
        public bool Matches(CommandSequence _input)
        {
            return Matches(_input.m_sequence);
        }

        /// <summary>
        /// Checks if an input list of hardware commands matches the sequence.
        /// </summary>
        /// <param name="_input">Sequence that should match.</param>
        /// <returns>Returns true if the input sequence AND timeouts match.</returns>
        public bool Matches(List<CommandResponse> _input)
        {
            // Check lengths. If they don't match, return false.
            if(m_sequence.Count > _input.Count) { return false; }
            
            // Loop through for match checking.
            for (int i = 0; i < m_sequence.Count; i++)
            {
                // Check command keys.
                if (!this.m_sequence[i].Equals(_input[i]))
                {
                    return false;
                }
            }

            // Return true if input matched.
            return true;
        }

        /// <summary>
        /// Check if the input sequence has all of its commands as active.
        /// </summary>
        /// <param name="_input">Input list of responses.</param>
        /// <returns>Returns true if the input sequence commands are all active.</returns>
        public bool IsSequenceActive(List<CommandResponse> _input)
        {
            bool active = true;

            foreach (CommandResponse cr in _input)
            {
                if (!cr.IsCommandActive())
                {
                    active = false;
                    break;
                }                
            }

            return active;
        }


        /// <summary>
        /// Check if the input sequence has all of its commands as active.
        /// </summary>
        /// <param name="_input">Input list of responses.</param>
        /// <returns>Returns true if the input sequence commands are all active.</returns>
        public bool IsSequenceActive(CommandSequence _sequence)
        {
            bool active = true;

            foreach (CommandResponse cr in _sequence.Sequence)
            {
                if (!cr.IsCommandActive())
                {
                    active = false;
                    break;
                }
            }

            return active;
        }

        #endregion

    }

    #endregion

    */

}
