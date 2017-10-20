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
    /// <param name="fire">whether or not the fire button ahs been pressed</param>
    public void fireProjPlayer(bool fire, Vector3 pos)
    {

        //passes in bool and player position
        m_projectile.fireProjectile(fire, pos);

    }

}
