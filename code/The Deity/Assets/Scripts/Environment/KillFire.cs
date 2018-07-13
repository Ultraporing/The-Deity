using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFire : MonoBehaviour {

    //Lea Kohl
    //Script to extinguish fire 
    public Fire m_FireScript;
    public HouseDestruction Fires;
    public Burnable m_Burn;
    public GameObject m_House;
    public float m_TimeLeft = 3;
    public ManagePoP m_ManagePoP;
    public bool m_CanRain = true;
    
    
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Tree") || collision.gameObject.name == "BurningHouse" || collision.gameObject.name == "BurningHouse(Clone)")
        {
            m_FireScript = collision.gameObject.GetComponentInChildren<Fire>();
            Fires = collision.gameObject.GetComponent<HouseDestruction>();
            m_Burn = collision.gameObject.GetComponent<Burnable>();
            if (m_FireScript != null)
            {
                if (collision.gameObject.CompareTag("Tree"))
                {
                    Destroy(m_FireScript.gameObject);
                }
            }
            else if (Fires != null)
            {
                m_Burn.isOnFire = false;
                Fires.m_Fires.SetActive(false);
                Fires.m_StartDestruction = false;
                Fires.m_SoundFire.Stop();
            }
        }
    }
    
    void Update()
    {
        //is balancing method (every 3 seconds 2 PoP Points are being removed
        m_TimeLeft -= Time.deltaTime;
        if (m_TimeLeft <= 0)
        {
            if (m_ManagePoP.m_PoP - 2 >= 0)
            {
                m_ManagePoP.m_PoP -= 2;
                m_CanRain = true;
            }
            else m_CanRain = false;
            m_TimeLeft = 5;
        }
    }
}
