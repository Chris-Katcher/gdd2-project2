using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana.Resources;
using UnityEngine;

namespace Arcana.UI.Elements
{

    #region Struct: UIPrefab.

    /// <summary>
    /// UIPrefab allows us to load prefabs later.
    /// </summary>
    public struct UIPrefab<T> where T: UnityEngine.Object
    {

        #region Data Members.

        #region Fields.

        /// <summary>
        /// ID of the prefab.
        /// </summary>
        private string m_id;

        /// <summary>
        /// Path to the prefab resource.
        /// </summary>
        private string m_path;

        /// <summary>
        /// Reference to the object.
        /// </summary>
        private GameObject m_object;

        #endregion

        #region Properties.

        /// <summary>
        /// ID of the prefab resource.
        /// </summary>
        public string ID
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        /// <summary>
        /// Path to the prefab resource.
        /// </summary>
        public string Path
        {
            get { return this.m_path; }
            set { this.m_path = value; }
        }

        /// <summary>
        /// Returns reference to the asset data.
        /// </summary>
        public UnityEngine.Object Reference
        {
            get { return this.Load(); }
        }

        /// <summary>
        /// Reference to the actual game object.
        /// </summary>
        public GameObject Instance
        {
            get { return this.GetInstance(); }
        }

        /// <summary>
        /// Returns an instantiation of the game object.
        /// </summary>
        public T Prefab
        {
            get { return UnityEngine.Object.Instantiate(Instance) as T; }
        }

        /// <summary>
        /// Returns an instantiation of the game object cast as a GameObject.
        /// </summary>
        public GameObject Clone
        {
            get { return UnityEngine.Object.Instantiate(Instance) as GameObject; }
        }

        /// <summary>
        /// Reference to the resource manager instance.
        /// </summary>
        public ResourceManager ResourceManager
        {
            get { return ResourceManager.GetInstance(); }
        }

        #endregion

        #endregion

        #region Constructor.

        /// <summary>
        /// Constructor for the UIPrefab struct.
        /// </summary>
        /// <param name="_id">ID of the prefab resource.</param>
        /// <param name="_path">Path to the prefab resource.</param>
        public UIPrefab(string _id, string _path)
        {
            this.m_id = _id;
            this.m_path = _path;
            this.m_object = null;
            this.InitializeResource();
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Adds the resource to the manager.
        /// </summary>
        private void InitializeResource()
        {
            // Add the resource to the manager.
            this.ResourceManager.AddResource(this.ID, this.Path, ResourceType.Prefab);
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns a reference to the asset without instantiating it.
        /// </summary>
        /// <returns>Returns the object reference.</returns>
        private UnityEngine.Object Load()
        {
            return ResourceManager.GetResource(this.ID).Load();
        }

        /// <summary>
        /// Returns object as a reference and instantiate it..
        /// </summary>
        /// <returns>Returns GameObject reference.</returns>
        private GameObject GetInstance()
        {
            if(this.m_object == null)
            {
                this.m_object = UnityEngine.Object.Instantiate(Load()) as GameObject;
            }
            return this.m_object;
        }

        #endregion

    }

    #endregion

}
