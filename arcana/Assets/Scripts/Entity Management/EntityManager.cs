/************************************************
 * EntityManager.cs
 * 
 * This file contains:
 * - The EntityManager class. (Child of ArcanaObject).
 * - The EntityType enum.
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
using Arcana.Environment;

namespace Arcana.Entities
{

    #region Class: EntityManager class.

    /////////////////////
    // Manager class.
    /////////////////////

    /// <summary>
    /// Manager responsible for the Entity items.
    /// </summary>
    [AddComponentMenu("Arcana/Managers/EntityManager")]
    public class EntityManager : ArcanaObject
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
            string result = "";

            switch (_type)
            {
                case EntityType.Player:
                    result = "(Player)";
                    break;
                case EntityType.Entity:
                    result = "(Entity)";
                    break;
                case EntityType.Environment:
                    result = "(Environment)";
                    break;
                case EntityType.NULL:
                    result = "(Null Entity)";
                    break;
                default:
                    result = "(Unknown Entity)";
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
        public static EntityManager instance = null;

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        /// <returns>Returns a component.</returns>
        public static EntityManager GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of EntityManager.");
                instance = Services.CreateEmptyObject("Entity Manager").AddComponent<EntityManager>();
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
        public static EntityManager Create(ArcanaObject _parent)
        {
            if (!HasInstance())
            {
                instance = _parent.GetComponent<EntityManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<EntityManager>(_parent);
            }

            return instance;
        }

        #endregion

        #region Entity ID Naming Methods.

        /// <summary>
        /// Global collection mapping names to entities.
        /// </summary>
        private static Dictionary<string, Entity> s_collection = null;

        /// <summary>
        /// Collection of used names.
        /// </summary>
        private static List<string> s_usedNames = null;

        /// <summary>
        /// Return all named entities.
        /// </summary>
        public static Dictionary<string, Entity> AllEntities
        {
            get
            {
                if (s_collection == null)
                {
                    s_collection = new Dictionary<string, Entity>();
                }

                return s_collection;
            }
        }

        /// <summary>
        /// Reference to collection of used names.
        /// </summary>
        public static List<string> EntityNames
        {
            get
            {
                if (s_usedNames == null)
                {
                    s_usedNames = new List<string>();
                }

                return s_usedNames;
            }
        }

        /// <summary>
        /// Returns true if names are in the collection.
        /// </summary>
        public static bool HasNames
        {
            get { return (s_usedNames != null && s_usedNames.Count > 0); }
        }

        /// <summary>
        /// Check if an Entity already has the input name. Case insensitive.
        /// </summary>
        /// <param name="_name">Name to check.</param>
        /// <returns>Returns true if the name is already in the collection.</returns>
        public static bool IsTaken(string _name)
        {
            if (HasNames)
            {
                // Check if the collection has the name (trimmed of whitespace).
                return EntityNames.Contains(_name.ToUpper().Trim());
            }

            // If there are no names, return false. (It can't be taken if empty).
            return false;
        }

        /// <summary>
        /// Unregister, and register, Entity, to update name changes.
        /// </summary>
        /// <param name="e">Entity to reregister</param>
        public static void UpdateRegistry(Entity e, bool overwrite = true)
        {
            UnregisterEntity(e);
            RegisterEntity(e, overwrite);
        }

        /// <summary>
        /// Add the Entity to the collection.
        /// </summary>
        /// <param name="e">Entity to add.</param>
        /// <param name="overwrite">Flag to overwrite existing entity if it exists.</param>
        public static void RegisterEntity(Entity e, bool overwrite = true)
        {
            // If you don't want to overwrite an existing Entity. (On by default).
            if (!overwrite)
            {
                // Verify the name.
                e.SetID(VerifyName(e.EntityID));
            }

            // If it isn't contained within the dictionary.
            if (AllEntities.ContainsKey(e.EntityID))
            {
                // Overwrite entry.
                AllEntities[e.EntityID] = e;
            }
            else
            {
                // Add the entity.
                AllEntities.Add(e.EntityID, e);
            }
        }

        /// <summary>
        /// Removes entity key-pair if it already exists.
        /// </summary>
        /// <param name="e">Entity to remove.</param>
        public static void UnregisterEntity(Entity e)
        {
            // If there are entities to deregister:
            if (HasNames && AllEntities.Count > 0)
            {
                // We only want to remove entities that are already registered,
                // not just entities that may share names.
                if (AllEntities.ContainsValue(e))
                {
                    RemoveName(e.EntityID);
                }
            }
        }

        /// <summary>
        /// Returns verified name, and, adds it to the collection.
        /// </summary>
        /// <param name="_name">Name to add.</param>
        /// <returns>Returns verified name.</returns>
        private static string VerifyName(string _name)
        {
            // Get the available, verified name.
            string v_name = GetAvailableName(_name);

            // Add the verified name to the collection.
            EntityNames.Add(v_name);

            // Return the v_name.
            return v_name;
        }
        
        /// <summary>
        /// Remove the name from the collections.
        /// </summary>
        /// <param name="_name">Name of entity to remove.</param>
        private static void RemoveName(string _name)
        {
            if (HasNames)
            {
                // Remove the name.
                if (EntityNames.Contains(_name))
                {
                    EntityNames.Remove(_name);
                }

                // Remove the map.
                if (AllEntities.ContainsKey(_name))
                {
                    AllEntities.Remove(_name);
                }
            }
        }

        /// <summary>
        /// Returns input name, as long as it isn't in the collection, modifying it should it already exist.
        /// </summary>
        /// <param name="_name">Name to modify, should it be unavailable.</param>
        /// <param name="level">Level of recursion to apply to entity naming.</param>
        /// <returns>Returns the final form of the name.</returns>
        private static string GetAvailableName(string _name, int level = 0)
        {
            // Storage for name.
            string name = _name.Trim();

            // If name is empty, give it the default name.
            if (name.Length == 0)
            {
                name = "Untitled Entity";
            }

            // Check recursion level.
            if (level > 0)
            {
                // Append clone number should it be applicable.
                name = name + " (" + level.ToString() + ")";
            }
            
            // If the input name is not taken, 
            if (!IsTaken(name))
            {
                // If not taken, we can return it.
                return name;
            }

            // If current name is taken, enter another level of recursion.
            return GetAvailableName(_name, level + 1);
        }

        #endregion

        #endregion

        #region Data Members.

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// List of Entities.
        /// </summary>
        private List<Entity> m_entities = null;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Reference to objects with Entity components.
        /// </summary>
        public List<Entity> Entities
        {
            get
            {
                if (this.m_entities == null)
                {
                    this.m_entities = new List<Entity>();
                }
                return this.m_entities;
            }
        }

        /// <summary>
        /// Determine if the manager is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((this.m_entities == null) || (this.m_entities.Count == 0));
            }
        }
        
        #endregion

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Create the data members for the EntityManager.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Initialize the base values.
                base.Initialize();

                // Set this name.
                this.Name = "Entity Manager";

                // Initialize the entity manager.
                Debugger.Print("Initializing entity manager.", this.Self.name);

                // Make the new list.
                this.m_entities = new List<Entity>();

                // In the context of a manager ArcanaObject,
                // This means that we can pool the objects
                // that it manages,
                // so long as its Entities are poolable, themselves.
                this.IsPoolable = true;
            }
        }

        #endregion

        #region Service Methods.

        /////////////////////
        // Service Methods.
        /////////////////////

        /// <summary>
        /// Checks if entity already exists in list.
        /// </summary>
        /// <param name="e">Entity to search for.</param>
        /// <returns>Returns true if it is contained within the list.</returns>
        public bool HasEntity(Entity e)
        {
            return this.Entities.Contains(e);
        }

        /// <summary>
        /// Checks if a given entity is available to be reused.
        /// </summary>
        /// <param name="e">Entity to check.</param>
        /// <returns>Returns true if entity is inactive.</returns>
        public bool IsAvailable(Entity e)
        {
            // When an element is marked as inactive, it's available to be reused.
            return (e.IsAvailable());
        }
        
        /// <summary>
        /// Get next available will return the first inactive Entity in the list it can find.
        /// If it is unavailable, it will create a new Entity.
        /// </summary>
        /// <param name="_type">Type of Entity to request.</param>
        /// <returns>Returns entity of given type if it is available.</returns>
        public Entity GetNextAvailable(ArcanaObject _parent, EntityType _type)
        {
            // Temp value.
            Entity available = null;

            // Only return an available entity if object pooling is on.
            if (this.IsPoolable)
            {
                // Find the first available entity in the list.
                foreach (Entity e in this.m_entities)
                {
                    if (IsAvailable(e))
                    {
                        available = e;
                        break; // Break out the for loop.
                    }
                }
            }

            // If available is still null, make a new Entity.
            if (available == null)
            {
                // Creates default entity.
                available = MakeEntity(_parent, _type);
            }

            // Run the make available function on the entity.
            available.MakeAvailable();

            // Return available.
            return available;
        }

        #endregion

        #region Mutator Methods.

        /////////////////////
        // Mutator Methods.
        /////////////////////

        /// <summary>
        /// Makes an entity component and adds it to this manager's list.
        /// </summary>
        /// <param name="_parent">Parent to add the Entity component to.</param>
        /// <param name="_type">Type of Entity to add.</param>
        /// <returns>Returns an Entity component.</returns>
        public Entity MakeEntity(ArcanaObject _parent, EntityType _type)
        {
            // Get a container to return the component.
            Entity component = null;
            ArcanaObject parent = _parent;

            // Check if the parent is null.
            if (parent == null || parent.IsNull)
            {
                Debugger.Print("Creating new parent due to null input...", this.Name, this.Debug);

                // Create a new, empty object and add a new arcana object component to it.
                parent = Services.CreateEmptyObject().AddComponent<ArcanaObject>();

                // Initialize the parent object.
                parent.Initialize();
            }

            // Checking against all of the components currently in the parent.
            foreach (Entity entity in parent.GetComponents<Entity>())
            {
                // Check if parent has an Entity on it already.
                component = parent.GetComponent<Entity>();

                // If the Entity matches the input type, we can return it.
                if (component.IsType(_type))
                {
                    Debugger.Print("Component of input type " + Parse(_type) + " already exists on parent " + parent.Name + ".", this.Name, this.Debug);

                    // Add if it doesn't exist already to the collection.
                    AddEntity(component);

                    // Leave the loop, with data in hand.
                    break;
                }
            }

            // If the Entity doesn't match the input type, we can create a new Entity component of said type.
            if (component == null)
            {
                Debugger.Print("Adding new " + Parse(_type) + " Entity component to parent " + parent.Name + ".", this.Name, this.Debug);
                component = parent.Self.AddComponent<Entity>();
                component.AddType(_type);
            }

            // Add the component.
            AddEntity(component);

            // Return the entity.
            return component;
        }

        /// <summary>
        /// Makes a platform component and adds it to this manager's list.
        /// </summary>
        /// <param name="_parent">Parent to add the Entity component to.</param>
        /// <param name="_type">Type of Entity to add.</param>
        /// <returns>Returns an Entity component.</returns>
        public Platform MakePlatform(ArcanaObject _parent, EnvironmentType _type)
        {
            // Get a container to return the component.
            Platform component = null;
            ArcanaObject parent = _parent;

            // Check if the parent is null.
            if (parent == null || parent.IsNull)
            {
                Debugger.Print("Creating new parent due to null input...", this.Name, this.Debug);

                // Create a new, empty object and add a new arcana object component to it.
                parent = Services.CreateEmptyObject().AddComponent<ArcanaObject>();

                // Initialize the parent object.
                parent.Initialize();
            }

            // Checking against all of the components currently in the parent.
            foreach (Platform entity in parent.GetComponents<Platform>())
            {
                // Check if parent has an Entity on it already.
                component = parent.GetComponent<Platform>();

                // If the Entity matches the input type, we can return it.
                if (component.HasEnvironmentType(_type))
                {
                    // Add if it doesn't exist already to the collection.
                    AddEntity(component);

                    // Leave the loop, with data in hand.
                    break;
                }
            }

            // If the Entity doesn't match the input type, we can create a new Entity component of said type.
            if (component == null)
            {
                component = parent.Self.AddComponent<Platform>();
                component.AddEnvironmentType(_type);
            }

            // Add the component.
            AddEntity(component);

            // Return the entity.
            return component;

        }

        /// <summary>
        /// Pauses a selection of entities.
        /// </summary>
        /// <param name="entities">List of entities to pause.</param>
        public void Pause(List<Entity> entities)
        {
            // Pause entities.
            if (!IsEmpty)
            {
                foreach (Entity e in entities)
                {
                    Pause(e);
                }
            }
        }

        /// <summary>
        /// Pause a single entity.
        /// </summary>
        /// <param name="e">Entity to pause.</param>
        public void Pause(Entity e)
        {
            if (e != null && HasEntity(e))
            {
                e.Pause();
            }
        }

        /// <summary>
        /// Resume a selection of entities.
        /// </summary>
        /// <param name="entities">List of entities to resume.</param>
        public void Resume(List<Entity> entities)
        {
            // Resume entities.
            if (!IsEmpty)
            {
                foreach (Entity e in entities)
                {
                    Resume(e);
                }
            }
        }

        /// <summary>
        /// Resume a single entity.
        /// </summary>
        /// <param name="e">Entity to resume.</param>
        public void Resume(Entity e)
        {
            if (e != null && HasEntity(e))
            {
                e.Resume();
            }
        }
        
        /// <summary>
        /// Entity is added to the list.
        /// </summary>
        /// <param name="e">Entity to add.</param>
        private void AddEntity(Entity e)
        {
            if (!HasEntity(e))
            {
                this.m_entities.Add(e);
            }
        }

        /// <summary>
        /// Entity is removed from the list.
        /// </summary>
        /// <param name="e">Entity to remove.</param>
        private void RemoveEntity(Entity e)
        {
            if (HasEntity(e))
            {
                this.m_entities.Remove(e);
            }
        }

        #endregion

    }

    #endregion

    #region Enum: EntityType.

    /////////////////////
    // Enum declaration for entity types.
    /////////////////////

    /// <summary>
    /// Entity type should reflect all its different implementations, 
    /// so it can be easily referenced.
    /// </summary>
    public enum EntityType
    {
        NULL,
        Entity,
        Environment,
        Player
    }

    #endregion

}
