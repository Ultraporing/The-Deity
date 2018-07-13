/*
    Written by Tobias Lenz
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to be attached to the Fire object to set things on fire
/// </summary>
public class Fire : MonoBehaviour {

	/// <summary>
    /// Nothing todo
    /// </summary>
	void Start () {
		
	}

    /// <summary>
    /// Nothing todo
    /// </summary>
    void Update () {
		
	}

    /// <summary>
    /// Checks the colliding objects if they can burn and stops the fire from falling once it hits something
    /// </summary>
    /// <param name="collision">The object wich the Fire collided with</param>
    private void OnCollisionEnter(Collision collision)
    {
        Burnable bur = collision.gameObject.GetComponentInChildren<Burnable>();
        if (bur != null)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            bur.Burn(this);
        }
    }
}
