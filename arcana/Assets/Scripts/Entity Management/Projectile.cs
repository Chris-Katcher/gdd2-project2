/************************************************
 * ProjectileMotion.cs
 * 
 * This file contains motion for all projectile classes.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Entities
{
    /// <summary>
    /// Handles base movement for projectiles
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        // Members for motion

        //Vectors
        public Vector3 m_position;//position
        public Vector3 m_direction;//direction to travel in
        public Vector3 m_acceleration;//proj acceleration
        public Vector3 m_velocity;//proj velocity

        //floats
        public float m_mass;
        public float m_maxSpeed;
        public float m_initialSpeed;

        //enum
        public enum Spell
        {

            fireW = 12,
            fireE = 102,
            fireP = 3,
            waterF = 21,
            waterE = 120,
            waterP = 30,
            earthF = 201,
            earthW = 210,
            earthP = 300

        };

        public Spell spell;

        //initial velocity to be used in testing
        private Vector3 initalVelocity;

        // Reference to projectile Launcher
        public GameObject projectile_go;

        /// <summary>
        /// Projectile Constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="force">Force to be applied to the projectil||Direction of travel</param>
        /// <param name="position"></param>
		/// <param name="type">Char for element 'F' for Fire, 'W' for water, 'G' for grass</param>
        public Projectile(float x, float y, Vector3 force, int type)
        {

            //instantiates the projectile_go and adds it to the scene
            // if char = F, make fire

			if (type == 100)
			{
				projectile_go = UnityEngine.Resources.Load("Fire") as GameObject;
			}
			// if char = W, make water
			else if (type == 10)
			{
				projectile_go = UnityEngine.Resources.Load("Water") as GameObject;
			}
			// if char = G, make grass
			else if (type == 1)
			{
				projectile_go = UnityEngine.Resources.Load("Grass") as GameObject;
			}
			// else debug log that incorrect char was set
			else
			{
				Debug.Log("Type '" + type + "' not recgonized. Please check the projectile manager script");
				return;
			}
       
            Instantiate(projectile_go, new Vector3(x,y,0), Quaternion.identity);

            //sets force for now UNUSED
            m_direction += force;

        }

        /// <summary>
        /// Base Constructor
        /// Use to set specific ammounts
        /// </summary>
        void Start()
        {

            //sets velocity
            m_velocity = new Vector3(m_direction.x, m_direction.y, m_direction.z);
			
			// Sets initial velocity
			initalVelocity = new Vector3(m_initialSpeed, m_initialSpeed, 0.0f);

			ApplyForce(initalVelocity);

        }

        /// <summary>
        /// Update the projectile. Called once per frame
        /// </summary>
        void Update()
        {

            //Updates the velocity
            this.UpdatePostition();

            //Updates the position
            this.SetTransform();

        }

        /// <summary>
        /// Updates the velocity of the projectile
        /// </summary>
        void UpdatePostition()
        {

            //divides final acceleration by mass
            Vector3 scaleAcceleration = m_acceleration / m_mass;


			m_velocity += scaleAcceleration * Time.deltaTime;


            //resets acceleration to 0
            m_acceleration = Vector3.zero;

        }

        

        /// <summary>
        /// Returns the Vector2 direction of a passed in angle || Can be placed into a helper script
        /// </summary>
        /// <param name="angle">The degree angle passed into the function</param>
        /// <returns></returns>
        Vector2 AngleToHeading(float angle)
        {

            //converts the passed in angle to radians
            float radianAngle = (float)(angle * Math.PI / 180.0);

            Vector2 dirVector = new Vector2((float)Math.Cos(radianAngle), (float)Math.Sin(radianAngle));

            //turns into unit vector
            dirVector.Normalize();

            return dirVector;

        }

        /// <summary>
        /// Normalizes a passed in Vector2 || Can be placed into a helper script
        /// </summary>
        /// <param name="vect">The vector to be normalized</param>
        /// <returns></returns>
        Vector2 normalizeVector(Vector3 vect)
        {

            //normalizes the vector to be a unit length of one
            vect.Normalize();

            return vect;

        }

        /// <summary>
        /// Updates the position based upon the velocity
        /// </summary>
        void SetTransform()
        {

            transform.position += (m_velocity * Time.deltaTime);

        }

        /// <summary>
        /// Updates the acceleration
        /// </summary>
        /// <param name="p_force">The force to apply</param>
        public void ApplyForce(Vector3 p_force)
        {

			//if acceleration == 0, then set equal to the force. Avoids nulls.
			if (m_acceleration == Vector3.zero)
            {

                m_acceleration = p_force * 5;

            }
            //else, add the force to the acceleration
            else
            {
					m_acceleration += p_force * 5;

            }

        }

		/// <summary>
		/// Stops all projectile movement and disables collider.
		/// </summary>
		public void Stop()
		{
			m_velocity = Vector3.zero;
			m_mass = 100;
			GetComponent<BoxCollider2D>().enabled = !enabled;
		}

        
    }

}
