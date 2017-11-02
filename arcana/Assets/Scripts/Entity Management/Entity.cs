/************************************************
 * Entity.cs
 * 
 * This file contains:
 * - The Entity class. (Child of ArcanaObject).
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

namespace Arcana.Entities
{

    #region Class: Entity class.

    /////////////////////
    // Class declaration.
    /////////////////////

    /// <summary>
    /// All Entities have a location in world space can be toggled as visible or invisible.
    /// </summary>
    [AddComponentMenu("Arcana/Entities/Entity")]
    public class Entity : ArcanaObject
    {

        #region Static Methods.

        #region Enum Parsing Methods.

        /// <summary>
        /// Parse the type as a string using the EntityType.
        /// </summary>
        /// <param name="_type">Type of Entity to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(EntityType _type)
        {
            return EntityManager.Parse(_type);
        }

        #endregion

        #region Component Factory Methods.

        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <returns>Creates a new component and adds it to the parent.</returns>
        public static Entity Create(ArcanaObject _parent)
        {
            return ComponentFactory.Create<Entity>(_parent);
        }

        /// <summary>
        /// Clone a component and set it equal to another.
        /// </summary>
        /// <param name="_parent">Parent to add clone to.</param>
        /// <param name="_template">Component to clone.</param>
        /// <returns>Returns a new component that has been cloned.</returns>
        public static Entity Clone(ArcanaObject _parent, Entity _template)
        {
            return (Entity)(Entity.Create(_parent)).Clone(_template);
        }

        #endregion

        #endregion

        #region Data Members.

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////
        
        /// <summary>
        /// The type of Entity this Entity component represents (a composite of various types).
        /// </summary>
        private List<EntityType> m_types;

        /// <summary>
        /// Identifying name of the component that can differ from the object.
        /// </summary>
        private string m_name;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Returns true if null ArcanaObject or if of null EntityType.
        /// </summary>
        public override bool IsNull
        {
            get
            {
                return (base.IsNull || this.EntityTypes.Contains(EntityType.NULL));
            }
        }

        /// <summary>
        /// Returns the types of this entity.
        /// </summary>
        public List<EntityType> EntityTypes
        {
            get
            {
                if (this.m_types == null)
                {
                    this.m_types = new List<EntityType>();
                }
                return this.m_types;
            }
        }

        /// <summary>
        /// Identifying name associated with this Entity.
        /// </summary>
        public string EntityID
        {
            get
            {
                return this.m_name;
            }
        }

        #endregion

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initializes the Entity with a null type.
        /// </summary>
        public override void Initialize()
        {
            // Call the base initialization method.
            base.Initialize();

            // Set the default type.
            this.m_types = this.EntityTypes;
            this.m_types.Add(EntityType.NULL); // Null entity by default.

            // Set the name to untitled by default.
            this.SetID("Untitled Entity");            
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Return Entities from list of children.
        /// </summary>
        /// <returns>Returns list of entities.</returns>
        public List<Entity> GetChildEntities()
        {
            return this.GetChildren<Entity>();
        }

        /// <summary>
        /// Returns whether or not the Entity contains a certain type.
        /// </summary>
        /// <param name="_type">Checks if type exists.</param>
        /// <returns>Return true if it has the input type.</returns>
        private bool HasType(EntityType _type)
        {
            return this.EntityTypes.Contains(_type);
        }
        
        /// <summary>
        /// Check if it matches the input type. EntityType.NULL will never be true.
        /// </summary>
        /// <param name="_type">EntityType to check.</param>
        /// <returns>Returns true if there is a match.</returns>
        public bool IsType(EntityType _type)
        {
            // Null types cannot be checked for a match.
            if (!this.IsNull)
            {
                return HasType(_type);
            }

            // Will return false by default.
            return false;
        }
        
        /// <summary>
        /// Check if ID value matches input.
        /// </summary>
        /// <param name="_id">Input value to compare.</param>
        /// <returns>Returns true if there is a match.</returns>
        public bool HasID(string _id)
        {
            // Trimmed, upper, with no edge whitespace.
            string input = _id.ToUpper().Trim();

            // If the input value isn't empty, check if there is a match.
            return (input.Length > 0) && (this.m_name.ToUpper().Trim() == input);
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Add Entity as a child if it doesn't already exist.
        /// </summary>
        /// <param name="_entity">Entity to add as a child.</param>
        public void AddChildEntity(Entity _entity)
        {
            if (!GetChildEntities().Contains(_entity))
            {
                this.AddChild(_entity);
            }
        }

        /// <summary>
        /// Remove type from the composite list.
        /// </summary>
        /// <param name="_type">Type to remove from this entity.</param>
        public void RemoveType(EntityType _type)
        {
            // Remove type if it exists.
            if (HasType(_type))
            {
                this.EntityTypes.Remove(_type);
            }
        }

        /// <summary>
        /// Add type to the composite list.
        /// </summary>
        /// <param name="_type">Type to make this entity.</param>
        public void AddType(EntityType _type)
        {
            // If already null.
            if (HasType(EntityType.NULL))
            {
                // Remove null type and add current type.
                if (_type != EntityType.NULL)
                {
                    this.EntityTypes.Remove(EntityType.NULL);
                    this.EntityTypes.Add(_type);
                    return;
                }

                // If null, do nothing.
                return;
            }
            else
            {
                // Else, if not already null.
                // Add current type if it doesn't already have it.
                if (!HasType(_type))
                {
                    this.EntityTypes.Add(_type);
                }
            }
        }

        /// <summary>
        /// Set the name of this Entity, if (and only if) the name doesn't already exist in pool of used names.
        /// </summary>
        /// <param name="_id"></param>
        public void SetID(string _id)
        {
            // Get the verified version of the name, added to this.
            this.m_name = _id.ToUpper().Trim();

            // Update the EntityManager registry.
            EntityManager.UpdateRegistry(this);
        }

        #endregion

    }

    #endregion

}
