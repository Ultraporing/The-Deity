/*
    Written by Tobias Lenz
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates a GameObject in the given direction and speed
/// </summary>
public class GameobjectRotator : MonoBehaviour {

    public Vector3 m_RotationVector = Vector3.zero;

	// Rotate the GameObject
	void Update () {
        transform.Rotate(m_RotationVector * Time.deltaTime);
	}
}
