using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats_Rotator : MonoBehaviour {

    //Lea Kohl

        // put this Script at any given Object to make it rotate around its on axis
    void Update()
    {
        transform.Rotate(Vector3.up, 10 * Time.deltaTime, Space.Self);
    }
}
