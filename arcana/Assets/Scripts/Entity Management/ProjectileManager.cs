using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Entities
{
    class ProjectileManager : MonoBehaviour
    {

        private bool fire1 = false;


        public List<Projectile> projectiles = new List<Projectile>();


        private void createProjectile()
        {

            Projectile proj = new Projectile(0, 0, 0);
            //proj.m_position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            proj.m_mass = 10.0f;

           

            this.projectiles.Add(proj);

        }

        public void fireProjectile(bool fire1)
        {
            this.fire1 = fire1;

            if (fire1)
            {

                createProjectile();

            }

           
        }
    }
}
