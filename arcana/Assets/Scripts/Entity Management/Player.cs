using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcana;
using Arcana.InputManagement;
using Arcana.UI;
using Arcana.Entities;

public class Player : MonoBehaviour {

    private ProjectileManager m_projectile;

    // Use this for initialization
    void Start () {

        gameObject.AddComponent<ProjectileManager>();
        m_projectile = gameObject.GetComponent<ProjectileManager>();

    }
	
	// Update is called once per frame
	void Update () {
       

    }

    private void FixedUpdate()
    {
        m_projectile.updateProjectiles();
    }


    /// <summary>
    /// fires a projectile based upon a projectile
    /// </summary>
    /// <param name="fire">whether or not the fire button has been pressed</param>
    public void fireProjPlayer(bool fire1, bool fire2, bool fire3, bool rightTrigger, Vector3 pos, bool facingRight)
    {
        if (facingRight)
        {
            pos.x += .5f;
            pos.y += .1f;
        } else
        {
            pos.x -= .5f;
            pos.y += .1f;
        }

		//passes in bool and player position
		// if else prevents players from mashing all buttons at once  

		//really janky mehtod to determine whihc button has been presed. NEEDS IMPROVEMENT
		if (rightTrigger == true)
		{

			m_projectile.fireProjectile(fire1, fire2, fire3, rightTrigger, pos, facingRight);

		}
		else if (fire1)
		{
			m_projectile.fireProjectile(fire1, fire2, fire3, rightTrigger, pos, facingRight);
		}
		else if (fire2)
		{
			m_projectile.fireProjectile(fire1, fire2, fire3, rightTrigger, pos, facingRight);
		}
		else if (fire3)
		{
			m_projectile.fireProjectile(fire1, fire2, fire3, rightTrigger, pos, facingRight);
		}

	}

}


