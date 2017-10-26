using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Entities
{
    class ProjectileManager : MonoBehaviour
    {

        private int spellValue = 0;
        private int inputCount = 0;

        private enum curCast
        {

            noSpell = 0,
            oneSpell = 1,
            twoSpell = 2,
            fullSpell = 3

        }
        private curCast current;

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

            Projectile proj = new Projectile(x, y, force, type);
   
            //adds it to list of projectiles
            this.projectiles.Add(proj);

        }

        /// <summary>
        /// Fires a projectile
        /// </summary>
        /// <param name="fire1">Bool == Whether or no the fire button has been pressed</param>
        /// <param name="position">Position to be created at</param>
        public void fireProjectile(bool fire1, bool fire2, bool fire3, bool rightTrigger, Vector3 position)
        {

            determineCast();

            //if rigth trigger has been pressed and three spells have been inputed, then create a projectile and reset variables
            if (rightTrigger == true && current == curCast.fullSpell)
            {

                createProjectile(position.x, position.y, new Vector3(4.0f, 0.0f, 0.0f), spellValue);
                spellValue = 0;
                inputCount = 0;
                current = curCast.noSpell;

            }
            else if (fire1 == true && inputCount < 3)
            {

                //creates a projectile of type 'Fire'
                inputCount += 1;
                spellValue += 100;

            }
            else if(fire2 == true && inputCount < 3)
            {

                inputCount += 1;
                spellValue += 10;

            }
            else if(fire3 == true && inputCount < 3)
            {

                inputCount += 1;
                spellValue += 1;

            }
           
        }
        public void determineCast()
        {

            switch (inputCount)
            {

                case 1:
                    current = curCast.oneSpell;
                    break;
                case 2:
                    current = curCast.twoSpell;
                    break;
                case 3:
                    current = curCast.fullSpell;
                    break;

            }


        }

        public void resetSpell()
        {

            inputCount = 0;
            spellValue = 0;
            current = curCast.noSpell;

        }
		/*public void fireProjectile(bool fire2, Vector3 position)
		{

			//if fire has been pressed, create a projectile
			if (fire2 == true)
			{

				//creates a projectile of type 'Water'
				createProjectile(position.x, position.y, new Vector3(4.0f, 0.0f, 0), 10);

			}


		}
		public void fireProjectile(bool fire3, Vector3 position)
		{

			//if fire has been pressed, create a projectile
			if (fire3 == true)
			{

				//creates a projectile of type 'Grass'
				createProjectile(position.x, position.y, new Vector3(4.0f, 0.0f, 0), 1);

			}


		}*/

	}
}
