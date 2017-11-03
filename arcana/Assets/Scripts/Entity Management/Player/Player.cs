using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcana;
using Arcana.InputManagement;
using Arcana.UI;
using Arcana.Entities;

public class Player : MonoBehaviour {

    ProjectileManager m_projectile;
	SystemController m_system;
	GameObject m_systemGO;

    // Use this for initialization
    void Start ()
	{
		m_systemGO = GameObject.Find("SystemControllerGO");
		m_system = m_systemGO.GetComponent<SystemController>();
        //gameObject.AddComponent<ProjectileManager>();
        m_projectile = m_system.GetComponent<ProjectileManager>();

    }
	
	// Update is called once per frame
	void Update () {
		//m_projectile.updateProjectiles();

	}

    private void FixedUpdate()
    {
        //m_projectile.updateProjectiles();
    }


    /// <summary>
    /// fires a projectile based upon a projectile
    /// </summary>
    /// <param name="fire">whether or not the fire button has been pressed</param>
    public void fireProjPlayer(bool fire1, bool fire2, bool fire3, Vector3 pos, bool facingRight)
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

		if (fire1)
		{
			m_projectile.fireProjectile(fire1, fire2, fire3, pos, facingRight);
		}
		else if (fire2)
		{
			m_projectile.fireProjectile(fire1, fire2, fire3, pos, facingRight);
		}
		else if (fire3)
		{
			m_projectile.fireProjectile(fire1, fire2, fire3, pos, facingRight);
		}

	}

}


