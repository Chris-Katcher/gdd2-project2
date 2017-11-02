/************************************************
 * Resource.cs
 * 
 * This file contains:
 * - The Resource class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Arcana.Resources
{

    #region Class: Resource class.

    /// <summary>
    /// Represents an asset to be loaded.
    /// </summary>
    public class Resource
    {

        #region Data Members

        #region Fields.

        /// <summary>
        /// Resource ID.
        /// </summary>
        private string m_id;

        /// <summary>
        /// Filepath to the resource.
        /// </summary>
        private string m_resourcePath;

        /// <summary>
        /// Cached reference to object.
        /// </summary>
        private UnityEngine.Object m_resource = null;

        /// <summary>
        /// Type of resource.
        /// </summary>
        private ResourceType m_type = ResourceType.NULL;

        #endregion

        #region Properties.

        /// <summary>
        /// Return the resource ID.
        /// </summary>
        public string ID
        {
            get { return this.m_id; }
        }
        
        /// <summary>
        /// Returns the resource type.
        /// </summary>
        public ResourceType Type
        {
            get { return this.m_type; }
            protected set { this.m_type = value; }
        }

        /// <summary>
        /// Return reference to the resource.
        /// </summary>
        public UnityEngine.Object ResourceObject
        {
            get {
                if (this.m_resource == null && IsValid)
                {
                    this.m_resource = Load();
                }
                return this.m_resource; }
        }

        /// <summary>
        /// Return true if the object resource is not null.
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return (this.m_resource != null) ;
            }
        }

        /// <summary>
        /// Reference to the resource filepath.
        /// </summary>
        public string Path
        {
            get { return this.m_resourcePath; }
            set { this.m_resourcePath = (value).Trim(); }
        }

        /// <summary>
        /// Reference to resource filepath validity.
        /// </summary>
        public bool IsValid
        {
            get { return !IsNull; }
        }

        /// <summary>
        /// Returns true if the filepath is empty.
        /// </summary>
        private bool IsNull
        {
            get
            {
                return (this.m_type == ResourceType.NULL) || (this.Path.Length == 0);
            }
        }
        
        #endregion

        #endregion

        #region Constructor.

        /// <summary>
        /// Create a resource struct using the input filepath.
        /// </summary>
        /// <param name="_path">Filepath to assign.</param>
        public Resource(string _id, string _path)
        {
            this.m_id = _id;
            this.m_resourcePath = _path.Trim();
            this.m_type = ResourceType.NULL;
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Return true if the resource's type is matching the input type.
        /// </summary>
        /// <param name="_type">Input type to match.</param>
        /// <returns>Returns true if input type matches.</returns>
        public bool HasType(ResourceType _type)
        {
            return (this.m_type == _type);
        }

        /// <summary>
        /// Returns the resource retrieved from the UnityEngine resources.
        /// </summary>
        /// <returns>Loaded asset.</returns>
        protected UnityEngine.Object Load()
        {
            if (!IsLoaded && IsValid)
            {
                this.m_resource = UnityEngine.Resources.Load(this.Path);
            }

            if (IsLoaded)
            {
                return this.m_resource;
            }

            // Return nothing, if invalid.
            return null;
        }

        /// <summary>
        /// Load in a file's bytes.
        /// </summary>
        /// <returns>Return byte array containing the file's contents.</returns>
        public byte[] LoadData()
        {
            if (IsValid)
            {
                return File.ReadAllBytes(this.Path);
            }
            else
            {
                return new byte[0];
            }
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set the resource ID.
        /// </summary>
        /// <param name="_id">ID to assign.</param>
        public void SetID(string _id)
        {
            this.m_id = _id.Trim();
        }

        #endregion

    }

    #endregion

}
