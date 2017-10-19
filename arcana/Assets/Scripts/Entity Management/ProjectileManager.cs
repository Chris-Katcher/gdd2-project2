using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Entities
{
    class ProjectileManager : MonoBehaviour
    {

        public List<Projectile> projectiles = new List<Projectile>();

        /// <summary>
        /// Creates a new projectile
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <param name="force">the force to be exerted on the projectile||its travel direction</param>
        /// <param name="position">the starting position of the projectile</param>
        private void createProjectile(float x, float y, Vector3 force)
        {

            //creates the projectile proj
            Projectile proj = new Projectile(x, y, force);   

            //adds it to list of projectiles
            this.projectiles.Add(proj);

        }

        /// <summary>
        /// Fires a projectile
        /// </summary>
        /// <param name="fire1">Bool == Whether or no the fire button has been pressed</param>
        /// <param name="position">Position to be created at</param>
        public void fireProjectile(bool fire1, Vector3 position)
        {

            //if fire has been pressed, create a projectile
            if (fire1 == true)
            {

                //creates a projectile
                createProjectile(position.x,position.y,new Vector3(4.0f,0.0f,0));

            }

           
        }
    }
}
