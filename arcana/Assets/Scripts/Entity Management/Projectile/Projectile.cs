﻿/************************************************
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
        public float m_maxSpeed = 5.0f;
        public float m_initialSpeed = 1.0f;

        //initial velocity to be used in testing
        //private Vector3 initalVelocity = new Vector3(0.0f, -1.0f, 0.0f);

        // Reference to projectile Launcher
        //public GameObject projectile_go;

        private Rigidbody2D proj_rb;

        /// <summary>
        /// Projectile Constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="force">Force to be applied to the projectil||Direction of travel</param>
        /// <param name="position"></param>
        public void init_projectile(float x, float y, Vector3 force)
        {

            //instantiates the projectile_go and adds it to the scene
            //GameObject projectile_go = UnityEngine.Resources.Load("FireBall") as GameObject;
            //Instantiate(projectile_go, new Vector3(x+1,y+1,0), Quaternion.identity);

            //proj_rb = this.GetComponent<Rigidbody2D>();

            float speed = 1.0f;
            float proj_force = 1000f;
            //* speed * proj_force
            //sets force for now UNUSED
            //proj_rb.AddForce(new Vector2(1000f, 0 ));

        }

        /// <summary>
        /// Base Constructor
        /// Use to set specific ammounts
        /// </summary>
        void Start()
        {

            //sets velocity
            //m_velocity = new Vector3(m_direction.x, m_direction.y, m_direction.z);

            //sets mass
            m_mass = 1.0f;
            
            //ApplyForce(initalVelocity);

        }

        /// <summary>
        /// Update the projectile. Called once per frame
        /// </summary>
        void Update()
        {

            //Updates the velocity
            //this.UpdatePostition();
            //proj_rb.velocity = Vector2.right * 1.0f * 100000f;
            //Updates the position
            //this.SetTransform();

        }

        void OnCollisionStay2D(Collision2D coll)
        {
            if (coll.transform.name != "Wizzard1(Clone)" && coll.transform.name != "FireBall(Clone)") 
            {
                //Destroy(gameObject);
            }
        }

        public void updatePos()
        {
            //proj_rb.velocity = Vector2.right * 1.0f * 100000f;

            //if (1.0f * proj_rb.velocity.x < m_maxSpeed)
            //{

            //    proj_rb.velocity = Vector2.right * 1.0f * 100000f;

            //}

            //if (Mathf.Abs(proj_rb.velocity.x) > m_maxSpeed)
            //{
            //    proj_rb.velocity = new Vector2(Mathf.Sign(proj_rb.velocity.x) * m_maxSpeed, proj_rb.velocity.y);
            //}
        }

        /// <summary>
        /// Updates the velocity of the projectile
        /// </summary>
        void UpdatePostition()
        {

            //applys a force to get acceleration
            ApplyForce(new Vector3(1.0f,1.0f,0.0f));

            //divides final acceleration by mass
            Vector3 scaleAcceleration = m_acceleration / m_mass;

            //updates velocity based upon acceleration
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
            if(m_acceleration == Vector3.zero)
            {

                m_acceleration = p_force * 5;

            }
            //else, add the force to the acceleration
            else
            {

                m_acceleration += p_force * 5;

            }

        }

        
    }

}