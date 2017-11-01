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


    /// <summary>
    /// fires a projectile based upon a projectile
    /// </summary>
    /// <param name="fire">whether or not the fire button ahs been pressed</param>
    public void fireProjPlayer(bool fire1, bool fire2, bool fire3)
    {
		// if else prevents players from mashing all buttons at once.
		if (fire1)
		{
			//passes in bool and player position
			m_projectile.fireProjectile1(fire1, transform.position);
		}
		else if (fire2)
		{
			m_projectile.fireProjectile2(fire2, transform.position);
		}
		else if (fire3)
		{
			m_projectile.fireProjectile3(fire3, transform.position);
		}

    }

	//TODO: add other projectiles 

}
