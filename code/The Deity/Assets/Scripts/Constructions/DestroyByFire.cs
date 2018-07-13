using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByFire : MonoBehaviour {

    //Lea Kohl
    public Burnable m_Burn;

    public float m_BurnTime;
    

	// Use this for initialization
	void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {
		
        if (m_Burn.isOnFire == true)
        {
            Destroy(gameObject, m_BurnTime);
        }
	}
}
