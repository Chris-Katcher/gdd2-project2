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
		/// <param name="type">The element of projectile to make 'F for fire, 'W' for water, 'G' Grass</param>
        private void createProjectile(float x, float y, Vector3 force, int type)
        {

            //creates the projectile proj
            Projectile proj = new Projectile(x, y, force, type);   

            //adds it to list of projectiles
            this.projectiles.Add(proj);

        }

        /// <summary>
        /// Fires a projectile
        /// </summary>
        /// <param name="fire1">Bool == Whether or no the fire button has been pressed</param>
        /// <param name="position">Position to be created at</param>
        public void fireProjectile1(bool fire1, Vector3 position)
        {

            //if fire has been pressed, create a projectile
            if (fire1 == true)
            {

                //creates a projectile of type 'Fire'
                createProjectile(position.x,position.y,new Vector3(4.0f,0.0f,0), 100);

            }

           
        }

		public void fireProjectile2(bool fire2, Vector3 position)
		{

			//if fire has been pressed, create a projectile
			if (fire2 == true)
			{

				//creates a projectile of type 'Water'
				createProjectile(position.x, position.y, new Vector3(4.0f, 0.0f, 0), 10);

			}


		}
		public void fireProjectile3(bool fire3, Vector3 position)
		{

			//if fire has been pressed, create a projectile
			if (fire3 == true)
			{

				//creates a projectile of type 'Grass'
				createProjectile(position.x, position.y, new Vector3(4.0f, 0.0f, 0), 1);

			}


		}

	}
}
