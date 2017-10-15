/************************************************
 * Manager.cs
 * 
 * This file contains abstract definition for the Manager class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana
{

    /// <summary>
    /// Manager is the abstract class Manager classes will inherit.
    /// </summary>
    /// <typeparam name="T">Type associated with the member of the class</typeparam>
    public abstract class Manager<T> : MonoBehaviour
    {

        #region Initialization Methods

        /////////////////////
        // Initialization methods.
        /////////////////////

        public abstract void Initialize();
        public abstract void IsInitialized();

        #endregion

        #region Accessor Methods

        /////////////////////
        // Accessor methods.
        /////////////////////

        #endregion

        #region Mutator Methods

        /////////////////////
        // Mutator methods.
        /////////////////////

        #endregion

        #region Service Methods

        /////////////////////
        // Service methods.
        /////////////////////

        #endregion

    }
}
