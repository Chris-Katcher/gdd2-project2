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

		// public accesable version of current enumeration
		public Spell spell;

		private enum curCast
		{

			noSpell = 0,
			oneSpell = 1,
			twoSpell = 2,
			fullSpell = 3

		}
		private curCast current;

		public List<GameObject> projectiles = new List<GameObject>();
        public Projectile proj;
        public GameObject projectile_go;
        public Rigidbody2D projectile_rb;

		/// <summary>
		/// Creates a new projectile
		/// </summary>
		/// <param name="x">x position</param>
		/// <param name="y">y position</param>
		/// <param name="force">the force to be exerted on the projectile||its travel direction</param>
		/// <param name="position">the starting position of the projectile</param>
		private void createProjectile(float x, float y, Vector3 force, int type)
		{

			//Projectile proj = new Projectile();
			init_projectile(x, y, force, type);

			

		}

	
		/// <summary>
		/// Fires a projectile
		/// </summary>
		/// <param name="fire1">Bool == Whether or no the fire button has been pressed</param>
		/// <param name="position">Position to be created at</param>
		public void fireProjectile(bool fire1, bool fire2, bool fire3, bool rightTrigger, Vector3 position, bool facingRight)
        {

			determineCast();

			//if rigth trigger has been pressed and three spells have been inputed, then create a projectile and reset variables
			if (rightTrigger == true && current == curCast.fullSpell)
			{

				createProjectile(position.x, position.y, facingRight ? new Vector3(4.0f, 0.0f, 0.0f) : new Vector3(-4.0f, 0.0f, 0.0f), spellValue);
				spellValue = 0;
				inputCount = 0;
				current = curCast.noSpell;

			}

			//if fire has been pressed, create a projectile
			else if (fire1 == true && inputCount < 3)
			{

				//creates a projectile of type 'Fire'
				inputCount += 1;
				spellValue += 100;

			}
			else if (fire2 == true && inputCount < 3)
			{

				inputCount += 1;
				spellValue += 10;

			}
			else if (fire3 == true && inputCount < 3)
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

		public enum Spell
		{

			comboS,//one of each
			fireW,//2 FIRE AND 1 WATER
			fireE,//2 FIRE AND 1 EARTH
			fireP,//3 FIRE
			waterF,//2 WATER AND 1 FIRE
			waterE,//2 WATER AND 1 EARTH
			waterP,//3 WATER
			earthF,//2 EARTH AND 1 FIRE
			earthW,//2 EARTH AND 1 WATER
			earthP//3 EARTH

		};

		/// <summary>
		/// Projectile Constructor
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="force">Force to be applied to the projectil||Direction of travel</param>
		/// <param name="position"></param>
		public void init_projectile(float x, float y, Vector3 force, int type)
		{

			//instantiates the projectile_go and adds it to the scene

			//switch statement that passes in the type and then sets the correct enumeration to use
			switch (type)
			{
				case 111:
					spell = Spell.comboS;
					break;
				case 300:
					spell = Spell.fireP;
					break;
				case 210:
					spell = Spell.fireW;
					break;
				case 201:
					spell = Spell.fireE;
					break;
				case 30:
					spell = Spell.waterP;
					break;
				case 120:
					spell = Spell.waterF;
					break;
				case 21:
					spell = Spell.waterE;
					break;
				case 3:
					spell = Spell.earthP;
					break;
				case 12:
					spell = Spell.earthW;
					break;
				case 102:
					spell = Spell.earthF;
					break;
				default:
					spell = Spell.fireW;
					break;

			}

			//PLACEHOLDER CODE: Takes the curent enumeration and creates the apporopriate projectile. Only 3 atm
			//combo projectile
			if (this.spell == Spell.comboS)
			{
				projectile_go = UnityEngine.Resources.Load("Fire") as GameObject;
			}

			//fire projectiles
			else if (this.spell == Spell.fireE)
			{
				projectile_go = UnityEngine.Resources.Load("Fire") as GameObject;
			}
			else if (this.spell == Spell.fireW)
			{
				projectile_go = UnityEngine.Resources.Load("Fire") as GameObject;
			}
			else if (this.spell == Spell.fireP)
			{
				projectile_go = UnityEngine.Resources.Load("Fire") as GameObject;
			}

			//water projectiles
			else if (this.spell == Spell.waterF)
			{
				projectile_go = UnityEngine.Resources.Load("Water") as GameObject;
			}
			else if (this.spell == Spell.waterE)
			{
				projectile_go = UnityEngine.Resources.Load("Water") as GameObject;
			}
			else if (this.spell == Spell.waterP)
			{
				projectile_go = UnityEngine.Resources.Load("Water") as GameObject;
			}

			//earth projectiles
			else if (this.spell == Spell.earthF)
			{
				projectile_go = UnityEngine.Resources.Load("Grass") as GameObject;
			}
			else if (this.spell == Spell.earthW)
			{
				projectile_go = UnityEngine.Resources.Load("Grass") as GameObject;
			}
			else if (this.spell == Spell.earthP)
			{
				projectile_go = UnityEngine.Resources.Load("Grass") as GameObject;
			}
			// else debug log that incorrect char was set
			else
			{
				Debug.Log("Type '" + type + "' not recgonized. Please check the projectile manager script");
				return;
			}

			//adds it to list of projectiles
			projectiles.Add(Instantiate(projectile_go, new Vector3(x, y, 0), Quaternion.identity));

			projectiles[projectiles.Count - 1].GetComponent<Projectile>().m_direction = force;


		}


		public void updateProjectiles()
        {
            foreach(GameObject p in projectiles)
            {
                Rigidbody2D rb = p.GetComponent<Rigidbody2D>();
                float y = (float)Math.Cos(Time.frameCount );
                //rb.AddForce(Vector2.up * y * 150f);
            }
        }
    }


}
