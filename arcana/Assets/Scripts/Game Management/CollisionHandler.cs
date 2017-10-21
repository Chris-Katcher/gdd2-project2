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
using UnityEngine;

namespace Arcana.Physics
{
	
    /// <summary>
    /// Resolves collisions between entities.
    /// </summary>
    public class CollisionHandler: MonoBehaviour
	{
		// refrences to interacting objects;
		public GameObject primiary;
		public GameObject secondary;
		BoxCollider2D primiaryCollider;
		BoxCollider2D secondaryCollider;
		// TODO: primiary updates to the instance of the parent, but secondary does not.
		// This class needs refrences to the projectile manager
		// The projectile manager needs active lists of projectiles of each 'element'
		// This class then needs to be updated to reflect the checking of each list of projectiles, as opposed to individual projectiles
		// If you need my help, I'll be avalible sporatically throughout the day. 
		// Facebook messenger is the best way to contact me.


		// possible behaviors
		[Header("Behaviors")]

		public bool destroyPrimiary;
		public bool destroySecondary;
		public bool setLoseState;

		// Other important varibles
		Vector3 offScreen;

		private void Start()
		{
			offScreen = new Vector3(100, 100, 100);
			primiaryCollider = primiary.GetComponent<BoxCollider2D>();
			secondaryCollider = secondary.GetComponent<BoxCollider2D>();
		}

		// Every frame, sees if there is a collision, and if so, calls whatever methods have been
		// 'selected' in the inspector.
		private void Update()
		{
			if (DetectCollision())
			{
				if (destroyPrimiary)
				{
					DestroyPrimiary();
				}
				if (destroySecondary)
				{
					DestroySecondary();
				}
				if (setLoseState)
				{
					SetLoseState();
				}
				
			}
		}

		private bool DetectCollision()
		{
			if (primiaryCollider.IsTouching(secondaryCollider))
			{
				Debug.Log("Collision!");
				return true;
			}
			else return false;
		}

		// Moves Primiary to location way off screen
		private void DestroyPrimiary()
		{
			primiary.transform.position = offScreen;
			// TODO: Call Stop
		}

		// Moves Secondary to location way off screen
		private void DestroySecondary()
		{
			secondary.transform.position = offScreen;
			// TODO: Call Stop
		}

	
		// Used to cause a player to lose.
		private void SetLoseState()
		{
			//TODO: Set state to lose
		}


	}

	
}
