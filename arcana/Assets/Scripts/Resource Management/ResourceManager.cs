/************************************************
 * ResourceManager.cs
 * 
 * This file contains:
 * - The ResourceManager class. (Child of ArcanaObject).
 * - The ResourceType enum.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana;
using Arcana.Utilities;
using Arcana.Resources.Art;
using Arcana.Resources.Sound;
using UnityEngine;

namespace Arcana.Resources
{

    #region Class: ResourceManager class.

    /////////////////////
    // Manager class.
    /////////////////////

    /// <summary>
    /// Manages, caches, and loads all resources needed for the project.
    /// </summary>
    [AddComponentMenu("Arcana/Managers/ResourceManager")]
    public class ResourceManager : ArcanaObject
    {

        #region Static Methods.

        #region Enum Parsing Methods.

        /// <summary>
        /// Parse the type as a string using the ResourceType.
        /// </summary>
        /// <param name="_type">Type of Resource to parse.</param>
        /// <returns>Returns a string.</returns>
        public static string Parse(ResourceType _type)
        {
            string result = "";

            switch (_type)
            {
                case ResourceType.Art:
                    result = "(Artistic Resource)";
                    break;
                case ResourceType.Sound:
                    result = "(Auditory Resource)";
                    break;
                default:
                    result = "(Unknown Resource)";
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
        public static ResourceManager instance = null;

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        /// <returns>Returns a component.</returns>
        public static ResourceManager GetInstance()
        {
            if (instance == null)
            {
                Debugger.Print("Creating new instance of ResourceManager.");
                instance = Services.CreateEmptyObject("Resource Manager").AddComponent<ResourceManager>();
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
        public static ResourceManager Create(ArcanaObject _parent)
        {
            if (!HasInstance())
            {
                instance = _parent.GetComponent<ResourceManager>();
            }

            if (!HasInstance())
            {
                instance = ComponentFactory.Create<ResourceManager>(_parent);
            }

            return instance;
        }

        #endregion

        #region Resource ID Naming Methods.

        #region Static Fields.

        /// <summary>
        /// Global collection mapping names to resources.
        /// </summary>
        private static Dictionary<string, Resource> s_collection = null;

        /// <summary>
        /// Collection of used names.
        /// </summary>
        private static List<string> s_usedNames = null;

        #endregion

        #region Static Properties.

        /// <summary>
        /// Return all named resources.
        /// </summary>
        public static Dictionary<string, Resource> AllResources
        {
            get
            {
                if (s_collection == null)
                {
                    s_collection = new Dictionary<string, Resource>();
                }

                return s_collection;
            }
        }

        /// <summary>
        /// Reference to collection of used names.
        /// </summary>
        public static List<string> ResourceIDs
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

        #endregion

        #region Naming Methods.

        /// <summary>
        /// Returns true if names are in the collection.
        /// </summary>
        public static bool HasNames
        {
            get { return (s_usedNames != null && s_usedNames.Count > 0); }
        }

        /// <summary>
        /// Check if a Resource already has the input id. Case insensitive.
        /// </summary>
        /// <param name="_name">Name to check.</param>
        /// <returns>Returns true if the id is already in the collection.</returns>
        public static bool IsTaken(string _name)
        {
            string key = MakeKey(_name);
            if(key.Length == 0) { return true; }

            if (HasNames)
            {
                // Check if the collection has the name (trimmed of whitespace).
                return ResourceIDs.Contains(MakeKey(_name));
            }

            // If there are no names, return false. (It can't be taken if empty).
            return false;
        }

        /// <summary>
        /// Add the Resource to the collection.
        /// </summary>
        /// <param name="resource">Resource to add.</param>
        public static void Register(Resource resource)
        {
            string check = MakeKey(resource.ID);

            if (!IsTaken(check))
            {
                resource.SetID(check);
                ResourceIDs.Add(check);

                if (!IsKey(check))
                {
                    AllResources.Add(check, resource);
                }
                else
                {
                    AllResources[check] = resource;
                }
            }
        }

        /// <summary>
        /// Return ID as key.
        /// </summary>
        /// <param name="_id">ID as key.</param>
        /// <returns>Return string ID.</returns>
        private static string MakeKey(string _id)
        {
            return _id.Trim().ToUpper();
        }

        /// <summary>
        /// Checks if value is a key of the map.
        /// </summary>
        /// <param name="_key">Key to check if used.</param>
        /// <returns>Returns true if used.</returns>
        private static bool IsKey(string _key)
        {
            return AllResources.ContainsKey(_key);
        }
        
        #endregion

        #endregion

        #endregion

        #region Data Members.

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Contains a list of all resources.
        /// </summary>
        private List<Resource> m_resources = null;

        #endregion

        #region Properties.

        /////////////////////
        // Properties.
        /////////////////////

        /// <summary>
        /// Reference to resources.
        /// </summary>
        public List<Resource> Resources
        {
            get
            {
                if (this.m_resources == null)
                {
                    this.m_resources = new List<Resource>();
                }
                return this.m_resources;
            }
        }

        /// <summary>
        /// Determine if the manager is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((this.m_resources == null) || (this.m_resources.Count == 0));
            }
        }

        #endregion

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Create the data members for the ResourceManager.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Initialized)
            {
                // Call the base initialization function.
                base.Initialize();

                // Set the name.
                this.Name = "Resource Manager";

                // Initialize the resource manager.
                Debugger.Print("Initializing resource manager.", this.Self.name);

                // Make the new list.
                this.m_resources = new List<Resource>();
            }
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Check if the resource exists in the list.
        /// </summary>
        /// <param name="_resource">Resource to search for.</param>
        /// <returns>Returns true if exists.</returns>
        public bool HasResource(Resource _resource)
        {
            return this.Resources.Contains(_resource);
        }

        /// <summary>
        /// Check if resource is loaded.
        /// </summary>
        /// <param name="_resource">Resource to load.</param>
        /// <returns>Returns true if loaded.</returns>
        public bool IsLoaded(Resource _resource)
        {
            if (_resource != null)
            {
                return _resource.IsLoaded;
            }
            return false;
        }

        #endregion

        #region Accessor Methods

        /// <summary>
        /// Loads a resource by name.
        /// </summary>
        /// <param name="_id">Name of the resource to load.</param>
        /// <returns>Loads a resource and returns it when loaded.</returns>
        public Resource Load(string _id)
        {
            Resource resource = GetResource(_id);

            if (resource != null)
            {
                // Forces load of resource.
                UnityEngine.Object temp = resource.ResourceObject;
                return resource;
            }

            return null;
        }


        /// <summary>
        /// Return a resource of the input name.
        /// </summary>
        /// <param name="_id">ID of the resource.</param>
        /// <returns>Returns Resource</returns>
        public Resource GetResource(string _id)
        {
            string key = MakeKey(_id);

            if (IsTaken(key))
            {
                return ResourceManager.AllResources[key];
            }

            return null;
        }

        /// <summary>
        /// Return a resource of a specific name, cast as an ArtResource.
        /// </summary>
        /// <param name="_id">ID of the resource.</param>
        /// <returns>Returns ArtResource</returns>
        public ArtResource GetArtResource(string _id)
        {
            Resource resource = GetResource(_id);

            if (resource != null)
            {
                return resource as ArtResource;
            }

            return null;
        }

        /// <summary>
        /// Returns a list of all art resources.
        /// </summary>
        /// <returns>Returns collection of art resources.</returns>
        public List<ArtResource> GetArtResources()
        {
            List<ArtResource> response = new List<ArtResource>();

            foreach (Resource resource in this.Resources)
            {
                if (resource.HasType(ResourceType.Art))
                {
                    response.Add(resource as ArtResource);
                }
            }

            return response;
        }

        /// <summary>
        /// Return a resource of a specific name, cast as an SoundResource.
        /// </summary>
        /// <param name="_id">ID of the resource.</param>
        /// <returns>Returns SoundResource</returns>
        public SoundResource GetSoundResource(string _id)
        {
            Resource resource = GetResource(_id);

            if (resource != null)
            {
                return resource as SoundResource;
            }

            return null;
        }

        /// <summary>
        /// Returns a list of all audio resources.
        /// </summary>
        /// <returns>Returns collection of audio resources.</returns>
        public List<SoundResource> GetSoundResources()
        {
            List<SoundResource> response = new List<SoundResource>();

            foreach (Resource resource in this.Resources)
            {
                if (resource.HasType(ResourceType.Sound))
                {
                    response.Add(resource as SoundResource);
                }
            }

            return response;
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Add a resource to the resource manager.
        /// </summary>
        /// <param name="_id">ID of resource.</param>
        /// <param name="_path">Resource filepath.</param>
        /// <param name="_type">Resource type.</param>
        /// <returns>Returns true if resource is successfully added.</returns>
        public bool AddResource(string _id, string _path, ResourceType _type)
        {
            string key = MakeKey(_id);

            if (key.Length == 0)
            {
                Debugger.Print("Invalid name.");
                return false;
            }

            Resource resource = null;

            switch (_type)
            {
                case ResourceType.NULL:
                    resource = CreateResource(key, _path);
                    break;
                case ResourceType.Art:
                    resource = CreateArtResource(key, _path) as Resource;
                    break;
                case ResourceType.Sound:
                    resource = CreateSoundResource(key, _path) as Resource;
                    break;
            }

            if (resource != null)
            {
                this.Resources.Add(resource);
                ResourceManager.Register(resource);
            }

            Debugger.Print("Null resource cannot be registered.");
            return false;
        }

        /// <summary>
        /// Create and register a resource.
        /// </summary>
        /// <param name="_id">Name to give.</param>
        /// <param name="_path">Path the resource has.</param>
        /// <param name="_type">Type of resource to create.</param>
        /// <returns>Returns the created resource.</returns>
        private Resource CreateResource(string _id, string _path, ResourceType _type)
        {
            Resource resource;
            string key = MakeKey(_id);

            if (!IsTaken(key))
            {
                switch (_type)
                {
                    case ResourceType.NULL:
                        resource = new Resource(key, _path);
                        break;
                    case ResourceType.Art:
                        resource = new ArtResource(key, _path) as Resource;
                        break;
                    case ResourceType.Sound:
                        resource = new SoundResource(key, _path) as Resource;
                        break;
                    default:
                        return null;
                }

                return resource;
            }

            return null;
        }

        /// <summary>
        /// Create and register a resource.
        /// </summary>
        /// <param name="_id">Name to give.</param>
        /// <param name="_path">Path the resource has.</param>
        /// <returns>Returns the created resource.</returns>
        private Resource CreateResource(string _id, string _path)
        {
            string key = MakeKey(_id);

            if (!IsTaken(key))
            {
                return new Resource(_id, _path);
            }

            return null;
        }

        /// <summary>
        /// Create and register an art resource.
        /// </summary>
        /// <param name="_id">Name to give.</param>
        /// <param name="_path">Path the resource has.</param>
        /// <returns>Returns the created resource.</returns>
        private ArtResource CreateArtResource(string _id, string _path)
        {
            string key = MakeKey(_id);

            if (!IsTaken(key))
            {
                return new ArtResource(_id, _path);
            }

            return null;
        }

        /// <summary>
        /// Create and register an audio resource.
        /// </summary>
        /// <param name="_id">Name to give.</param>
        /// <param name="_path">Path the resource has.</param>
        /// <returns>Returns the created resource.</returns>
        private SoundResource CreateSoundResource(string _id, string _path)
        {
            string key = MakeKey(_id);

            if (!IsTaken(key))
            {
                return new SoundResource(_id, _path);
            }

            return null;
        }

        /// <summary>
        /// Removes all unused assets before calling the base.
        /// </summary>
        public override void DestroySelf()
        {
            UnityEngine.Resources.UnloadUnusedAssets();
            base.DestroySelf();
        }

        #endregion

    }

    #endregion

    #region Enum: ResourceType

    /// <summary>
    /// Types of assets that can be loaded.
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// Null resource is what a resource is by default.
        /// </summary>
        NULL,

        /// <summary>
        /// An artistic resource will be represented by ArtResource objects.
        /// </summary>
        Art,

        /// <summary>
        /// An auditory resource will be represented by SoundResource objects.
        /// </summary>
        Sound
    }

    #endregion

}
