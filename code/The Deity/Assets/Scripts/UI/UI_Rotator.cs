using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Rotator : MonoBehaviour {

	//Lea Kohl
	void Update () {
        transform.Rotate(Vector3.up,10 * Time.deltaTime, Space.Self);
    }
}
