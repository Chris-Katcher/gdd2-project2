/************************************************
 * Status.cs
 * 
 * This file contains:
 * - The Status class. (Inherits MonoBehaviour)
 * - The ComponentStatus enum.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Utilities
{

    #region Class: Status class.

    /// <summary>
    /// The Status object is a component that stores the current state of the element it's attached.
    /// </summary>
    [AddComponentMenu("Arcana/Utilities/Status")]
    public class Status : MonoBehaviour
    {

        #region Static Methods.

        #region Enum Parsing Method.

        /// <summary>
        /// Parse the type as a string using the appropriate enum.
        /// </summary>
        /// <param name="_type">Enum value to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(ComponentStatus _type)
        {
            string result = "";

            switch (_type)
            {
                case ComponentStatus.Initialized:
                    result = "(Initializing)";
                    break;
                case ComponentStatus.Running:
                    result = "(Running)";
                    break;
                case ComponentStatus.Active:
                    result = "(Active)";
                    break;
                case ComponentStatus.Inactive:
                    result = "(Inactive)";
                    break;
                case ComponentStatus.Pause:
                    result = "(Pause)";
                    break;
                case ComponentStatus.Alive:
                    result = "(Alive)";
                    break;
                case ComponentStatus.Dead:
                    result = "(Dead)";
                    break;
                case ComponentStatus.Destroy:
                    result = "(Destroy)";
                    break;
                case ComponentStatus.Visible:
                    result = "(Visible)";
                    break;
                case ComponentStatus.Displayable:
                    result = "(Displayable)";
                    break;
                case ComponentStatus.NULL:
                    result = "(Null)";
                    break;
                default:
                    result = "(Unknown Status)";
                    break;
            }

            return result;
        }

        #endregion

        #region Component Factory Methods.

        /// <summary>
        /// Creates a new Status component.
        /// </summary>
        /// <returns>Creates a new status component and adds it to the parent.</returns>
        public static Status Create(ArcanaObject _parent)
        {
            return ComponentFactory.Create<Status>(_parent);
        }

        /// <summary>
        /// Clone a component and set it equal to another.
        /// </summary>
        /// <param name="_parent">Parent to add clone to.</param>
        /// <param name="_template">Status to clone.</param>
        /// <returns>Returns a new component that has been cloned.</returns>
        public static Status Clone(ArcanaObject _parent, Status _template)
        {
            return Status.Create(_parent).Clone(_template);
        }

        #endregion
        
        #endregion

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
        /// Initialization flag for the status component.
        /// </summary>
        private bool m_initialized = false;

        /// <summary>
        /// Collection of current states.
        /// </summary>
        private List<ComponentStatus> m_status = null;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////
        
        /// <summary>
        /// Debug mode value.
        /// </summary>
        public bool Debug
        {
            get { return this.m_debug; }
            set { this.m_debug = value; }
        }

        /// <summary>
        /// Returns true if the Status component has been initialized.
        /// </summary>
        public bool InitializedComponent
        {
            get { return this.m_initialized; }
        }

        /// <summary>
        /// Returns true if the component this Status belongs to has been initialized.
        /// </summary>
        public bool Initialized
        {
            get { return this.IsInitialized(); }
        }
        
        /// <summary>
        /// Checks if the Status object has any states.
        /// </summary>
        public bool HasStatus
        {
            get
            {
                // Returns true if the collection has a state objects and isn't null or emtpy.
                return (this.m_status != null && this.m_status.Count > 0 && !this.m_status.Contains(ComponentStatus.NULL));
            }
        }
        
        /// <summary>
        /// Checks if the current status is null.
        /// </summary>
        public bool IsNull
        {
            get { return this.m_status.Contains(ComponentStatus.NULL); }
        }

        /// <summary>
        /// Returns a list of the current status.
        /// </summary>
        private List<ComponentStatus> CurrentStatus
        {
            get
            {
                return this.m_status;
            }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.
        //
        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the component.
        /// </summary>
        public void Initialize()
        {
            if (!this.InitializedComponent)
            {
                this.m_status = new List<ComponentStatus>();
                this.m_initialized = true;
            }
        }

        /// <summary>
        /// Clone the data from the input component template.
        /// </summary>
        /// <param name="_template"></param>
        public Status Clone(Status _template)
        {
            if(this != _template)
            {
                Debugger.Print("Cloning Status component.");
                this.m_initialized = _template.m_initialized;
                this.m_status = _template.CurrentStatus;
            }

            return this;
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Gets a list of all names included within the status.
        /// </summary>
        /// <returns>Returns list with names of status enums.</returns>
        private List<string> GetStatusNames()
        {
            // Create empty list.
            List<string> result = new List<string>();

            if (HasStatus)
            {
                // For every status, parse its value and return it.
                foreach (ComponentStatus status in this.m_status)
                {
                    result.Add(Status.Parse(status));
                }
            }

            // Return the list.
            return result;
        }

        /// <summary>
        /// Get's a message declaring the state of the Status component in human readable form.
        /// </summary>
        /// <returns>Returns a report of all active states.</returns>
        public string GetReport()
        {
            // Default message.
            string report = "No state.";

            if (HasStatus)
            {
                // Return all currently active status names as comma separated list.
                report = "Current " + gameObject.name + " Status: [" + Services.Concat(", ", GetStatusNames().ToArray()) + "]";
            }

            // Return the report.
            return report;
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Checks a <see cref="ComponentStatus"/> to see if it is a current state of the component.
        /// </summary>
        /// <param name="_check"><see cref="ComponentStatus"/> to check for.</param>
        /// <returns>Returns true if the <see cref="ComponentStatus"/> is owned by the collection.</returns>
        public bool IsState(ComponentStatus _check)
        {
             // If it doesn't have a null state and contains at least one status.
             return (HasStatus && this.m_status.Contains(_check));
        }

        /// <summary>
        /// Check if running is a current status.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool IsRunning()
        {
            return IsState(ComponentStatus.Running);
        }

        /// <summary>
        /// Check if paused.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool IsPaused()
        {
            return IsState(ComponentStatus.Pause);
        }

        /// <summary>
        /// Check if alive.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool IsAlive()
        {
            return IsState(ComponentStatus.Alive);
        }

        /// <summary>
        /// Check if dead.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool IsDead()
        {
            return IsState(ComponentStatus.Dead);
        }
        
        /// <summary>
        /// Check if set to be destroy.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool IsDestroy()
        {
            return IsState(ComponentStatus.Destroy);
        }
        
        /// <summary>
        /// Check if active.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool IsActive()
        {
            return IsState(ComponentStatus.Active);
        }

        /// <summary>
        /// Check if inactive.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool IsInactive()
        {
            return IsState(ComponentStatus.Inactive);
        }

        /// <summary>
        /// Check if being initialized.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool IsInitialized()
        {
            if (HasStatus)
            {
                // If it has the init state, it IS initialized.
                return IsState(ComponentStatus.Initialized);
            }

            // If there is no statuses, it's not initialized.
            return false;
        }

        /// <summary>
        /// Check if visible.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool IsVisible()
        {
            return IsState(ComponentStatus.Visible);
        }

        /// <summary>
        /// Check if HUD should be displayed.
        /// </summary>
        /// <returns>Returns status of state.</returns>
        public bool DisplayGUI()
        {
            return IsState(ComponentStatus.Displayable);
        }
        
        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Add state to the list if it doesn't already exist.
        /// </summary>
        /// <param name="_status">State to add.</param>
        private void AddState(ComponentStatus _status)
        {
            if (!HasStatus)
            {
                this.m_status = new List<ComponentStatus>();
            }

            if (!IsState(_status))
            {
                Debugger.Print("Adding state " + Status.Parse(_status) + " to " + gameObject.name, _mode: m_debug);
                this.m_status.Add(_status);
            }
        }
        
        /// <summary>
        /// Remove state to the list if it exists.
        /// </summary>
        /// <param name="_status">State to remove.</param>
        private void RemoveState(ComponentStatus _status)
        {
            if (HasStatus && IsState(_status))
            {
                Debugger.Print("Removing state " + Status.Parse(_status) + " from " + gameObject.name, _mode: m_debug);
                this.m_status.Remove(_status);
            }
        }

        /// <summary>
        /// Add the pause state to the status.
        /// </summary>
        public void Pause()
        {
            this.AddState(ComponentStatus.Pause);
        }

        /// <summary>
        /// Remove the pause state from the status.
        /// </summary>
        public void Resume()
        {
            this.RemoveState(ComponentStatus.Pause);
        }

        /// <summary>
        /// Add the running state to the status.
        /// </summary>
        public void Run()
        {
            this.AddState(ComponentStatus.Running);
        }

        /// <summary>
        /// Remove the running state from the status.
        /// </summary>
        public void Stop()
        {
            this.RemoveState(ComponentStatus.Running);
        }
        
        /// <summary>
        /// Add the activate state to the object and remove the inactive state.
        /// </summary>
        public void Activate()
        {
            this.AddState(ComponentStatus.Active);
            this.RemoveState(ComponentStatus.Inactive);
        }

        /// <summary>
        /// Add the inactivate state to the object and remove the active state.
        /// </summary>
        public void Deactivate()
        {
            this.AddState(ComponentStatus.Inactive);
            this.RemoveState(ComponentStatus.Active);
        }

        /// <summary>
        /// Add the visible state.
        /// </summary>
        public void Show()
        {
            this.AddState(ComponentStatus.Visible);
        }

        /// <summary>
        /// Remove the visible state.
        /// </summary>
        public void Hide()
        {
            this.RemoveState(ComponentStatus.Visible);
        }

        /// <summary>
        /// Add the displayable state.
        /// </summary>
        public void ShowGUI()
        {
            this.AddState(ComponentStatus.Displayable);
        }

        /// <summary>
        /// Remove the displayable state.
        /// </summary>
        public void HideGUI()
        {
            this.RemoveState(ComponentStatus.Displayable);
        }

        /// <summary>
        /// Add the alive state and remove the dead state.
        /// </summary>
        public void Revive()
        {
            this.AddState(ComponentStatus.Alive);
            this.RemoveState(ComponentStatus.Dead);

        }

        /// <summary>
        /// Add the dead state and remove the alive state.
        /// </summary>
        public void Kill()
        {
            this.AddState(ComponentStatus.Dead);
            this.RemoveState(ComponentStatus.Alive);
        }

        /// <summary>
        /// Remove all other states and add the destroy state.
        /// </summary>
        public void Destroy()
        {
            this.m_status = new List<ComponentStatus>();
            this.AddState(ComponentStatus.Destroy);
        }

        /// <summary>
        /// Set the initialization state for the component this Status belongs to.
        /// </summary>
        public void Initialize(bool _init)
        {
            if (_init)
            {
                this.AddState(ComponentStatus.Initialized);
            }
            else
            {
                this.RemoveState(ComponentStatus.Initialized);
            }
        }

        #endregion

    }

    #endregion

    #region Enum: ComponentStatus.

    /// <summary>
    /// Represents a status that is given to a component.
    /// </summary>
    public enum ComponentStatus
    {
        NULL,
        Initialized,
        Running,
        Pause,
        Alive,
        Dead,
        Active,
        Inactive,
        Visible,
        Displayable,
        Destroy
    }

    #endregion

}
