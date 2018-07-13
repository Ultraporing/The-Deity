using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePoP : MonoBehaviour {

    //Lea Kohl 
    //PoP is needed to perform miracles
    public int m_PoP;
    int m_MaxPoP;
    public float m_TimeLeft;
  
	void Start () {
        m_PoP = 10;
        m_MaxPoP = 100;
        m_TimeLeft = 15;
	}
	
	void Update () {

        m_TimeLeft -= Time.deltaTime;

        if (m_TimeLeft <= 0)
        {
            //the amount of time between the PoP increases depends on the number of villagers
            m_PoP =  m_PoP + (int) PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM / 10;
            if (m_PoP > m_MaxPoP) m_PoP = m_MaxPoP;
            if (PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.Count <= 6) m_TimeLeft = 25;
            else if (PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.Count <= 8) m_TimeLeft = 30;
            else if (PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.Count <= 10) m_TimeLeft = 35;
            else m_TimeLeft = 40;
        }
    }
}
