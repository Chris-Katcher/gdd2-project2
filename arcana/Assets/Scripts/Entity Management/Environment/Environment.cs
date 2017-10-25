using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Arcana.Entities;


namespace Arcana.Environment
{

    #region Factory: EnvironmentFactory factory.

    /// <summary>
    /// Constructs Environment entities.
    /// </summary>
    public class EnvironmentFactory : EntityFactory
    {
        /// <summary>
        /// Set up parameters to be used with this factory's product.
        /// </summary>
        /// <param name="_type">EntityType to be assigned.</param>
        /// <param name="_position">Initial position of the environment.</param>
        /// <returns></returns>
        public sealed override Constraints CreateSettings(EntityType _type = EntityType.Environment, Vector2? _position = null)
        {
            return base.CreateSettings(_type, _position);
        }
    }

    #endregion

    #region Class: Environment class.

    /// <summary>
    /// Environment keeps track of other environment pieces (obstacles, platforms, walls), and is an entity itself.
    /// </summary>
    public class Environment : Entity
    {

        #region Data Members.

        /////////////////////
        // Data members.
        /////////////////////

        /// <summary>
        /// Physical objects that can be placed in the scene are props.
        /// </summary>
        private List<Entity> m_props;
        
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

        #region Service Methods.

        #region Initialization Methods.

        /// <summary>
        /// Initialize the environment.
        /// </summary>
        protected sealed override void Initialize()
        {
            // Call the base class's initialization function.
            base.Initialize();

            // Make the props object.
            this.m_props = new List<Entity>();

        }

        #endregion

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
        public sealed override bool IsAvailable()
        {
            return (!HasProps() && base.IsAvailable());
        }

        /// <summary>
        /// Run when asked to make entity available.
        /// </summary>
        public sealed override void MakeAvailable()
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

}
