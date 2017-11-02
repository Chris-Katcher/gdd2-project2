/************************************************
 * SoundResource.cs
 * 
 * This file contains implementation for the SoundResource class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana.Resources;
using UnityEngine;

namespace Arcana.Resources.Sound
{
    /// <summary>
    /// Represents an audio-based asset.
    /// </summary>
    public class SoundResource : Resource
    {

        #region Constructor.

        /// <summary>
        /// Creates an audio resource from a filepath.
        /// </summary>
        /// <param name="_path">Filepath to audio asset.</param>
        public SoundResource(string _id, string _path, ResourceType _type = ResourceType.Sound) : base(_id, _path)
        {
            this.Type = _type;
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Get the asset as an object.
        /// </summary>
        /// <returns>Returns an Object.</returns>
        public UnityEngine.Object Get()
        {
            return Load();
        }

        /// <summary>
        /// Get the asset as an AudioClip.
        /// </summary>
        /// <returns>Returns an audio clip.</returns>
        public AudioClip GetAudioClip()
        {
            return Load() as AudioClip;
        }

        #endregion
        
    }
}
