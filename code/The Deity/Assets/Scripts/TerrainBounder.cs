/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Constructions;
using Assets.Scripts.Environment.Planet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// Checks if the Player is out of bounds and teleports him back
/// </summary>
public class TerrainBounder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	/// <summary>
    /// If the player is out of bounds put him back into bounds. 
    /// If he falls through the floor port him to the bonfire
    /// </summary>
	void Update () {
        if (transform.position.x > 500)
            transform.position = new Vector3(498, transform.position.y, transform.position.z);
        else if (transform.position.x < 0)
            transform.position = new Vector3(2, transform.position.y, transform.position.z);

        if (transform.position.z > 500)
            transform.position = new Vector3(transform.position.x, transform.position.y, 498);
        else if (transform.position.z < 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, 2);

        if(transform.position.y < 100)
        {
            transform.position = PlanetDatalayer.Instance.GetManager<BuildingManager>().GetBuildingsOfType<Bonfire>()[0].transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        }
    }
}
