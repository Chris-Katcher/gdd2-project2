using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana
{
    /// <summary>
    /// Represents a Factory schema for creating Monobehavior/GameObjects, and returning them.
    /// </summary>
    public interface IFactory<T> where T: MonoBehaviour
    {
        
        // Some classes may even contain Dictionary<string, Constraints> presets;

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

    }
}
