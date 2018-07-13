using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseFire : MonoBehaviour {

    //Lea Kohl
    public Burnable m_Burn;

    public float m_BurnTime;
    public GameObject m_BurningHouse;


    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (m_Burn != null)
        {
            if (m_Burn.isOnFire == true)
            {
                Instantiate(m_BurningHouse, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
