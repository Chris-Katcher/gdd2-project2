/************************************************
 * CollisionHandler.cs
 * 
 * This file contains implementation for the CollisionHandler class.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana.Entities;
using UnityEngine;

namespace Arcana.Physics
{

	/// <summary>
	/// Resolves collisions between entities.
	/// </summary>
	public class CollisionHandler : MonoBehaviour
	{
		// refrences to interacting objects;

		public List<List<GameObject>> allProj;
		GameObject SC_Go;
		SystemController sc;

		public ProjectileManager projectileManager;
		
		public float SimpleCollisionDistance = 1.0f;

		public bool isWater;
		public bool isFire;
		public bool isGrass;
		public bool isPlayer;

		// Other important varibles
		Vector3 offScreen;

		private void Start()
		{
			SC_Go = GameObject.Find("SystemControllerGO");
			sc = SC_Go.GetComponent<SystemController>();
			projectileManager = sc.m_projectile;
			allProj = projectileManager.allProj;

			offScreen = new Vector3(1000, 1000, 1000);
		}

		private void Update()
		{
			if (gameObject.tag == "Projectile")
			{
				CheckProjectileCollision();
			}
			else if (gameObject.tag == "Player")
			{
				
			}
			else
			{
				Debug.Log("Tag not recgonized or not assigned. Make sure object is tagged as 'Projectile' or 'Player' in the inspector");
			}
			
		}

		private bool DetectCollision(GameObject primiary, GameObject secondary)
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

		private void CheckProjectileCollision()
		{
			for (int bigList = 0; bigList <= allProj.Count - 1; bigList++)
			{
				for (int indivProj = 0; indivProj <= allProj[bigList].Count - 1; indivProj++)
				{
					// only runs logic if not the same projectile and each is active.
					if (gameObject != allProj[bigList][indivProj] && gameObject.activeSelf)
					{
						if (DetectCollision(gameObject, allProj[bigList][indivProj]))
						{
							// Fire
							if (bigList == 0)
							{
								if (isFire)
								{
									DestroyBoth(gameObject, allProj[bigList][indivProj]);
								}
								else if (isWater)
								{
									DestroyOne(allProj[bigList][indivProj]);
								}
								else if (isGrass)
								{
									DestroyOne(gameObject);
								}
							}

							// Water
							if (bigList == 1)
							{
								if (isFire)
								{
									DestroyOne(gameObject);
								}
								else if (isWater)
								{
									DestroyBoth(gameObject, allProj[bigList][indivProj]);
								}
								else if (isGrass)
								{
									DestroyOne(allProj[bigList][indivProj]);
								}
							}

							// Earth
							if (bigList == 2)
							{
								if (isFire)
								{
									DestroyOne(allProj[bigList][indivProj]);
								}
								else if (isWater)
								{
									DestroyOne(gameObject);
								}
								else if (isGrass)
								{
									DestroyBoth(gameObject, allProj[bigList][indivProj]);
								}
							}

						}
					}
				}
			}
		}

		private void CheckPlayerCollision()
		{

		}

		// Moves Primiary to location way off screen
		private void DestroyOne(GameObject primiary)
		{
			primiary.transform.position = offScreen;
			primiary.SetActive(false);
		}


		private void DestroyBoth(GameObject primiary, GameObject secondary)
		{
			primiary.transform.position = offScreen;
			primiary.SetActive(false);
			secondary.transform.position = offScreen;
			secondary.SetActive(false);
		}


		// Used to cause a player to lose.
		private void SetLoseState()
		{
			//TODO: Set state to lose
		}


	}


}
