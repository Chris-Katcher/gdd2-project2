using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Entities
{
    class ProjectileManager : MonoBehaviour
    {

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
        private void createProjectile(float x, float y, Vector2 force)
        {

            //creates the projectile proj
            projectile_go = UnityEngine.Resources.Load("FireBall") as GameObject;


            projectile_go = Instantiate(projectile_go, new Vector3(x , y , 0), Quaternion.identity);
            projectile_rb = projectile_go.GetComponent<Rigidbody2D>();
            projectile_rb.AddForce(force);
            proj = projectile_go.GetComponent<Projectile>();
            //proj.init_projectile(x, y, force);
            //adds it to list of projectiles
            this.projectiles.Add(projectile_go);

        }

        /// <summary>
        /// Fires a projectile
        /// </summary>
        /// <param name="fire1">Bool == Whether or no the fire button has been pressed</param>
        /// <param name="position">Position to be created at</param>
        public void fireProjectile(bool fire1, Vector3 position, bool facingRight)
        {

            //if fire has been pressed, create a projectile
            if (fire1 == true)
            {
                Vector2 force;
                if (facingRight)
                {
                    force = Vector2.right * 1.0f * 750f;
                    force.y += UnityEngine.Random.Range(-300, 300);
                } else
                {
                    force = Vector2.left * 1.0f * 750f;
                    force.y += UnityEngine.Random.Range(-300, 300);
                }
                createProjectile(position.x, position.y, force);
                //creates a projectile
            }
        }

        public void updateProjectiles()
        {
            
        }
    }
}
