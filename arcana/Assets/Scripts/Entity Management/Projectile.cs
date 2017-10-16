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
        public Vector3 m_position;
        public Vector3 m_direction;
        public Vector3 m_acceleration;
        public Vector3 m_velocity;

        public float m_mass;
        public float m_maxSpeed;
        public float m_initialSpeed;

        Vector3 initalVelocity;

        // Reference to projectile Launcher
        public GameObject Parent;

        // Use this for initialization
        void Start()
        {

            initalVelocity = new Vector3(m_initialSpeed, 0);
            ApplyForce(initalVelocity);

        }

        // Update is called once per frame
        void Update()
        {

            UpdatePostition();
            SetTransform();

        }

        // properly updates position of object
        void UpdatePostition()
        {

            m_position = gameObject.transform.position;

            m_velocity += m_acceleration * Time.deltaTime;

            m_position += m_velocity * Time.deltaTime;

            m_direction = m_velocity.normalized;

            m_acceleration = Vector3.zero;

        }

        /// <summary>
        /// Returns the Vector2 direction of a passed in angle || Can be placed into a helper script
        /// </summary>
        /// <param name="angle"></param>
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
        /// <param name="vect"></param>
        /// <returns></returns>
        Vector2 normalizeVector(Vector2 vect)
        {

            vect.Normalize();

            return vect;

        }

        void SetTransform()
        {

            GetComponent<Transform>().position += (m_velocity * Time.deltaTime);
            /*float posX = GetComponent<Transform>().position.x + (m_velocity.x * Time.deltaTime);
            float posY = GetComponent<Transform>().position.y + (m_velocity.y * Time.deltaTime);

            gameObject.transform.position.Set(posX, posY,0);*/

        }

        public void ApplyForce(Vector3 p_force)
        {

            m_acceleration += (p_force / m_mass);

        }
    }

}
