/************************************************
 * ArcanaObject.cs
 * 
 * This file contains:
 * - The ArcanaObject base class. (Inherits MonoBehaviour)
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Utilities;
using Arcana.InputManagement;

namespace Arcana
{
    
    #region Class: ArcanaObject class.

    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// All Arcana objects should inherit this behaviour.
    /// </summary>
    [AddComponentMenu("Arcana/ArcanaObject")]
    public class ArcanaObject : MonoBehaviour
    {

        #region Data Members.

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////
                
        /// <summary>
        /// Debug mode for this component.
        /// </summary>
        private bool m_debug = true;
        
        /// <summary>
        /// Duplication permissions flag.
        /// </summary>
        private bool m_cloneable = false;

        /// <summary>
        /// Object pooling permissions flag.
        /// </summary>
        private bool m_poolable = false;

        /// <summary>
        /// Status component located on the GameObject.
        /// </summary>
        private Status m_status = null;

        /// <summary>
        /// Children of this object.
        /// </summary>
        private List<ArcanaObject> m_children = null;

        /// <summary>
        /// Input director associated with this object.
        /// </summary>
        private Director m_inputDirector = Director.None;

        /// <summary>
        /// Reference to the system's control scheme.
        /// </summary>
        protected ControlScheme m_scheme = null;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns initialization flag for this ArcanaObject.
        /// </summary>
        public bool Initialized
        {
            get { return (this.m_status != null && this.m_status.Initialized); }
        }

        /// <summary>
        /// Returns the children of the ArcanaObject.
        /// </summary>
        public bool HasChildren
        {
            get { return (this.m_children != null && this.m_children.Count > 0); }
        }

        /// <summary>
        /// Debug mode value.
        /// </summary>
        public bool Debug
        {
            get { return this.m_debug; }
            set { this.m_debug = value; }
        }

        /// <summary>
        /// Returns this object's <see cref="GameObject"/> reference.
        /// </summary>
        public GameObject Self
        {
            get { return this.gameObject; }
        }

        /// <summary>
        /// Return the collection of children, if any.
        /// </summary>
        public List<ArcanaObject> Children
        {
            get {
                if(this.m_children == null)
                {
                    this.m_children = new List<ArcanaObject>();
                }

                return this.m_children;
            }
        }

        /// <summary>
        /// Returns the number of children in the collection of children.
        /// </summary>
        public int NumberOfChildren
        {
            get {
                if (HasChildren)
                {
                    return this.m_children.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Name of the component's GameObject.
        /// </summary>
        public string Name
        {
            get { return this.gameObject.name; }
            set { if (value.Length > 0) { this.gameObject.name = value; } }
        }
        
        /// <summary>
        /// Returns true if this component's <see cref="GameObject"/> reference is null.
        /// </summary>
        public virtual bool IsNull
        {
            get {
                return (this.gameObject == null);
            }
        }

        /// <summary>
        /// Returns true if the item can be cloned.
        /// </summary>
        public bool IsCopyable
        {
            get { return (this.m_cloneable); }
        }

        /// <summary>
        /// Returns true if the item can be pooled.
        /// </summary>
        public virtual bool IsPoolable
        {
            get { return (this.m_poolable); }
            set { this.m_poolable = value; }
        }

        /// <summary>
        /// Returns the Status component.
        /// </summary>
        public Status Status
        {
            get { return (this.m_status); }
        }

        /// <summary>
        /// Returns the director value.
        /// </summary>
        public Director Director
        {
            get { return (this.m_inputDirector); }
            set { this.m_inputDirector = value; }
        }

        /// <summary>
        /// Returns reference to the control scheme.
        /// </summary>
        public virtual ControlScheme Controls
        {
            get
            {
                if (this.m_scheme == null)
                {
                    this.m_scheme = InitializeControls();
                }
                return this.m_scheme;
            }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.
                        
        /// <summary>
        /// Initialize the <see cref="ArcanaObject"/>.
        /// </summary>
        public virtual void Start()
        {
            this.Initialize();
        }

        /// <summary>
        /// <see cref="MonoBehaviour"/> function run every update.
        /// </summary>
        public virtual void Update()
        {
            if (!this.Initialized)
            {
                // Initialize, if it hasn't been initialized yet.
                this.Initialize();
            }
            else
            {
                // Handle user input.
                // HandleInput();

                // If this object is set to be destroyed, destroy it.
                if (this.Status.IsDestroy())
                {
                    DestroySelf();
                    return;
                }
            }            
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the component's Status, name, and children collection.
        /// </summary>
        public virtual void Initialize()
        {
            if (!Initialized)
            {
                // Set members.
                this.Debug = false; // Set the debug mode.
                this.m_status = ComponentFactory.Create<Status>(this); // Create the Status component.
                this.m_children = new List<ArcanaObject>(); // Create the children collection.

                if (this.m_status == null)
                {
                    Debugger.Print("This is null.", this.Self.name, true);
                    this.m_status = ComponentFactory.Create<Status>(this); // Create the Status component.
                }

                // Run initialization functions.                
                this.m_status.Initialize(); // Set component as initialized.
                this.m_status.Initialize(true); // Set this component as initialized.
            }
        }

        /// <summary>
        /// Copy data from the input component into this one.
        /// </summary>
        /// <param name="_template">Template to pull data from.</param>
        public virtual ArcanaObject Clone(ArcanaObject _template)
        {
            if (_template.IsCopyable)
            {
                // Ensure they are different.
                if (this != _template)
                {
                    this.Name = _template.Name + "(Clone)";
                    this.m_status.Clone(_template.Status);
                    this.m_status.Initialize(_template.Initialized);
                }
            }

            return this;
        }

        /// <summary>
        /// Initialize the object's input controls.
        /// </summary>
        protected virtual ControlScheme InitializeControls()
        {
            if (this.m_scheme == null)
            {
                this.m_scheme = this.Self.GetComponent<ControlScheme>();
            }

            if (this.m_scheme == null)
            {
                BuildControlScheme();
            }

            return this.m_scheme;
        }

        #endregion

        #region Input Methods.

        /// <summary>
        /// Create a control scheme. For system level actions, by default.
        /// </summary>
        protected virtual void BuildControlScheme()
        {
            if (this.m_scheme == null)
            {
                this.m_scheme = InputManager.GetInstance().AddControlScheme(this);
                this.m_scheme.Initialize();
            }
        }

        /// <summary>
        /// Handle input.
        /// </summary>
        protected virtual void HandleInput()
        {
            // Do nothing in the base class.
        }

        /// <summary>
        /// Return the action from the control scheme.
        /// </summary>
        /// <param name="_id">ID of the action to request.</param>
        /// <returns>Returns an action.</returns>
        protected Action GetAction(string _id)
        {
            return Action.GetAction(_id, this.Director);
        }

        /// <summary>
        /// Link an action to perform with a command.
        /// </summary>
        /// <param name="_action">Action to perform.</param>
        /// <param name="_trigger">Trigger that will cause action.</param>
        /// <param name="_control">Binding that will cause action.</param>
        /// <param name="_response">Response type that will trigger action.</param>
        public void RegisterControl(Action _action, Trigger _trigger)
        {
            if (this.Controls != null)
            {
                this.Controls.AddMap(_action, _trigger);
            }
        }

        /// <summary>
        /// Link an action to perform with a command.
        /// </summary>
        /// <param name="_action">Action to perform.</param>
        /// <param name="_control">Binding that will cause action.</param>
        /// <param name="_response">Response type that will trigger action.</param>
        public void RegisterControl(Action _action, Control _control, ResponseMode _mode = ResponseMode.None)
        {

            // Set up reference.
            Trigger trigger;

            if (_mode == ResponseMode.None)
            {
                trigger = ControlScheme.CreateTrigger(_control);
            }
            else
            {
                trigger = ControlScheme.CreateTrigger(_control, _mode);
            }

            RegisterControl(_action, trigger);
        }

        /// <summary>
        /// Link an action to perform with a command.
        /// </summary>
        /// <param name="_action">Action to perform.</param>
        /// <param name="_control">Binding that will cause action.</param>
        /// <param name="_response">Response type that will trigger action.</param>
        public void RegisterControl(string _actionID, Control _control, ResponseMode _mode = ResponseMode.None)
        {
            Action action = ControlScheme.CreateAction(_actionID, this.Director);
            RegisterControl(action, _control, _mode);
        }

        #endregion

        #region Status Methods.

        /// <summary>
        /// Checks if the object is available to be repurposed.
        /// </summary>
        /// <returns>Returns true if inactive.</returns>
        public virtual bool IsAvailable()
        {
            return (this.Status.IsInactive() && this.m_poolable);
        }

        /// <summary>
        /// Make the object available for object pooling.
        /// </summary>
        public virtual void MakeAvailable()
        {
            this.Initialize();
            this.Status.Activate();
        }

        /// <summary>
        /// Pause this component and all its children.
        /// </summary>
        public virtual void Pause()
        {
            if (!this.Status.IsPaused())
            {
                // Pause all children.
                this.PauseChildren();

                // Pause this object.
                this.Status.Pause();
            }
        }

        /// <summary>
        /// Pause all child components.
        /// </summary>
        protected virtual void PauseChildren()
        {
            // Pause all children.
            if (HasChildren)
            {
                // Pause each child.
                foreach (ArcanaObject child in this.m_children)
                {
                    // Pause that child and its children.
                    child.Pause();
                }
            }
        }

        /// <summary>
        /// Resume this component and all its children.
        /// </summary>
        public virtual void Resume()
        {
            if (this.Status.IsPaused())
            {
                // Resume all children.
                this.ResumeChildren();

                // Resume this object.
                this.Status.Resume();
            }
        }

        /// <summary>
        /// Resume all child components.
        /// </summary>
        protected virtual void ResumeChildren()
        {
            // Resume all children.
            if (HasChildren)
            {
                // Resume each child.
                foreach (ArcanaObject child in this.m_children)
                {
                    // Resume that child and its children.
                    child.Resume();
                }
            }
        }

        /// <summary>
        /// Activate the object.
        /// </summary>
        public virtual void Activate()
        {
            if (!this.Status.IsActive())
            {
                this.Status.Activate();
            }
        }

        /// <summary>
        /// Deactivate the object.
        /// </summary>
        public virtual void Deactivate()
        {
            if (!this.Status.IsInactive())
            {
                this.Status.Deactivate();
            }
        }

        /// <summary>
        /// Start the object.
        /// </summary>
        public virtual void Run()
        {
            if (!this.Status.IsRunning())
            {
                this.Status.Run();
            }
        }

        /// <summary>
        /// Stop the object.
        /// </summary>
        public virtual void Stop()
        {
            if (this.Status.IsRunning())
            {
                this.Status.Stop();
            }
        }

        /// <summary>
        /// Set respective Status state.
        /// </summary>
        public virtual void Kill()
        {
            if (!this.Status.IsDead())
            {
                this.Status.Kill();
            }
        }
        
        /// <summary>
        /// Set respective Status state.
        /// </summary>
        public virtual void Revive()
        {
            if (!this.Status.IsAlive())
            {
                this.Status.Revive();
            }
        }

        /// <summary>
        /// Set respective Status state.
        /// </summary>
        public virtual void Hide()
        {
            this.Status.Hide();
        }

        /// <summary>
        /// Set respective Status state.
        /// </summary>
        public virtual void Show()
        {
            this.Status.Show();
        }

        /// <summary>
        /// Set respective Status state.
        /// </summary>
        public virtual void HideGUI()
        {
            this.Status.HideGUI();
        }

        /// <summary>
        /// Set respective Status state.
        /// </summary>
        public virtual void ShowGUI()
        {
            this.Status.ShowGUI();
        }
        
        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns a list of child components that match the input type.
        /// </summary>
        /// <typeparam name="T">Type of ArcanaObject to return.</typeparam>
        /// <returns>Returns list of components of type T.</returns>
        public List<T> GetChildren<T>() where T : ArcanaObject
        {
            List<T> response = new List<T>();

            // If the object has children.
            if (HasChildren)
            {
                Debugger.Print("Searching for ArcanaObject and subclassess...");
                foreach (ArcanaObject child in this.m_children)
                {
                    if (Services.IsSameOrSubclassOf(child.GetType(), typeof(T)))
                    {
                        Debugger.Print("Found child component of type " + typeof(T).Name);
                        response.Add((T)child);
                    }
                }
            }

            return response;
        }

        /// <summary>
        /// Return a list of all children in the object.
        /// </summary>
        /// <returns>Returns collection of children.</returns>
        public List<ArcanaObject> GetChildren()
        {
            return this.m_children;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Marks all children for removal.
        /// </summary>
        public virtual void ClearChildren()
        {
            if (HasChildren)
            {
                foreach (ArcanaObject child in this.m_children)
                {
                    child.Status.Destroy();
                }
            }
        }

        /// <summary>
        /// Remove child from collection.
        /// </summary>
        /// <param name="_child">Child to remove.</param>
        /// <returns>Returns the child object.</returns>
        public virtual ArcanaObject RemoveChild(ArcanaObject _child)
        {
            // Check if parameter is null.
            if (_child == null || _child.IsNull)
            {
                Debugger.Print("Cannot remove null child component from " + this.Name + "'s children.");
                return null;
            }

            if (HasChildren) { 
                // If the element is already contained, remove it.
                if (this.Children.Contains(_child))
                {
                    // Return the child object.
                    Debugger.Print("Removing component from " + this.Name + ".");
                    this.Children.Remove(_child);
                    return _child;
                }
            }

            // If the element isn't currently in the children collection.
            Debugger.Print("Component is not a child member of " + this.Name + ".");

            // Return child.
            return _child;
        }

        /// <summary>
        /// Add a child to the collection of children.
        /// </summary>
        /// <param name="_child">Add child.</param>
        /// <returns>Returns the chidl.</returns>
        public virtual ArcanaObject AddChild(ArcanaObject _child)
        {
            // Check if parameter is null.
            if (_child == null || _child.IsNull)
            {
                Debugger.Print("Cannot add null child component to " + this.Name + "'s children.");
                return null;
            }

            // Check if it has children.
            if (HasChildren)
            {
                // If the element is already contained, don't add it.
                if (this.Children.Contains(_child))
                {
                    // Return the child object.
                    Debugger.Print("Component is already a child of " + this.Name + ".");
                    return _child;
                }
            }

            // If the element isn't currently in the children collection.
            Debugger.Print("Adding component as child of " + this.Name + ".");
            this.Children.Add(_child);

            // Return child.
            return _child;        
        }

        /// <summary>
        /// Set the cloneable flag.
        /// </summary>
        /// <param name="_flag">Value to set flag to.</param>
        protected virtual void SetCopyable(bool _flag)
        {
            this.m_cloneable = _flag;
        }

        /// <summary>
        /// Destroy yourself. (And perform any operations before OnDestroy will be called.
        /// </summary>
        public virtual void DestroySelf()
        {
            ClearChildren();
            Destroy(this.Self);
        }

        #endregion

    }

    #endregion
    
}
