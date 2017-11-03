/************************************************
 * ComponentFactory.cs
 * 
 * This file contains:
 * - The ComponentFactory factory class.
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

    #region Factory: ComponentFactory factory.

    /////////////////////
    // Factory declaration.
    /////////////////////

    /// <summary>
    /// Factory structure allowing the creation of MonoBehaviour components and cloning of components.
    /// </summary>
    [AddComponentMenu("Arcana/Component Factory")]
    public class ComponentFactory : MonoBehaviour
    {

        #region Static Methods.

        /////////////////////
        // Static methods for instancing.
        /////////////////////
        
        /// <summary>
        /// Static instance of the factory. (We only want one).
        /// </summary>
        public static ComponentFactory instance = null;

        /// <summary>
        /// Returns the single instance of the factory.
        /// </summary>
        /// <returns>Returns a ComponentFactory MonoBehaviour component.</returns>
        public static ComponentFactory GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of ComponentFactory.", "Component Factory", debugMode);
                instance = Services.CreateEmptyObject("Component Factory").AddComponent<ComponentFactory>();
            }

            return instance;
        }

        /// <summary>
        /// Create a new component, add it to the parent <see cref="GameObject"/>, and return its reference.
        /// </summary>
        /// <typeparam name="T"><see cref="MonoBehaviour"/> type <see cref="Component"/> to create.</typeparam>
        /// <param name="_parent">Parent <see cref="ArcanaObject"/> to add new <see cref="Component"/> to.</param>
        /// <returns>Returns reference to new component.</returns>
        public static T Create<T>(ArcanaObject _parent) where T: MonoBehaviour
        {
            return GetInstance().CreateComponent<T>(_parent);
        }

        /// <summary>
        /// Create a new component, add it to the parent <see cref="GameObject"/>, and return its reference.
        /// </summary>
        /// <typeparam name="T"><see cref="MonoBehaviour"/> type <see cref="Component"/> to create.</typeparam>
        /// <param name="_parent">Parent <see cref="ArcanaObject"/> to add new <see cref="Component"/> to.</param>
        /// <param name="_template">Template to copy information from.</param>
        /// <returns>Returns reference to cloned component.</returns>
        public static T Clone<T>(ArcanaObject _parent, T _template) where T : ArcanaObject
        {
            return GetInstance().CloneComponent<T>(_parent, _template);
        }

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// When started, just set instance reference to this object.
        /// </summary>
        public void Start()
        {
            instance = this;
        }

        #endregion

        #region Constructor.

        /////////////////////
        // Private constructor.
        /////////////////////

        /// <summary>
        /// Debug mode for component factory.
        /// </summary>
        public static bool debugMode = false;

        /// <summary>
        /// Private constructor that creates the factory when requested for the very first time.
        /// </summary>
        private ComponentFactory()
        {
            ComponentFactory.instance = this;
            debugMode = false;
        }

        #endregion

        #region Component Creation Methods.
        
        /// <summary>
        /// Returns a <see cref="MonoBehaviour"/> type <see cref="Component"/>, and adds it to the requesting object.
        /// </summary>
        /// <typeparam name="T"><see cref="MonoBehaviour"/> type <see cref="Component"/> to create.</typeparam>
        /// <param name="_parent">Parent <see cref="ArcanaObject"/> to add new <see cref="Component"/> to.</param>
        /// <returns></returns>
        private T CreateComponent<T>(ArcanaObject _parent) where T: MonoBehaviour
        {
            // Create temp reference.
            T component = null;
            string componentType = typeof(T).Name;

            // Check if the parent is null.
            if (_parent == null || _parent.IsNull)
            {
                Debugger.Print("Cannot add " + componentType + "component to null parent.", this.name, debugMode);
                return component; // Is null.
            }

            // Check if the component already exists on the object.
            Debugger.Print("Creating " + componentType + " component for " + _parent.Name, this.name, debugMode);
            component = _parent.GetComponent<T>();

            // If retrieved reference is null, create a new component.
            if (component == null)
            {
                Debugger.Print("Creating new " + componentType + " component.", this.name, debugMode);
                component = _parent.Self.AddComponent<T>();
            }
            else
            {
                Debugger.Print(componentType + " component already exists.", this.name, debugMode);
            }
            
            // Return the created component.
            return component;
        }


        /// <summary>
        /// Returns a <see cref="MonoBehaviour"/> type <see cref="Component"/>, and adds it to the requesting object.
        /// </summary>
        /// <typeparam name="T"><see cref="MonoBehaviour"/> type <see cref="Component"/> to create.</typeparam>
        /// <param name="_parent">Parent <see cref="ArcanaObject"/> to add new <see cref="Component"/> to.</param>
        /// <param name="_template">Template to copy information from.</param>
        /// <returns></returns>
        private T CloneComponent<T>(ArcanaObject _parent, T _template) where T : ArcanaObject
        {
            // Create temp reference.
            T clone = null;
            string componentType = typeof(T).Name;

            // Check if the parent is null.
            if (_parent == null || _parent.IsNull)
            {
                Debugger.Print("Cannot add clone of " + componentType + "component to null parent.", this.name, debugMode);
                return clone; // Is null.
            }

            // Check if the component already exists on the object.
            Debugger.Print("Creating cloen of " + componentType + " component for " + _parent.Name, this.name, debugMode);
            clone = _parent.GetComponent<T>();

            // If retrieved reference is null, create a new component.
            if (clone == null)
            {
                Debugger.Print("Creating new " + componentType + " component.", this.name, debugMode);
                clone = _parent.Self.AddComponent<T>();
            }
            else
            {
                Debugger.Print(componentType + " component already exists.", this.name, debugMode);
            }
            
            Debugger.Print("Cloning " + componentType + " component into new component.", this.name, debugMode);
            clone.Clone(_template);

            // Return the cloned component.
            return clone;
        }
        
        #endregion

    }

    #endregion

}
