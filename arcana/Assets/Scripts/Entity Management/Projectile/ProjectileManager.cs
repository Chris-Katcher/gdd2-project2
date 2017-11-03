using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Entities
{
    public class ProjectileManager : MonoBehaviour
    {

		public List<List<GameObject>> allProj = new List<List<GameObject>>();

		public List<GameObject> f_projectiles = new List<GameObject>();
		public List<GameObject> w_projectiles = new List<GameObject>();
		public List<GameObject> e_projectiles = new List<GameObject>();
		public Projectile proj;
        public GameObject projectile_go;
        public Rigidbody2D projectile_rb;

		public float SimpleCollisionDistance = 1.0f;

		/// <summary>
		/// Fills All projectile list with the individual projectile lists
		/// ORDER IS IMPORTANT. DO NOT MESS WITH ORDER.
		/// </summary>
		private void Start()
		{
			allProj.Add(f_projectiles);
			allProj.Add(w_projectiles);
			allProj.Add(e_projectiles);
		}

		/// <summary>
		/// Creates a new projectile
		/// </summary>
		/// <param name="x">x position</param>
		/// <param name="y">y position</param>
		/// <param name="force">the force to be exerted on the projectile||its travel direction</param>
		/// <param name="position">the starting position of the projectile</param>
		private void createProjectile(float x, float y, Vector3 force, char type)
		{

			//Projectile proj = new Projectile();
			init_projectile(x, y, force, type);

			
		}

	
		/// <summary>
		/// Fires a projectile
		/// </summary>
		/// <param name="fire1">Bool == Whether or no the fire button has been pressed</param>
		/// <param name="position">Position to be created at</param>
		public void fireProjectile(bool fire1, bool fire2, bool fire3, Vector3 position, bool facingRight)
        {
			//if fire has been pressed, create a projectile
			if (fire1 == true)
			{
				//creates a projectile of type 'Fire'
				createProjectile(position.x, position.y, facingRight ? new Vector3(4.0f, 0.0f, 0.0f) : new Vector3(-4.0f, 0.0f, 0.0f), 'F');

			}
			else if (fire2 == true)
			{
				createProjectile(position.x, position.y, facingRight ? new Vector3(4.0f, 0.0f, 0.0f) : new Vector3(-4.0f, 0.0f, 0.0f), 'W');
			}
			else if (fire3 == true)
			{
				createProjectile(position.x, position.y, facingRight ? new Vector3(4.0f, 0.0f, 0.0f) : new Vector3(-4.0f, 0.0f, 0.0f), 'E');
			}

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

		/// <summary>
		/// Projectile Constructor
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="force">Force to be applied to the projectil||Direction of travel</param>
		/// <param name="position"></param>
		public void init_projectile(float x, float y, Vector3 force, char type)
		{

			if (type == 'F')
			{
				projectile_go = UnityEngine.Resources.Load("Fire") as GameObject;
				//adds it to list of projectiles
				f_projectiles.Add(Instantiate(projectile_go, new Vector3(x, y, 0), Quaternion.identity));

				f_projectiles[f_projectiles.Count - 1].GetComponent<Projectile>().m_direction = force;
			}

			//water projectiles
			else if (type == 'W')
			{
				projectile_go = UnityEngine.Resources.Load("Water") as GameObject;
				//adds it to list of projectiles
				w_projectiles.Add(Instantiate(projectile_go, new Vector3(x, y, 0), Quaternion.identity));

				w_projectiles[w_projectiles.Count - 1].GetComponent<Projectile>().m_direction = force;
			}
			

			//earth projectiles
			else if (type == 'E')
			{
				projectile_go = UnityEngine.Resources.Load("Grass") as GameObject;
				//adds it to list of projectiles
				e_projectiles.Add(Instantiate(projectile_go, new Vector3(x, y, 0), Quaternion.identity));

				e_projectiles[e_projectiles.Count - 1].GetComponent<Projectile>().m_direction = force;
			}
			// else debug log that incorrect char was set
			else
			{
				Debug.Log("Type '" + type + "' not recgonized. Please check the projectile manager script");
				return;
			}
		}


		public void updateProjectiles()
        {
			
			// cycle through lists and save which iteration
			for (int i = 0; i < allProj.Count - 1; i++)
			{
				
				// cycle through projectiles and save which interation
				for (int j = 0; j < allProj[i].Count - 1; j++)
				{
					
					// if in first list (fire)
					if (i == 0)
					{
						checkCollisions('F', allProj[i][j]);
					}
					// if in second list (water)
					else if (i == 1)
					{
						checkCollisions('W', allProj[i][j]);
					}
					// if in third list (earth)
					else if (i == 2)
					{
						checkCollisions('E', allProj[i][j]);
					}
					else
					{
						Debug.Log("Trying to access projectile list out of range");
					}
				}
            }
			
		}

		private void checkCollisions(char type, GameObject primiary)
		{
			
			// foreach active object
			for (int i = 0; i < allProj.Count - 1; i++)
			{
				for (int j = 0; j < allProj[i].Count - 1; j++)
				{
					Debug.Log("Primiary: " + primiary.name + "Secondary: " + allProj[i][j].name);
					// Only runs logic if the objects are colliding, both are active, and the objects aren't the same.
					if (IsColliding(primiary, allProj[i][j]) && allProj[i][j].activeSelf && primiary != allProj[i][j])
					{
						Debug.Log("Collision on active");
						// if fire
						if (i == 0)
						{
							if (type == 'F')
							{
								DestroyBoth(primiary, allProj[i][j]);
							}
							else if (type == 'W')
							{
								DestroySelf(primiary, allProj[i][j]);
							}
							else if (type == 'E')
							{
								DestroyOther(primiary, allProj[i][j]);
							}
						}
						// if water
						else if (i == 1)
						{

							if (type == 'F')
							{
								DestroyOther(primiary, allProj[i][j]);
							}
							else if (type == 'W')
							{
								DestroyBoth(primiary, allProj[i][j]);
							}
							else if (type == 'E')
							{
								DestroySelf(primiary, allProj[i][j]);
							}
						}
						// if earth
						else if (i == 2)
						{

							if (type == 'F')
							{
								DestroySelf(primiary, allProj[i][j]);
							}
							else if (type == 'W')
							{
								DestroyOther(primiary, allProj[i][j]);
							}
							else if (type == 'E')
							{
								DestroyBoth(primiary, allProj[i][j]);
							}
						}
						else
						{
							Debug.Log("Trying to access projectile list out of range");
						}
					}
				}
			}
		}

		private bool IsColliding(GameObject primiary, GameObject secondary)
		{
			if (primiary.transform.position.x - SimpleCollisionDistance < secondary.transform.position.x + SimpleCollisionDistance && 
				primiary.transform.position.x + SimpleCollisionDistance > secondary.transform.position.x - SimpleCollisionDistance &&
				primiary.transform.position.y + SimpleCollisionDistance > secondary.transform.position.y - SimpleCollisionDistance &&
				primiary.transform.position.y - SimpleCollisionDistance < secondary.transform.position.y + SimpleCollisionDistance)
			{
				//Debug.Log("Collision");
				return true;
			}
			else return false;
		}

		private void DestroySelf(GameObject primiary, GameObject secondary)
		{
			primiary.SetActive(false);
		}
		private void DestroyOther(GameObject primiary, GameObject secondary)
		{
			secondary.SetActive(false);
		}
		private void DestroyBoth(GameObject primiary, GameObject secondary)
		{
			primiary.SetActive(false);
			secondary.SetActive(false);
		}
	}


}
