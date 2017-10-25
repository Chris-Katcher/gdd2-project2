/************************************************
 * Environment.cs
 * 
 * This file contains:
 * - The Environment class. (Child of Entity).
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Entities;

namespace Arcana.Environment
{
    
    #region Class: Environment class.

    /// <summary>
    /// Environment keeps track of other environment pieces (obstacles, platforms, walls), and is an entity itself.
    /// </summary>
    public class Environment : Entity
    {

        #region Data Members.

        #region Fields.

        /////////////////////
        // Data members.
        /////////////////////

        /// <summary>
        /// Physical objects that can be placed in the scene are props.
        /// </summary>
        private List<Entity> m_props;

        // TODO: Add component for determining boundaries.

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns the collection of props in the scene.
        /// </summary>
        public List<Entity> Props
        {
            get { return this.m_props; }
        }

        #endregion

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the environment.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the base class's initialization function.
                base.Initialize();

                // Make the props object.
                this.m_props = new List<Entity>();

                // TODO: Change Entity to Props.
                // TODO: Initialize the spawnable area.
            }
        }

        #endregion
        
        #region Accessor Methods.

        /// <summary>
        /// Determines if the environment is empty or not.
        /// </summary>
        /// <returns>Returns true if there are elements in the prop list.</returns>
        public bool HasProps()
        {
            return (this.m_props != null && this.m_props.Count > 0);
        }

        /// <summary>
        /// If this has props, the environment is not available to be reused.
        /// </summary>
        /// <returns>Returns true if available.</returns>
        public override bool IsAvailable()
        {
            return (!HasProps() && base.IsAvailable());
        }

        /// <summary>
        /// Run when asked to make entity available.
        /// </summary>
        public override void MakeAvailable()
        {
            this.m_props = new List<Entity>();
            this.Status.Activate();
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Add prop to the props list.
        /// </summary>
        /// <param name="e">Prop to add.</param>
        public void AddProp(Entity e)
        {
            this.m_props.Add(e);
        }

        /// <summary>
        /// Remove the prop from the props list.
        /// </summary>
        /// <param name="e">Prop to remove.</param>
        public void RemoveProp(Entity e)
        {
            if (this.m_props.Contains(e))
            {
                this.m_props.Remove(e);
            }
        }
        
        #endregion

    }

    #endregion

    #region Enum: EnvironmentType

    // TODO: Create environment types.

    #endregion

}
