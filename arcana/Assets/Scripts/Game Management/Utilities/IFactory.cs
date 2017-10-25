// This
// file
// is
// no
// longer
// in
// use... (Deprecated).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Utilities
{

    #region // Interface: IFactory

    /// <summary>
    /// (Deprecated) Represents a Factory schema for creating Monobehavior/GameObjects, and returning them.
    /// </summary>
    public interface IFactory<T> where T: MonoBehaviour, IFactoryElement
    {

        // Some classes may even contain Dictionary<string, Constraints> presets;

        /// <summary>
        /// Get the single instance of the factory.
        /// </summary>
        /// <returns>Returns a single instance of the factory.</returns>
        IFactory<T> GetInstance();

        /// <summary>
        /// Returns a created <see cref="MonoBehaviour"/>, with the option to pass in a series of terms.
        /// </summary>
        /// <param name="parent">GameObject receiving the <see cref="MonoBehaviour"/></param>
        /// <returns>Returns appropriate <see cref="MonoBehaviour"/> component.</returns>
        T CreateComponent(GameObject parent, Constraints parameters);

        /// <summary>
        /// Returns a default <see cref="MonoBehaviour"/>, initialized with no input parameters.
        /// </summary>
        /// <returns>Returns appropriate <see cref="MonoBehaviour"/> component.</returns>
        T CreateComponent(GameObject parent);

        /// <summary>
        /// Returns a default <see cref="MonoBehaviour"/>, initialized with no input parameters,
        /// in it's own empty GameObject.
        /// </summary>
        /// <returns>Returns appropriate <see cref="MonoBehaviour"/> component.</returns>
        T CreateComponent();

    }

    #endregion

    #region // Interface: IFactoryElement

    /// <summary>
    /// Represents a schema for elements that can be created by a factory.
    /// </summary>
    public interface IFactoryElement
    {

        /// <summary>
        /// Initialize a parameter associated with the given key, passing in the value that will be cast based on the context.
        /// </summary>
        /// <param name="key">ID of the data member being set.</param>
        /// <param name="value">Value to (cast) and give to the data member.</param>
        void Initialize(string parameter, object value);

    }

    #endregion

}
