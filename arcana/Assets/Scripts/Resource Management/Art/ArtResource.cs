/************************************************
 * ArtResource.cs
 * 
 * This file contains:
 * - The ArtResource class.
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

namespace Arcana.Resources.Art
{

    #region Class: ArtResource class.

    /// <summary>
    /// Respresents an art asset.
    /// </summary>
    public class ArtResource : Resource
    {

        #region Constructor.

        /// <summary>
        /// Creates an art resource from a filepath.
        /// </summary>
        /// <param name="_path">Filepath to art asset.</param>
        public ArtResource(string _id, string _path, ResourceType _type = ResourceType.Art) : base(_id, _path)
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
        /// Return the texture contained by the art asset.
        /// </summary>
        /// <returns>Returns a Texture object.</returns>
        public Texture GetTexture()
        {
            return Load() as Texture;
        }

        /// <summary>
        /// Return the 2D texture contained by the art asset.
        /// </summary>
        /// <returns>Returns a Texture2D object.</returns>
        public Texture2D GetTexture2D()
        {
            return Load() as Texture2D;
        }
        
        /// <summary>
        /// Returns the resource as a sprite (created from a Texture2D).
        /// </summary>
        /// <returns>Returns a sprite object.</returns>
        public Sprite GetSprite(Texture2D _image = null, Rect? _bounds = null, Vector2? _pivot = null)
        {
            Texture2D image = _image;
            Vector2 pivot = new Vector2(0.5f, 0.5f);

            if (_image == null)
            {
                // Retrieve the loaded datat if there isn't an input texture.
                image = GetTexture2D();
            }
            
            if (image == null)
            {
                // Set it to a white texture, if null.
                image = new Texture2D(2, 2);
                image.LoadImage(LoadData());
            }

            // Boundaries of the image.
            Rect bounds = new Rect(0, 0, image.width, image.height);

            if (_bounds.HasValue)
            {
                 bounds = _bounds.Value;
            }

            // Pivot (anchor) location of the image.
            if (_pivot.HasValue)
            {
                pivot = _pivot.Value;
            }

            // Return the created sprite.
            return Sprite.Create(image, bounds, pivot);
        }

        #endregion

    }

    #endregion

}
