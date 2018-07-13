using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainControllerVR : MonoBehaviour {


    public Camera m_helpCamera;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(m_helpCamera.transform.position, Vector3.forward, out hit);

    }
}
