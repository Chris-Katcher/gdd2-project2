/************************************************
 * ArcanaObject.cs
 * 
 * This file contains:
 * - The ArcanaObject base class. (Inherits MonoBehaviour)
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Utilities;

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
            get { return (this.gameObject == null); }
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

        #endregion

        #endregion

        #region UnityEngine Methods.

        #region UnityEngine Editor values.

        /// <summary>
        /// UnityEngine debug flag.
        /// </summary>
        public bool _debug = false;

        /// <summary>
        /// UnityEngine active/inactive flag.
        /// </summary>
        public bool _active = true;

        /// <summary>
        /// Debug color to draw for unity.
        /// </summary>
        public Color _debugColor = Color.yellow;
        
        #endregion
                
        /// <summary>
        /// <see cref="MonoBehaviour"/> function run before first update.
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

            // Initialize, if it hasn't been initialized yet.
            this.Initialize();

            // If this object is set to be destroyed, destroy it.
            if (this.Status.IsDestroy())
            {
                DestroySelf();
            }

            if (this.Status.IsActive())
            {
                Debugger.Print("This object is active.", this.Self.name, _debug);
            }

            if (this.Status.IsInactive())
            {
                Debugger.Print("This object is inactive.", this.Self.name, _debug);
            }

            if (_active)
            {
                this.Status.Activate();
            }
            else
            {
                this.Status.Deactivate();
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
                
                // Give our object a Status component.
                this.m_status = ComponentFactory.Create<Status>(this);
                
                // Give our object a children component.
                this.m_children = new List<ArcanaObject>();
                
                // Set component as initialized.
                this.m_status.Initialize();
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
